/**
*	care team management
*	@module care team index
*/

define(['services/session', 'services/datacontext', 'viewmodels/patients/index', 'models/base', 'viewmodels/shell/shell', 'viewmodels/home/contacts/index'],
    function (session, datacontext, patientsIndex, modelConfig, shell, contactsIndex) {

        var fullScreenWidget = ko.observable();
        var leftColumnOpen = ko.observable(true);
		var activeWidgetOpen = ko.observable(true);
		var inactiveWidgetOpen = ko.observable(false);
		var nextId = 0;
		
		checkDataContext();
		
        var selectedPatient = ko.computed(function () {
            return patientsIndex.selectedPatient();
        });
		
		var careMemberRoles = ko.observableArray([]);
		var careMemberRolesGroup = 2;
		careMemberRoles( datacontext.getContactTypes( careMemberRolesGroup, 'root' ) );
		
		var pcmContactSubType = ko.observable( ko.utils.arrayFirst( careMemberRoles(), function( contactType ){
			return contactType.role() == 'Primary Care Manager';
		}));
		var pcpContactSubType = ko.observable( ko.utils.arrayFirst( careMemberRoles(), function( contactType ){
			return contactType.name() == 'Primary Care Physician';	//note name instead of role since theres a typo in the db...
		}));
		
		var careMembers = ko.computed(function(){
			var members = [];
			if (selectedPatient() && selectedPatient().contactCard() && selectedPatient().contactCard().careTeam() ) {
                members = selectedPatient().contactCard().careTeam().members();
            }
			return members;
		}).extend({ throttle: 50 });
		
		var selectedCareMember = ko.observable();
		var selectedPatientToken = selectedPatient.subscribe( function( newSelectedPatient ){
			selectedCareMember(null);
		});
		
		//widgets
		function widget(data, column) {
			var self = this;
			self.name = ko.observable(data.name);
			self.path = ko.observable(data.path);
			self.isOpen = ko.observable(data.open);
			self.column = column;
			self.isFullScreen = ko.observable(false);
			//self.filtersOpen = ko.observable(true);
			self.activationData = { widget: self, careMembers: careMembers, sortFunction: data.sortFunction };
			self.allowAdd = data.allowAdd;
			self.statusIds = data.statusIds;
		}

		function column(name, open, widgets) {
			var self = this;
			self.name = ko.observable(name);
			// self.isOpen = ko.observable(open).extend({ notify: 'always' });
			// self.isOpen.subscribe(function () {
				// computedOpenColumn(self);
			// });
			self.widgets = ko.observableArray();
			$.each(widgets, function (index, item) {
				self.widgets.push(new widget(item, self))
			});
		}
		
		//Members in the Active section appear in Core (TRUE first), then Role Ascending order
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
		
		//Members in the Inactive section appear in Updated Date Descending, Role Ascending order
		var inactiveMembersSort = function (l, r) { 			
			var leftUpdatedOn = l.updatedOn() ? l.updatedOn() : l.createdOn();
			leftUpdatedOn = moment( leftUpdatedOn ).format("MM/DD/YYYY"); //date only
			var rightUpdatedOn = r.updatedOn() ? r.updatedOn() : r.createdOn();
			rightUpdatedOn = moment( rightUpdatedOn ).format("MM/DD/YYYY"); //date only
			if( leftUpdatedOn == rightUpdatedOn ){
				var leftRole = l.computedRoleName() ? l.computedRoleName().toLowerCase() : '';
				var rightRole = r.computedRoleName() ? r.computedRoleName().toLowerCase() : '';
				return leftRole == rightRole ? 0 : (leftRole > rightRole ? 1 : -1);
			}
			else{
				return leftUpdatedOn < rightUpdatedOn ? 1 : -1;
			}
			return 0;
		}; 
		
		var careTeamColumn = ko.observable(new column('careMembers', false, [
						{ name: 'Active Team', path: 'viewmodels/patients/widgets/careMembers', open: true, statusIds: [1] , allowAdd: true, sortFunction: activeMembersSort }, 
						{ name: 'Team History', path: 'viewmodels/patients/widgets/careMembers', open: false, statusIds: [2,3], allowAdd: false, sortFunction: inactiveMembersSort }
					]));
		
		function minimizeThisColumn(sender) {
            sender.column.isOpen(false);
        }

        function maximizeThisColumn(sender) {
            sender.column.isOpen(true);
        }
		
		//dialog
		var modalShowing = ko.observable(false);
		var modalEntity = ko.observable(new ModalEntity(modalShowing));
		var newCareMember = ko.observable();				
		var newCareTeam = ko.observable();
		
		function saveAndAddAnother(){			
			saveOverride().then( addCareMember );
			modalShowing(false);
		}
		
		function saveOverride () {
			if( !selectedPatient().contactCard().careTeam() ){
				//team has not yet been created:
				newCareTeam( datacontext.createEntity('CareTeam', 
						{ 	id: -1, 
							contactId: selectedPatient().contactId(),
							createdById: session.currentUser().userId()
						}) );
				newCareTeam().members = ko.observableArray();
				newCareTeam().members.push( newCareMember() );
				return datacontext.saveCareTeam( newCareTeam() ).then( saveTeamCompleted );
			}
			else{
				//add/save one member to an existing team
				//	note: the new member should already be here inside members:				
				return datacontext.saveCareTeam( selectedPatient().contactCard().careTeam() ).then( saveTeamCompleted );
			}
							
			function saveTeamCompleted( team ){				
			};			            
        };
		
        function cancelOverride () {
			modalShowing(false);
			modalEntity().careMember().entityAspect.rejectChanges();			
        };
		
		var modalSettings = {
			title: 'Assign Care Team',
			entity: modalEntity, 
			templatePath: 'viewmodels/templates/careMember.edit', 
			showing: modalShowing, 
			saveOverride: saveOverride, 
			cancelOverride: cancelOverride, 
			deleteOverride: null, 
			classOverride: 'modal-lg',
			customButtons: [
				{	btnEnabled: modalEntity().canSave, btnFunction: saveAndAddAnother, btnText: 'Save + New' }
			]
		};
		
        var modal = new modelConfig.modal(modalSettings);
		
		function canAddCareMember(member){
			return true;	//TODO validate team role duplicates /PCM and other logic here	
		}
		
		function ModalEntity(modalShowing) {
            var self = this;
            self.careMember = ko.observable();
            self.canSaveObservable = ko.observable(true);
            self.canSave = ko.computed({
                read: function () {
                    var memberok = false;
                    if (self.careMember()) {                        
						memberok = self.careMember().isValid() && canAddCareMember( self.careMember() );
                    }
                    return memberok && self.canSaveObservable();
                },
                write: function (newValue) {
                    self.canSaveObservable(newValue);
                }
            });
            // Object containing parameters to pass to the modal
            self.activationData = { careMember: self.careMember, careTeamMembers: careMembers, 
									selectedPatient: selectedPatient, careMemberRoles: careMemberRoles, 
									canSave: self.canSave, pcmContactSubType: pcmContactSubType,
									pcpContactSubType: pcpContactSubType, showing: modalShowing,
									addContactReturnedCallback: addContactReturned };			
        }
		
		function addContactReturned(contact){
			// called back from add contact dialog:
		    //back to add care member: show the screen without search. show the new created contact:
			var modalSettings = {
				title: 'Assign Care Team',
				entity: modalEntity, 
				templatePath: 'viewmodels/templates/careMember.edit', 
				showing: modalShowing, 
				saveOverride: saveOverride, 
				cancelOverride: cancelOverride, 
				deleteOverride: null, 
				classOverride: 'modal-lg',
				customButtons: [
					{	btnEnabled: modalEntity().canSave, btnFunction: saveAndAddAnother, btnText: 'Save + New' }
				]
			};
			
			var modal = new modelConfig.modal(modalSettings);
			if (contact) {
                //new contact created - attach to the care member:
				newCareMember().contactId(contact.id());
				modalSettings.title += ' - ' + contact.fullName();								
			}
			else{
				//add contact canceled:
				newCareMember().contactId(null);				
			}
			modalEntity().careMember( newCareMember() );				 
			shell.currentModal(modal);
			modalShowing(true);
		}
		
		function addCareMember(){
			var teamId = selectedPatient()? selectedPatient().contactCard()? ( selectedPatient().contactCard().careTeam()? selectedPatient().contactCard().careTeam().id(): null ) : null : null;
			newCareMember( datacontext.createEntity('CareMember', 
						{ 	id: --nextId, 
							contactId: null,	//no contact yet
							careTeamId: teamId,
							distanceUnit: 'mi',
							statusId: 1,		//active 
							core: false,
							dataSource: 'Engage',
							createdById: session.currentUser().userId() 
						}) );
			newCareMember().isNew(true);			
            modalEntity().careMember( newCareMember() );
            shell.currentModal(modal);
            modalShowing(true);
		}
		
		function editCareMemberContact(member){
			if( member.contact() ){
				contactsIndex.editContact( member.contact() );
			}
		}
		
		function editCareMember(member){			
			var modalSettings = {
				title: 'Edit Care Team Member - ' + member.contact().fullName(),
				entity: modalEntity, 
				templatePath: 'viewmodels/templates/careMember.edit', 
				showing: modalShowing, 
				saveOverride: saveOverride, 
				cancelOverride: cancelOverride, 
				deleteOverride: null, 
				classOverride: 'modal-lg'				
			};
			
			var modal = new modelConfig.modal(modalSettings);
			
			modalEntity().careMember( member );
			//show the screen without search. show the contact. 
            shell.currentModal(modal);
            modalShowing(true);
		}
		
		function deleteCareMember(member){
			if( confirm('are you sure you want to delete the care member: ' + member.contact().fullName() + ' (' + member.computedRoleName() + ') ') ){
				if( member.contactId() && member.careTeamId() ){
					datacontext.deleteCareTeamMember( member ).then( deleteCompleted );
				}
			}
		}
		
		function deleteCompleted(){
			//selectedCareMember(null);
		}
		
		function activate(){
			return true;
		};
				
		// Toggle the widget to / from fullscreen
        function toggleFullScreen(widgetname) {
            // If this widget is already fullscreen,
            if (widgetname === fullScreenWidget()) {
                // Remove full screen widget
                fullScreenWidget(null);
            } else {
                // Else, set it as the full screen widget
                fullScreenWidget(widgetname);
            }
        }
		
		
		function toggleOpenColumn() {
            leftColumnOpen(!leftColumnOpen());
        }
	
		function checkDataContext() {
		    if (!datacontext) {
		        datacontext = require('services/datacontext');
		    }
		}

		var showEditButton = ko.computed(function(){
			var member = selectedCareMember();			
			if( member && member.isEditable() ){
				return true;
			}
			else{
				return false;
			}
		}).extend({ throttle: 100 });
					
		var showDeleteButton = ko.computed(function(){
			return showEditButton();
		}).extend({ throttle: 100 });
		
		var selectedCareMemberName = ko.computed( function(){
			var name = '';
			var member = selectedCareMember();
			var contact = member? member.contact() : null;
			if( member && contact ){
				name = ' - ' + contact.firstName() + ' ' + contact.lastName();
			}
			return name;			
		}).extend({ throttle: 100 }); 
		
		function detached(){			
		}
		
		var isComposed = ko.observable(true);
		
		var vm = {
			activate: activate,
			detached: detached,
			isComposed: isComposed,
			careTeamColumn: careTeamColumn,
			selectedPatient: selectedPatient,
			addCareMember: addCareMember,
			showEditButton: showEditButton,
			showDeleteButton: showDeleteButton,	
			editCareMember: editCareMember,
			deleteCareMember: deleteCareMember,
			editCareMemberContact: editCareMemberContact,
			careMembers: careMembers,
			selectedCareMember: selectedCareMember,
			selectedCareMemberName: selectedCareMemberName,
			careMemberRoles: careMemberRoles,
			pcmContactSubType: pcmContactSubType,
			pcpContactSubType: pcpContactSubType,
			toggleOpenColumn: toggleOpenColumn,
			fullScreenWidget: fullScreenWidget,
			leftColumnOpen: leftColumnOpen,
			toggleFullScreen: toggleFullScreen,
			activeWidgetOpen: activeWidgetOpen,
			minimizeThisColumn: minimizeThisColumn,
            maximizeThisColumn: maximizeThisColumn			
		};
		
		return vm;
		
	}
);