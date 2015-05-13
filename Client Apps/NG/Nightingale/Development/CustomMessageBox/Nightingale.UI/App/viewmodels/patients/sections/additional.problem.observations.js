define(['models/base', 'config.services', 'services/datacontext', 'services/session'],
    function (models, servicesConfig, datacontext, session) {

        var ctor = function () {
            var self = this;
            // A list of observations
            self.problemObservations = ko.observableArray();
            // A Bloodhound adapter to use when searching for obs in the typeahead
        };

        ctor.prototype.filterProblemObservations = function () {
            var self = this;
            var tempArray = self.problemObservations().slice(0);
            // Remove items already matching a current active not-deleted problem observation
            var filteredArray = ko.utils.arrayFilter(tempArray, function (obs) {
                var truthy = true;
                ko.utils.arrayForEach(self.selectedPatient().observations(), function (patsObs) {
                    if (patsObs.state() && (patsObs.state().name().toLowerCase() === 'active') && (patsObs.name() === obs.name() && !patsObs.deleteFlag())) {
                        truthy = false;
                    }
                });
                return truthy;
            });
            self.problemObservations(filteredArray);
            self.probObsBloodhound.transport.constructor.resetCache();
        };

        ctor.prototype.addNewObservation = function () {
            var self = this;
            datacontext.observationsSaving(true);
            var newObservation = ko.observable();
            // Get observation by name
            var matchedObservation = ko.utils.arrayFirst(self.problemObservations(), function (obs) {
                return obs.name().toLowerCase() === self.selectedObservation().toLowerCase();
            });
            if (matchedObservation) {
                datacontext.initializeObservation(newObservation, 'PatientObservation', matchedObservation.id(), matchedObservation.typeId(), self.selectedPatient().id()).then(dataReturned);
            }
            function dataReturned(data) {
                // If it is a new observation, set it as so so it will
                // deleted from cache if changes are cancelled
                newObservation().isNew(true);
                self.getProblemObservations();
                self.selectedObservation(null);
            }
        }

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            // I did not want to hard code this Id in
            // But was promised that it will never change
            // So reluctantly, here it is
            self.problemObservationTypeId = '533d8278d433231deccaa62d';
            //self.activeDataType = self.settings.activeDataType;
            self.selectedPatient = self.settings.selectedPatient;
            // The selected observation
            self.selectedObservation = ko.observable();
            self.canAdd = ko.computed(function () {
                var thisValue = false;
                // If a value has been selected,
                if (self.selectedObservation()) {
                    // And the value matches a valid observation,
                    var matchedObservation = ko.utils.arrayFirst(self.problemObservations(), function (addtlObs) {
                        return addtlObs.name().toLowerCase() === self.selectedObservation().toLowerCase();
                    });
                    if (matchedObservation) {
                        thisValue = true;
                    }
                }
                if (datacontext.observationsSaving()) { thisValue = false; }
                return thisValue;
            });
            self.probObsBloodhound = new Bloodhound({
                datumTokenizer: function (d) {
                    return Bloodhound.tokenizers.whitespace(d.name());
                },
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                limit: 100,
                remote: {
                    url: '%QUERY',
                    transport: function (url, options, onSuccess, onError) {
                        var deferred = $.Deferred();
                        deferred.done(function () { onSuccess(this); });

                        var filterVal = url.toLowerCase();
                        var result = self.problemObservations().filter(function (item) {
                            return !!~item.name().toLowerCase().indexOf(filterVal);
                        });
                        deferred.resolveWith(result);
                        return deferred.promise();
                    }
                }
            });
            self.probObsBloodhound.initialize();
        };

        ctor.prototype.attached = function () {
            var self = this;
            if (self.problemObservations().length === 0) {
                self.getProblemObservations();
            }
        };

        ctor.prototype.getProblemObservations = function () {
            var self = this;
            if (self.selectedPatient()) {
                // Should already have all observations in cache, no need to hit server
                return datacontext.getEntityList(self.problemObservations, 'Observation', 'fakeEndPoint', 'typeId', self.problemObservationTypeId, false).then(dataReturned);
            }
            function dataReturned() {
                // Temporary holder array
                self.filterProblemObservations();
            }
        }

        return ctor;
    });