define(['config.models', 'config.services', 'services/datacontext', 'services/session', 'viewmodels/patients/goals/index'],
    function (models, servicesConfig, datacontext, session, goalsIndex) {

        var goalEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Goal');
        });

        var ctor = function () {
            var self = this;
        };
        
        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.selectedPatient = self.settings.selectedPatient;
            self.activeGoal = self.settings.activeGoal;
            self.setActiveGoal = function (sender) {
                if (!self.isEditing()) {
                    self.getGoalDetails(sender);
                    self.activeGoal(sender);
                }
            };
            self.isEditing = goalsIndex.isEditing;
            self.editGoal = function (sender) {
                if (!self.isEditing()) {
                    self.getGoalDetails(sender);
                    self.isEditing(true);
                    self.activeGoal(sender);
                }
            };
            self.deleteGoal = function (sender) {
                // Prompt the user to confirm deletion
                var result = confirm('You are about to delete a goal.  Press OK to continue, or cancel to return without deleting.');
                // If they press OK,
                if (result === true) {
                    self.activeGoal(null);
                    self.isEditing(false);
                    datacontext.deleteGoal(sender);
                    // Proceed to navigate away
                }
                else {                    
                    return false;
                }
            };
        };

        ctor.prototype.getGoalDetails = function (goal) {
            var goalId = goal.id();
            var patientId = goal.patientId();
            datacontext.getEntityById(null, goalId, goalEndPoint().EntityType, goalEndPoint().ResourcePath + patientId + '/Goal/', true);
        }

        ctor.prototype.attached = function () {
        };

        return ctor;
    });