define(['config.models', 'config.services', 'services/datacontext', 'services/session'],
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
                return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Observation');
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
                    var tempObservations = [];
                    ko.utils.arrayForEach(theseObservations, function (obs) {
                        if ((obs.typeId() === self.activeDataType().id()) && (obs.standard() === true)) {
                            tempObservations.push(obs);
                        }
                    });
                    if (tempObservations.length === 0) {
                        datacontext.getEntityList(self.basicObservations, self.observationsEndPoint().EntityType, self.observationsEndPoint().ResourcePath + self.selectedPatient().id() + '/Observation', 'typeId', self.activeDataType().id(), true).then(dataReturned);
                    } else {
                        console.log('Setting basic observations to this - ', tempObservations);
                        self.basicObservations(tempObservations);
                    }
                }
                function dataReturned(data) {
                    // Reset the search engine's cache
                    console.log(self.basicObservations());
                }
            }
            self.activeDataType.subscribe(function (newValue) {
                console.log('Clearing basic observations');
                self.basicObservations([]);
                // Go get a list of care members for the currently selected patient
                self.getBasicObservationsById();
            });
            if (self.basicObservations().length === 0) {
                self.getBasicObservationsById();
            }
            self.computedBasicObservations = ko.computed(function () {
                console.log('Firing computed basic observations');
                var theseObservations = self.basicObservations();
                return theseObservations.sort(self.alphabeticalNameSort)
            }).extend({ throttle: 50 });
            self.addNew = function (sender) {
                var newObservation = ko.observable();
                if (moment(sender.startDate()).isValid() && sender.computedValue().value()) {
                    // Go save this and initialize a new one
                    datacontext.initializeObservation(newObservation, 'Observation', sender.observationId(), sender.typeId(), sender.patientId()).then(dataReturned);
                }
                function dataReturned() {
                    // Blow up the previous value for each returned observations' values
                    ko.utils.arrayForEach(newObservation().values(), function (value) {
                        value.previousValue().value(null);
                        value.previousValue().source(null);
                        value.previousValue().startDate(null);
                        value.previousValue().unit(null);
                    });
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

        return ctor;
    });