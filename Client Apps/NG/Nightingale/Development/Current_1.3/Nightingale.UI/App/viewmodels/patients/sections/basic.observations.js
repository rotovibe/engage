define(['models/base', 'config.services', 'services/datacontext', 'services/session'],
    function (models, servicesConfig, datacontext, session) {

        var ctor = function () {
            var self = this;
            //// A list of additional observations
            self.basicObservations = ko.observableArray();
            self.observationsEndPoint = ko.computed(function () {
                var currentUser = session.currentUser();
                if (!currentUser) {
                    return '';
                }
                return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'PatientObservation');
            });
        };

        ctor.prototype.alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.activeDataType = self.settings.activeDataType;
            self.selectedPatient = self.settings.selectedPatient;
            self.getBasicObservationsById = function () {
                if (self.activeDataType()) {
                    // Get a list of this patient's observations
                    var theseObservations = self.selectedPatient().observations();
                    // Use a temporary array to hold the new observations
                    var tempObservations = [];
                    // For each of the patients observations,
                    ko.utils.arrayForEach(theseObservations, function (obs) {
                        // If the observation is new,
                        if (obs.isNew() && (obs.typeId() === self.activeDataType().id()) && (obs.standard() === true)) {
                            if (tempObservations.indexOf(obs) === -1) {
                                tempObservations.push(obs);   
                            }
                        }
                    });
                    // If no observations were found,
                    if (tempObservations.length === 0 && self.activeDataType().name() !== 'Problems') {
                        // Go initialize a bunch more
                        datacontext.getEntityList(self.basicObservations, self.observationsEndPoint().EntityType, self.observationsEndPoint().ResourcePath + self.selectedPatient().id() + '/Observation', 'typeId', self.activeDataType().id(), true).then(dataReturned);
                    } else {
                        // Or else, set the found observations
                        self.basicObservations(tempObservations);
                    }
                }
                function dataReturned(data) {
                    // For each of the observations initialized,
                    ko.utils.arrayForEach(self.basicObservations(), function (obs) {
                        // As long as it matches what we are looking for, (all should)
                        if ((obs.typeId() === self.activeDataType().id()) && (obs.standard() === true)) {
                            // Set it to new
                            obs.isNew(true);
                        }
                    });
                    // Reset the search engine's cache
                }
            }
            //self.activeDataTypeToken = self.activeDataType.subscribe(function (newValue) {
            //     self.basicObservations([]);
            //     // Go get a list of care members for the currently selected patient
            //     self.getBasicObservationsById();
            // });
            if (self.basicObservations().length === 0) {
                self.getBasicObservationsById();
            }
            self.computedBasicObservations = ko.computed(function () {
                var theseObservations = self.basicObservations();
                theseObservations = ko.utils.arrayFilter(theseObservations, function(obs) {
                    return obs.isNew();
                });
                return theseObservations.sort(self.alphabeticalNameSort)
            }).extend({ throttle: 50 });
            self.addNew = function (sender) {
                var newObservation = ko.observable();
                if (moment(sender.startDate()).isValid()) {
                    // Go save this and initialize a new one
                    datacontext.initializeObservation(newObservation, 'PatientObservation', sender.observationId(), sender.typeId(), sender.patientId()).then(dataReturned);                        
                }
                function dataReturned() {
                    // Blow up the previous value for each returned observations' values
                    ko.utils.arrayForEach(newObservation().values(), function (value) {
                        value.previousValue().value(null);
                        value.previousValue().source(null);
                        value.previousValue().startDate(null);
                        value.previousValue().unit(null);
                    });
                    newObservation().isNew(true);
                    newObservation().entityAspect.acceptChanges();
                    // Get the index of the sent observation
                    var senderIndex = self.basicObservations.indexOf(sender);
                    // Add the new observation at a specific index
                    self.basicObservations.splice(senderIndex + 1, 0, newObservation());
                    // Set the new observation to null
                    newObservation(null);
                }
            }
        };

        ctor.prototype.attached = function () {

        };

        ctor.prototype.detached = function () {
            var self = this;
            // if (self.activeDataTypeToken) {
            //     self.activeDataTypeToken.dispose();
            // }
        };

        return ctor;
    });