define(['models/base', 'config.services', 'services/datacontext', 'services/session', 'viewmodels/patients/medications/index', 'viewmodels/shell/shell', 'viewmodels/patients/data/index', 'viewmodels/patients/index'],
    function (modelConfig, servicesConfig, datacontext, session, medicationsIndex, shell, dataIndex, patientsIndex) {

        var ctor = function () {
            var self = this;
        };

        var typeNotSet = {
            id: ko.observable(''),
            name: ko.observable('Not Set')
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.selectedPatient = self.settings.selectedPatient;
            self.widget = self.settings.widget;
            self.defaultSort = self.settings.defaultSort;
            // self.medSuppTypes = ko.computed(datacontext.enums.medSuppTypes);
            self.selectedMedicationTypes = ko.observableArray();
            self.statusIds = self.widget.statusIds;
            self.allowAdd = self.widget.allowAdd;
            self.resetFilters = function () {
                self.selectedMedicationTypes([]);
            };
            self.refreshView = function () {
                patientsIndex.getPatientMedications();
            }
            self.addMedication = function () {
                dataIndex.addData();
            };
            self.refreshData = function () {
                console.log('refresh the data');
            };
            self.medicationSaving = ko.computed(datacontext.medicationSaving);
            self.selectedSortColumn = ko.observable();
            self.myComputedMedications = ko.observableArray();
            // Which categories are available to filter on
            self.medSuppTypes = ko.computed(function () {
                var thesetypes = datacontext.enums.medSuppTypes();
                var theseTypes = thesetypes.slice(0, thesetypes.length);
                // If there is not already a not set option,
                if (theseTypes.indexOf(typeNotSet) === -1) {
                    // Add it also
                    theseTypes.push(typeNotSet);
                }
                return theseTypes;
            });
            // If there is selected categories, we need to filter out all the others
            self.activeFilters = ko.computed(function () {
                var selectedmedicationtypes = self.selectedMedicationTypes();
                var returnfilters = [];
                // for each status passed in, only query for those status ids
                ko.utils.arrayForEach(self.statusIds, function (statid) {
                    returnfilters.push(new modelConfig.Parameter('statusId', statid, '=='));
                });
                // Check if there are categories selected
                if (selectedmedicationtypes.length > 0) {
                    // Grab all the other categories
                    var othertypes = ko.utils.arrayFilter(self.medSuppTypes(), function (medtype) {
                        // Check if this category is a selected category
                        var matchingtype = ko.utils.arrayFirst(selectedmedicationtypes, function (selectType) {
                            return selectType.id() === medtype.id();
                        });
                        // If there is a match found,
                        if (matchingtype) {
                            // Don't return this category
                            return false;
                        } else {
                            // If not do return it
                            return true;
                        }
                    });
                    ko.utils.arrayForEach(othertypes, function (medtype) {
                        returnfilters.push(new modelConfig.Parameter('typeId', medtype.id(), '!='));
                        // returnfilters.push(new modelConfig.Parameter('typeId', null, '!='));
                    });
                    // Not set is null, so only filter out nulls if that option isn't selected
                    var foundselectednull = false;
                    ko.utils.arrayForEach(selectedmedicationtypes, function (medtype) {
                        if (medtype.id() === '') {
                            foundselectednull = true;
                        }
                    });
                    // If we didn't select for nulls
                    if (!foundselectednull) {
                        // Don't return nulls either
                        returnfilters.push(new modelConfig.Parameter('typeId', '', '!='));
                        returnfilters.push(new modelConfig.Parameter('typeId', null, '!='));
                    }
                }
                return returnfilters;
            });
            self.myMedications = ko.computed(function () {
                var selectedmedicationtypes = self.selectedMedicationTypes();
                var selectedpatient = self.selectedPatient();
                var theseMedications = [];
                var sortDefaultProp = self.defaultSort ? self.defaultSort : 'name';
                //Subscribe to localcollection todos
                var allMedications = self.selectedPatient().medications();
                var orderProp = self.selectedSortColumn() ? self.selectedSortColumn() : sortDefaultProp;
                // Add the second and third orders to the string
                var finalOrderString = orderProp;
                // Go get the todos
                var finalmedications = [];
                var tempmedications = [];

                var params = [];
                params.push(new modelConfig.Parameter('patientId', selectedpatient.id(), '=='));

                // If there are filters,
                if (self.activeFilters().length > 0) {
                    // Add them as parameters
                    ko.utils.arrayForEach(self.activeFilters(), function (param) {
                        params.push(param);
                    });
                }

                if (selectedmedicationtypes.length === 1) {
                    params.push(new modelConfig.Parameter('typeId', selectedmedicationtypes[0].id(), '=='));
                }
                var patientId = self.selectedPatient().id();
                tempmedications = datacontext.getPatientMedicationsQuery(params, orderProp);
                // Don't return new meds
                finalmedications = ko.utils.arrayFilter(tempmedications, function (tempmed) {
                    return !tempmed.isNew();
                });
                return finalmedications;
            }).extend({ throttle: 1 });
            self.activeColumns = ko.computed(function () {
                return ['expand','name', 'sortdate', 'status'];
            });
            self.toggleSort = function (sender) {
                // If the current column is the one to sort by
                if (self.selectedSortColumn() && self.selectedSortColumn().indexOf(sender.sortProperty) !== -1) {
                    // If it ends in desc
                    if (self.selectedSortColumn() && self.selectedSortColumn().substr(self.selectedSortColumn().length - 4, 4) === 'desc' ) {
                // Clear the sort column, as it should be undone
                        self.selectedSortColumn(null);
                    } else {
                        // Else set it as the sort column
                        self.selectedSortColumn(sender.sortProperty + ' desc');
                    }
                } else {
                    // Else set it as the new sort column
                    self.selectedSortColumn(sender.sortProperty);
                }
            };
            self.isOpen = self.widget.isOpen;
            self.isFullScreen = self.widget.isFullScreen;
            self.filtersHeaderOpen = ko.observable(true);
            self.filtersOpen = ko.observable(false);
            self.toggleHeaderOpen = function  (sender, widgetOpen) {
                if (widgetOpen()) {
                    sender(!sender());
                }
            }
            // Toggle which column is open
            self.toggleOpenColumn = function () {
                self.widget.column.isOpen(!self.widget.column.isOpen());
            }
            self.toggleFullScreen = function (sender) {
                self.isFullScreen(!self.isFullScreen());
            }
            // Toggle whether the widget is open or not
            self.toggleWidgetOpen = function (sender) {
                // Find how many widgets are open
                var openwidgets = ko.utils.arrayFilter(self.widget.column.widgets(), function (wid) {
                    return wid.isOpen();
                });
                // If the widget is the only open widget
                if (openwidgets.length === 1 && openwidgets[0] === self.widget) {
                    // Do nothing
                } else {
                    sender.isOpen(!sender.isOpen());
                }
            };
            self.addMedication = function (goal) {
                dataIndex.addData();
            };
            self.minimizeThisColumn = function () {
                self.widget.column.isOpen(false);
            }
            self.maximizeThisColumn = function () {
                self.widget.column.isOpen(true);
            }

        };

        ctor.prototype.toggleExpand = function (sender) {
            sender.isExpanded(!sender.isExpanded());
            if (!sender.isOpen()) {
                sender.isOpen(true);
            }
        };

        ctor.prototype.detached = function(){
            var self = this;
            // dispose computeds:
            self.medicationSaving.dispose();
            self.medSuppTypes.dispose();
            self.activeFilters.dispose();
            self.myMedications.dispose();
            self.activeColumns.dispose();
        }
        return ctor;

    });;