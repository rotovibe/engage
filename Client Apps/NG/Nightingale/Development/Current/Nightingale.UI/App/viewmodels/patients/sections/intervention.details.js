﻿define(['models/base', 'config.services', 'services/datacontext', 'services/session', 'viewmodels/patients/goals/index', 'viewmodels/shell/shell'],
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
			self.isDetailsExpanded = ko.observable(false);
			self.hasDetails = ko.computed( function(){
				var details = self.intervention.details();
				return (details != null && details.length > 0);
			});
			self.toggleDetailsExpanded = function(){
				var isOpen = self.isDetailsExpanded();
				var details = self.intervention.details();
				if( !details && !isOpen ){
					return;
				}	
				self.isDetailsExpanded( !self.isDetailsExpanded() );
			}
            self.editIntervention = function (intervention) {
                // Make sure we have the most up to date goal info
                getGoalDetails(intervention.goal());
                // Edit this intervention
                var modalEntity = ko.observable(new ModalEntity(intervention, 'description'));
                var saveOverride = function () {					
                    saveIntervention(intervention);
                };
                var cancelOverride = function () {
					intervention.clearDirty();
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

        function saveIntervention(intervention) {			
			function saved(){
				intervention.clearDirty();
			}
            // Call the save intervention method			
			intervention.checkAppend();			
            datacontext.saveIntervention(intervention).then(saved);
        }

        function cancel (item) {
            item.entityAspect.rejectChanges();
        }

        return ctor;

        function getGoalDetails(goal) {
            goalsIndex.getGoalDetails(goal, true);
        }
        
        function ModalEntity(entity) {
            var self = this;
            self.entity = entity;
            // Object containing parameters to pass to the modal
            self.activationData = { entity: self.entity };
            
            self.canSave = ko.computed(function () {
                var result = self.entity.isValid(); //subscribe to intervention.isValid                
                return result;
            });
        }

    });