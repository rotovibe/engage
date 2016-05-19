/**
*	manages the care team patient section on the left bar.
*	@module care.team
*/
define(['models/base', 'services/datacontext', 'services/session', 'viewmodels/shell/shell'],
    function (modelConfig, datacontext, session, shell) {

        var alphabeticalSort = function (l, r) { 			
			return (l.contact().lastName() == r.contact().lastName()) ? (l.contact().lastName() > r.contact().lastName() ? 1 : -1) : (l.contact().lastName() > r.contact().lastName() ? 1 : -1) 
		};

        var ctor = function () {
			var self = this;			
        };
        
        ctor.prototype.activate = function (settings) {
            var self = this;
            // Get the selected patient that was passed in
            self.selectedPatient = settings.selectedPatient;
            // Get a list of all of the care team
            self.careMembers = ko.computed( function(){
				var team = self.selectedPatient.careTeam();
				var allMembers = team ? team.members() : [];
				var members = ko.utils.arrayFilter( allMembers, function(member){
					return !member.isNew();
				});
				return members;
			}).extend({ throttle: 50 });
			
            // The view state of the section (open or not)
            self.isOpen = ko.observable(true);			
            // Create a list of primary care team members to display in the widget
            self.primaryCareTeam = ko.computed(function () {
				var careMembers = self.careMembers();	//listen to changes in assigned care members (assignedToMe returns result)
                // Create an empty array to fill with problems				
                var thisCareTeam = [];
                // Sort the team
                var searchCareTeam = careMembers.sort(alphabeticalSort);
                // Create a filtered list of care teams,
                ko.utils.arrayForEach(searchCareTeam, function (careMember) {
                    // If they are a member of the primary care team,
                    if (careMember.core() && !careMember.isNew() && careMember.statusId() == 1) {
                        // Add them to the team
                        thisCareTeam.push(careMember);
                    }
                });
                // Return the team
                return thisCareTeam;
            }).extend({ throttle: 50 });
			
			self.isSaving = ko.observable(false);
			self.canAssignToMe = ko.computed( function(){
				var zerolength = self.primaryCareTeam().length === 0;
				var isPatientLoaded = self.selectedPatient.isLoaded();
				var isSaving = self.isSaving();
				return zerolength && isPatientLoaded && !isSaving;
			}).extend({ throttle: 50 });
			
			self.canReassignToMe = ko.computed( function () {
				var primaryCareTeam = self.primaryCareTeam();
				var isPatientLoaded = self.selectedPatient.isLoaded();
				var isSaving = self.isSaving();
				if ( primaryCareTeam.length > 0 && isPatientLoaded ) {
					var thisMatchedCareManager = ko.utils.arrayFirst( primaryCareTeam, function (caremanager) {
					    return caremanager.contact().userId() === session.currentUser().userId();
					});
					return ( thisMatchedCareManager && !isSaving );
				}
				return false;
			}).extend({ throttle: 50 });
		
		//TODO: probably not needed:
            // Create a list of secondary care team members
            self.secondaryCareTeam = ko.computed(function () {
                // Create an empty array to fill with problems
                var thisCareTeam = [];
                // Sort them
                // var searchCareTeam = self.careMembers().sort(alphabeticalSort);
                // // Create a filtered list of care teams,
                // ko.utils.arrayForEach(searchCareTeam, function (careMember) {
                    // // If they are not part of the primary care team,
                    // if (!careMember.primary() && !careMember.isNew()) {
                        // // Make them part of the secondary team
                        // thisCareTeam.push(careMember);
                    // }
                // });
                return thisCareTeam;
            }).extend({ throttle: 50 });
            self.saveType = ko.observable();
            self.saveOverride = function () { 
                // Get the current primary care manager
                var careMemberType = ko.utils.arrayFirst(datacontext.enums.careMemberTypes(), function (cmType) {
                    return cmType.name() === 'Care Manager';
                });
                if (careMemberType) {
                    var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
                        return caremanager.id() === session.currentUser().userId();
                    });
                    // Grab the first primary listed care member
					var members = self.selectedPatient.careTeam() ? self.selectedPatient.careTeam().members() : [];
                    var thisCareMember = ko.utils.arrayFirst(members, function (ctMember) {
                        return ctMember.primary();
                    });
                    datacontext.saveCareMemberOld(thisCareMember, self.saveType());
                }
            };
            self.cancelOverride = function () {
                datacontext.cancelEntityChanges(self.selectedPatient);
				var members = self.selectedPatient.careTeam() ? self.selectedPatient.careTeam().members() : [];
                ko.utils.arrayForEach(members, function (cm) {
                    datacontext.cancelEntityChanges(cm);
                });
            };
            self.editModalShowing = ko.observable(false);
            self.modalEntity = ko.observable(new ModalEntity(self.selectedPatient, self.saveType));
			var modalSettings = {
				title: 'Edit Care Team',
				showSelectedPatientInTitle: true,
				entity: self.modalEntity, 
				templatePath: 'viewmodels/templates/care.team.edit', 
				showing: self.editModalShowing, 
				saveOverride: self.saveOverride, 
				cancelOverride: self.cancelOverride, 
				deleteOverride: null, 
				classOverride: null
			}
            self.modal = new modelConfig.modal(modalSettings);
        };

        function ModalEntity(patient, savetype) {
            var self = this;
            self.selectedPatient = ko.observable(patient);
            self.saveType = savetype;
            // Create a computed property to subscribe to all of
            // the patients' observations and make sure they are
            // valid
            self.canSaveObservable = ko.observable(false);
            self.canSave = ko.computed({
                read: function () {
                    return self.canSaveObservable();
                },
                write: function (newValue) {
                    self.canSaveObservable(newValue);
                }
            });
            // Object containing parameters to pass to the modal
            self.activationData = { selectedPatient: self.selectedPatient, canSave: self.canSave, saveType: self.saveType };
        }

		ctor.prototype.assignToMe = function () {
			var self = this;
            // Get the care manager type
            var careMemberType = ko.utils.arrayFirst(datacontext.enums.careMemberTypes(), function (cmType) {
                return cmType.name() === 'Care Manager';
            });			
            if (!self.isSaving() && careMemberType) {				
				self.isSaving(true);
                var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
                    return caremanager.id() === session.currentUser().userId();
                });											
                var thisCareMember = datacontext.createEntity('CareMember', { id: -1, patientId: self.selectedPatient.id(), preferredName: thisMatchedCareManager.preferredName(), typeId: careMemberType.id(), gender: 'n', primary: true, contactId: session.currentUser().userId() });
				function saveCareManagerCompleted() {
				}
                return datacontext.saveCareMemberOld(thisCareMember, 'Insert').then( saveCareManagerCompleted );
            } else{
				console.log('assignToMe blocked since it is currently saving');
			}
        };
        
        //TODO: change to new care member / care team/ member endpoints and contact types:
        ctor.prototype.reassignToMe = function () {
            var self = this;
            // Get the care manager type
            var careMemberType = ko.utils.arrayFirst(datacontext.enums.careMemberTypes(), function (cmType) {
                return cmType.name() === 'Care Manager';
            });
            if (!self.isSaving() && careMemberType) {
				self.isSaving(true);
                var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
                    return caremanager.id() === session.currentUser().userId();
                });
                // Grab the first primary listed care member
				var members = self.selectedPatient.careTeam() ? self.selectedPatient.careTeam().members() : [];
                var thisCareMember = ko.utils.arrayFirst(members, function (ctMember) {
                    return ctMember.primary();
                });
                thisCareMember.preferredName(thisMatchedCareManager.preferredName());
                thisCareMember.gender('n');
                thisCareMember.contactId(thisMatchedCareManager.id());
				function saveCareManagerCompleted() {					
				}
                datacontext.saveCareMemberOld(thisCareMember, 'Update').then( saveCareManagerCompleted );
            }
        };

        //TODO: change to new care member / care team/ member endpoints and contact types:
        ctor.prototype.saveCareTeam = function (caremanagerid) {
            var careManagerId = caremanagerid ? caremanagerid : session.currentUser().userId();
            // Get the care manager type
            var careMemberType = ko.utils.arrayFirst(datacontext.enums.careMemberTypes(), function (cmType) {
                return cmType.name() === 'Care Manager';
            });
            if (careMemberType) {
                var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
                    return caremanager.id() === careManagerId;
                });
                var thisCareMember = datacontext.createEntity('CareMember', { id: -1, patientId: self.selectedPatient.id(), preferredName: thisMatchedCareManager.preferredName(), typeId: careMemberType.id(), gender: 'n', primary: true, contactId: careManagerId });
                datacontext.saveCareMemberOld(thisCareMember, 'Insert');
            }
        }

        ctor.prototype.editCareTeam = function () {
			//TODO: rewrite with new care team
            // var self = this;
            // self.editModalShowing(true);
            // shell.currentModal(self.modal);
        }

        ctor.prototype.attached = function () {
        };

		ctor.prototype.detached = function() {
			var self = this;
            //dispose computeds:
			self.primaryCareTeam.dispose();
			self.canAssignToMe.dispose();
			self.canReassignToMe.dispose();
			self.secondaryCareTeam.dispose();									
			
			//dispose subscriptions:
            // ko.utils.arrayForEach(subscriptionTokens, function (token) {
                // token.dispose();
            // });
        }
        return ctor;
    });