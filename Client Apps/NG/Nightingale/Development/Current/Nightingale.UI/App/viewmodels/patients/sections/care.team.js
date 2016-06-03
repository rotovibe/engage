/**
*	manages the care team patient section on the left bar.
*	@module care.team
*/
define(['models/base', 'services/datacontext', 'services/session', 'viewmodels/shell/shell', 'viewmodels/patients/team/index'],
    function (modelConfig, datacontext, session, shell, teamIndex) {		

        // var alphabeticalSort = function (l, r) {
			// if( l.contact() && !r.contact() ){
				// return 1;
			// }
			// if( !l.contact() && r.contact() ) return -1;
			// if( !l.contact() && !r.contact() ) return 0;
			// return (l.contact().lastName() == r.contact().lastName()) ? (l.contact().lastName() > r.contact().lastName() ? 1 : -1) : (l.contact().lastName() > r.contact().lastName() ? 1 : -1) 
		// };

		var activeMembersSort = function (l, r) { 			
			if( l.core() && ! r.core() ) return -1;
			if( !l.core() && r.core() )	return 1;
			if( l.core() == r.core() ){
				var leftRole = l.computedRoleName() ? l.computedRoleName().toLowerCase() : '';
				var rightRole = r.computedRoleName() ? r.computedRoleName().toLowerCase() : '';
				return leftRole == rightRole ? 0 : (leftRole > rightRole ? 1 : -1);
			}
			return 0;
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
				var members = team ? team.members() : [];
				// var members = ko.utils.arrayFilter( allMembers, function(member){
					// return !member.isNew() && member.contactId() && member.contact() && member.core() && member.statusId() == 1;
				// });
				return members;
			}).extend({ throttle: 50 });
			
            // The view state of the section (open or not)
            self.isOpen = ko.observable(true);
			
			self.primaryCareManager = ko.computed( function(){
				var team = self.selectedPatient.careTeam();
				if( team ){
					return team.primaryCareManagers().length > 0 ? team.primaryCareManagers()[0] : null;
				}
				return null;
			});
			
			self.primaryCarePhysician = ko.computed( function(){
				var team = self.selectedPatient.careTeam();
				if( team ){
					return team.primaryCarePhysicians().length > 0 ? team.primaryCarePhysicians()[0] : null;
				}
				return null;
			});
			
            // Create a list of primary care team members to display in the widget
            self.primaryCareTeam = ko.computed(function () {
				var careMembers = self.careMembers();	//listen to changes in assigned care members (assignedToMe returns result)                
                var thisCareTeam = [];
                var pcp = self.primaryCarePhysician();
				var pcm = self.primaryCareManager();
				
                // Create a filtered list of care teams,
                ko.utils.arrayForEach(careMembers, function (careMember) {
                    // If they are a member of the primary care team,
                    if (careMember.core() && !careMember.isNew() && careMember.statusId() == 1) {
						//exclude pcp and pcm:
						if( !( pcp && careMember.id() == pcp.id() || ( pcm && careMember.id() == pcm.id() ) ) ){
							thisCareTeam.push(careMember);
						}
                    }
                });
				thisCareTeam = thisCareTeam.sort(activeMembersSort);
                // Return the team
                return thisCareTeam;
            }).extend({ throttle: 50 });
			
			self.isSaving = ko.observable(false);
			self.canAssignToMe = ko.computed( function(){
				var isPatientLoaded = self.selectedPatient.isLoaded();
				var isSaving = self.isSaving();
				var careMembers = self.careMembers();
				var pcm = self.primaryCareManager();
				if( !pcm ){
					var thisUserCareMember = ko.utils.arrayFirst( careMembers, function (member) {
						//find if the current user is in the team with pcm role:
					    return member.contactId() === session.currentUser().userId()
								&& member.roleId() == teamIndex.pcmContactSubType().id();
					});
				}
				return isPatientLoaded && !isSaving && !thisUserCareMember;
			}).extend({ throttle: 50 });
			
			self.canReassignToMe = ko.computed( function () {
				var careMembers = self.careMembers();
				var pcm = self.primaryCareManager();
				var isPatientLoaded = self.selectedPatient.isLoaded();
				var isSaving = self.isSaving();
				if ( careMembers.length > 0 && isPatientLoaded && pcm ) {
					//the PCM is assigned
					var PCMCareManager = ko.utils.arrayFirst( careMembers, function (member) {
						//find if the the current user is a member in this team, and its not the assigned(active core) pcm, and its role is PCM:
					    return member.contactId() == session.currentUser().userId() && member.id() != pcm.id() 
							&& member.contactId() != pcm.contactId()
							&& member.roleId() == pcm.roleId();
					});
					return ( PCMCareManager && !isSaving );
				}
				return false;
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

		ctor.prototype.addCareMember = function(){
			teamIndex.addCareMember();
		};
		
		ctor.prototype.assignToMe = function () {
			//assign the current user as a PCM role:
			var self = this;
			var pcmRoleId = teamIndex.pcmContactSubType().id();
			var selectedPatient = self.selectedPatient;
			var teamId = selectedPatient.careTeamId();
            var userCareManager = datacontext.getUserCareManager();
			var userContactId = userCareManager.id();	//the caremanager id is actually a contact id (get care managers call returns them as contacts)
			var careTeam;
            if (!self.isSaving() && pcmRoleId) {
				var pcm = self.primaryCareManager();
				var PCMCareManager = ko.utils.arrayFirst( self.careMembers(), function (member) {
					//find if the the current user is a member in this team, and its not the assigned(active core) pcm, and its role is PCM:
					return member.contactId() == session.currentUser().userId() && member.id() != pcm.id() 
						&& member.contactId() != pcm.contactId()
						&& member.roleId() == pcm.roleId();
				});
				if( PCMCareManager ){
					//reassign the user:
					return self.reassignToMe();
				}	
				self.isSaving(true);
				var newCareMember = datacontext.createEntity('CareMember', 
						{ 	id: -1, 
							contactId: userContactId,
							roleId: pcmRoleId,
							careTeamId: teamId,
							distanceUnit: 'mi',
							statusId: 1,		//active 
							core: true,
							dataSource: 'Engage',
							createdById: session.currentUser().userId()							
						});
				newCareMember.isNew(true);
                
				function saveTeamCompleted( team ){
					if( team ){					
						self.selectedPatient.careTeam(team);
						self.isSaving(false);
					}
				};
				
				if( !teamId ){
					//team has not yet been created:
					careTeam = datacontext.createEntity('CareTeam', 
							{ 	id: -1, 
								contactId: self.selectedPatient.contactId(),
								patientId: self.selectedPatient.id(),
								createdById: session.currentUser().userId()
							});
					careTeam.members = ko.observableArray();
					careTeam.members.push( newCareMember );					
				}
				else{
					//add/save one member to an existing team
					//	note: the new member should already be here inside members:
					careTeam = self.selectedPatient.careTeam();										
				}
				return datacontext.saveCareTeam( careTeam ).then( saveTeamCompleted );				
            } else{
				console.log('assignToMe blocked since it is currently saving');
			}
        };
        
        //TODO: remove / change to new care member / care team/ member endpoints and contact types:
        ctor.prototype.reassignToMe = function () {
            var self = this;
            
			function saveTeamCompleted( team ){
				if( team ){					
					self.selectedPatient.careTeam(team);
					self.isSaving(false);
				}
			};
			
            if (!self.isSaving()) {
				self.isSaving(true);
				
				var pcm = self.primaryCareManager();
				var isPatientLoaded = self.selectedPatient.isLoaded();
				
				if ( self.careMembers().length > 0 && isPatientLoaded && pcm ) {					
					var userCareMember = ko.utils.arrayFirst( self.careMembers(), function (member) {
						//find if the the current user is a member in this team, and its not the assigned(active core) pcm, and its role is PCM:
					    return member.contactId() == session.currentUser().userId() && member.id() != pcm.id() 
							&& member.contactId() != pcm.contactId()
							&& member.roleId() == pcm.roleId();
					});
					pcm.statusId( 2 ); //TBD how to retire the current pcm ??
					userCareMember.core( true );
					userCareMember.statusId( 1 );
					datacontext.saveCareTeam( self.selectedPatient.careTeam() ).then( saveTeamCompleted );
				}
            }
        };

        //TODO: remove/ change to new care member / care team/ member endpoints and contact types:
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

        //TODO: remove this functionality
        // ctor.prototype.editCareTeam = function () {			
            // // var self = this;
            // // self.editModalShowing(true);
            // // shell.currentModal(self.modal);
        // }

        ctor.prototype.attached = function () {
        };

		ctor.prototype.detached = function() {
			var self = this;
            //dispose computeds:
			self.primaryCareTeam.dispose();
			self.canAssignToMe.dispose();
			self.canReassignToMe.dispose();
			self.primaryCarePhysician.dispose();
			self.primaryCareManager.dispose();
			
			//dispose subscriptions:
            // ko.utils.arrayForEach(subscriptionTokens, function (token) {
                // token.dispose();
            // });
        }
        return ctor;
    });