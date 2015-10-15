/**
*	Individual Status section in individual details 
*	@module status
*/
define(['models/base', 'services/datacontext', 'viewmodels/shell/shell'],
    function (modelConfig, datacontext, shell) {
		var subscriptionTokens= [];
        var ctor = function () {

        };
		
        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.selectedPatient = self.settings.selectedPatient;            												
            self.statusModalShowing = ko.observable(false);
            self.saveStatus = function () {
                datacontext.saveIndividual(self.selectedPatient);
            }
            self.cancelStatus = function () {
                self.selectedPatient.entityAspect.rejectChanges();
            }
			var modalSettings = {
				title: 'Edit Status',
				showSelectedPatientInTitle: true,
				entity: self.selectedPatient, 
				templatePath: 'templates/patient.status.html', 
				showing: self.statusModalShowing, 
				saveOverride: self.saveStatus, 
				cancelOverride: self.cancelStatus, 
				deleteOverride: null, 
				classOverride: null
			}
            self.modal = new modelConfig.modal(modalSettings);
            self.isOpen = ko.observable(false);
            self.isEditing = ko.observable(false);
            self.isExpanded = ko.observable(false);
            self.toggleEditing = function () {
                if (self.isEditing()) {
                    self.statusModalShowing(false);
                }
                else {
                    shell.currentModal(self.modal);
                    self.statusModalShowing(true);
                    var editingToken = self.statusModalShowing.subscribe(function () {
                        self.isEditing(false);
                        editingToken.dispose();
                    });
                }
                self.isEditing(!self.isEditing());
                self.isOpen(true);
            };
			/**
			*	computed. when the status is active set the reason back to null.
			*	when the status is set to anything other than active and the reason is null - the reason updates to "Unknown" (default).
			*	@method updateStatusReason
			*/
			self.updateStatusReason = ko.computed( function(){				
				var statusId = self.selectedPatient.statusId();
				if( statusId && statusId === '1' ){
					self.selectedPatient.reasonId(null);
					return true;
				}
				else{
					if( self.selectedPatient.reasonId() === null ){
						self.selectedPatient.setDefaultStatusReason();
					}
				}
				return false;
			});
        };

        ctor.prototype.attached = function () {
        };
		ctor.prototype.detached = function() { 
			var self = this;
			ko.utils.arrayForEach(subscriptionTokens, function (token) {
				token.dispose();
			});
			//computed cleanup:
			self.updateStatusReason.dispose();				
		}
        return ctor;
    });