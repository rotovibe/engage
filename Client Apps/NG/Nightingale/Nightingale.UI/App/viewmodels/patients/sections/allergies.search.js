define(['models/base', 'config.services', 'services/datacontext', 'services/session'],
    function (models, servicesConfig, datacontext, session) {

        // Create an end point to use
        var endpoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Search/allergy', 'AllergySearch');

        var ctor = function () {
            var self = this;
            // A list of observations
            self.allergies = ko.observableArray();
            // A Bloodhound adapter to use when searching for obs in the typeahead
        };

        ctor.prototype.filterAllergies = function () {
            var self = this;
            var tempArray = self.allergies().slice(0);
            // Remove items already matching a current active not-deleted problem observation
            var filteredArray = ko.utils.arrayFilter(tempArray, function (obs) {
                var truthy = true;
                ko.utils.arrayForEach(self.selectedPatient().allergies(), function (patsObs) {
                    if (patsObs.state() && (patsObs.state().name().toLowerCase() === 'active') && (patsObs.name() === obs.name() && !patsObs.deleteFlag())) {
                        truthy = false;
                    }
                });
                return truthy;
            });
            self.allergies(filteredArray);
            self.allergyBloodhound.transport.constructor.resetCache();
        };

        ctor.prototype.addNewAllergy = function () {
            var self = this;
            datacontext.allergySaving(true);
            var newAllergy = ko.observable();
            // Get observation by name
            var matchedAllergy = ko.utils.arrayFirst(self.allergies(), function (obs) {
                return obs.Name.toLowerCase() === self.trimmedAllergy().toLowerCase();
            });
            // If the matching allergy is found,
            if (matchedAllergy) {
                // Initialize an instance of it
                datacontext.initializeAllergy(newAllergy, 'PatientAllergy', matchedAllergy.Id, self.selectedPatient().id()).then(dataReturned);
            } else {
                // Else create a new one
                // Strip out the word new
                var cleanedAllergy = self.trimmedAllergy().replace(' (New)','');
                datacontext.allergySaving(true);
                datacontext.initializeNewAllergy(cleanedAllergy).then(initializedNew);

                function initializedNew(data) {
                    // Get the new allergy from the results
                    var createdAllergy = data.results[0];
                    // Set matched allergy to an object to reference later
                    matchedAllergy = { Id: createdAllergy.id(), Name: createdAllergy.name(), DisplayName: createdAllergy.name() };
                    datacontext.initializeAllergy(newAllergy, 'PatientAllergy', createdAllergy.id(), self.selectedPatient().id(), true).then(dataReturned);
                }
            }
            
            function dataReturned(data) {
                // If it is a new observation, set it as so so it will
                // deleted from cache if changes are cancelled
                newAllergy().isNew(true);
                newAllergy().allergyName(matchedAllergy.Name);
                datacontext.allergySaving(false);
                // self.getAllergies();
                self.selectedAllergy('');
            }
        }

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.selectedPatient = self.settings.selectedPatient;
            // The selected observation
            self.selectedAllergy = ko.observable('');
            self.trimmedAllergy = ko.computed(function () { return self.selectedAllergy().trim()});
            self.resultsMessage = ko.observable('');
            self.canAdd = ko.computed(function () {
                var thisValue = false;
                var allergSaving = datacontext.allergySaving();
                var trimAllerg = self.trimmedAllergy();
                // If a value has been selected,
                if (trimAllerg) {
                    // And the value matches a valid observation,
                    var matchedAllergy = ko.utils.arrayFirst(self.allergies(), function (retAllergy) {
                        return retAllergy.Name.toLowerCase().trim() === trimAllerg.toLowerCase();
                    });
                    if (!matchedAllergy) {
                        // Check if the allergy ends in (New),
                        thisValue = trimAllerg.substr(trimAllerg.length - 5) === '(New)';
                    }
                    if (matchedAllergy && matchedAllergy.isDuplicate !== true) {
                        thisValue = true;
                    }
                }
                if (allergSaving) { thisValue = false; }
                return thisValue;
            });
            self.allergyBloodhound = new Bloodhound({
                datumTokenizer: function (d) {
                    return Bloodhound.tokenizers.whitespace(d.name());
                },
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                limit: 17,
                remote: {
                    url: servicesConfig.remoteServiceName + '/' + endpoint.ResourcePath,        
                    replace: function (url, query) {
                        searchQuery = query;
                        return url + '?Take=15&SearchTerm=' + searchQuery;
                    },
                    ajax: {
                        beforeSend: function (jqXhr, settings) {
                            jqXhr.setRequestHeader('Content-Type', 'application/json; charset=UTF-8');
                            jqXhr.setRequestHeader('Token', session.currentUser().aPIToken());
                        },
                        type: 'GET'
                    },
                    // Filter out what is returned from the database
                    filter: function (parsedResponse) {
                        // If there is a message returned,
                        if (parsedResponse.Message) {
                            self.resultsMessage('');
                            // Set it as the results message
                            self.resultsMessage(parsedResponse.Message);
                        } else {
                            // Else clear it
                            self.resultsMessage('');
                        }
                        self.allergyBloodhound.clear();
                        var selectedallergy = self.trimmedAllergy();
                        // Check if the allergy has already been added to the individual
                        var matchingPatientAllergy = ko.utils.arrayFirst(self.selectedPatient().allergies(), function (patallerg) {
                            return (patallerg.allergyName().toLowerCase().trim() === selectedallergy.toLowerCase() && patallerg.status().name() === 'Active');
                        });
                        ko.utils.arrayForEach(parsedResponse.Allergies, function (allerg) {
                            allerg.DisplayName = allerg.Name;
                            // Check if the allergy has already been added to the individual
                            var matchingAllergy = ko.utils.arrayFirst(self.selectedPatient().allergies(), function (patallerg) {
                                return patallerg.allergyName().toLowerCase().trim() === allerg.Name.toLowerCase().trim() && patallerg.status().name() === 'Active';
                            });
                            if (matchingAllergy || (matchingPatientAllergy && matchingPatientAllergy.allergyName().toLowerCase().trim() === allerg.Name.toLowerCase().trim())) {
                                allerg.DisplayName = '(Duplicate) ' + allerg.DisplayName;
                                allerg.isDuplicate = true;
                            }
                        });
                        self.allergies(parsedResponse.Allergies);
                        var newAllergy = { Id: -1, Name: selectedallergy, DisplayName: selectedallergy + ' (New)' };
                        // Check if the results match the text
                        var matchingResponse = ko.utils.arrayFirst(parsedResponse.Allergies, function (allerg) {
                            return allerg.Name.toLowerCase().trim() === selectedallergy.toLowerCase();
                        });
                        // If it doesn't match anything else, add a new one
                        if (!matchingResponse && !matchingPatientAllergy) {
                            parsedResponse.Allergies.push(newAllergy);
                        }
                        return parsedResponse.Allergies;
                    },
                    rateLimitWait: 500,
                    cache: false
                }
            });
            self.allergyBloodhound.initialize();
        };

        ctor.prototype.attached = function () {
            var self = this;
            if (self.allergies().length === 0) {
                //self.getAllergies();
            }
        };

        ctor.prototype.getAllergies = function () {
            var self = this;
            if (self.selectedPatient()) {
                // Should already have all observations in cache, no need to hit server
                //return datacontext.getEntityList(self.allergies, 'Allergy', 'fakeEndPoint', 'typeId', self.allergyTypeId, false).then(dataReturned);
            }
            function dataReturned() {
                // Temporary holder array
                self.filterAllergies();
            }
        }

        return ctor;
    });