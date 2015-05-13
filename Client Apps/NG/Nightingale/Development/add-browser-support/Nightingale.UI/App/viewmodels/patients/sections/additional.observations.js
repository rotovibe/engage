define(['models/base', 'config.services', 'services/datacontext', 'services/session'],
    function (models, servicesConfig, datacontext, session) {

        var ctor = function () {
            var self = this;
            // A Bloodhound adapter to use when searching for addt'l obs in the typeahead
        };

        ctor.prototype.addNewObservation = function () {
            var self = this;
            var newObservation = ko.observable();
            // Get observation by name
            var matchedObservation = ko.utils.arrayFirst(self.additionalObservations(), function (obs) {
                return obs.name() === self.selectedAdditionalObservation();
            });
            if (matchedObservation) {
                datacontext.initializeObservation(newObservation, 'PatientObservation', matchedObservation.id(), matchedObservation.typeId(), self.selectedPatient().id()).then(dataReturned);
            }
            function dataReturned() {
                newObservation().isNew(true);
                self.additionalObservationsToEnter.push(newObservation());
                self.selectedAdditionalObservation(null);
            }
        }

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.activeDataType = self.settings.activeDataType;
            self.selectedPatient = self.settings.selectedPatient;
            // A list of additional observations
            self.additionalObservations = ko.observableArray();
            // A list of additional observations to enter data for
            self.additionalObservationsToEnter = ko.observableArray();
            // The selected additional observation
            self.selectedAdditionalObservation = ko.observable();
            self.canAdd = ko.computed(function () {
                var thisValue = false;
                // If a value has been selected,
                if (self.selectedAdditionalObservation()) {
                    // And the value matches a valid additional observation,
                    var matchedObservation = ko.utils.arrayFirst(self.additionalObservations(), function (addtlObs) {
                        return addtlObs.name() === self.selectedAdditionalObservation();
                    });
                    if (matchedObservation) {
                        thisValue = true;
                    }
                }
                return thisValue;
            });
            self.addtlObsBloodhound = new Bloodhound({
                datumTokenizer: function (d) {
                    return Bloodhound.tokenizers.whitespace(d.name());
                },
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                remote: {
                    url: '%QUERY',
                    transport: function (url, options, onSuccess, onError) {
                        var deferred = $.Deferred();
                        deferred.done(function () { onSuccess(this); });

                        var filterVal = url.toLowerCase();
                        var result = self.additionalObservations().filter(function (item) {
                            return !!~item.name().toLowerCase().indexOf(filterVal);
                        });
                        deferred.resolveWith(result);
                        return deferred.promise();
                    }
                }
            });
            self.addtlObsBloodhound.initialize();

            self.getAdditionalObservationsById = function () {
                if (self.activeDataType()) {
                    // Get a list of this patient's observations
                    var theseObservations = self.selectedPatient().observations();
                    var tempObservations = [];
                    ko.utils.arrayForEach(theseObservations, function (obs) {
                        if ((obs.typeId() === self.activeDataType().id()) && (obs.standard() === false && obs.isNew())) {
                            tempObservations.push(obs);
                        }
                    });
                    self.additionalObservationsToEnter(tempObservations);
                }
                function dataReturned(data) {
                    // Reset the search engine's cache
                }
            }

            self.activeDataType.subscribe(function (newValue) {
                // Clear the selections
                self.selectedAdditionalObservation(null);

                self.additionalObservations([]);
                // Go get a list of the additional observations
                self.getAdditionalObservations();
                self.getAdditionalObservationsById();
            });
            // Clone an observation
            self.addNew = function (sender) {
                var newObservation = ko.observable();
                if (moment(sender.startDate()).isValid() && sender.computedValue().value()) {
                    // Go save this and initialize a new one
                    return datacontext.initializeObservation(newObservation, 'PatientObservation', sender.observationId(), sender.typeId(), sender.patientId()).then(dataReturned);
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
                    var senderIndex = self.additionalObservationsToEnter.indexOf(sender);
                    // Add the new observation at a specific index
                    self.additionalObservationsToEnter.splice(senderIndex + 1, 0, newObservation());
                    // Set the new observation to null
                    newObservation(null);
                }
            }
        };

        ctor.prototype.attached = function () {
            var self = this;
            if (self.additionalObservations().length === 0) {
                self.getAdditionalObservations();
                self.getAdditionalObservationsById();
            }
        };

        ctor.prototype.getAdditionalObservations = function () {
            var self = this;
            // Don't get more observations if allergies are selected
            if (self.selectedPatient() && self.activeDataType() && self.activeDataType().id() !== -1 && self.activeDataType().id() !== -2) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'patient/' + self.selectedPatient().id() + '/Observation/Type/' + self.activeDataType().id() + '/MatchLibrary/', 'Observation');
                return datacontext.getEntityList(self.additionalObservations, 'Observation', endPoint.ResourcePath, 'typeId', self.activeDataType().id(), false).then(dataReturned);
            }
            function dataReturned() {
                // Filter the observations
                var nonstandardObservations = ko.utils.arrayFilter(self.additionalObservations(), function (addtl) {
                    // Return only non-standard observations
                    return !addtl.standard();
                });
                self.additionalObservations(nonstandardObservations);
                // Reset the search engine's cache
                self.addtlObsBloodhound.transport.constructor.resetCache();
            }
        }

        return ctor;
    });