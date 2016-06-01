
/**
*	
*	Register all of the user related models in the entity manager (initialize function) and provide other non-entity models
*	@module contactCard
*/
define(['services/session', 'services/validatorfactory', 'services/customvalidators', 'services/formatter'],
	function (session, validatorFactory, customValidators, formatter) {		

	    var datacontext;
		var DT = breeze.DataType;
		var Validator = breeze.Validator;				
		var LANGUAGE_ALREADY_EXIST = 'Language already associated';
		
		// Expose the model module to the requiring modules
		var contactModels = {
		    initialize: initialize
		};
		return contactModels;

		// Initialize the entity models in the entity manager
		function initialize(metadataStore) {

		    // Contact Card information
		    metadataStore.addEntityType({
		        shortName: "ContactCard",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            patientId: { dataType: "String" },
					isPatient: { dataType: "Boolean" },
					userId:  { dataType: "String" },
					isUser: { dataType: "Boolean" },
					firstName: { dataType: "String" },
					middleName: { dataType: "String" },
					lastName: { dataType: "String" },
					preferredName: { dataType: "String" },
					gender: { dataType: "String", defaultValue: 'N' },
		            timeZoneId: { dataType: "String" },
		            preferredTimesOfDayIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
		            preferredDaysOfWeekIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
		            languages: { complexTypeName: "ContactLanguage:#Nightingale", isScalar: false },
		            modes: { complexTypeName: "ContactMode:#Nightingale", isScalar: false },
		            emails: { complexTypeName: "Email:#Nightingale", isScalar: false },
		            phones: { complexTypeName: "Phone:#Nightingale", isScalar: false },
		            addresses: { complexTypeName: "Address:#Nightingale", isScalar: false },
					contactTypeId:  { dataType: "String" },
					contactSubTypes: { complexTypeName: "ContactSubType:#Nightingale", isScalar: false },
					externalRecordId: { dataType: "String" },
					dataSource:  { dataType: "String", defaultValue: 'Engage' },
					statusId: { dataType: "Int64", defaultValue: 1 },
					deceasedId: { dataType: "Int64", defaultValue: 2 },
					prefix: { dataType: "String" },
					suffix: { dataType: "String" },
					createdOn: { dataType: "DateTime" },
					updatedOn: { dataType: "DateTime" },
					createdById: { dataType: "String" },
					updatedById: { dataType: "String" }
		        },
		        navigationProperties: {
		            patient: {
		                entityTypeName: "Patient", isScalar: true,
		                associationName: "Patient_ContactCard", foreignKeyNames: ["patientId"]
		            },
		            timeZone: {
		                entityTypeName: "TimeZone", isScalar: true,
		                associationName: "TimeZone_ContactCards", foreignKeyNames: ["timeZoneId"]
		            },					
					contactType: {
						entityTypeName: "ContactTypeLookup", isScalar: true,
						associationName: "ContactTypeLookup_ContactCard", foreignKeyNames: ["contactTypeId"]
					},
					contactStatus: {
						entityTypeName: "ContactStatus", isScalar: true,
						associationName: "Contact_ContactStatus", foreignKeyNames: ["statusId"]
					},
					deceased: {
						entityTypeName: "Deceased", isScalar: true,
						associationName: "Contact_Deceased", foreignKeyNames: ["deceasedId"]
					},
					createdBy: {
						entityTypeName: "CareManager", isScalar: true,
						associationName: "ContactCard_CreatedBy", foreignKeyNames: ["createdById"]
					},
					updatedBy: {
						entityTypeName: "CareManager", isScalar: true,
						associationName: "ContactCard_UpdatedBy", foreignKeyNames: ["updatedById"]
					},
		        }
		    });

			metadataStore.addEntityType({
		        shortName: "ContactSearch",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            patientId: { dataType: "String" },
					isPatient: { dataType: "Boolean" },
					userId:  { dataType: "String" },
					isUser: { dataType: "Boolean" },
					firstName: { dataType: "String" },
					middleName: { dataType: "String" },
					lastName: { dataType: "String" },
					preferredName: { dataType: "String" },
					gender: { dataType: "String", defaultValue: 'N' },
		            timeZoneId: { dataType: "String" },
		            preferredTimesOfDayIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
		            preferredDaysOfWeekIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
		            languages: { complexTypeName: "ContactLanguage:#Nightingale", isScalar: false },
		            modes: { complexTypeName: "ContactMode:#Nightingale", isScalar: false },
		            emails: { complexTypeName: "Email:#Nightingale", isScalar: false },
		            phones: { complexTypeName: "Phone:#Nightingale", isScalar: false },
		            addresses: { complexTypeName: "Address:#Nightingale", isScalar: false },
					contactTypeId:  { dataType: "String" },
					contactSubTypes: { complexTypeName: "ContactSubType:#Nightingale", isScalar: false },
					externalRecordId: { dataType: "String" },
					dataSource:  { dataType: "String", defaultValue: 'Engage' },
					statusId: { dataType: "Int64", defaultValue: 1 },
					deceasedId: { dataType: "Int64", defaultValue: 2 },
					prefix: { dataType: "String" },
					suffix: { dataType: "String" },
					createdOn: { dataType: "DateTime" },
					updatedOn: { dataType: "DateTime" },
					createdById: { dataType: "String" },
					updatedById: { dataType: "String" }
		        },
		        navigationProperties: {
		            patient: {
		                entityTypeName: "Patient", isScalar: true,
		                associationName: "Patient_ContactCard", foreignKeyNames: ["patientId"]
		            },
		            timeZone: {
		                entityTypeName: "TimeZone", isScalar: true,
		                associationName: "TimeZone_ContactCards", foreignKeyNames: ["timeZoneId"]
		            },					
					contactType: {
						entityTypeName: "ContactTypeLookup", isScalar: true,
						associationName: "ContactTypeLookup_ContactCard", foreignKeyNames: ["contactTypeId"]
					},
					contactStatus: {
						entityTypeName: "ContactStatus", isScalar: true,
						associationName: "Contact_ContactStatus", foreignKeyNames: ["statusId"]
					},
					deceased: {
						entityTypeName: "Deceased", isScalar: true,
						associationName: "Contact_Deceased", foreignKeyNames: ["deceasedId"]
					},
					createdBy: {
						entityTypeName: "CareManager", isScalar: true,
						associationName: "ContactCard_CreatedBy", foreignKeyNames: ["createdById"]
					},
					updatedBy: {
						entityTypeName: "CareManager", isScalar: true,
						associationName: "ContactCard_UpdatedBy", foreignKeyNames: ["updatedById"]
					},
		        }
		    });

			metadataStore.addEntityType({
		        shortName: "ContactCarememberSearch",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            patientId: { dataType: "String" },
					isPatient: { dataType: "Boolean" },
					userId:  { dataType: "String" },
					isUser: { dataType: "Boolean" },
					firstName: { dataType: "String" },
					middleName: { dataType: "String" },
					lastName: { dataType: "String" },
					preferredName: { dataType: "String" },
					gender: { dataType: "String", defaultValue: 'N' },
		            timeZoneId: { dataType: "String" },
		            preferredTimesOfDayIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
		            preferredDaysOfWeekIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
		            languages: { complexTypeName: "ContactLanguage:#Nightingale", isScalar: false },
		            modes: { complexTypeName: "ContactMode:#Nightingale", isScalar: false },
		            emails: { complexTypeName: "Email:#Nightingale", isScalar: false },
		            phones: { complexTypeName: "Phone:#Nightingale", isScalar: false },
		            addresses: { complexTypeName: "Address:#Nightingale", isScalar: false },
					contactTypeId:  { dataType: "String" },
					contactSubTypes: { complexTypeName: "ContactSubType:#Nightingale", isScalar: false },
					externalRecordId: { dataType: "String" },
					dataSource:  { dataType: "String", defaultValue: 'Engage' },
					statusId: { dataType: "Int64", defaultValue: 1 },
					deceasedId: { dataType: "Int64", defaultValue: 2 },
					prefix: { dataType: "String" },
					suffix: { dataType: "String" },
					createdOn: { dataType: "DateTime" },
					updatedOn: { dataType: "DateTime" },
					createdById: { dataType: "String" },
					updatedById: { dataType: "String" }
		        },
		        navigationProperties: {
		            patient: {
		                entityTypeName: "Patient", isScalar: true,
		                associationName: "Patient_ContactCard", foreignKeyNames: ["patientId"]
		            },
		            timeZone: {
		                entityTypeName: "TimeZone", isScalar: true,
		                associationName: "TimeZone_ContactCards", foreignKeyNames: ["timeZoneId"]
		            },					
					contactType: {
						entityTypeName: "ContactTypeLookup", isScalar: true,
						associationName: "ContactTypeLookup_ContactCard", foreignKeyNames: ["contactTypeId"]
					},
					contactStatus: {
						entityTypeName: "ContactStatus", isScalar: true,
						associationName: "Contact_ContactStatus", foreignKeyNames: ["statusId"]
					},
					deceased: {
						entityTypeName: "Deceased", isScalar: true,
						associationName: "Contact_Deceased", foreignKeyNames: ["deceasedId"]
					},
					createdBy: {
						entityTypeName: "CareManager", isScalar: true,
						associationName: "ContactCard_CreatedBy", foreignKeyNames: ["createdById"]
					},
					updatedBy: {
						entityTypeName: "CareManager", isScalar: true,
						associationName: "ContactCard_UpdatedBy", foreignKeyNames: ["updatedById"]
					},
		        }
		    });
			
		    // Phone complex type
		    metadataStore.addEntityType({
		        shortName: "Phone",
		        namespace: "Nightingale",
		        isComplexType: true,
		        dataProperties: {
		            id: { dataType: "String" },
		            number: { dataType: "String" },
		            typeId: { dataType: "String" },
		            optOut: { dataType: "Boolean" },
		            isText: { dataType: "Boolean" },
		            phonePreferred: { dataType: "Boolean" },
		            textPreferred: { dataType: "Boolean" },
					dataSource: { dataType: "String"}					
		        }
		    });			
			
		    // Email complex type
		    metadataStore.addEntityType({
		        shortName: "Email",
		        namespace: "Nightingale",
		        isComplexType: true,
		        dataProperties: {
		            id: { dataType: "String" },
		            text: { dataType: "String" },
		            typeId: { dataType: "String" },
		            optOut: { dataType: "Boolean" },
		            preferred: { dataType: "Boolean" }
		        }
		    });

		    // Contact Language complex type for a patient
		    metadataStore.addEntityType({
		        shortName: "ContactLanguage",
		        namespace: "Nightingale",
		        isComplexType: true,
		        dataProperties: {
		            lookUpLanguageId: { dataType: "String" },
		            preferred: { dataType: "Boolean" }
		        }
		    });

		    // Contact Mode complex type for a patient
		    metadataStore.addEntityType({
		        shortName: "ContactMode",
		        namespace: "Nightingale",
		        isComplexType: true,
		        dataProperties: {
		            lookUpModeId: { dataType: "String" },
		            optOut: { dataType: "Boolean" },
		            preferred: { dataType: "Boolean" }
		        }
		    });

		    // Address
		    metadataStore.addEntityType({
		        shortName: "Address",
		        namespace: "Nightingale",
		        isComplexType: true,
		        dataProperties: {
		            id: { dataType: "String" },
		            line1: { dataType: "String" },
		            line2: { dataType: "String" },
		            line3: { dataType: "String" },
		            city: { dataType: "String" },
		            stateId: { dataType: "String" },
		            postalCode: { dataType: "String" },
		            typeId: { dataType: "String" },
		            optOut: { dataType: "Boolean" },
		            preferred: { dataType: "Boolean" }
		        }
		    });

			metadataStore.addEntityType({
		        shortName: "ContactTypeLookup",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
					parentId: { dataType: "String" },
		            name: { dataType: "String" },
					role: { dataType: "String" },
					group: { dataType: "String" }
		        }
		    });
			
			metadataStore.addEntityType({
		        shortName: "ContactSubType",
		        namespace: "Nightingale",
		        isComplexType: true,
		        dataProperties: {
		            id: { dataType: "String"  },//, isPartOfKey: true
					subTypeId:  { dataType: "String" },
		            specialtyId: { dataType: "String" },
		            subSpecialtyIds:  { complexTypeName: "Identifier:#Nightingale", isScalar: false }
		        }
		    });

		    metadataStore.registerEntityTypeCtor(
				'ContactCard', null, contactCardInitializer);
			metadataStore.registerEntityTypeCtor(
				'ContactSearch', null, contactCardInitializer);					
			metadataStore.registerEntityTypeCtor(
				'ContactCarememberSearch', null, contactCardInitializer);
		    metadataStore.registerEntityTypeCtor(
				'ContactLanguage', null, contactLanguageInitializer);
		    metadataStore.registerEntityTypeCtor(
				'ContactMode', null, contactModeInitializer);
		    metadataStore.registerEntityTypeCtor(
				'Address', null, addressInitializer);
		    metadataStore.registerEntityTypeCtor(
				'Phone', null, phoneInitializer);
		    metadataStore.registerEntityTypeCtor(
				'Email', null, emailInitializer);
			metadataStore.registerEntityTypeCtor(
				'ContactSubType', null, contactSubTypeInitializer);
			
			function contactSubTypeInitializer(contactSubType) {
				contactSubType.isNew = ko.observable(false);
				contactSubType.subTypeName = ko.computed( function() {
					checkDataContext();
					var name = null;
					var subTypeId = contactSubType.subTypeId();
					if( subTypeId ){
						var subType = datacontext.getContactTypeLookupById( subTypeId );
						if( subType && subType.length ){
							name = subType[0].name();
						}
					}
					return name;
				}).extend({ throttle: 50 });
				contactSubType.specialtyName = ko.computed( function() {
					checkDataContext();
					var name = null;
					var specialtyId = contactSubType.specialtyId();
					if( specialtyId ){
						var specialty = datacontext.getContactTypeLookupById( specialtyId );
						if( specialty && specialty.length ){
							name = specialty[0].name();
						}
					}
					return name;
				}).extend({ throttle: 50 });
				contactSubType.subSpecialtyString = ko.computed(function(){
					checkDataContext();
					var name = null;
					var subSpecialtyIds = contactSubType.subSpecialtyIds();
					if( subSpecialtyIds.length ){
						ko.utils.arrayForEach( subSpecialtyIds, function(sub){
							var contactType = datacontext.getContactTypeLookupById( sub.id() );
							if( contactType && contactType.length > 0 ){
								if( name && name.length ){
									name += ', ';
								}
								else{
									name = '';
								}
								name += contactType[0].name();
							}
						});
					}
					return name;
				}).extend({ throttle: 50 });
			}
			
		    function contactCardInitializer(contactCard) {				
				contactCard.isNew = ko.observable(false);				
		        contactCard.activeTab = ko.observable('General');				
		        contactCard.prefCommMethods = ko.computed(function () {
		            checkDataContext();
		            var commModeString = '';
		            // Go through the language ids for this person,
		            ko.utils.arrayForEach(contactCard.modes(), function (thisMode) {
		                if (thisMode.optOut()) {
		                    thisMode.preferred(false);
		                }
		                // If the mode is preferred,
		                if (thisMode.preferred()) {
		                    // And find the mode name
		                    var preferredMode = ko.utils.arrayFirst(datacontext.enums.communicationModes(), function (mode) {
		                        return mode.id() === thisMode.lookUpModeId();
		                    });
		                    // Add it to the string of preferred modes
		                    commModeString += preferredMode.name() + ', ';
		                }
		            });
		            // If the string is longer than zero,
		            if (commModeString.length > 0) {
		                // Remove the trailing comma and space
		                commModeString = commModeString.substr(0, commModeString.length - 2);
		            }
		            // Return the list of preferred communications
		            return commModeString;
		        });
		        contactCard.secondaryLanguagesText = ko.computed(function () {
		            checkDataContext();
		            var languageString = '';
		            // Go through the language ids for this person,
		            ko.utils.arrayForEach(contactCard.languages(), function (contactLanguage) {
		                // If the language is not the preferred,
		                if (!contactLanguage.preferred()) {
		                    // And find the language name
		                    var thisLanguage = ko.utils.arrayFirst(datacontext.enums.languages(), function (language) {
		                        return language.id() === contactLanguage.lookUpLanguageId();
		                    });
		                    languageString += thisLanguage.name() + ', ';
		                }
		            });
		            // If the string is longer than zero,
		            if (languageString.length > 0) {
		                // Remove the trailing comma and space
		                languageString = languageString.substr(0, languageString.length - 2);
		            }
		            // Return the list of preferred communications
		            return languageString;
		        });
		        contactCard.preferredTimesOfDay = ko.computed(function () {
		            checkDataContext();
		            var timeOfDayString = '';
		            // Go through the language ids for this person,
		            ko.utils.arrayForEach(contactCard.preferredTimesOfDayIds(), function (todId) {
		                // And find the language name
		                var thisTimeOfDay = ko.utils.arrayFirst(datacontext.enums.timesOfDay(), function (tod) {
		                    return tod.id() === todId.id();
		                });
		                timeOfDayString += thisTimeOfDay.name() + ', ';
		            });
		            // If the string is longer than zero,
		            if (timeOfDayString.length > 0) {
		                // Remove the trailing comma and space
		                timeOfDayString = timeOfDayString.substr(0, timeOfDayString.length - 2);
		            }
		            // Return the list of preferred communications
		            return timeOfDayString;
		        });
		        contactCard.preferredDaysOfWeek = ko.computed(function () {
		            checkDataContext();
		            // Create a string to hold the days of the week
		            var daysOfWeekString = '';
		            // If all days are selected,
		            if (contactCard.preferredDaysOfWeekIds().length === 7) {
		                daysOfWeekString = 'Any';
		            }
		            // If not,
		            else {
		                // Go through the language ids for this person,
		                ko.utils.arrayForEach(contactCard.preferredDaysOfWeekIds(), function (dowId) {
		                    // And find the language name
		                    var thisDayOfWeek = ko.utils.arrayFirst(datacontext.enums.daysOfWeek(), function (dow) {
		                        // And return the day of the week that matches
		                        return dow.Id === dowId.id();
		                    });
		                    // Add the code to the day of the week's string
		                    daysOfWeekString += thisDayOfWeek.Code + ', ';
		                });
		                // If the string is longer than zero,
		                if (daysOfWeekString.length > 0) {
		                    // Remove the trailing comma and space
		                    daysOfWeekString = daysOfWeekString.substr(0, daysOfWeekString.length - 2);
		                }
		            }
		            // Return the list of preferred communications
		            return daysOfWeekString;
		        });
		        contactCard.addPhone = function () {
		            // Add a phone here without properties
		            checkDataContext();
		            // Find the matching phone mode
		            var thismode = ko.utils.arrayFirst(contactCard.modes(), function (mode) {
		                return (mode && mode.name() === 'Phone');
		            });
		            // If the phone mode is not opted out,
		            if (thismode && !thismode.optOut()) {
		            	// Add a new phone record
			            var nextId = ((contactCard.phones().length + 1) * -1);
			            var defaultTypeId = datacontext.enums.phoneTypes()[0].id();
			            var newPhone = datacontext.createComplexType('Phone', { id: nextId, typeId: defaultTypeId, dataSource: "Engage" });
						newPhone.isFocused(true);
			            contactCard.phones.push(newPhone);		            	
		            }					
		        }
		        contactCard.removePhone = function (phone) {
		            contactCard.phones.remove(phone);
		        }
		        contactCard.addEmail = function () {
		            // Add a phone here without properties
		            checkDataContext();
		            // Find the matching email mode
		            var thismode = ko.utils.arrayFirst(contactCard.modes(), function (mode) {
		                return (mode && mode.name() === 'Email');
		            });
		            // If the email mode is not opted out,
		            if (thismode && !thismode.optOut()) {
		            	// Add a new email record
			            var nextId = contactCard.emails().length + 1;
			            var defaultTypeId = datacontext.enums.emailTypes()[0].id();
			            var newEmail = datacontext.createComplexType('Email', { id: nextId, typeId: defaultTypeId });
			            contactCard.emails.push(newEmail);
		            }
		        }
		        contactCard.removeEmail = function (email) {
		            contactCard.emails.remove(email);
		        }
		        contactCard.addAddress = function () {
		            // Add a phone here without properties
		            checkDataContext();
		            // Find the matching address mode
		            var thismode = ko.utils.arrayFirst(contactCard.modes(), function (mode) {
		                return (mode && mode.name() === 'Mail');
		            });
		            // If the address mode is not opted out,
		            if (thismode && !thismode.optOut()) {
		            	// Add a new address record
			            var nextId = contactCard.addresses().length + 1;
			            var defaultTypeId = datacontext.enums.addressTypes()[0].id();
			            var newAddress = datacontext.createComplexType('Address', { id: nextId, typeId: defaultTypeId });
			            contactCard.addresses.push(newAddress);
			        }
		        }
		        contactCard.removeAddress = function (address) {
		            contactCard.addresses.remove(address);
		        }
		        contactCard.newLanguage = ko.observable(null);
				contactCard.languageValidationErrors = ko.observableArray([]);
				contactCard.canAddLanguage = ko.computed( function(){
					var newLanguage = contactCard.newLanguage();
					var languages = contactCard.languages();
					var alreadyExist = false;
					var errors = [];
					if( newLanguage ){
						ko.utils.arrayForEach( languages, function( language ){
							if( language.name() === newLanguage.name() ){
								alreadyExist = true;
								errors.push({Message: LANGUAGE_ALREADY_EXIST});
							}
						});						
					}
					contactCard.languageValidationErrors(errors);
					return ( newLanguage && !alreadyExist );
				});
		        contactCard.addLanguage = function () {
					if( !contactCard.canAddLanguage() ) return;
		            // Add a language here
		            checkDataContext();
		            var thisLanguage = contactCard.newLanguage();
		            var thisNewLanguage = datacontext.createComplexType('ContactLanguage', { lookUpLanguageId: thisLanguage.id(), name: thisLanguage.name() });
		            contactCard.languages.push(thisNewLanguage);
		            contactCard.newLanguage(null);
		        }
		        contactCard.removeLanguage = function (language) {
		            contactCard.languages.remove(language);
		        }
		        //contactCard.phoneOptedOut = ko.observable(false);
		        contactCard.preferredPhone = ko.computed({
		            read: function () {
		                // Go through each of the phones
		                var thisPhone;
		                ko.utils.arrayForEach(contactCard.phones(), function (phone) {
		                    // If the phone is preferred, and not opted out,
		                    if (phone.phonePreferred() && !phone.optOut()) {
		                        // Return it
		                        thisPhone = phone;
		                    }
		                    if (phone.optOut()) {
		                        phone.phonePreferred(false);
		                    }
		                });
		                if (thisPhone) {
		                    return thisPhone;
		                }
		                // If no phones are found that are preferred, return null
		                return null;
		            },
		            write: function (newValue) {
		                // If the newValue is not null,
		                if (newValue) {
		                    // Set it's preferred property to true
		                    newValue.phonePreferred(true);
		                }
		                // Set all of the other phones to false
		                ko.utils.arrayForEach(contactCard.phones(), function (phone) {
		                    // As long as it is not the one we just set to true
		                    if (phone !== newValue) {
		                        phone.phonePreferred(false);
		                    }
		                });
		            }
		        }).extend({ throttle: 50 });
		        contactCard.preferredText = ko.computed({
		            read: function () {
		                // Go through each of the phones
		                var thisPhone;
		                ko.utils.arrayForEach(contactCard.phones(), function (phone) {
		                    // If the phone is preferred,
		                    if (phone.textPreferred() && !phone.optOut()) {
		                        // Return it
		                        thisPhone = phone;
		                    }
		                    if (phone.optOut()) {
		                        phone.textPreferred(false);
		                    }
		                });
		                if (thisPhone) {
		                    return thisPhone;
		                }
		                // If no phones are found that are preferred, return null
		                return null;
		            },
		            write: function (newValue) {
		                // If the newValue is not null,
		                if (newValue) {
		                    // Set it's preferred property to true
		                    newValue.textPreferred(true);
		                }
		                // Set all of the other phones to false
		                ko.utils.arrayForEach(contactCard.phones(), function (phone) {
		                    // As long as it is not the one we just set to true
		                    if (phone !== newValue) {
		                        phone.textPreferred(false);
		                    }
		                });
		            }
		        }).extend({ throttle: 50 });
		        contactCard.preferredEmail = ko.computed({
		            read: function () {
		                // Go through each of the emails
		                var thisEmail;
		                ko.utils.arrayForEach(contactCard.emails(), function (email) {
		                    // If the phone is preferred, and not opted out
		                    if (email.preferred() && !email.optOut()) {
		                        // Return it
		                        thisEmail = email;
		                    }
		                    if (email.optOut()) {
		                        email.preferred(false);
		                    }
		                });
		                if (thisEmail) {
		                    return thisEmail;
		                }
		                // If no phones are found that are preferred, return null
		                return null;
		            },
		            write: function (newValue) {
		                // If the newValue is not null,
		                if (newValue) {
		                    // Set it's preferred property to true
		                    newValue.preferred(true);
		                }
		                // Set all of the other phones to false
		                ko.utils.arrayForEach(contactCard.emails(), function (email) {
		                    // As long as it is not the one we just set to true
		                    if (email !== newValue) {
		                        email.preferred(false);
		                    }
		                });
		            }
		        }).extend({ throttle: 50 });
		        contactCard.preferredAddress = ko.computed({
		            read: function () {
		                // Go through each of the addresses
		                var thisAddress;
		                ko.utils.arrayForEach(contactCard.addresses(), function (address) {
		                    // If the address is preferred, and not opted out
		                    if (address.preferred() && !address.optOut()) {
		                        // Return it
		                        thisAddress = address;
		                    }
		                    if (address.optOut()) {
		                        address.preferred(false);
		                    }
		                });
		                if (thisAddress) {
		                    return thisAddress;
		                }
		                // If no addresses are found that are preferred, return null
		                return null;
		            },
		            write: function (newValue) {
		                // If the newValue is not null,
		                if (newValue) {
		                    // Set it's preferred property to true
		                    newValue.preferred(true);
		                }
		                // Set all of the other phones to false
		                ko.utils.arrayForEach(contactCard.addresses(), function (address) {
		                    // As long as it is not the one we just set to true
		                    if (address !== newValue) {
		                        address.preferred(false);
		                    }
		                });
		            }
		        }).extend({ throttle: 50 });

				contactCard.fullName = ko.computed( function(){
					var fullName = '';
					var prefix = contactCard.prefix();
					var suffix = contactCard.suffix();
					var firstName = contactCard.firstName();
					var lastName = contactCard.lastName();
					var middleName = contactCard.middleName();
					
					if( prefix ){
						fullName += prefix + ' ' ;
					}
					if( firstName ){						
						fullName += firstName + ' ';
					}
					if( middleName ){						
						fullName += middleName + ' ';
					}
					if( lastName ){						
						fullName += lastName + ' ';
					}					
					if( suffix ){						
						fullName += suffix;
					}
					fullName = fullName.trim();
					return fullName;
				}).extend({ throttle: 50 });
				
				contactCard.genderModel = ko.computed({
					read: function () {
						checkDataContext();
						var thisGender;
						var gender = contactCard.gender()? contactCard.gender().toLowerCase() : '';
						if (gender === 'm' || gender === 'male') {
							contactCard.gender('M');
							thisGender = ko.utils.arrayFirst(datacontext.enums.genders(), function (item) {
								return 'm' === item.Id;
							});
						}
						else if (gender === 'f' || gender === 'female') {
							contactCard.gender('F');
							thisGender = ko.utils.arrayFirst(datacontext.enums.genders(), function (item) {
								return 'f' === item.Id;
							});
						}
						else {
							contactCard.gender('N');
							thisGender = ko.utils.arrayFirst(datacontext.enums.genders(), function (item) {
								return 'n' === item.Id;
							});
						}
						return thisGender;
					},
					write: function (newValue) {
						contactCard.gender(ko.unwrap(newValue).Id.toUpperCase());
					}
				}).extend({ throttle: 50 });
				
				contactCard.firstLastOrPreferredName = ko.computed( function(){
					var preferred = contactCard.preferredName();
					var firstName = contactCard.firstName();
					if( !firstName ) {
						firstName = '';
					}
					var lastName = contactCard.lastName();
					if( !lastName ) {
						lastName = '';
					}
					return preferred? preferred : (firstName + ' ' + lastName);
				}).extend({throttle: 100});
				
				function getDetailedSubTypeText(subType){
					//subtype, specialty and sub specialty
					var subTypeText = '';							
					subTypeText += subType.subTypeName();
					if( subType.specialtyId() ){
						subTypeText += ' - ' + subType.specialtyName();								
						if(subType.subSpecialtyString()){
							subTypeText += ' (' + subType.subSpecialtyString() + ')';
						}								
					}
					return subTypeText;
				}
				
				contactCard.detailedSubTypes = ko.computed( function(){
					var subTypeStrings = [];
					if( contactCard.contactSubTypes && contactCard.contactSubTypes().length ){						
						
						ko.utils.arrayForEach( contactCard.contactSubTypes(), function( subType ){
							var subTypeText = getDetailedSubTypeText(subType);							
							subTypeStrings.push( subTypeText );
						});
						
					}
					return subTypeStrings;
				}).extend({ throttle: 50 });
				
				function getSubTypeSummaryText(subType){
					//subtype and specialty text only
					var text = subType.subTypeName();
					if( subType.specialtyId() ){
						text += ' - ' + subType.specialtyName();
					}
					return text;
				}
				
				contactCard.contactSummary = ko.computed( function(){
					var summary = '';					
					if( contactCard.contactSubTypes && contactCard.contactSubTypes().length ){						
						var subTypesText = '';
						ko.utils.arrayForEach( contactCard.contactSubTypes(), function( subType ){
							if( subTypesText.length ) {
								subTypesText += ', ';
							}
							subTypesText += getSubTypeSummaryText(subType);
						});
						summary += subTypesText;
					}
					if( contactCard.preferredPhone() ){
						var phoneNumber = contactCard.preferredPhone().number();
						phoneNumber = phoneNumber.replace( /\D/g, '');
						formattedPhone = formatter.formatSeparators( phoneNumber, 'XXX-XXX-XXXX', '-');
						if( summary.length ){
							summary += ', ';
						}
						summary += formattedPhone;
					}					
					if( contactCard.preferredAddress() ){
						if( summary.length ){
							summary += ', ';
						}
						summary += contactCard.preferredAddress().cityState();
					}
					return summary;
				}).extend({ throttle: 50 });
				
		        contactCard.preferredLanguage = ko.computed({
		            read: function () {
		                // Go through each of the languages
		                var thisLanguage;
		                ko.utils.arrayForEach(contactCard.languages(), function (language) {
		                    // If the language is preferred,
		                    if (language.preferred()) {
		                        // Return it
		                        thisLanguage = language;
		                    }
		                });
		                if (thisLanguage) {
		                    return thisLanguage;
		                }
		                // If no languages are found that are preferred, return null
		                return null;
		            },
		            write: function (newValue) {
		                // If the newValue is not null,
		                if (newValue) {
		                    // Set it's preferred property to true
		                    newValue.preferred(true);
		                }
		                // Set all of the other phones to false
		                ko.utils.arrayForEach(contactCard.languages(), function (language) {
		                    // As long as it is not the one we just set to true
		                    if (language !== newValue) {
		                        language.preferred(false);
		                    }
		                });
		            }
		        }).extend({ throttle: 50 });

		        contactCard.phoneOptedOut = ko.computed(function () {
		            var returnValue = false;
		            ko.utils.arrayForEach(contactCard.modes(), function (mode) {
		                if (mode && mode.name() === 'Phone') {
		                    var newValue = mode.optOut();
		                    if (newValue) {
		                        optOutOfPhones(contactCard);
		                    }
		                    returnValue = newValue;
		                }
		            });
		            return returnValue;
		        });
		        contactCard.textOptedOut = ko.computed(function () {
		            var returnValue = false;
		            ko.utils.arrayForEach(contactCard.modes(), function (mode) {
		                if (mode && mode.name() === 'Text') {
		                    var newValue = mode.optOut();
		                    if (newValue) {
		                        optOutOfTexts(contactCard);
		                    }
		                    returnValue = newValue;
		                }
		            });
		            return returnValue;
		        });
		        contactCard.addressOptedOut = ko.computed(function () {
		            var returnValue = false;
		            ko.utils.arrayForEach(contactCard.modes(), function (mode) {
		                if (mode && mode.name() === 'Mail') {
		                    var newValue = mode.optOut();
		                    if (newValue) {
		                        optOutOfAddresses(contactCard);
		                    }
		                    returnValue = newValue;
		                }
		            });
		            return returnValue;
		        });
		        contactCard.emailOptedOut = ko.computed(function () {
		            var returnValue = false;
		            ko.utils.arrayForEach(contactCard.modes(), function (mode) {
		                if (mode && mode.name() === 'Email') {
		                    var newValue = mode.optOut();
		                    if (newValue) {
		                        optOutOfEmails(contactCard);
		                    }
		                    returnValue = newValue;
		                }
		            });
		            return returnValue;
		        });
				
				if( contactCard.isPatient() && contactCard.patient() ){
					//sync dup properties from patient: (TBD - this may not be necessary later on)
					if( !contactCard.firstName() ){
						contactCard.firstName( contactCard.patient().firstName() );
					}
					if( !contactCard.lastName() ){
						contactCard.lastName( contactCard.patient().lastName() );
					}
				}
				
				//validation:				
				contactCard.phoneValidationErrors = ko.observableArray([]);								
								
				contactCard.profileValidationErrors = ko.observableArray([]);
				contactCard.profileValidationErrorsArray = ko.computed(function () {
			        var thisArray = [];
			        ko.utils.arrayForEach(contactCard.profileValidationErrors(), function (error) {
			            thisArray.push(error.PropName);
			        });
			        return thisArray;
			    });
				
				contactCard.isDirty = ko.observable(false);
                contactCard.clearDirty = function(){
                    contactCard.isDirty(false);
                };
                contactCard.watchDirty = function () {
                    var propToken = contactCard.entityAspect.propertyChanged.subscribe(function (newValue) {
                        contactCard.isDirty(true);
                        propToken.dispose();
                    });
                    var subTypesToken = contactCard.contactSubTypes.subscribe(function (newValue) {
                        contactCard.isDirty(true);
                        subTypesToken.dispose();
                    });
                };
				
				/**
				*	validate the profile tab part of a contact card
				*	@method hasProfileErrors
				*/
				function hasProfileErrors(){
					var profileErrors = [];
					var errorsFound = false;
					var firstName = contactCard.firstName();
					var lastName = contactCard.lastName();
					//var hasChanges = contactCard.isDirty();
					var isPatient = contactCard.isPatient();
					if( !firstName || !firstName.trim().length ){
							profileErrors.push({ PropName: 'firstName', Message: "'First Name' is required"});							
							errorsFound = true;
					}
					if( !lastName || !lastName.trim().length ){
							profileErrors.push({ PropName: 'lastName', Message: "'Last Name' is required"});
							errorsFound = true;
					}
					contactCard.profileValidationErrors(profileErrors);
					return errorsFound;
				}
				/**
				*	validates all phones in the contact card phones collection.					
				*	@method hasPhoneErrors
				*/
				function hasPhoneErrors(){
					var phoneErrors = [];
					var errorsFound = false;
					ko.utils.arrayForEach( contactCard.phones(), function(phone){
						phone.validate();
						var isValid = phone.isValid();
						if( !isValid ){
							phoneErrors.push({Message: phone.validationMessage()});
							errorsFound = true;
						}
					});
					contactCard.phoneValidationErrors(phoneErrors);	
					return errorsFound;
				}
				
			    contactCard.phoneValidationErrorsArray = ko.computed(function () {
			        var thisArray = [];
			        ko.utils.arrayForEach(contactCard.phoneValidationErrors(), function (error) {
			            thisArray.push(error.PropName + error.Value);
			        });
			        return thisArray;
			    }).extend({ throttle: 50 });
				
				contactCard.isDuplicate = ko.observable(false);
				contactCard.isDuplicateTested = ko.observable(true);
				
				/**
				*	computed. tracks for any validation errors on all tabs of the contact card.
				*	@method isValid 
				*/
				contactCard.isValid = ko.computed(function(){															
					var profileErrors = hasProfileErrors();
					var phoneErrors = hasPhoneErrors();					
					return !phoneErrors && !profileErrors;
				}).extend({ throttle: 50 });							
					
				contactCard.isEditable = ko.computed( function(){
					return contactCard.dataSource() == 'Engage';
				}).extend({ throttle: 50 });
				
		        // Can the contact card save?  Fake validation goes here
		        contactCard.canSave = ko.computed(function () {
		            return contactCard.isValid();
		        }).extend({ throttle: 50 });
		        // Method to save changes to the patient
		        contactCard.saveChanges = function () {
		            checkDataContext();
		            // Go save the entity, pass in which parameters should be different
		            return datacontext.saveContactCard(contactCard);
		        }
		        // Method on the modal to cancel changes to the patient
		        contactCard.cancelChanges = function () {
		            checkDataContext();
		            datacontext.cancelAllChangesToContactCard(contactCard);
		        }
		    }

		    function contactLanguageInitializer(contactLanguage) {
		        checkDataContext();
		        contactLanguage.name = ko.computed(function () {
		            var thisLanguage = ko.utils.arrayFirst(datacontext.enums.languages(), function (language) {
		                return language.id() === contactLanguage.lookUpLanguageId();
		            });
		            if (thisLanguage) {
		                return thisLanguage.name();
		            }
		        });
		    }

		    function contactModeInitializer(contactMode) {
		        checkDataContext();
		        contactMode.name = ko.computed(function () {
		            var thisMode = ko.utils.arrayFirst(datacontext.enums.communicationModes(), function (mode) {
		                return mode.id() === contactMode.lookUpModeId();
		            });
		            if (thisMode) {
		                return thisMode.name();
		            }
		        });
		    }

		    function phoneInitializer(phone) {
		        phone.type = ko.computed(function () {
		            checkDataContext();
		            var thisTypeId = phone.typeId();
		            var thisType = null;
		            // If there is a type id on the phone,
		            if (thisTypeId) {
		                // Get the type
		                thisType = ko.utils.arrayFirst(datacontext.enums.phoneTypes(), function (item) {
		                    return thisTypeId === item.id();
		                });
		            }
		            return thisType;
		        });
		        phone.formattedNumber = ko.computed(function () {
		            var thisPhoneNumber = phone.number();
		            if (thisPhoneNumber) {
		                thisPhoneNumber = thisPhoneNumber.toString();
		                return thisPhoneNumber.replace(/(\d{3})-?(\d{3})-?(\d{4})/, '($1) $2-$3');
		            }
		            return null;
		        });
				phone.isValid = ko.observable(true);
				phone.validationMessage = ko.observable();
				
				/**
				*	validate a phone number. include generating error message into phone.validationMessage.
				*	( note that this works better than breeze custom validation ).
				*	@method phone.validate
				*/
				phone.validate = function(){
					if( phone.number() && phone.number().match(/^\d{3}-?\d{3}-?\d{4}$/) ){
						phone.isValid(true);
						phone.validationMessage(null);
						return true;
					}
					else{
						phone.isValid(false);
						var msg;
						if(phone.number() && phone.number().length > 0){														
							msg = phone.number() + ' is not a valid Phone Number';
							if( phone.number().length < 12 ){
								msg += ' (must have 10 digits)';
							}
						}
						else {
							msg = 'Phone Number is required';
						}
						phone.validationMessage( msg );
						return false;
					}
				}
				phone.isFocused = ko.observable(false);				
		    }

		    function emailInitializer(email) {
		        email.type = ko.computed(function () {
		            checkDataContext();
		            var thisTypeId = email.typeId();
		            var thisType = null;
		            // If there is a type id on the phone,
		            if (thisTypeId) {
		                // Get the type
		                thisType = ko.utils.arrayFirst(datacontext.enums.emailTypes(), function (item) {
		                    return thisTypeId === item.id();
		                });
		            }
		            return thisType;
		        });
		    }

		    function addressInitializer(address) {
		        address.type = ko.computed(function () {
		            checkDataContext();
		            var thisTypeId = address.typeId();
		            var thisType = null;
		            // If there is a type id on the phone,
		            if (thisTypeId) {
		                // Get the type
		                thisType = ko.utils.arrayFirst(datacontext.enums.addressTypes(), function (item) {
		                    return thisTypeId === item.id();
		                });
		            }
		            return thisType;
		        });
		        address.state = ko.computed(function () {
		            checkDataContext();
		            var thisStateId = address.stateId();
		            if (thisStateId) {
		                var thisState = ko.utils.arrayFirst(datacontext.enums.states.peek(), function (state) {
		                    return state.id() === thisStateId;
		                });
		                return thisState;
		            }
		            return null;
		        });
				address.cityState = ko.computed( function(){
					var city = address.city() || '';
		            var state = address.state() ? address.state().code() : '';		            
		            return city + ', ' + state;
				});
		        address.cityStateZip = ko.computed(function () {
		            var city = address.city() || '';
		            var state = address.state() ? address.state().code() : '';
		            var postalCode = address.postalCode() || '';
		            return city + ', ' + state + '  ' + postalCode;
		        });
		        // Full formatted address with line breaks
		        address.fullFormatted = ko.computed(function () {
		        	var addLine1 = address.line1() ? address.line1() + ' \x0A' : '';
		        	var addLine2 = address.line2() ? address.line2() + ' \x0A' : '';
		        	var addLine3 = address.line3() ? address.line3() + ' \x0A' : '';
		        	var cityStateZip = address.cityStateZip();
		        	return addLine1 + addLine2 + addLine3 + cityStateZip;
		        });
		    }
            
		    function optOutOfPhones(contactCard) {
		        ko.utils.arrayForEach(contactCard.phones(), function (phone) {
		            phone.phonePreferred(false);
		        });
		    }

		    function optOutOfTexts(contactCard) {
		        ko.utils.arrayForEach(contactCard.phones(), function (phone) {
		            phone.textPreferred(false);
		        });
		    }

		    function optOutOfAddresses(contactCard) {
		        ko.utils.arrayForEach(contactCard.addresses(), function (address) {
		            address.preferred(false);
		        });
		    }

		    function optOutOfEmails(contactCard) {
		        ko.utils.arrayForEach(contactCard.emails(), function (email) {
		            email.preferred(false);
		        });
		    }
		}

		function checkDataContext() {
		    if (!datacontext) {
		        datacontext = require('services/datacontext');
		    }
		}
	});