define(['services/session', 'services/datacontext', 'viewmodels/shell/shell', 'models/base', 'services/local.collections'],
	function(session, datacontext, shell, modelConfig, localCollections){
				
        var fullScreenWidget = ko.observable();
        var leftColumnOpen = ko.observable(true);

		//contact dialog:
		var modalShowing = ko.observable(false);
		var modalEntity = ko.observable(new ModalEntity(modalShowing));
		var theContact = ko.observable();				
		
		function saveOverride () {
			
			theContact().saveChanges().then( saveCompleted );
			theContact().clearDirty();
	
			
            function saveCompleted(contact) {
                contact.isNew(false);
                localCollections.contacts.push(theContact());				
				contact.clearDirty();
            }
        };
		
        function cancelOverride () {
			modalShowing(false);
			modalEntity().contactCard().cancelChanges();
			modalEntity().contactCard().clearDirty();	
        };
		
		var modalSettings = {
			title: 'Add Contact',
			entity: modalEntity, 
			templatePath: 'viewmodels/templates/contact.edit', 
			showing: modalShowing, 
			saveOverride: saveOverride, 
			cancelOverride: cancelOverride, 
			deleteOverride: null, 
			classOverride: null//'modal-lg'
		};
		
        var modal = new modelConfig.modal(modalSettings);
		
		function ModalEntity(modalShowing) {
            var self = this;
            self.contactCard = ko.observable();
            self.canSaveObservable = ko.observable(true);
            self.canSave = ko.computed({
                read: function () {
                    var contactok = false;
                    if (self.contactCard()) {                        
						contactok = self.contactCard().isValid() && !self.contactCard().isDuplicate() && self.contactCard().isDuplicateTested();
                    }
                    return contactok && self.canSaveObservable();
                },
                write: function (newValue) {
                    self.canSaveObservable(newValue);
                }
            });
            // Object containing parameters to pass to the modal
            self.activationData = { contactCard: self.contactCard, canSave: self.canSave, showing: modalShowing  };
        }
		
		function addContact(){
			//navigate to add contact dialog
			
			var newModes = [];
			ko.utils.arrayForEach( datacontext.enums.communicationModes(), function(mode) {
				var newMode = { lookUpModeId: mode.id(), optOut: false, preferred: false };
				newModes.push( newMode );
			});
			theContact( datacontext.createEntity('ContactCard', { id: -1, statusId: 1, modes: newModes, createdById: session.currentUser().userId() }));
			theContact().isNew(true);
			theContact().activeTab("Profile");
			
			theContact().watchDirty();
            modalEntity().contactCard( theContact() );
            shell.currentModal(modal);
            modalShowing(true);
		}
		
		function editContact( contact ){	
		
			theContact( contact() );
			if( theContact().isPatient() ){
				theContact().activeTab("General");	
			}
			else{
				theContact().activeTab("Profile");
			}
			modalEntity().contactCard( contact() );
			
			var modalSettings = {
				title: 'Edit Communication Preferences',
				showSelectedPatientInTitle: true,
				entity: modalEntity, 
				templatePath: 'viewmodels/templates/contact.edit', 
				showing: modalShowing, 
				saveOverride: saveOverride, 
				cancelOverride: cancelOverride, 
				deleteOverride: null, 
				classOverride: null//'modal-lg'
			};
		
			var modal = new modelConfig.modal(modalSettings);
			
			shell.currentModal(modal);
            modalShowing(true);
		}
		//end contact dialog
		
		function activate(){
			return true;
		}
		
		function toggleOpenColumn() {
            leftColumnOpen(!leftColumnOpen());
        }
		
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
		
		var selectedContact = ko.observable();
		function isShowEditButton(){			
			return selectedContact && selectedContact();			
		}
		function detached(){
		
		}
		
		function deleteContact(){
			alert('delete');
		}
		
		var showDeleteButton = ko.computed(function(){
			if( selectedContact() ){
				return true;
			}
		});
		var showEditButton = ko.computed(function(){
			if( selectedContact() ){
				return true;
			}
		});
		var vm = {
			activate: activate,
			detached: detached,
			
			selectedContact: selectedContact,
			showEditButton: showEditButton,
			showDeleteButton: showDeleteButton,	
			editContact: editContact,
			deleteContact: deleteContact,
			addContact: addContact,
			toggleOpenColumn: toggleOpenColumn,
			fullScreenWidget: fullScreenWidget,
			leftColumnOpen: leftColumnOpen,
			toggleFullScreen: toggleFullScreen			
		}
		
		return vm;			
	});