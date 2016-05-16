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
					startDate: { dataType: "DateTime" },
					endDate: { dataType: "DateTime" },
					createdOn: { dataType: "DateTime" },
					updatedOn: { dataType: "DateTime" },
		            statusId: { dataType: "String" },
		            sourceId: { dataType: "String" },
		            systemName: { dataType: "String" },
		            allergyTypeIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
		            reactionIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
		            severityId: { dataType: "String" },
		            notes: { dataType: "String" },
                    code: { dataType: "String" },
                    codingSystemId: { dataType: "String" }
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

				allergy.startDateErrors = ko.observableArray([]);	//datetimepicker validation errors
				allergy.endDateErrors = ko.observableArray([]);	//datetimepicker validation errors
				allergy.validationErrors = ko.observableArray([]);
				allergy.isValid = ko.computed( function() {
					var hasErrors = false;
					var allergyErrors = [];
					var context = {maxDate: 'today'};
					var startDate = allergy.startDate();
					var endDate = allergy.endDate();
					var startDateErrors = allergy.startDateErrors();
					var endDateErrors = allergy.endDateErrors();
					if( startDateErrors.length > 0 ){
						//datetimepicker validation errors:
						ko.utils.arrayForEach( startDateErrors, function(error){
							allergyErrors.push({ PropName: 'startDate', Message: allergy.allergyName() + ' Start Date ' + error.Message});
							hasErrors = true;
						});
					}
					if( endDate ){
						if( endDateErrors.length > 0 ){
							ko.utils.arrayForEach( endDateErrors, function(error){
								allergyErrors.push({ PropName: 'endDate', Message: allergy.allergyName() + ' End Date ' + error.Message});
								hasErrors = true;
							});
						}
						if( startDateErrors.length == 0 && endDateErrors.length == 0 && startDate && endDate ){
							//startDate - endDate range: both dates exist and valid:
							if( moment(startDate).isAfter( moment( endDate ) ) ){
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

				/**
				*	computed. to allow forcing the datetimepicker control to set the start date as invalid.
				*	this is needed when the date is valid but range is wrong.
				*	@method allergy.setInvalidStartDate
				*/
				allergy.setInvalidStartDate = ko.computed( function(){
					var validationErrorsArray = allergy.validationErrorsArray();
					return (validationErrorsArray && validationErrorsArray.indexOf('startDate') !== -1);
				});
								/**
				*	computed. to allow forcing the datetimepicker control to set the end date as invalid.
				*	this is needed when the date is valid but range is wrong.
				*	@method allergy.setInvalidEndDate
				*/

				allergy.setInvalidEndDate = ko.computed( function(){
					var validationErrorsArray = allergy.validationErrorsArray();
					return (validationErrorsArray && validationErrorsArray.indexOf('endDate') !== -1);
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