define(['plugins/router', 'services/navigation', 'config.services', 'models/base', 'services/session', 'services/datacontext', 'viewmodels/patients/index', 'viewmodels/shell/shell'],
    function (router, navigation, servicesConfig, modelConfig, session, datacontext, patientsIndex, shell) {

        var subscriptionTokens= [];

        var selectedPatient = ko.computed(function () {
            return patientsIndex.selectedPatient();
        });

        var endDateSort = function (a,b) { var x = a.endDate(); var y = b.endDate(); if (x == y) { return 0; } if (isNaN(x) || x > y) { return -1; } if (isNaN(y) || x < y) { return 1; }}

        var navToken = navigation.currentRoute.subscribe(function () {
            if (selectedPatient()) {
                ko.utils.arrayForEach(selectedPatient().goals(), function (thisGoal) {
                    if(thisGoal.isExpanded) {
                        thisGoal.isExpanded(false);
                        thisGoal.isOpen(false);
                    }
                });
            }
        });
        subscriptionTokens.push(navToken);

        var widgets = ko.observableArray();
        var activeGoal = ko.observable();
        var activeTask = ko.observable();
        var activeIntervention = ko.observable();
        var initialized = false;
        var isComposed = ko.observable(true);

        var newGoal = ko.observable();
        var goalModalShowing = ko.observable(true);

        var goalsEndPoint = ko.computed(function() {
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'goal', 'Goal');
        });

        var goalEndPoint = ko.computed(function () {
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Goal');
        });

        function widget(data, column) {
            var self = this;
            self.name = ko.observable(data.name);
            self.path = ko.observable(data.path);
            self.column = column;
            self.isFullScreen = ko.observable(false);
            self.statusIds = data.statusIds;
            self.canAdd = ko.observable(data.canadd);
            self.emptyMesssage = data.emptymessage;
            self.sortOverride = data.sortoverride;
            self.isOpen = ko.observable(data.open);
        }

        function Column(name, open, widgets) {
            var self = this;
            self.name = ko.observable(name);
            self.widgets = ko.observableArray();
            self.isOpen = ko.observable(open).extend({ notify: 'always' });
            var ioToken = self.isOpen.subscribe(function() {
                computedOpenColumn(self);
            });
            subscriptionTokens.push(ioToken);
            $.each(widgets, function (index, item) {
                self.widgets.push(new widget(item, self))
            });
        }

        var goalColumn = ko.observable(new Column('goalColumn', true, [{ name: 'Active Goals', path: 'patients/widgets/goals.html', statusIds: ['1','3'], canadd: true, open: true }, { name: 'Goal History', path: 'patients/widgets/goals.html', statusIds: ['2','4'], canadd: false, open: false, emptymessage: 'No history available.', sortoverride: endDateSort }]));
        var detailsColumn = ko.observable(new Column('goalDetails', false, [{ name: 'GoalDetails', path: 'viewmodels/patients/sections/goal.details', open: true }, { name: 'TaskDetails', path: 'viewmodels/patients/sections/task.details', open: true }, { name: 'InterventionDetails', path: 'viewmodels/patients/sections/intervention.details', open: true }]));
        var openColumn = ko.observable();
        var computedOpenColumn = ko.computed({
            read: function () {
                return openColumn();
            },
            write: function (value) {
                if (!value.isOpen()) {
                    if (value === openColumn() && value.name() === 'goalColumn') {
                        openColumn(detailsColumn());
                    } else if (value === openColumn()) {
                        openColumn(goalColumn());
                    }
                } else {
                    openColumn(value);
                }
            }
        });

        var vm = {
            activate: activate,
            attached: attached,
            deactivate: deactivate,
            detached: detached,
            activeGoal: activeGoal,
            activeTask: activeTask,
            addEntity: addEntity,
            activeIntervention: activeIntervention,
            isComposed: isComposed,
            computedOpenColumn: computedOpenColumn,
            goalColumn: goalColumn,
            detailsColumn: detailsColumn,
            widgets: widgets,
            minimizeThisColumn: minimizeThisColumn,
            maximizeThisColumn: maximizeThisColumn,
            toggleFullScreen: toggleFullScreen,
            selectedPatient: selectedPatient,
            cancelChanges: cancelChanges,
            addGoal: addGoal,
            editGoal: editGoal,
            title: 'Goals',
            getGoalDetails: getGoalDetails,
            toggleWidgetOpen: toggleWidgetOpen
        };

        return vm;

        function addEntity (type, goal, startDate, assignedToId) {
            var thisPatientId = selectedPatient().id();

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

        function editGoal(goal, msg) {
            var modalEntity = ko.observable(new ModalEntity(goal));
            var saveOverride = function () {
                datacontext.saveGoal(modalEntity().goal);
            };
            var cancelOverride = function () {
                var goalCancel = modalEntity().goal;
                goalCancel.entityAspect.rejectChanges();
                getGoalDetails(goalCancel, true);
            };
            msg = msg ? msg : 'Edit Goal';
            var modalSettings = {
                title: msg,
                showSelectedPatientInTitle: true,
                entity: modalEntity,
                templatePath: 'viewmodels/templates/goal.edit',
                showing: goalModalShowing,
                saveOverride: saveOverride,
                cancelOverride: cancelOverride,
                deleteOverride: null,
                classOverride: null
            };
            var modal = new modelConfig.modal(modalSettings);
            goalModalShowing(true);
            shell.currentModal(modal);
        }

        function toggleWidgetOpen(sender) {
            var openwidgets = ko.utils.arrayFilter(sender.column.widgets(), function (wid) {
                return wid.isOpen();
            });
            if (openwidgets.length === 1 && openwidgets[0] === sender) {
            } else {
                sender.isOpen(!sender.isOpen());
            }
        }

        function setOpenColumn(sender) {
            openColumn(sender);
        }

        function minimizeThisColumn(sender) {
            sender.column.isOpen(false);
        }

        function maximizeThisColumn(sender) {
            sender.column.isOpen(true);
        }

        function toggleFullScreen(sender) {
            sender.isFullScreen(!sender.isFullScreen());
        }


        function addGoal() {
            datacontext.initializeEntity(newGoal, 'Goal', selectedPatient().id()).then(goalReturned);
            function goalReturned(data) {
                var thisGoal = data.results[0];
                thisGoal.statusId(1);
                thisGoal.startDate(new Date(moment().format('MM/DD/YYYY')));
                thisGoal.isNew(true);
                thisGoal.patientId(selectedPatient().id());
                editGoal(thisGoal, 'Add Goal');
            }
        };

        function ModalEntity(goal) {
            var self = this;
            self.goal = goal;
            self.activationData = { goal: self.goal };
            self.canSave = ko.computed(function () {
                return (self.goal.name() && self.goal.sourceId());
            });
        }

        function cancelChanges() {
            var thisGoal = activeGoal();
            thisGoal.entityAspect.rejectChanges();
            ko.utils.arrayForEach(thisGoal.tasks(), function (task) {
                task.entityAspect.rejectChanges();
            });
            ko.utils.arrayForEach(thisGoal.barriers(), function (barrier) {
                barrier.entityAspect.rejectChanges();
            });
            ko.utils.arrayForEach(thisGoal.interventions(), function (intervention) {
                intervention.entityAspect.rejectChanges();
            });
        }

        function activate() {
            isComposed(false);
            var spToken = selectedPatient.subscribe(function (newValue) {
                activeGoal(null);
            });
            subscriptionTokens.push(spToken);
            openColumn(goalColumn());
        }

        function attached() {
            if (!initialized) {
                initialized = true;
            }
            isComposed(true);
        }

        function deactivate() {
            isComposed(false);
        }

        function detached() {

        }

        function getGoalsForSelectedPatient() {
            datacontext.getEntityList(null, goalsEndPoint().EntityType, goalsEndPoint().ResourcePath, null, null, false);
        }

        function getGoalDetails (goal, forceReload) {
            var goalId = goal.id();
            var patientId = goal.patientId();
            var alreadyLoaded = forceReload ? true : !goal.isLoaded;
            datacontext.getEntityById(null, goalId, goalEndPoint().EntityType, goalEndPoint().ResourcePath + patientId + '/Goal/', alreadyLoaded).then(goalHasLoaded);

            function goalHasLoaded (result) {
                goal.isLoaded = true;
            }
        };

    });