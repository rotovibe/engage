define(['models/base', 'config.services', 'services/datacontext', 'services/session'],
    function (models, servicesConfig, datacontext, session) {

        // Create an end point to use
        var endpoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Search/Meds/ProprietaryNames', 'PatientMedication');

        var ctor = function () {
            var self = this;
            // A list of observations
            self.medications = ko.observableArray();
            self.newMedication = ko.observable();
            // A Bloodhound adapter to use when searching for obs in the typeahead
        };

        ctor.prototype.types = ko.computed(datacontext.enums.medSuppTypes);
        ctor.prototype.categories = ko.computed(datacontext.enums.medicationCategories);
        ctor.prototype.statuses = ko.computed(datacontext.enums.medicationStatuses);

        ctor.prototype.addNewMedication = function () {
            var self = this;
            datacontext.medicationSaving(true);
            var createdMedication = ko.observable();
            // Get observation by name
            var matchedMedication = ko.utils.arrayFirst(self.medications(), function (obs) {
                return obs.Text.toLowerCase() === self.trimmedMedication().toLowerCase();
            });
            // If the matching medication is found,
            if (matchedMedication) {
                // Initialize an instance of it
                //datacontext.initializeMedication(newMedication, 'PatientMedication', matchedMedication.Id, self.selectedPatient().id()).then(dataReturned);
                createdMedication(datacontext.initializeNewPatientMedication(self.selectedPatient()));
                createdMedication.name(matchedMedication.Name);
            } else {
            }
        }

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.selectedPatient = self.settings.selectedPatient;
            // The selected observation
            self.trimmedMedication = ko.computed(function () { 
                var thismed = self.newMedication();
                if (thismed) {
                    return thismed.name();
                } else {
                    return '';
                }
            });
            self.resultsMessage = ko.observable('');
            // Initialize this widget
            self.initializeMedSearch(self.newMedication);
            self.canAdd = ko.computed(function () {
                var thisValue = false;
                var allergSaving = datacontext.medicationSaving();
                var trimAllerg = self.trimmedMedication();
                var patientmedications = self.selectedPatient().medications();
                var newMedicationExists = ko.utils.arrayFirst(patientmedications, function (patmed) {
                    return patmed.isNew();
                });
                // If a value has been selected,
                if (trimAllerg) {
                    // And the value matches a valid observation,
                    var matchedMedication = ko.utils.arrayFirst(self.medications(), function (retMedication) {
                        return retMedication.Text.toLowerCase().trim() === trimAllerg.toLowerCase();
                    });
                    if (!matchedMedication) {
                        // Check if the medication ends in (New),
                        thisValue = trimAllerg.substr(trimAllerg.length - 5) === '(New)';
                    }
                    if (matchedMedication && matchedMedication.isDuplicate !== true) {
                        thisValue = true;
                    }
                }
                if (allergSaving || newMedicationExists) { thisValue = false; }
                return thisValue;
            });
            self.medicationBloodhound = new Bloodhound({
                datumTokenizer: function (d) {
                    return Bloodhound.tokenizers.whitespace(d.name());
                },
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                limit: 17,
                remote: {
                    url: servicesConfig.remoteServiceName + '/' + endpoint.ResourcePath,        
                    replace: function (url, query) {
                        searchQuery = query;
                        return url + '?Take=15&Term=' + searchQuery;
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
                        self.medicationBloodhound.clear();
                        var selectedmedication = self.trimmedMedication();
                        // Check if the medication has already been added to the individual
                        var matchingPatientMedication = ko.utils.arrayFirst(self.selectedPatient().medications(), function (patallerg) {
                            return patallerg.name().toLowerCase().trim() === selectedmedication.toLowerCase();
                        });
                        ko.utils.arrayForEach(parsedResponse.ProprietaryNames, function (allerg) {
                            allerg.DisplayName = allerg.Text;
                            // Check if the medication has already been added to the individual
                            var matchingMedication = ko.utils.arrayFirst(self.selectedPatient().medications(), function (patallerg) {
                                return patallerg.name().toLowerCase().trim() === allerg.Text.toLowerCase().trim();
                            });
                            if (matchingMedication || (matchingPatientMedication && matchingPatientMedication.name().toLowerCase().trim() === allerg.Text.toLowerCase().trim())) {
                                allerg.DisplayName = '(Duplicate) ' + allerg.DisplayName;
                                allerg.isDuplicate = true;
                            }
                        });
                        self.medications(parsedResponse.ProprietaryNames);
                        var newMedication = { Id: -1, Text: selectedmedication, DisplayName: selectedmedication + ' (New)' };
                        // Check if the results match the text
                        var matchingResponse = ko.utils.arrayFirst(parsedResponse.ProprietaryNames, function (allerg) {
                            return allerg.Text.toLowerCase().trim() === selectedmedication.toLowerCase();
                        });
                        // If it doesn't match anything else, add a new one
                        if (!matchingResponse && !matchingPatientMedication) {
                            parsedResponse.ProprietaryNames.push(newMedication);
                        }
                        return parsedResponse.ProprietaryNames;
                    },
                    rateLimitWait: 500,
                    cache: false
                }
            });
            self.medicationBloodhound.initialize();
        };

        ctor.prototype.initializeMedSearch = function () {
            var self = this;
            // Check for an already created medication
            var foundMed = ko.utils.arrayFirst(self.selectedPatient().medications(), function (med) {
                return med.isNew();
            });
            if (foundMed) {
                self.newMedication(foundMed);
            } else {
                self.newMedication(datacontext.initializeNewPatientMedication(self.selectedPatient()));
            }
        }

        return ctor;
    });