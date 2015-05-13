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
            self.intervention = self.settings.intervention;
            self.isExpanded = self.intervention.goal().isExpanded;
            self.editIntervention = function (intervention) {
                // Make sure we have the most up to date goal info
                getGoalDetails(intervention.goal());
                // Edit this intervention
                var modalEntity = ko.observable(new ModalEntity(intervention, 'description'));
                var saveOverride = function () {
                    saveIntervention(intervention);
                };
                var cancelOverride = function () {
                    cancel(intervention);
                    getGoalDetails(intervention.goal());
                };
                editIntervention('Edit Intervention', modalEntity, 'viewmodels/templates/intervention.edit', saveOverride, cancelOverride);
            };
            self.deleteIntervention = function (intervention) {
                // Prompt the user to confirm deletion
                var result = confirm('You are about to delete an intervention.  Press OK to continue, or cancel to return without deleting.');
                // If they press OK,
                if (result === true) {
                    var thisGoal = intervention.goal();
                    // Trigger the goal to refresh
                    intervention.entityAspect.rejectChanges();
                    intervention.deleteFlag(true);
                    datacontext.saveIntervention(intervention).then(saveCompleted);

                    function saveCompleted () {
                        if (intervention && intervention.goal()) {
                            intervention.goal().interventions.remove(intervention);
                        }
                        if (intervention && intervention.patientGoalId) {
                            intervention.patientGoalId(null);
                        }                        
                    }
                }
                else {                    
                    return false;
                }
            };
        };

        function editIntervention (msg, entity, path, saveoverride, canceloverride) {
            var modal = new modelConfig.modal(msg, entity, path, modalShowing, saveoverride, canceloverride);
            modalShowing(true);
            shell.currentModal(modal);
        }

        function saveIntervention(intervention) {
            // Call the save intervention method
            datacontext.saveIntervention(intervention);
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