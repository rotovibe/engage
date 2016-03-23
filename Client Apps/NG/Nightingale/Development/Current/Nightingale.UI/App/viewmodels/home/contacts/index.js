define(['services/session', 'services/datacontext', 'viewmodels/shell/shell', 'models/base'],
	function(session, datacontext, shell, modelConfig){
				
        var fullScreenWidget = ko.observable();
        var leftColumnOpen = ko.observable(true);
		
		//contact dialog:
		var modalShowing = ko.observable(false);
		var modalEntity = ko.observable(new ModalEntity(modalShowing));
		var newContact = ko.observable();
		
		function saveOverride () {
			console.log('add contact save clicked');
            //datacontext.saveContactCard(newContact(), 'Insert').then(saveCompleted);
			return setTimeout(function(){ saveCompleted(); },1);	//TODO: this is temporary simulating a save
			
            function saveCompleted(contact) {
				datacontext.detachEntity(newContact());	//TODO: this is temporary !!
				console.log('add contact save completed (dummy)');
                // contact.isNew(false);
                // localCollections.contacts.push(newContact());
				//var dummy = myToDos().length;
				//contact.clearDirty();
            }
        };
		
		
        function cancelOverride () {
			datacontext.detachEntity(newContact());	//TODO: this is temporary !!
			console.log('add contact cancel clicked');
			
            //datacontext.cancelEntityChanges(modalEntity().contact());
			//modalEntity().contact().clearDirty();
        };
		var modalSettings = {
			title: 'Add Contact',
			entity: modalEntity, 
			templatePath: 'viewmodels/templates/contact.edit', 
			showing: modalShowing, 
			saveOverride: saveOverride, 
			cancelOverride: cancelOverride, 
			deleteOverride: null, 
			classOverride: 'modal-lg'
		}
        var modal = new modelConfig.modal(modalSettings);
		
		
		
		function ModalEntity(modalShowing) {
            var self = this;
            self.contact = ko.observable();
            self.canSaveObservable = ko.observable(true);
            self.canSave = ko.computed({
                read: function () {
                    var contactok = false;
                    if (self.contact()) {                        
						contactok = self.contact().isValid();
                    }
                    return contactok && self.canSaveObservable();
                },
                write: function (newValue) {
                    self.canSaveObservable(newValue);
                }
            });
            // Object containing parameters to pass to the modal
            self.activationData = { contact: self.contact, canSave: self.canSave, showing: modalShowing  };
        }
		
		function addContact(){
			//navigate to add contact dialog
			newContact(datacontext.createEntity('ContactCard', { id: -1, statusId: 1, createdById: session.currentUser().userId() }));
            //newContact().isNew(true);
			//newContact().watchDirty();
            modalEntity().contact(newContact());
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
		
		function editContact(){
			alert('edit');
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