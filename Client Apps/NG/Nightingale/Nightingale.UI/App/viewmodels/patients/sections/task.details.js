define(['models/base', 'config.services', 'services/datacontext', 'services/session', 'viewmodels/patients/goals/index', 'viewmodels/shell/shell'],
    function (modelConfig, servicesConfig, datacontext, session, goalsIndex, shell) {

        var ctor = function () {
            var self = this;
        };

        var modalShowing = ko.observable(true);

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };
            self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
            self.settings = settings;
            self.task = self.settings.activeTask;
            self.isFullScreen = ko.observable(false);
            self.isDetailsExpanded = ko.observable(false);
            self.hasDetails = ko.computed( function(){
                var details = self.task() ? self.task().details() : [];
                return (details != null && details.length > 0);
            });
            self.toggleFullScreen = function () {
                self.isFullScreen(!self.isFullScreen());
            };
            self.toggleDetailsExpanded = function(){
                var isOpen = self.isDetailsExpanded();
                var details = self.task().details();
                if( !details && !isOpen ){
                    return;
                }
                self.isDetailsExpanded( !self.isDetailsExpanded() );
            };

            self.addBarrier = function (task) {
                goalsIndex.addEntity('Barrier', task.goal()).then(doSomething);

                function doSomething(barrier) {
                    var newBarrierId = datacontext.createComplexType('Identifier', { id: barrier.id() });
                    self.task().barrierIds().push(newBarrierId);
                    self.editBarrier(barrier, 'Add Barrier');
                }
            };

            self.editBarrier = function (barrier, msg) {
                var thisGoal = barrier.goal();
                var modalEntity = ko.observable(new ModalEntity(barrier, 'name'));
                var saveOverride = function () {
                    saveBarrier(barrier);
                    saveTask(self.task());
                };
                var cancelOverride = function () {
                    cancel(barrier);
                    cancel(self.task());
                    getGoalDetails(thisGoal);
                };
                msg = msg ? msg : 'Edit Barrier';
                editEntity(msg, modalEntity, 'viewmodels/templates/barrier.edit', saveOverride, cancelOverride);
            };

            self.editTask = function (task) {
                getGoalDetails(task.goal());
                var modalEntity = ko.observable(new ModalEntity(task, 'description'));
                var saveOverride = function () {
                    saveTask(task)
                };
                var cancelOverride = function () {
                    cancel(task);
                    getGoalDetails(task.goal());
                };
                editEntity('Edit Task', modalEntity, 'viewmodels/templates/task.edit', saveOverride, cancelOverride);
            };
            self.deleteTask = function (task) {
                var result = confirm('You are about to delete a task.  Press OK to continue, or cancel to return without deleting.');
                if (result === true) {
                    var thisGoal = task.goal();
                    task.entityAspect.rejectChanges();
                    task.deleteFlag(true);
                    datacontext.saveTask(task).then(saveCompleted);
                    self.settings.activeTask(null);

                    function saveCompleted() {
                        if (task && task.goal()) {
                            task.goal().tasks.remove(task);
                        }
                        if (task && task.patientGoalId) {
                            task.patientGoalId(null);
                        }
                    }
                }
                else {
                    return false;
                }
            };
        };

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
            }
            var modal = new modelConfig.modal(modalSettings);
            modalShowing(true);
            shell.currentModal(modal);
        }

        function saveBarrier (barrier) {
            barrier.checkAppend();
            datacontext.saveBarrier(barrier);
        }

        function saveTask (task) {
            task.checkAppend();
            datacontext.saveTask(task);
        }

        function cancel (item) {
            item.entityAspect.rejectChanges();
        }

        return ctor;

        function getGoalDetails(goal) {
            goalsIndex.getGoalDetails(goal, true);
        }

        function ModalEntity(entity, reqpropname) {
            var self = this;
            self.entity = entity;
            self.activationData = { entity: self.entity };
            self.canSave = ko.computed(function () {
                var result = self.entity[reqpropname]();
                return result;
            });
        }

    });