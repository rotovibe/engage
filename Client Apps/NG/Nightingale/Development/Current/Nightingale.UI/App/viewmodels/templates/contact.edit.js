/**
*	add /edit contact dialog module that will be used from the new home->contacts tab
*	this module composes templates/contactcard.html inside the dialog
*	@module contact.edit
*/
define([ 'services/datacontext', 'services/local.collections', 'viewmodels/home/contacts/index' ],
	function( datacontext, localCollections, contactsIndex ){

		var ctor = function () {
            var self = this;					
        };
		
		var subscriptionTokens = [];
		var contactTypeGroupId = 1;		
		var newId = 0;
		ctor.prototype.tabs = contactsIndex.tabs;
		var tabIndex = contactsIndex.tabIndex;
		
		ctor.prototype.activate = function( settings ){
			var self = this;
			self.showing = settings.showing;
			self.contactCard = settings.contactCard;
			self.daysOfWeek = datacontext.enums.daysOfWeek;
			self.timesOfDay = datacontext.enums.timesOfDay;
			self.timeZones = datacontext.enums.timeZones;
			self.addressTypes = datacontext.enums.addressTypes;
			self.stateList = datacontext.enums.states;
			self.languages = datacontext.enums.languages;
			self.emailTypes = datacontext.enums.emailTypes;
			self.phoneTypes = datacontext.enums.phoneTypes;
			self.genders = datacontext.enums.genders;
			self.deceasedStatuses = datacontext.enums.deceasedStatuses;			
			self.contactStatuses = datacontext.enums.contactStatuses;
			self.saveFunction = settings.saveFunction ? settings.saveFunction : null;
			self.allContactTypes = contactsIndex.allContactTypes;
			
			if( self.contactCard().isNew() ){
				
				//check for duplicate only when creating new:
				
				var firstNameToken = self.contactCard().firstName.subscribe( function( newValue ){
					var firstName = newValue;
					var lastName = self.contactCard().lastName();
					self.checkDuplicateContact( firstName, lastName );
				});
				subscriptionTokens.push( firstNameToken );
				
				var lastNameToken = self.contactCard().lastName.subscribe( function( newValue ){
					var lastName = newValue;				
					var firstName = self.contactCard().firstName();				
					self.checkDuplicateContact( firstName, lastName );
				});
				subscriptionTokens.push( lastNameToken );
			}
			
			
			self.checkDuplicateContact = function( firstName, lastName ){
				var contactTypeId = self.contactCard().contactTypeId();
				self.contactCard().isDuplicate( false );
				self.contactCard().isDuplicateTested(false);
				if( lastName ){
					lastName = lastName.trim();
				}
				if( firstName ){
					firstName = firstName.trim();
				}
				if( contactTypeId && firstName && lastName ){
					var params = {
						contactTypeIds: [],
						contactSubTypeIds: null,
						firstName: firstName,
						lastName: lastName,
						filterType: 'ExactMatch',
						take: 50,
						skip: 0
					};
					params.contactTypeIds.push( self.contactCard().contactTypeId() );
					return datacontext.getContacts( null, params ).then( self.contactsReturned );
				}
			};
						
			self.contactsReturned = function( contacts ){
				if( contacts && contacts.length > 0 ){
					self.contactCard().isDuplicate( true );					
				}
				self.contactCard().isDuplicateTested( true );
			}
			
			self.tabErrorsUpdater = ko.computed(function(){
				var phoneErrors = self.contactCard().phoneValidationErrors().length;				
				if( phoneErrors ){					
					self.tabs()[tabIndex.phone].hasErrors(true);
				}
				else{
					self.tabs()[tabIndex.phone].hasErrors(false);
				}
				
				var isDuplicateProfile = self.contactCard().isDuplicate();
				var profileErrors = self.contactCard().profileValidationErrors().length;
				if( profileErrors || isDuplicateProfile ){
					self.tabs()[tabIndex.profile].hasErrors(true);
				}
				else{
					self.tabs()[tabIndex.profile].hasErrors(false);
				}
			}).extend({ throttle: 100 });
			
			//contact type permutations editor:	
			self.isDuplicateSubType = ko.observable(false);
			self.selectedSubType = ko.observable();
			self.selectedSpecialty = ko.observable();
			self.selectedSubSpecialties = ko.observableArray([]);
						
			self.contactSubTypes = ko.computed( function(){
				//return children of selected type
				var subTypes = [];
				var typeId = self.contactCard().contactTypeId();
				self.selectedSubType(null);
				self.selectedSpecialty(null);
				self.selectedSubSpecialties([]);				
				if( typeId ){
					subTypes = getContactTypeChildren( typeId );										
				}
				return subTypes;
			}).extend({ throttle: 20 });
			
			self.contactSpecialties = ko.computed( function(){
				//return children of selected sub type
				var specialties = [];
				var subTypeId = self.selectedSubType() ? self.selectedSubType() : null;				
				self.selectedSpecialty(null);
				self.selectedSubSpecialties([]);
				if( subTypeId ){
					specialties = getContactTypeChildren( subTypeId );										
				}
				return specialties;
			}).extend({ throttle: 20 });
			
			self.contactSubSpecialties = ko.computed( function(){
				//return children of selected specialty
				var subSpecialties = [];
				var specialtyId = self.selectedSpecialty() ? self.selectedSpecialty() : null;				
				self.selectedSubSpecialties([]);
				if( specialtyId ){
					subSpecialties = getContactTypeChildren( specialtyId );										
				}
				return subSpecialties;
			}).extend({ throttle: 20 });
									
			self.checkDuplicateSubType = function(){
				//ENG-207: verify that the subtype combination does not exist
				//	if the combination already exist, show a message somehow and block the "Add"				
				// not tested yet:
				var sub = {					
					subTypeId: self.selectedSubType(), 
					specialtyId: self.selectedSpecialty()? self.selectedSpecialty() : null,
					subSpecialtyIds: []
				};
				//add the selected subSpecialtyIds:
				ko.utils.arrayForEach( self.selectedSubSpecialties(), function(subspecial){
					sub.subSpecialtyIds.push( subspecial );
				});
				
				var duplicates = ko.utils.arrayFilter(self.contactCard().contactSubTypes(), function(subt){
					var dup = false;
					if( sub.subTypeId == subt.subTypeId() ){
						dup = true;	//same subTypeId
						if( sub.specialtyId && subt.specialtyId() ){
							if(sub.specialtyId == subt.specialtyId() ){
								dup = true;	//same specialtyId
							}
							else{							
								dup = false;
							}
						}
						else{
							//specialtyId is not mandatory:	
							if( sub.specialtyId || subt.specialtyId() ){
								dup = false;	//one side has specialtyId, one does not.
							}
							else{
								dup = true;	//both sides dont have specialtyId
							}
						}
						if(dup){
							//verify the subSpecialty							
							if( sub.subSpecialtyIds.length == subt.subSpecialtyIds().length && subt.subSpecialtyIds().length == 0 ){
								dup = true; //no subSpecialties selected on both sides
							}
							else{
								if( sub.subSpecialtyIds.length != subt.subSpecialtyIds().length ){
									dup = false; //different lengths
								}
								else{
									ko.utils.arrayForEach( subt.subSpecialtyIds(), function (subSpecialtyId){
										if( dup ){
											var subDups = ko.utils.arrayFilter(sub.subSpecialtyIds, function(subsid){ 
												return subsid.id() == subSpecialtyId.id()
											});
											if( subDups.length == 0 ){
												dup = false;	
											}
										}
									});
								}
							}
						}						
					}
						
					return dup;
				});
				
				self.isDuplicateSubType( duplicates && duplicates.length > 0 );
				return self.isDuplicateSubType();
			}
			
			self.showInvalidSubSpecialties = ko.computed( function(){
				var subSpecialties = self.selectedSubSpecialties();
				var isDuplicateSubType = self.isDuplicateSubType();
				return isDuplicateSubType && subSpecialties && subSpecialties.length;
			});
			
			self.showInvalidSpecialty = ko.computed( function(){
				var specialty = self.selectedSpecialty();
				var isDuplicateSubType = self.isDuplicateSubType();
				return isDuplicateSubType && specialty;
			}); 
			
			self.canAddContactSubType = ko.computed( function(){
				var contactType = self.contactCard().contactTypeId();
				var selectedSubType = self.selectedSubType();
				var selectedSubSpecialties = self.selectedSubSpecialties();
				var isDuplicateSubType = self.checkDuplicateSubType();
				return contactType && selectedSubType && !isDuplicateSubType;// && !disableAddContactSubType;
			}).extend({ throttle: 50 });
			
			self.addContactSubType = function(){								
				
				//after subtype permutation validation ok:
				//create the subType combination object:
				var subType = datacontext.createComplexType( 'ContactSubType', //createEntity
				{ 
					id: --newId, 
					subTypeId: self.selectedSubType(), 
					specialtyId: self.selectedSpecialty()? self.selectedSpecialty() : null//,
				});
				//add the selected subSpecialtyIds:
				ko.utils.arrayForEach( self.selectedSubSpecialties(), function(sub){
					subType.subSpecialtyIds.push( sub );
				});
				
				//attach the subType to the collection:
				self.contactCard().contactSubTypes.push(subType);				
				
				//clear controls:
				self.selectedSubType(null);
				self.selectedSpecialty(null);
				self.selectedSubSpecialties([]);
			}
			
			self.removeContactSubType = function(subType){									
				//remove it from the contactCard
				self.contactCard().contactSubTypes.remove( subType );
			}
			self.getSubSpecialtyIds = function(){
				var ids = [];
				ko.utils.arrayForEach( self.selectedSubSpecialties(), function(item){
					ids.push( { id: item.id() });
				});
				return ids;
			}
			self.getSubSpecialties = function() {
				var subs = [];
				ko.utils.arrayForEach( self.selectedSubSpecialties(), function(item){
					subs.push( item );
				});
				return subs;
			}
			
			self.contactTypes = contactsIndex.contactTypes;
			// self.contactTypes = ko.observableArray([]);
			// self.contactTypes( datacontext.getContactTypes( contactTypeGroupId, 'root' ) );
			self.contactTypesShowing = ko.observable(true);
			// if( !defaultContactType() ){
				// defaultContactType( findDefaultContactType(self.contactTypes) )
			// }						
			
			if( self.contactCard().isNew() ){
				self.contactCard().contactTypeId( contactsIndex.defaultContactType().id() );
			}
						
			self.setActiveTab = contactsIndex.setActiveTab;
			
			if( self.contactCard() && self.contactCard().activeTab() && self.contactCard().activeTab() == 'General' ){
				
				//hide the profile tab.
				self.tabs()[tabIndex.profile].isShowing = false;
				//hide the contact type dropdown
				self.contactTypesShowing(false);	
			}else{
				self.tabs()[tabIndex.profile].isShowing = true;
				self.contactTypesShowing(true);
			}
			
			self.forceSave = function(){
				//save despite having dups:
				if( self.saveFunction ){
					self.saveFunction();
				}
				else{
					self.contactCard().saveChanges();
				}				
				self.showing(false);
			}
			self.contactCard().clearDirty();
			return true;
		}
		
		function getContactTypeChildren( typeId ){
			return contactsIndex.getContactTypeChildren( typeId );
			// var subTypes = ko.utils.arrayFilter( allContactTypes(), function(item){
				// return ( item.parentId() && item.parentId() == typeId ) 
			// });
			// return subTypes;
		}
		
		// function findDefaultContactType( contactTypes ){
			// var defaultType = null;
			// var types = ko.utils.arrayFilter( contactTypes(), function(node){
				// return node.name() == 'Person';
			// });
			// if( types && types.length > 0 ){
				// defaultType = types[0];
			// }
			// return defaultType;
		// };
		
		ctor.prototype.detached = function(){
			var self = this;			
			self.tabErrorsUpdater.dispose();
			self.contactSubTypes.dispose();
			self.contactSpecialties.dispose();
			self.contactSubSpecialties.dispose();
			self.canAddContactSubType.dispose();
			ko.utils.arrayForEach(subscriptionTokens, function (token) {
                token.dispose();
            });
			subscriptionTokens = [];
		}
		return ctor;		
	}
);