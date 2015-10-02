define(['models/base', 'config.services', 'services/datacontext', 'services/session', 'viewmodels/patients/goals/index', 'viewmodels/shell/shell'],
    function (modelConfig, servicesConfig, datacontext, session, goalsIndex, shell) {

        var modalShowing = ko.observable(true);

        var ctor = function () {
            var self = this;
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };
            self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
            self.settings = settings;
            self.goal = self.settings.goal;
            self.selectedPatient = goalsIndex.selectedPatient;
            self.computedGoal = ko.computed(function () { return self.goal; }).extend({ throttle: 60 });
            self.edit = function () {
                // Edit this goal
                goalsIndex.editGoal(self.goal, 'Edit Goal');
            }
            self.delete = function () {
                // Prompt the user to confirm deletion
                var result = confirm('You are about to delete a goal.  Press OK to continue, or cancel to return without deleting.');
                // If they press OK,
                if (result === true) {
                    // self.activeGoal(null);
                    // self.isEditing(false);
                    datacontext.deleteGoal(self.goal);
                    // Proceed to navigate away
                }
                else {                    
                    return false;
                }
            };
            self.addBarrier = function (goal) {
                self.addEntity('Barrier', goal).then(doSomething);

                function doSomething(barriers) {
                    // Show the modal
                    self.editBarrier(barriers, 'Add Barrier');
                }
            };
            self.editBarrier = function (barrier, msg) {
                var thisGoal = barrier.goal();
                // Edit this barrier
                var modalEntity = ko.observable(new ModalEntity(barrier, 'name'));
                var saveOverride = function () {
                    saveBarrier(barrier)
                };
                var cancelOverride = function () {
                    cancel(barrier);
                    getGoalDetails(thisGoal);
                };
                msg = msg ? msg : 'Edit Barrier';
                editEntity(msg, modalEntity, 'viewmodels/templates/barrier.edit', saveOverride, cancelOverride);
            };
        };

        ctor.prototype.addEntity = function (type, goal, startDate, assignedToId) {
            var self = this;
            var thisPatientId = self.selectedPatient().id();
            //var thisPatientId = self.activeGoal.patientId();
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

        function save (goal) {
            // TODO : Call the save goal method
            datacontext.saveGoal(goal);
        }

        function saveBarrier (barrier) {
            datacontext.saveBarrier(barrier);
        }

        function cancel(item) {
            item.entityAspect.rejectChanges();
        }

        function getGoalDetails (goal) {
            goalsIndex.getGoalDetails(goal, true);
        };

        function editEntity (msg, entity, path, saveoverride, canceloverride) {
			var modalSettings = {
				title: msg,
				entity: entity, 
				templatePath: path, 
				showing: modalShowing, 
				saveOverride: saveoverride, 
				cancelOverride: canceloverride
			}
            var modal = new modelConfig.modal(modalSettings);
            modalShowing(true);
            shell.currentModal(modal);
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

        return ctor;
    });