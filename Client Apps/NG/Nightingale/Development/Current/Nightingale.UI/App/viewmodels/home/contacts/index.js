define(['services/session', 'services/datacontext', 'viewmodels/shell/shell', 'models/base', 'services/local.collections'],
	function(session, datacontext, shell, modelConfig, localCollections){
				
        var fullScreenWidget = ko.observable();
        var leftColumnOpen = ko.observable(true);

		checkDataContext();
		
		var tabs = ko.observableArray([
			new Tab('Profile', null, '/NightingaleUI/Content/images/patient_neutral_small.png', 'Phone blue small'),
			new Tab('General', null, '/NightingaleUI/Content/images/settings_blue.png', 'Phone blue small'),
			new Tab('Phone', 'icon-phone blue', null),
			new Tab('Text', 'icon-sms blue', null),
			new Tab('Email', 'icon-email blue', null),
			new Tab('Address', 'icon-address blue', null),
			new Tab('Language', null, '/NightingaleUI/Content/images/nav_population.png', 'Language blue small')			
		]);
		
		var tabIndex = {
			profile: 0,
			general: 1,
			phone: 2,
			text: 3,
			email: 4,
			address: 5,
			language: 6
		};
		
		
		
		function Tab(name, cssClass, imgSource, imgAlt){
			var self = this;
			self.name = name;
			self.cssClass = cssClass;
			self.imgSource = imgSource;
			self.imgAlt = imgAlt;
			self.isShowing = true;
			self.hasErrors = ko.observable(false);
		}
		
		function setActiveTab( contactCard, name ){
			contactCard.activeTab(name);
		}
		
		
		//contact dialog:
		var modalShowing = ko.observable(false);
		var modalEntity = ko.observable(new ModalEntity(modalShowing));
		var theContact = ko.observable();				
		var myContactSearchResults = ko.observableArray([]);
		var contactsReturned = ko.observableArray([]);
		var contactsTake = ko.observable(100);
		var contactsSkip = ko.observable(0);
		var maxContactsCount = ko.observable(400);
		var searching = ko.observable(false);
		var totalCount = ko.observable();
		
		function saveOverride () {
			
			theContact().saveChanges().then( saveCompleted );
			theContact().clearDirty();
				
            function saveCompleted(contact) {
                if (contact) {
                    //only insert returns the object
					contact.isNew(false);					
					contact.clearDirty();
				}				               
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
		
		function editPatientContact( contact ){
			contact.activeTab("General");
			startEditContactDialog( contact, 'Edit Communication Preferences', true );
		}
		
		function editContact( contact ){						
			contact.activeTab("Profile");
			var title = 'Edit Contact - ' + contact.firstName() + ' ' + contact.lastName();
			startEditContactDialog( contact, title, false );			
		}
		
		function startEditContactDialog( contact, titleText, showPatientName ){
			theContact( contact );
			modalEntity().contactCard( contact );
			
			var modalSettings = {
				title: titleText,
				showSelectedPatientInTitle: showPatientName,
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
		
		//contact search
		var contactTypeGroupId = 1;
		
		var contactTypes = ko.observableArray([]);
		contactTypes( datacontext.getContactTypes( contactTypeGroupId, 'root' ) );
		var allContactTypes = ko.observableArray([]);
		var typesList = datacontext.getContactTypes( contactTypeGroupId, false );
		allContactTypes(typesList);
				
		var criteriaContactTypeId = ko.observable();
		var criteriaContactSubTypes = ko.observableArray();
		var criteriaContactStatuses = ko.observableArray();
		var contactStatuses = datacontext.enums.contactStatuses;
		var activeContactStatus = ko.observable();
		var activeStatuses = ko.utils.arrayFilter( contactStatuses(), function(status){
			return status.name() == 'Active';
		});
		if( activeStatuses.length ){
			activeContactStatus( activeStatuses[0] );
		}
		
		var criteriaFirstName = ko.observable();
		var criteriaLastName = ko.observable();
		//var criteriaPhone =  datacontext.createComplexType('Phone', { id: -1 });
		var statesList = datacontext.enums.states;			
		var defaultContactType = ko.observable();
		var initialized = false;
		
		function activate(){
			var self = this;
			if (!initialized) {
                initializeViewModel();													
                initialized = true;				
            }
			self.contactsShowingText = ko.computed( function(){
				var text = '';
				var myContactSearchResults = self.myContactSearchResults();
				var totalQueryCount = totalCount();
				if( myContactSearchResults.length ){
					text = ' - ' + myContactSearchResults.length + ' showing'; //TODO: X out of Y
				}
				if( myContactSearchResults.length < totalQueryCount ){
					text += ' out of ' + totalQueryCount;
				}
				return text;
			}).extend({ throttle: 100 });
				
			self.showResetFilters = ko.computed( function(){				
				var show = false;
				var criteriaFirstName = self.criteriaFirstName();
				var criteriaLastName = self.criteriaLastName();
				var defaultContactType = self.defaultContactType();
				var criteriaContactTypeId = self.criteriaContactTypeId();
				var criteriaContactSubTypes = self.criteriaContactSubTypes();
				var activeContactStatus = self.activeContactStatus();
				var criteriaContactStatuses = self.criteriaContactStatuses();
				
				var selectedStatuses = ko.utils.arrayFilter( criteriaContactStatuses, function(status){
					return status.id() == activeContactStatus.id();
				});
				
				if( criteriaContactTypeId && defaultContactType && defaultContactType.id() == criteriaContactTypeId ){
					//default contact type is selected
					if( selectedStatuses.length == 1 && criteriaContactStatuses.length == 1 ){
						//only activ status is selected
						if( criteriaContactSubTypes.length == 0 ){
							//no subtypes							
							if( !criteriaFirstName && !criteriaLastName ){
								show = false;
							}
							else{
								show = true;
							}								
						}
						else{
							show = true;
						}
						
					}
					else{
						show = true;
					}						
				}
				else{
					show = true;
				}				
				return show;
			}).extend({ throttle: 100 });
			
			self.canSearchContacts = ko.computed( function(){
				var canSearch = false;
				var firstName = self.criteriaFirstName();
				var lastName = self.criteriaLastName();
				var statuses = self.criteriaContactStatuses();
				var subTypes = self.criteriaContactSubTypes();
				var searching = self.searching? self.searching() : false;
				
				if( statuses && statuses.length > 0 ){
					if( subTypes.length > 0 ){
						canSearch = true;	//status/s and type/s are selected
					}
					else{
						if( firstName && firstName.trim().length > 0 && lastName && lastName.trim().length > 0 ){
							canSearch = true;	//status/s and first and last name
						}					
					}
				}
				canSearch = canSearch && !searching;	//block until search returned
				return canSearch;
			}).extend({ throttle: 100 });
		
			return true;
			
		}	//activate
		
		function initializeViewModel(){
			
			if (session.currentUser().settings()) {
				var totalQueryCount = datacontext.getSettingsParam('TotalQueryCount');
				if( totalQueryCount && !isNaN(totalQueryCount)){
					//the max contacts to load
					maxContactsCount( parseInt( totalQueryCount ) );					
                }
			    var take = datacontext.getSettingsParam('QueryTake');
			    if( take && !isNaN(take) ){
				    //the take
				    contactsTake( parseInt( take ) );					
			    }
            }
			initialized = true;
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
		
		function deleteContact(){
			alert('delete');
		}
		
		var selectedContactId = ko.computed( function(){
			var contact = selectedContact;
			if( contact() ){
				return contact().id();
			}
			else{
				return '';
			}
		}).extend({ throttle: 100 });
		
		var showEditButton = ko.computed(function(){
			var hasContactSelected = selectedContactId();
			if( hasContactSelected && selectedContact().isEditable() ){
				return true;
			}
			else{
				return false;
			}
		}).extend({throttle: 500});
		
		var showDeleteButton = ko.computed(function(){
			return showEditButton();
		}).extend({throttle: 500});
		
		function resetTabs(){
			ko.utils.arrayForEach(tabs, function(tab){
				tab.isShowing = true;
				tab.hasErrors(false);
			});
		}
		
		function selectContact( contact ){
			resetTabs();
			contact.activeTab("Profile");
			selectedContact( contact );
		}
		
		//load more:
		var canLoadMoreContacts = ko.observable(false);
				
		var maxContactsLoaded = ko.observable(false);
					
		function findDefaultContactType( contactTypes ){
			var defaultType = null;
			var types = ko.utils.arrayFilter( contactTypes(), function(node){
				return node.name() == 'Person';
			});
			if( types && types.length > 0 ){
				defaultType = types[0];
			}
			return defaultType;
		};

		var noResultsFound = ko.observable(false);
		
		function resetFilters(){
			if( !defaultContactType() ){
				defaultContactType( findDefaultContactType(contactTypes) )
			}
			criteriaContactTypeId( defaultContactType().id() );
			criteriaContactSubTypes([]);			
			
			//set active status as selected in the criteria as default:						
			criteriaContactStatuses([]);
			if( activeContactStatus() ){
				criteriaContactStatuses.push(activeContactStatus());
			}
			criteriaFirstName(null);
			criteriaLastName(null);
			myContactSearchResults([]);
			contactsReturned([]);
			canLoadMoreContacts(false);
			maxContactsLoaded(false);
			noResultsFound(false);
			selectedContact(null);
		}
		resetFilters();
				
		function getContactTypeChildren( typeId ){
			var subTypes = ko.utils.arrayFilter( allContactTypes(), function(item){
				return ( item.parentId() && item.parentId() == typeId ) 
			});
			return subTypes;
		}
		
		var contactSubTypes = ko.computed( function(){
			//return children of selected type
			var subTypes = [];
			var typeId = criteriaContactTypeId();
			if( typeId ){
				subTypes = getContactTypeChildren( typeId );										
			}
			return subTypes;
		}).extend({ throttle: 100 });						
		
		function clearCacheAndLoad(){
			
			searching(true);
			noResultsFound(false);			
			var contacts = datacontext.getLocalContacts();			
			myContactSearchResults([]);
			totalCount(0);
			contactsSkip(0);
			selectedContact(null);
			setTimeout( function(){
				//short delay to allow the ko data binding to release references to these contacts, before removing them: 
				if( contacts && contacts.length > 0 ){										
					ko.utils.arrayForEach( contacts, function(contact){
						if( contact ){
							//if( selectedContact() && contact.id() !== selectedContact().id() ){
								//remove from breeze cache:
								contact.entityAspect.setDeleted();
								contact.entityAspect.acceptChanges();
							//}
						}
					});					
				}				
				loadContacts();	//load first block with the new sort
			}, 50);
		}
		
		function loadContacts(){
			
			var params = {
						contactTypeIds: [],
						contactSubTypeIds: [],
						contactStatuses: [],
						firstName: criteriaFirstName(),
						lastName: criteriaLastName(),
						filterType: 'StartsWith',
						take: contactsTake(),
						skip: contactsSkip()
			};
			params.contactTypeIds.push( criteriaContactTypeId() );
			ko.utils.arrayForEach( criteriaContactSubTypes(), function(subType){
				params.contactSubTypeIds.push( subType.id() );
			});
			
			ko.utils.arrayForEach( criteriaContactStatuses(), function(status){
				
				//the search API wants the status enum NAME/s.
				//need to find the selected status as its enum object, since when multiselect selection values are clicked,
				//they are returned as type "Identifier", and dont have the "name" prop
				
				var statusObjects = ko.utils.arrayFilter( contactStatuses(), function(statusEnum){
					return statusEnum.id() == status.id();
				});
				if( statusObjects.length > 0 ){
					//the selected status object now has the "name" prop that we need
					params.contactStatuses.push( statusObjects[0].name() );
				}
			});
			contactsReturned([]);
			return datacontext.getContacts( contactsReturned, params, totalCount, 'ContactCard' ).then( getContactsReturned );
		}
		
		function loadMoreContacts(){
			loadContacts();
		}
		
		function searchContacts(){
			clearCacheAndLoad();			
		}			

		function getContactsReturned(){
			
			ko.utils.arrayForEach( contactsReturned(), function(contact){
				myContactSearchResults.push( contact );
			});
			var skipped = contactsSkip();
			var nextSkip = skipped + contactsTake();
			if( nextSkip < totalCount() && nextSkip < maxContactsCount() ){
				contactsSkip( nextSkip );
				canLoadMoreContacts( true );
			}
			else{
				canLoadMoreContacts( false );
			}
			
			if( myContactSearchResults().length >= maxContactsCount() ){
				maxContactsLoaded( true );
			}
			else{
				maxContactsLoaded( false );
			}
			
			if( totalCount() == 0 ){
				//show no results message
				noResultsFound( true );
			}
			//allow the search button
			searching(false);
		}
		
		function checkDataContext() {
		    if (!datacontext) {
		        datacontext = require('services/datacontext');
		    }
		}
		
		function detached(){
			var self = this;
			//dispose computeds:
			self.contactsShowingText.dispose();
			self.showResetFilters.dispose();			
			self.canSearchContacts.dispose();
			self.showDeleteButton.dispose();
			self.showEditButton.dispose();
			self.contactSubTypes.dispose();
		}
		
		var activeDetailsTab = ko.observable('Profile');
		
		function setActiveDetailsTab( name ){
			self.activeDetailsTab(name);	
		}
		
		var vm = {
			activate: activate,
			detached: detached,
			
			tabs: tabs,
			tabIndex: tabIndex,
			setActiveTab: setActiveTab,
			searchContacts: searchContacts,			
			selectedContact: selectedContact,
			selectContact: selectContact,
			showEditButton: showEditButton,
			showDeleteButton: showDeleteButton,	
			editContact: editContact,
			editPatientContact: editPatientContact,
			deleteContact: deleteContact,
			addContact: addContact,			
			myContactSearchResults: myContactSearchResults,
			canLoadMoreContacts: canLoadMoreContacts,
			loadMoreContacts: loadMoreContacts,
			maxContactsLoaded: maxContactsLoaded,
			searching: searching,
			resetFilters: resetFilters,
			noResultsFound: noResultsFound,
			criteriaContactTypeId: criteriaContactTypeId,
			criteriaContactSubTypes: criteriaContactSubTypes,
			criteriaContactStatuses: criteriaContactStatuses,
			criteriaFirstName: criteriaFirstName,
			criteriaLastName: criteriaLastName,
			//criteriaPhone: criteriaPhone,			
			getContactTypeChildren: getContactTypeChildren,
			defaultContactType: defaultContactType,
			activeContactStatus: activeContactStatus,
			contactTypes: contactTypes,
			contactSubTypes: contactSubTypes,
			contactStatuses: contactStatuses,
			toggleOpenColumn: toggleOpenColumn,
			fullScreenWidget: fullScreenWidget,
			leftColumnOpen: leftColumnOpen,
			toggleFullScreen: toggleFullScreen			
		}
		
		return vm;			
	});