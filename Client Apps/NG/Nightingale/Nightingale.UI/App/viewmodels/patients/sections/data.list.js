define(['models/base', 'config.services', 'services/datacontext', 'services/session', 'viewmodels/patients/data/index', 'viewmodels/shell/shell'],
    function (modelConfig, servicesConfig, datacontext, session, dataIndex, shell) {

        // Service endpoint for getting history of an observation
        var observationsHistoryEndPoint = ko.computed(function () {
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'PatientObservation');
        });

        // Complex sort for observation groups
        var groupSort = function (l, r) {
            // Primary sort property
            var p1 = l.parentObservation().type().name();
            var p2 = r.parentObservation().type().name();

            // Secondary sort property
            var o1 = l.parentObservation().name().toLowerCase();
            var o2 = r.parentObservation().name().toLowerCase();
            
            if (p1 != p2) {
                if (p1 < p2) return -1;
                if (p1 > p2) return 1;
                return 0;
            }
            if (o1 < o2) return -1;
            if (o1 > o2) return 1;
            return 0;
        };

        // Sort by end date
        // var endDateSort = function (l, r) { return (l.endDate() == r.endDate()) ? (l.endDate() < r.endDate() ? 1 : -1) : (l.endDate() < r.endDate() ? 1 : -1) };
        var endDateSort = function (a,b) { var x = Date.parse(a.endDate()); var y = Date.parse(b.endDate()); if (x == y) { return 0; } if (isNaN(x) || x > y) { return -1; } if (isNaN(y) || x < y) { return 1; }}
        // Create a constructor for this view model
        var ctor = function () {
            var self = this;
        };
        
        // Activate callback used by router
        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            // The currently selected patient
            self.selectedPatient = ko.observable(self.settings.selectedPatient);
            // An array of groups of observations
            self.groupArray = ko.observableArray();
            self.blowUpObservations = ko.computed(function () {
                // Subscribe to the observable
                var something = dataIndex.needToRefreshObservations();
                // Set it back to false
                if (dataIndex.needToRefreshObservations()) {
                    dataIndex.needToRefreshObservations(false);
                    self.groupArray([]);
                }
                return false;
            })
            // A computed and filtered list of the current patients observations
            self.dataObservations = ko.computed(function() {
                var theseObservations = self.selectedPatient().observations();                
                // Filter the observations
                var tempObservations = ko.utils.arrayFilter(theseObservations, function (obs) {
                    // Only return the observations that are not new
                    return !obs.isNew() && !obs.deleteFlag();
                });
                return tempObservations;
            }).extend({ throttle: 50 });
            // A group of the computed observations
            self.dataObservationGroups = ko.computed({
                read: function () {
                    var theseObservations = self.dataObservations();
                    theseObservationGroups = groupObservations(theseObservations);
                    // For each group of observations,
                    ko.utils.arrayForEach(theseObservationGroups, function (obsgrp) {
                        // See if there is already a group with this name in the group array
                        var thisMatchedGroup = ko.utils.arrayFirst(self.groupArray(), function (arrayGroup) {
                            return arrayGroup.parentObservation() ? arrayGroup.parentObservation().name() === obsgrp.observations[0].name() : false;
                        });
                        // If a group doesn't yet exist,
                        if (!thisMatchedGroup) {
                            // Add a new group to the group of arrays
                            var newGroup = new ObservationGroup(obsgrp.observations);
                            self.groupArray.push(newGroup);
                        } else {
                            // Check if this group already has each observation
                            ko.utils.arrayForEach(obsgrp.observations, function (observation) {
                                // If the matched groups' observations list doesn't contain the observation already,
                                if (thisMatchedGroup.observations.indexOf(observation) === -1) {
                                    // Add it
                                    thisMatchedGroup.observations.push(observation);
                                } else {
                                    // Do nothing
                                }
                            });
                        }
                    });
                    return self.groupArray();
                },
                write: function (newValue) {
                    // When we write to the groups, we are just blowing them up
                    self.groupArray([]);
                }
                
            }).extend({ throttle: 50 });
            // We need to clean out the groups whenever the patient changes
            self.selectedPatient.subscribe(function () {
                self.dataObservationGroups(false);
            });
            // A sorted grouping of the observations 
            //      We sort separately to prevent re-sorting everytime a new observation is added
            self.computedObservationGroups = ko.computed(function () {
                var theseObsGroups = self.dataObservationGroups();
                ko.utils.arrayForEach(theseObsGroups, function (group) {
                    // If the group is empty
                    if (group && group.length === 0) {
                        // Remove it
                        self.dataObservationGroups.remove(group);
                    }
                });
                if (theseObsGroups && theseObsGroups.length > 0 ) {
                    return theseObsGroups.sort(groupSort);
                } else {
                    return [];
                }
            }).extend({ throttle: 500 });
            // Future story
            self.removeObservation = function (obs) {
                obs.deleteFlag(true);
                dataIndex.saveDataEntry();
            }
            self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
            self.alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };
        };

        ctor.prototype.toggleExpand = function (sender) {
            // Toggle open or closed
            sender.isOpen(!sender.isOpen());
            // If it has just been set to open,
            if (sender.isOpen()) {
                // Check if observations have been expanded yet,
                if (sender.hasBeenExpanded()) {
                    // We already have history, do nothing
                } else {
                    // Else, set the isLoading property to true
                    sender.isLoadingObs(true);
                    var thisObservationId = sender.parentObservation().observationId();
                    var patientId = sender.parentObservation().patientId();
                    // And go get more observations
                    datacontext.getEntityList(null, observationsHistoryEndPoint().EntityType, observationsHistoryEndPoint().ResourcePath + patientId + '/Observations/' + thisObservationId + '/Historical', null, null, true).then(dataReturned);
                }
            }

            function dataReturned(data) {
                // Turn off the spinner
                sender.isLoadingObs(false);
                sender.hasBeenExpanded(true);
            }
        };

        ctor.prototype.attached = function () {
        };

        return ctor;

        function groupObservations(observations) {
            // List of observation names
            var obsGroups = [];
            //  Grouped observations
            var roughGroups = [];
            // For each of the observations,
            ko.utils.arrayForEach(observations, function (obs) {
                // If the group of observations doesn't already contain the name of this group,
                if (obsGroups.indexOf(obs.name()) === -1) {
                    // Add it
                    obsGroups.push(obs.name());
                }
            });
            // For each of the unique groups,
            ko.utils.arrayForEach(obsGroups, function (group) {
                // Find the matching observations,
                var matchingObservations = ko.utils.arrayFilter(observations, function (obs) {
                    return group === obs.name();
                });
                // And add the group to rough groups
                roughGroups.push(new tempGroup(matchingObservations));
            });
            return roughGroups;
            function tempGroup(obs) {
                var self = this;
                self.observations = obs;
            }
        }

        // Group of observations
        function ObservationGroup (observations) {
            var self = this;
            self.isOpen = ko.observable(false);
            self.isLoadingObs = ko.observable(false);
            self.hasBeenExpanded = ko.observable(false);
            self.observations = ko.observableArray(observations);
            self.computedObservations = ko.computed(function () {
                // Force the computed to recompute when the end date changes
                ko.utils.arrayForEach(self.observations(), function (obs) {
                    var something = obs.endDate();
                });
                // Filter the results
                var theseObservations = ko.utils.arrayFilter(self.observations(), function (obs) {
                    // Only return those that are delete flag false
                    return !obs.deleteFlag();
                });
                if (theseObservations.length === 0) { return []; }
                theseObservations = theseObservations.sort(endDateSort);
                // Remove the parent observation
                return theseObservations.slice(1, theseObservations.length);
            }).extend({ throttle: 50 });
            self.parentObservation = ko.computed(function () {
                var theseObservations = self.observations()
                if (theseObservations.length === 0) { return []; }
                theseObservations = theseObservations.sort(endDateSort);
                var firstItem = theseObservations[0];
                return firstItem;
            }).extend({ throttle: 50 });
        }
        
    });;