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
            self.task = self.settings.task;
            self.isExpanded = self.task.goal().isExpanded;
            self.editTask = function (task) {
                // Make sure we have the most current details
                getGoalDetails(task.goal());
                // Edit this task
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
                // Prompt the user to confirm deletion
                var result = confirm('You are about to delete a task.  Press OK to continue, or cancel to return without deleting.');
                // If they press OK,
                if (result === true) {
                    var thisGoal = task.goal();
                    // Trigger the goal to refresh
                    task.entityAspect.rejectChanges();
                    task.deleteFlag(true);
                    datacontext.saveTask(task).then(saveCompleted);

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

        function saveTask (task) {
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

    });