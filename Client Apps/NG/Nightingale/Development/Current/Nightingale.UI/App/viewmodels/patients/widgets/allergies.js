define(['models/base', 'config.services', 'services/datacontext', 'services/session', 'viewmodels/patients/medications/index', 'viewmodels/shell/shell', 'viewmodels/patients/data/index', 'viewmodels/patients/index'],
    function (modelConfig, servicesConfig, datacontext, session, medicationsIndex, shell, dataIndex, patientsIndex) {

        // var modalShowing = ko.observable(true);

        var ctor = function () {
            var self = this;
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.selectedPatient = self.settings.selectedPatient;
            self.widget = self.settings.widget;
            self.allergyTypes = ko.computed(datacontext.enums.allergyTypes);
            self.selectedAllergyTypes = ko.observableArray();
            self.allergyStatuses = ko.computed(datacontext.enums.allergyStatuses);
            // Set the initial allergy status to be active
            var initialStatus = ko.utils.arrayFirst(self.allergyStatuses(), function (algsts) {
                // Only return the active status
                return algsts.name() === 'Active';
            });
            self.selectedAllergyStatuses = ko.observableArray([initialStatus]);
            self.canReset = ko.computed(function () {
                var statuses = self.selectedAllergyStatuses();
                var types = self.selectedAllergyTypes();
                // Check if it is still in the initial state
                var idsMatch = (statuses.length > 0 && '1' === statuses[0].id());
                if (statuses.length === 1 && idsMatch && types.length === 0) {
                    return false;
                } else {
                    return true;
                }
            });
            self.resetFilters = function () {
                self.selectedAllergyTypes([]);
                self.selectedAllergyStatuses([initialStatus]);
            };
            self.refreshData = function () {
                patientsIndex.getPatientsAllergies();
            };
            self.allergySaving = ko.computed(datacontext.allergySaving);
            self.selectedSortColumn = ko.observable();
            self.myComputedAllergies = ko.observableArray();
            self.myAllergies = ko.computed(function () {
                // var theseAllergies = self.selectedPatient().allergies();
                var selectedallergytypes = self.selectedAllergyTypes();
                var selectedallergystatuses = self.selectedAllergyStatuses();
                var selectedpatient = self.selectedPatient();
                var theseAllergies = [];
                //Subscribe to localcollection todos
                var allAllergies = self.selectedPatient().allergies();

                var orderProp = self.selectedSortColumn() ? self.selectedSortColumn() : 'allergyName';
                // Add the second and third orders to the string
                var finalOrderString = orderProp;
                // Go get the todos
                //theseAllergies = datacontext.getInterventionsQuery(params, orderProp);
                var finalallergies = [];

                var params = [];
                params.push(new modelConfig.Parameter('patientId', selectedpatient.id(), '=='));
                if (selectedallergystatuses.length === 1) {
                    params.push(new modelConfig.Parameter('statusId', selectedallergystatuses[0].id(), '=='));
                }
                var patientId = self.selectedPatient().id();
                finalallergies = datacontext.getPatientAllergiesQuery(params, orderProp);
                // If allergy types were selected,
                if (selectedallergytypes.length > 0) {
                    // For each of the selected types,
                    ko.utils.arrayForEach(selectedallergytypes, function (allgtypeid) {
                        // Filter out allergies that don't match the type selected
                        var tempAllergies = ko.utils.arrayFilter(finalallergies, function (allg) {
                            var matches = ko.utils.arrayFirst(allg.allergyTypeIds(), function (ati) {
                                return ati.id() === allgtypeid.id();
                            })
                            return !!matches;
                        });
                        // Check if we have already added the allergy
                        ko.utils.arrayForEach(tempAllergies, function (temps) {
                            // If not,
                            if (theseAllergies.indexOf(temps) === -1) {
                                // Add it
                                theseAllergies.push(temps);
                            }
                        });
                    });
                    finalallergies = theseAllergies;
                }
                return finalallergies;
            });
            self.activeAllergyColumns = ko.computed(function () {
                return ['expand','name','severity-small','status'];
            });
            self.toggleAllergySort = function (sender) {
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
            self.addAllergy = function (goal) {
                dataIndex.addData();
                // dataIndex.activeDataType(dataIndex.allergiesType);
                // dataIndex.modalEntity().activeDataType(dataIndex.allergiesType);

                function doSomething(task) {
                    // Show the modal
                    //self.editAllergy(task, 'Add Task');
                }
            };
            self.editAllergy = function (task, msg) {
                var thisGoal = task.goal();
                // Edit this task
                var modalEntity = ko.observable(new ModalEntity(task, 'description'));
                var saveOverride = function () {
                    saveTask(task);
                };
                var cancelOverride = function () {
                    cancel(task);
                    getGoalDetails(thisGoal);
                };
                msg = msg ? msg : 'Edit Task';
                editEntity(msg, modalEntity, 'viewmodels/templates/task.edit', saveOverride, cancelOverride);
            };
        };

        ctor.prototype.toggleExpand = function (sender) {
            sender.isExpanded(!sender.isExpanded());
            if (!sender.isOpen()) {
                sender.isOpen(true);
            }
        };

        ctor.prototype.addEntity = function (type, goal, startDate, assignedToId) {
            var self = this;
            var thisPatientId = self.selectedPatient().id();
            var thisGoalId = goal.id();
            return datacontext.initializeEntity(null, type, thisPatientId, thisGoalId).then(entityReturned);

            function entityReturned(data) {
                var thisId = data.httpResponse.data.Id;
                if (thisId) {
                    var params = {};
                    params.id = thisId;
                    params.patientGoalId = thisGoalId;
                    params.statusId = 1;
                    if (startDate) {
                        params.startDate = startDate;
                    }
                    if (assignedToId) {
                        params.assignedToId = assignedToId;
                    }
                    var thisEntity = datacontext.createEntity(type, params);
                    return thisEntity;
                }
                else {
                    var thisTask = data.results[0];
                    thisTask.startDate(new Date());
                    thisTask.statusId(1);
                    thisTask.patientGoalId(thisGoalId);
                    return thisTask;
                }
            }
        };

        return ctor;

        function save(goal) {
            datacontext.saveGoal(goal);
        }

        function saveIntervention(entity) {
            datacontext.saveIntervention(entity);
        }

        function saveTask(entity) {
            datacontext.saveTask(entity);
        }

        function saveGoal(entity) {
            datacontext.saveGoal(entity);
        }

        function cancel(item) {
            item.entityAspect.rejectChanges();
        }

        function getGoalDetails(goal) {
            medicationsIndex.getGoalDetails(goal);
        };

        function ModalEntity(entity, reqpropname) {
            var self = this;
            self.entity = entity;
            // Object containing parameters to pass to the modal
            self.activationData = { entity: self.entity };
            // Create a computed property to subscribe to all of
            // the patients' observations and make sure they are
            // valid
            self.canSave = ko.computed(function () {
                var result = self.entity[reqpropname]();
                // The active goal needs a property passed in from reqpropname
                return result;
            });
        }

        function sortAllergies (patientId, allergies, prop) {
            var theseAllergies = allergies;

            var finalAllergies = datacontext.singleSort(patientId, allergies, 'PatientAllergy', prop);

            return finalAllergies;
        }

    });;