/**
*	add /edit care member dialog module*	
*	@module careMember.edit
*/
define([ 'services/datacontext', 'services/local.collections', 'viewmodels/home/contacts/index' ],
	function( datacontext, localCollections, contactsIndex ){

		var subscriptionTokens = [];		
		var newId = 0;
				
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
							firstName: self.criteriaFirstName(),
							lastName: self.criteriaLastName(),
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
			self.selectedContact = ko.observable();
			var searchSettings = {
				selectedContact: self.selectedContact
			}
			self.contactSearch = new contactSearch( searchSettings );
			self.contactSearch.init();
			self.contactSubTypes = self.contactSearch.contactSubTypes;

			if( self.careMember().isNew() ){
				
				//check for duplicats?
				    //TODO: set defaults for the added careMember

				// var firstNameToken = self.careMember().firstName.subscribe( function( newValue ){
					// var firstName = newValue;
					// var lastName = self.careMember().lastName();
					// self.checkDuplicateContact( firstName, lastName );
				// });
				// subscriptionTokens.push( firstNameToken );
				
				// var lastNameToken = self.careMember().lastName.subscribe( function( newValue ){
					// var lastName = newValue;				
					// var firstName = self.careMember().firstName();				
					// self.checkDuplicateContact( firstName, lastName );
				// });
				// subscriptionTokens.push( lastNameToken );
			}
			
			self.isDuplicateMember = ko.computed( function(){
				return false; //TODO logic
			});
			
			self.computedRoles = ko.computed( function(){
				var selectedContact = self.selectedContact();
				//var contactSubTypes = self.contactSubTypes()
				//TODO: calc roles per contact and append static roles
				
			});
			// self.checkDuplicateContact = function( firstName, lastName ){
				// var contactTypeId = self.careMember().contactTypeId();
				// self.careMember().isDuplicate( false );
				// self.careMember().isDuplicateTested(false);
				// if( lastName ){
					// lastName = lastName.trim();
				// }
				// if( firstName ){
					// firstName = firstName.trim();
				// }
				// if( contactTypeId && firstName && lastName ){
					// var params = {
						// contactTypeIds: [],
						// contactSubTypeIds: null,
						// firstName: firstName,
						// lastName: lastName,
						// filterType: 'ExactMatch',
						// take: 50,
						// skip: 0
					// };
					// params.contactTypeIds.push( self.careMember().contactTypeId() );
					// return datacontext.getContacts( null, params ).then( self.contactsReturned );
				// }
			// };
						
												
			
			return true;
		}
		
		function detached(){
			var self = this;
            //dispose computeds
			self.isDuplicateMember.dispose();
			self.computedRoles.dispose();
			
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
			detached: detached			
		}
		return vm;		
	}
);