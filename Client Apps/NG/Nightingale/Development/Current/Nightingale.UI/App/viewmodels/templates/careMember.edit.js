/**
*	add /edit care member dialog module*	
*	@module careMember.edit
*/
define([ 'services/datacontext', 'services/local.collections', 'viewmodels/home/contacts/index' ],
	function( datacontext, localCollections, contactsIndex ){

		var subscriptionTokens = [];		
		var newId = 0;		
		var frequencies = datacontext.enums.careMemberFrequency;
		var careMemberStatuses = datacontext.enums.careMemberStatuses;
				
		function contactSearch( settings ){
			var self = this;
			self.criteriaFirstName = ko.observable();
			self.criteriaLastName = ko.observable();			
			self.criteriaContactTypeId = ko.observable();
			self.criteriaContactSubTypes = ko.observableArray();
			self.criteriaContactStatuses = ko.observableArray();
			self.searching = ko.observable(false);
			self.totalCount = ko.observable(0);
			self.noResultsFound = ko.observable(false);
			self.myContactSearchResults = ko.observableArray([]);
			self.selectedContact = settings.selectedContact;
			
			self.canSearchContacts = ko.computed( function(){
				var canSearch = false;
				var firstName = self.criteriaFirstName();
				var lastName = self.criteriaLastName();
				//var statuses = self.criteriaContactStatuses();
				var subTypes = self.criteriaContactSubTypes();
				var searching = self.searching? self.searching() : false;
				if( subTypes.length > 0 ){
					canSearch = true;// type/s are selected
				}
				else{
					if( firstName && firstName.trim().length > 0 && lastName && lastName.trim().length > 0 ){
						canSearch = true;	//first and last name
					}					
				}
				
				canSearch = canSearch && !searching;	//block until search returned
				return canSearch;
			}).extend({ throttle: 100 });
			
			self.defaultContactType = ko.observable();
			self.findDefaultContactType = function( contactTypes ){
				var defaultType = null;
				var types = ko.utils.arrayFilter( contactTypes(), function(node){
					return node.name() == 'Person';
				});
				if( types && types.length > 0 ){
					defaultType = types[0];
				}
				return defaultType;
			};
			
			var allContactTypes = ko.observableArray([]);
			var contactTypeGroupId = 1;
			var contactTypes = ko.observableArray([]);
			self.initialized = false;
			self.contactStatuses = datacontext.enums.contactStatuses;
			self.init = function(){
				contactTypes( datacontext.getContactTypes( contactTypeGroupId, 'root' ) );
				if( !self.defaultContactType() ){
					self.defaultContactType( self.findDefaultContactType(contactTypes) )
				}
				self.criteriaContactTypeId( self.defaultContactType().id() );
				
				var typesList = datacontext.getContactTypes( contactTypeGroupId, false );
				allContactTypes(typesList);
				
				//var contactStatuses = datacontext.enums.contactStatuses;
				
				var activeStatuses = ko.utils.arrayFilter( self.contactStatuses(), function(status){
					return status.name() == 'Active';
				});
				if( activeStatuses.length ){
					self.criteriaContactStatuses.push( activeStatuses[0] );
				}
				self.initialized = true;
			}
			
			self.contactsReturned = ko.observableArray([]);
			
			function getContactTypeChildren( typeId ){
				var subTypes = ko.utils.arrayFilter( allContactTypes(), function(item){
					return ( item.parentId() && item.parentId() == typeId ) 
				});
				return subTypes;
			}
			
			self.contactSubTypes = ko.computed( function(){
				//return children of selected type
				var subTypes = [];
				var typeId = self.criteriaContactTypeId();
				if( typeId ){
					subTypes = getContactTypeChildren( typeId );										
				}
				return subTypes;
			}).extend({ throttle: 100 });
			
			self.canLoadMoreContacts = ko.observable(false);
			self.maxContactsLoaded = ko.observable(false);
			
			self.resetFilters = function(){
				// if( !defaultContactType() ){
					// defaultContactType( findDefaultContactType(contactTypes) )
				// }
				//criteriaContactTypeId( defaultContactType().id() );
				self.criteriaContactSubTypes([]);			
				
				//set active status as selected in the criteria as default:						
				// criteriaContactStatuses([]);
				// if( activeContactStatus() ){
					// criteriaContactStatuses.push(activeContactStatus());
				// }
				self.criteriaFirstName(null);
				self.criteriaLastName(null);
				self.myContactSearchResults([]);
				self.contactsReturned([]);
				self.canLoadMoreContacts(false);
				self.maxContactsLoaded(false);
				self.noResultsFound(false);
				self.selectedContact(null);
			}
			
			self.showResetFilters = ko.computed( function(){				
				var show = false;
				var criteriaFirstName = self.criteriaFirstName();
				var criteriaLastName = self.criteriaLastName();
				var criteriaContactSubTypes = self.criteriaContactSubTypes();
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
				return show;
			}).extend({ throttle: 100 });
			
			self.contactsShowingText = ko.computed( function(){
				var text = '';
				var myContactSearchResults = self.myContactSearchResults();
				var totalQueryCount = self.totalCount();
				if( myContactSearchResults.length ){
					text = ' - ' + myContactSearchResults.length + ' showing';
				}
				if( myContactSearchResults.length < totalQueryCount ){
					text += ' out of ' + totalQueryCount;
				}
				return text;
			}).extend({ throttle: 100 });
			
			self.contactsTake = ko.observable(100);	//TODO: read params on init
			self.contactsSkip = ko.observable(0);
			self.maxContactsCount = ko.observable(400);
			
			self.showResultsHeader = ko.computed( function(){
				var myContactSearchResults = self.myContactSearchResults();
				return (myContactSearchResults.length > 0);
			}).extend({ throttle: 100 });
			
			function loadContacts(){
			
				var params = {
							contactTypeIds: [],
							contactSubTypeIds: [],
							contactStatuses: [],
							firstName: self.criteriaFirstName()? self.criteriaFirstName().trim() : null,
							lastName: self.criteriaLastName()? self.criteriaLastName().trim() : null,
							filterType: 'StartsWith',
							take: self.contactsTake(),
							skip: self.contactsSkip()
				};
				params.contactTypeIds.push( self.criteriaContactTypeId() );
				ko.utils.arrayForEach( self.criteriaContactSubTypes(), function(subType){
					params.contactSubTypeIds.push( subType.id() );
				});
				
				ko.utils.arrayForEach( self.criteriaContactStatuses(), function(status){
					
					//the search API wants the status enum NAME/s.
					//need to find the selected status as its enum object, since when multiselect selection values are clicked,
					//they are returned as type "Identifier", and dont have the "name" prop
					
					var statusObjects = ko.utils.arrayFilter( self.contactStatuses(), function(statusEnum){
						return statusEnum.id() == status.id();
					});
					if( statusObjects.length > 0 ){
						//the selected status object now has the "name" prop that we need
						params.contactStatuses.push( statusObjects[0].name() );
					}
				});
				self.contactsReturned([]);
				return datacontext.getContacts( self.contactsReturned, params, self.totalCount, 'ContactCarememberSearch' ).then( self.getContactsReturned );
			}
			
			self.getContactsReturned = function(list){
				var searchResults = self.myContactSearchResults();
				ko.utils.arrayPushAll( searchResults, self.contactsReturned() );
				self.myContactSearchResults.valueHasMutated();
				var skipped = self.contactsSkip();
				var nextSkip = skipped + self.contactsTake();
				if( nextSkip < self.totalCount() && nextSkip < self.maxContactsCount() ){
					self.contactsSkip( nextSkip );
					self.canLoadMoreContacts( true );
				}
				else{
					self.canLoadMoreContacts( false );
				}
				
				if( self.myContactSearchResults().length >= self.maxContactsCount() ){
					self.maxContactsLoaded( true );
				}
				else{
					self.maxContactsLoaded( false );
				}
				
				if( self.totalCount() == 0 ){
					//show no results message
					self.noResultsFound( true );
				}
				//allow the search button
				self.searching(false);
			}
			
			function clearCacheAndLoad(){
				self.searching(true);
				self.noResultsFound(false);			
				var contacts = datacontext.getLocalContacts('ContactCarememberSearch');			
				self.myContactSearchResults([]);
				self.totalCount(0);
				self.contactsSkip(0);
				self.selectedContact(null);
				setTimeout( function(){
					//short delay to allow the ko data binding to release references to these contacts, before removing them: 
					if( contacts && contacts.length > 0 ){										
						ko.utils.arrayForEach( contacts, function(contact){
							if( contact ){								
								//remove from breeze cache:
								contact.entityAspect.setDeleted();
								contact.entityAspect.acceptChanges();							
							}
						});					
					}				
					loadContacts();	//load first block with the new sort
				}, 50);
			}
			self.searchContacts = function(){
				clearCacheAndLoad();
			}
			self.loadMoreContacts = function(){
				loadContacts();
			}
						
			self.selectContact = function( contact ){				
				self.selectedContact(contact);
			}
		}	//contactSearch
		
		function activate( settings ){
			var self = this;
			self.showing = settings.showing;
			self.careMember = settings.careMember;
			self.careTeamMembers = settings.careTeamMembers;
			self.selectedPatient = settings.selectedPatient;
			self.selectedContact = ko.observable();
			self.careMemberRoles = settings.careMemberRoles;
			self.pcmContactSubType = settings.pcmContactSubType;
			self.pcpContactSubType = settings.pcpContactSubType;
			self.addContactReturnedCallback = settings.addContactReturnedCallback;
			self.editMode = ko.observable(false);
			if( self.careMember().contact() ){				
				self.selectedContact( self.careMember().contact() );
				//edit / add new got back from creating a new contact
				self.editMode(true);
			}				
			var searchSettings = {
				selectedContact: self.selectedContact
			}
			
			self.contactSearch = new contactSearch( searchSettings );
			self.contactSearch.init();			
			self.contactSubTypes = self.contactSearch.contactSubTypes;
			
			self.canAddContact = ko.computed( function(){				
				var showResults = self.contactSearch? self.contactSearch.showResultsHeader() : false;
				var editMode = self.editMode();
				var noResultsFound = self.contactSearch.noResultsFound();
				return ( showResults && !editMode ) || noResultsFound;				
			}).extend({throttle: 100});
			
			//assign selected contact to the member:
			var contactSelectedToken = self.selectedContact.subscribe( function(contact){
				if( contact ){
					self.careMember().contactId( contact.id() );
				}
				else{
					self.careMember().contactId( null );
				}
			});
			subscriptionTokens.push( contactSelectedToken );						
			
			self.contactAlreadyAssigned = ko.observable(false);
			
			self.validateCareMember = ko.computed( function(){
				// in addition to care member isValid, test validation rules related to the patient / current or new member / team here, 
				// and send hese errors to careMember.isValid
				
				var selectedContact = self.selectedContact();
				var selectedPatient = self.selectedPatient();
				var teamMembers = self.careTeamMembers();
				var roleId = self.careMember().roleId();
				var core = self.careMember().core();
				var statusId = self.careMember().statusId();
				var activeStatusId = contactsIndex.activeContactStatus()? contactsIndex.activeContactStatus().id() : null;
				var customRoleName = self.careMember().customRoleName();
				var errors = [];
				if( !selectedContact ){
					self.contactAlreadyAssigned( false );
				}
				
				else if( teamMembers.length ){
					//check for duplicate contact:
					var dup = ko.utils.arrayFirst( teamMembers, function(member){
						return (member.id() != self.careMember().id()) && member.statusId() == activeStatusId && 
								member.contactId() == selectedContact.id();
					});
					if( dup ){
						var roleName = dup.computedRoleName();
						if( roleName ){
							roleName = '(' + roleName + ')';
						}
						self.contactAlreadyAssigned( 'This contact is already assigned '+ roleName );	//not a validation error
						
						// errors.push(
							// { PropName: 'contact', Message: 'The contact:' + selectedContact.fullName() + ' is already assigned' });
					}
					else{						
						self.contactAlreadyAssigned( false );
					}
					
					//dont allow a patient to assigned as a member in his own team:
					var patientContactId = selectedPatient ? selectedPatient.contactId() : null;
					if( selectedContact && patientContactId && patientContactId == selectedContact.id() ){						
						errors.push(
							{ PropName: 'contact', Message: 'a patient cannot be assigned as a member of his own care team.' }
						);
					}
					
					//check if core pcp/pcm has already assigned and active:
					if( roleId && roleId != -1 && core && statusId == activeStatusId ){
						dup = ko.utils.arrayFirst( teamMembers, function(member){
							if( member.id() != self.careMember().id() &&
								member.statusId() == activeStatusId && 																	
								member.roleId() && roleId && 
								member.roleId() == roleId ){
									if (!member.customRoleName() && !customRoleName &&
										( 
											( self.pcmContactSubType() && member.roleId() == self.pcmContactSubType().id() ) || 
											( self.pcpContactSubType() && member.roleId() == self.pcpContactSubType().id() ) 
										)
									){
										return true; //this is a core active pcp / pcm that is already assigned.
									}
								}
							else{
								return false;
							}
						});
						if( dup ){
							var duplicateRoleName = 'this role';
							var toDupName = '';
							if( dup.roleId() == self.pcmContactSubType().id() ){
								duplicateRoleName = self.pcmContactSubType().role();								
							}
							else if( dup.roleId() == self.pcpContactSubType().id() ){
								duplicateRoleName = self.pcpContactSubType().role();								
							}
							if( dup.contact() && dup.contact().fullName() ){
								toDupName = ' as: ' + dup.contact().fullName();
							}
							errors.push(
									{ PropName: 'contact', Message: duplicateRoleName + ' is already assigned' + toDupName });
						}
					}
				}
				self.careMember().careTeamValidationErrors( errors );
				return false;
			}).extend({ throttle: 50 });
						
			self.computedRoles = ko.computed( function(){
				var roles = [];
				var selectedContact = self.selectedContact();
				var careMemberRoles = self.careMemberRoles();
				if( careMemberRoles && careMemberRoles.length > 0 ){
					ko.utils.arrayForEach( careMemberRoles, function( contactType ){
						roles.push( contactType );
					});
				}
			    //add the selected contact specific roles based on his contact types
				//	role is the role of the lowest contact type that exist in the contact sub type combination (sub type / specialty / sub specialty).
				//	select the first role of the selected contact as default member role.
				//
				var defaultRoleId = null;	
				if( selectedContact && selectedContact.contactSubTypes && selectedContact.contactSubTypes().length ){
					//console.log( selectedContact.fullName() + ' : ' + selectedContact.detailedSubTypes() );
					ko.utils.arrayForEach( selectedContact.contactSubTypes(), function( subType ){
						//roles are the lowest types in a type combination:
						if( subType.subSpecialtyIds().length ){
							ko.utils.arrayForEach( subType.subSpecialtyIds(), function(sub){
								var contactType = datacontext.getContactTypeLookupById( sub.id() );
								if( contactType && contactType.length > 0 ){
									roles.push( contactType[0] );
									if( !defaultRoleId ){
										defaultRoleId = sub.id();
									}
									
								}
							});							
						}
						else if( subType.specialtyId() ){							
							var specialty = datacontext.getContactTypeLookupById( subType.specialtyId() );
							if( specialty && specialty.length ){
								roles.push( specialty[0] );
								if( !defaultRoleId ){
									defaultRoleId = subType.specialtyId();
								}
							}							
						}
						else if( subType.subTypeId() ){
							var contactSubType = datacontext.getContactTypeLookupById( subType.subTypeId() );
							if( contactSubType && contactSubType.length ){
								roles.push( contactSubType[0] );
								if( !defaultRoleId ){
									defaultRoleId = subType.subTypeId();
								}
							}
						}
						
					});
				}
				if( defaultRoleId && self.careMember().isNew() ){
					//let the roles dropdown ko - dom content update before setting the default role
					setTimeout(function () {
						self.careMember().roleId( defaultRoleId );
					}, 200);	
				}
				roles.push({name: '- Other -', role: '- Other -', id: -1});
				return roles;
			}).extend({ throttle: 50 });
			
			self.showCustomRole = ko.computed( function(){
				var roleId = self.careMember().roleId();
				return roleId == -1;	// - Other - option selected
			}).extend({ throttle: 50 });
			
			self.existingNotesOpen = ko.observable(false);
			self.toggleOpen = function () {
				self.existingNotesOpen(!self.existingNotesOpen());
			};									
						
			self.createNewContact = function(){				
				//go to add contact dialog, save and come back with a contact.
				var firstName = self.contactSearch.criteriaFirstName();
				if( firstName && firstName.trim() ){
					firstName = firstName.trim();
				}
				var lastName = self.contactSearch.criteriaLastName();
				if( lastName && lastName.trim() ) {
					lastName = lastName.trim();
				}
				contactsIndex.addContact( 'CareMember', null, self.addContactReturnedCallback, firstName, lastName );
			}
			return true;
		}
		
		function detached(){
			var self = this;
            //dispose computeds
			self.validateCareMember.dispose();
			self.computedRoles.dispose();
			self.canAddContact.dispose();
			self.showCustomRole.dispose();
			
			self.contactSearch.canSearchContacts.dispose();
			self.contactSearch.contactSubTypes.dispose();
			self.contactSearch.showResetFilters.dispose();
			self.contactSearch.contactsShowingText.dispose();
			self.contactSearch.showResultsHeader.dispose();			
			
			ko.utils.arrayForEach(subscriptionTokens, function (token) {
                token.dispose();
            });
			subscriptionTokens = [];
		}
		var vm = {
			activate: activate,
			detached: detached,
			frequencies: frequencies,
			careMemberStatuses: careMemberStatuses
		}
		return vm;		
	}
);