/**
*	care team management
*	@module care team index
*/

define(['services/session', 'services/datacontext', 'viewmodels/patients/index', 'models/base', 'viewmodels/shell/shell'],
    function (session, datacontext, patientsIndex, modelConfig, shell) {

        var fullScreenWidget = ko.observable();
        var leftColumnOpen = ko.observable(true);
		var activeWidgetOpen = ko.observable(true);
		var inactiveWidgetOpen = ko.observable(false);
		
		var initialized = false;
		
		checkDataContext();
		
        var selectedPatient = ko.computed(function () {
            return patientsIndex.selectedPatient();
        });
		
		var careMembers = ko.computed(function(){
			var members = [];
			if (selectedPatient() && selectedPatient().careTeam()) {
                members = selectedPatient().careTeam().members();
            }
			return members;
		});
		
		var selectedCareMember = ko.observable();
		
		//widgets
		function widget(data, column) {
			var self = this;
			self.name = ko.observable(data.name);
			self.path = ko.observable(data.path);
			self.isOpen = ko.observable(data.open);
			self.column = column;
			self.isFullScreen = ko.observable(false);
			//self.filtersOpen = ko.observable(true);
			self.activationData = { widget: self, careMembers: careMembers, defaultSort: data.defaultSort };
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

		var careTeamColumn = ko.observable(new column('careMembers', false, [
						{ name: 'Active Team', path: 'viewmodels/patients/widgets/careMembers', open: true, statusIds: [1] , allowAdd: true }, 
						{ name: 'Team History', path: 'viewmodels/patients/widgets/careMembers', open: false, statusIds: [2,3], allowAdd: false }
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
		var theCareMember = ko.observable();
		
		function saveOverride () {
			
			//TODO: save theCareMember
			// theCareMember().saveChanges().then( saveCompleted );
			// theCareMember().clearDirty();
				
            // function saveCompleted(contact) {
                // if (contact) {
                    // //only insert returns the object
					// contact.isNew(false);					
					// contact.clearDirty();
				// }				               
            // }
        };
		
        function cancelOverride () {
			modalShowing(false);
			//modalEntity().careMember().cancelChanges();
			//modalEntity().careMember().clearDirty();	
        };
		
		var modalSettings = {
			title: 'Assign Care Team',
			entity: modalEntity, 
			templatePath: 'viewmodels/templates/careMember.edit', 
			showing: modalShowing, 
			saveOverride: saveOverride, 
			cancelOverride: cancelOverride, 
			deleteOverride: null, 
			classOverride: 'modal-lg'
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
            self.activationData = { careMember: self.careMember, canSave: self.canSave, showing: modalShowing  };
        }
		
		function addCareMember(){
			//var pid = selectedPatient()? selectedPatient().id() : null;
			//newCareMember( datacontext.createEntity('CareMember', { id: -1, patientId: pid, createdById: session.currentUser().userId() }));
			newCareMember().isNew(true);
			//newCareMember().activeTab("Profile");
			
			//newCareMember().watchDirty();
            modalEntity().careMember( newCareMember() );
            shell.currentModal(modal);
            modalShowing(true);
		}
		
		function editCareMember(member){
			//TODO
		}
		
		function deleteCareMember(member){
			//TODO
		}
		
		function activate(){
			if( !initialized ){
				initializeViewModel();
                initialized = true;
			}
			
		};
		
		function initializeViewModel(){
			var pid = selectedPatient()? selectedPatient().id() : null;
			newCareMember( datacontext.createEntity('CareMember', { id: -1, patientId: pid, createdById: session.currentUser().userId() }) );
			newCareMember().isNew(true);
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
			if( member ){//TODO: && member.dataSource() == 'Engage' && member.isEditable() ){
				return true;
			}
			else{
				return false;
			}
		});
					
		var showDeleteButton = ko.computed(function(){
			return showEditButton();
		});
		
		function detached(){
			var self = this;
			//dispose computeds:
			
			self.showDeleteButton.dispose();
			self.showEditButton.dispose();
			//self.selectedPatient.dispose();
			self.careMembers.dispose();
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
			careMembers: careMembers,
			selectedCareMember: selectedCareMember,
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