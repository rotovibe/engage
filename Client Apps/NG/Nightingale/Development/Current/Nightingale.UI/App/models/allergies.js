// Register all of the user related models in the entity manager (initialize function) and provide other non-entity models
define(['services/session', 'services/dateHelper'],
	function (session, dateHelper) {

	    var datacontext;

		var DT = breeze.DataType;

		// Expose the model module to the requiring modules
		var allergyModels = {
		    initialize: initialize
		};
		return allergyModels;

		// Initialize the entity models in the entity manager
		function initialize(metadataStore) {

		    // Note information
		    metadataStore.addEntityType({
		        shortName: "PatientAllergy",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            allergyName: { dataType: "String" },
		            allergyId: { dataType: "String" },
		            patientId: { dataType: "String" },
		            deleteFlag: { dataType: "Boolean" },
					startDate: { dataType: "String" },
					endDate: { dataType: "String" },
					createdOn: { dataType: "DateTime" },
					updatedOn: { dataType: "DateTime" },
		            statusId: { dataType: "String" },
		            sourceId: { dataType: "String" },
		            systemName: { dataType: "String" },
		            allergyTypeIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
		            reactionIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
		            severityId: { dataType: "String" },
		            notes: { dataType: "String" }
		        },
		        navigationProperties: {
		            patient: {
		                entityTypeName: "Patient", isScalar: true,
		                associationName: "Patient_Allergies", foreignKeyNames: ["patientId"]
		            },
		            severity: {
		                entityTypeName: "Severity", isScalar: true,
		                associationName: "Allergy_Severity", foreignKeyNames: ["severityId"]
		            },
		            status: {
		                entityTypeName: "AllergyStatus", isScalar: true,
		                associationName: "Allergy_Status", foreignKeyNames: ["statusId"]
		            },
		            source: {
		                entityTypeName: "AllergySource", isScalar: true,
		                associationName: "Allergy_Source", foreignKeyNames: ["sourceId"]
		            }
		        }
		    });

		    // Allergy State
		    metadataStore.addEntityType({
		        shortName: "AllergyStatus",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    metadataStore.registerEntityTypeCtor(
				'PatientAllergy', null, patientAllergyInitializer);

		    function patientAllergyInitializer(allergy) {
                allergy.isNew = ko.observable(false);	
                allergy.isUserCreated = ko.observable(false);
                allergy.typeString = ko.computed(function () {
		            checkDataContext();
		            var thisString = '';
		            ko.utils.arrayForEach(allergy.allergyTypeIds(), function (allg) {
		                var thisAllergyType = ko.utils.arrayFirst(datacontext.enums.allergyTypes(), function (faEnum) {
		                    return faEnum.id() === allg.id();
		                });
		                if (thisAllergyType) {
		                	thisString += thisAllergyType.name() + ', ';		                	
		                }
		            });
		            // If the string is longer than zero,
		            if (thisString.length > 0) {
		                // Remove the trailing comma and space
		                thisString = thisString.substr(0, thisString.length - 2);
		            }
		            if (thisString.length === 0) {
		                thisString = '-';
		            }
		            return thisString;
		        });
				allergy.validationErrors = ko.observableArray([]);
				allergy.isValid = ko.computed( function() {
					var hasErrors = false;
					var allergyErrors = [];
					var context = {maxDate: 'today'};
					var startDate = allergy.startDate();
					var endDate = allergy.endDate();
					if( startDate ){						
						var startDateError = dateHelper.isInvalidDate(startDate, context);
						if( startDateError != null ){
							allergyErrors.push({ PropName: 'startDate', Message: allergy.allergyName() + ' Start Date ' + startDateError.Message});
							hasErrors = true;
						}
					}
					if( endDate ){						
						var endDateError = dateHelper.isInvalidDate(endDate, context);
						if( endDateError != null ){
							allergyErrors.push({ PropName: 'endDate', Message: allergy.allergyName() + ' End Date ' + endDateError.Message});
							hasErrors = true;
						}
						if( startDate && !hasErrors ){
							//startDate - endDate range: both dates exist and valid:
							if( moment( startDate, "MM/DD/YYYY", true ).isAfter( moment( endDate, "MM/DD/YYYY", true ) ) ){
								allergyErrors.push({ PropName: 'endDate', Message: allergy.allergyName() + ' End Date must be on or after: ' + moment( startDate ).format("MM/DD/YYYY") });
								allergyErrors.push({ PropName: 'startDate', Message: allergy.allergyName() + ' Start Date must be on or before: ' + moment( endDate ).format("MM/DD/YYYY") });
								hasErrors = true;
							}
						}
					}
					allergy.validationErrors(allergyErrors);
					return !hasErrors;
				});
				
				/**
				*	computed. tracks for any validation errors and returns a list of the errored property names.
				*	this will be used in the property field css binding condition for invalid styling.
				*	@method allergy.validationErrorsArray
				*/
			    allergy.validationErrorsArray = ko.computed(function () {
			        var thisArray = [];
			        ko.utils.arrayForEach(allergy.validationErrors(), function (error) {
			            thisArray.push(error.PropName);
			        });
			        return thisArray;
			    });
				
				allergy.needToSave = function(){
					var result = (allergy.entityAspect.entityState.isModified() || allergy.isNew()) && allergy.sourceId();									
					return result;
				}
                allergy.reactionString = ko.computed(function () {
		            checkDataContext();
		            var thisString = '';
		            ko.utils.arrayForEach(allergy.reactionIds(), function (allg) {
		                var thisReacion = ko.utils.arrayFirst(datacontext.enums.reactions(), function (faEnum) {
		                    return faEnum.id() === allg.id();
		                });
		                if (thisReacion) {
		                	thisString += thisReacion.name() + ', ';		                	
		                }
		            });
		            // If the string is longer than zero,
		            if (thisString.length > 0) {
		                // Remove the trailing comma and space
		                thisString = thisString.substr(0, thisString.length - 2);
		            }
		            if (thisString.length === 0) {
		                thisString = '-';
		            }
		            return thisString;
		        });
		        // Inactivate this allergy
				allergy.setStatus = function(statusId, doneBannerMessage){
					checkDataContext();
		        	allergy.statusId(statusId);
	                // Save it
	                datacontext.saveAllergies([allergy], 'Update').then(saveCompleted);

	                function saveCompleted() {
	                    allergy.isNew(false);
	                    allergy.isUserCreated(false);
	                    allergy.entityAspect.acceptChanges();
	                    datacontext.createAlert('warning', doneBannerMessage);
	                }
				}
		        allergy.inactivate = function () {
					allergy.setStatus(2,'Allergy has been deactivated!');
		        }
				allergy.activatePatientAllergy = function () {
					allergy.setStatus(1,'Allergy has been activated!');					
				}
		        allergy.deletePatientAllergy = function(){
					var message = 'You are about to delete: ' + allergy.allergyName() +' from this individual.  Press OK to continue, or cancel to return without deleting.';                
					var result = confirm(message);				
					if (result === true) {
						checkDataContext();
						datacontext.deletePatientAllergy(allergy).then(deleted);
						function deleted () {
							return true;					                       
						}
					}
					else {                    
						return false;
					}
				}
		    }
		}

		function checkDataContext() {
		    if (!datacontext) {
		        datacontext = require('services/datacontext');
		    }
		}
	});