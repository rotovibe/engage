/**
*	add /edit care member dialog module*	
*	@module careMember.edit
*/
define([ 'services/datacontext', 'services/local.collections', 'viewmodels/home/contacts/index' ],
	function( datacontext, localCollections, contactsIndex ){

		var ctor = function () {
            var self = this;					
        };
		
		var subscriptionTokens = [];		
		var newId = 0;
		
		ctor.prototype.activate = function( settings ){
			var self = this;
			self.showing = settings.showing;
			self.careMember = settings.careMember;
			// self.daysOfWeek = datacontext.enums.daysOfWeek;
			// self.timesOfDay = datacontext.enums.timesOfDay;
			// self.timeZones = datacontext.enums.timeZones;
			// self.addressTypes = datacontext.enums.addressTypes;
			// self.stateList = datacontext.enums.states;
			// self.languages = datacontext.enums.languages;
			// self.emailTypes = datacontext.enums.emailTypes;
			// self.phoneTypes = datacontext.enums.phoneTypes;
			// self.genders = datacontext.enums.genders;
			// self.deceasedStatuses = datacontext.enums.deceasedStatuses;			
			// self.contactStatuses = datacontext.enums.contactStatuses;
			
			// self.allContactTypes = contactsIndex.allContactTypes;
			
			if( self.careMember().isNew() ){
				
				//check for duplicate only when creating new:
				
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
						
			// self.contactsReturned = function( contacts ){
				// if( contacts && contacts.length > 0 ){
					// self.careMember().isDuplicate( true );					
				// }
				// self.careMember().isDuplicateTested( true );
			// }
			
			
			//self.contactTypes = contactsIndex.contactTypes;
			
			// self.contactTypes( datacontext.getContactTypes( contactTypeGroupId, 'root' ) );
									
			
			if( self.careMember().isNew() ){
				//TODO: set defaults for the added careMember
				
			}
															
			// self.forceSave = function(){
				// //save despite having dups:
				// //self.careMember().saveChanges();
				// self.showing(false);
			// }
			//self.careMember().clearDirty();
			return true;
		}
		
		ctor.prototype.detached = function(){
			var self = this;			
			// self.tabErrorsUpdater.dispose();
			// self.contactSubTypes.dispose();
			// self.contactSpecialties.dispose();
			// self.contactSubSpecialties.dispose();
			// self.canAddContactSubType.dispose();
			ko.utils.arrayForEach(subscriptionTokens, function (token) {
                token.dispose();
            });
			subscriptionTokens = [];
		}
		return ctor;		
	}
);