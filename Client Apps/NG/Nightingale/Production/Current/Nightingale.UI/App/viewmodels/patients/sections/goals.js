define(['models/base', 'config.services', 'services/datacontext', 'services/session', 'viewmodels/patients/goals/index', 'viewmodels/shell/shell', 'durandal/activator'],
    function (modelConfig, servicesConfig, datacontext, session, goalsIndex, shell, activator) {

        var modalShowing = ko.observable(true);

        var ctor = function () {
            var self = this;
            self.token = {};
            self.selectedPatientChanged = false;
        };

        ctor.prototype.deactivate = function () {
            var self = this;
            self.token.dispose();
            self.computedGoals.dispose();
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.activeGoal = settings.activeGoal;
            self.activeTask = settings.activeTask;
            self.activeIntervention = settings.activeIntervention;
            self.goalsSortTwo = function (l, r) {
                var p1 = l.status().order();
                var p2 = r.status().order();

                var o1 = l.name().toLowerCase();
                var o2 = r.name().toLowerCase();

                if (p1 != p2) {
                    if (p1 < p2) return -1;
                    if (p1 > p2) return 1;
                    return 0;
                }
                if (o1 < o2) return -1;
                if (o1 > o2) return 1;
                return 0;
            };
            self.settings = settings;
            self.selectedPatient = goalsIndex.selectedPatient;
            self.token = self.selectedPatient.subscribe(function () {
                self.selectedPatientChanged = true;
                self.deactivate();
            });
            self.statusIds = self.settings.statusIds;
            self.emptyMessage = self.settings.emptyMessage ? self.settings.emptyMessage : 'No goals defined for the individual.';
            self.sortOverride = self.settings.sortOverride ? self.settings.sortOverride : self.goalsSortTwo;
            self.computedGoals = ko.computed({
                read: function () {
                    var theseGoals = ko.utils.arrayFilter(self.selectedPatient().goals(), function (thisGoal) {
                        return !thisGoal.isNew() && self.statusIds.indexOf(thisGoal.statusId()) !== -1;
                    });
                    ko.utils.arrayForEach(theseGoals, function (goal) {
                        if (!goal.isExpanded) {
                            goal.isExpanded = ko.observable(false);
                            goal.isOpen = ko.observable(false);
                        }
                        if (!goal.isNew()) {
                            getGoalDetails(goal);
                        }
                    });
                    if (theseGoals.length > 0) {
                        theseGoals = theseGoals.sort(self.sortOverride);
                    }
                    return theseGoals;
                },
                disposeWhen: function () {
                    return self.selectedPatientChanged;
                }
            }).extend({ throttle: 50 });
            self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
            self.alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };
            self.addTask = function (goal) {
                var startDate = new Date(moment().format('MM/DD/YYYY'));
                self.addEntity('Task', goal, startDate).then(doSomething);

                function doSomething(task) {
                    self.editTask(task, 'Add Task');
                }
            };
            self.addIntervention = function (goal) {
                var startDate = new Date(moment().format('MM/DD/YYYY'));
                self.addEntity('Intervention', goal, startDate, session.currentUser().userId()).then(doSomething);

                function doSomething(intervention) {
                    intervention.isNew(true);
                    intervention.watchDirty();
                    self.editIntervention(intervention, 'Add Intervention');
                }
            };
            self.editIntervention = function (intervention, msg) {
                var thisGoal = intervention.goal();
                var modalEntity = ko.observable(new ModalEntity(intervention, 'description'));
                var saveOverride = function () {
                    saveIntervention(intervention);
                    thisGoal.isOpen(false);
                    thisGoal.isOpen(true);
                };
                var cancelOverride = function () {
                    datacontext.cancelEntityChanges(intervention);
                    getGoalDetails(thisGoal);
                };
                msg = msg ? msg : 'Edit Intervention';
                editEntity(msg, modalEntity, 'viewmodels/templates/intervention.edit', saveOverride, cancelOverride);
            };
            self.editTask = function (task, msg) {
                var thisGoal = task.goal();
                var modalEntity = ko.observable(new ModalEntity(task, 'description'));
                var saveOverride = function () {
                    saveTask(task);
                    thisGoal.isOpen(false);
                    thisGoal.isOpen(true);
                };
                var cancelOverride = function () {
                    datacontext.cancelEntityChanges(task);
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

        ctor.prototype.setActiveGoal = function (goal) {
            this.activeGoal(goal);
            this.activeTask(null);
            this.activeIntervention(null);
        };

        ctor.prototype.setActiveTask = function (task) {
            this.activeTask(task);
            this.activeGoal(null);
            this.activeIntervention(null);
        };

        ctor.prototype.setActiveIntervention = function (intervention) {
            this.activeIntervention(intervention);
            this.activeGoal(null);
            this.activeTask(null);
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
                    thisTask.startDate(new Date(moment().format('MM/DD/YYYY')));
                    thisTask.statusId(1);
                    thisTask.patientGoalId(thisGoalId);
                    return thisTask;
                }
            }
        };

        return ctor;

        function editEntity (msg, entity, path, saveoverride, canceloverride) {
            var modalSettings = {
              title: msg,
              showSelectedPatientInTitle: true,
              entity: entity,
              templatePath: path,
              showing: modalShowing,
              saveOverride: saveoverride,
              cancelOverride: canceloverride,
              deleteOverride: null,
              classOverride: null
            };
            var modal = new modelConfig.modal(modalSettings);
            modalShowing(true);
            shell.currentModal(modal);
        }

        function save(goal) {
            datacontext.saveGoal(goal);
        }

        function saveIntervention (entity) {
            entity.isNew(false);
            datacontext.saveIntervention(entity);
        }

        function saveTask (entity) {
            datacontext.saveTask(entity);
        }

        function saveGoal (entity) {
            datacontext.saveGoal(entity);
        }

        function cancel(item) {
            item.entityAspect.rejectChanges();
        }

        function getGoalDetails (goal) {
            goalsIndex.getGoalDetails(goal);
        }

        function ModalEntity(entity, reqpropname) {
            var self = this;
            self.entity = entity;
            self.activationData = { entity: self.entity };
            self.canSave = ko.computed(function () {
                var result = self.entity.isValid();
                return result;
            });
        }

    });