// Register all of the user related models in the entity manager (initialize function) and provide other non-entity models
define(['services/session', 'services/validatorfactory', 'services/customvalidators', 'services/dateHelper'],
	function (session, validatorFactory, customValidators, dateHelper) {

		var datacontext;

		var DT = breeze.DataType;

	    // Validation error message to populate entity's collection
		function validationError(propName, message) {
		    var self = this;
		    self.PropName = propName;
		    self.Message = message;
		}

		// Expose the model module to the requiring modules
		var observationModels = {
			initialize: initialize
		};
		return observationModels;

		// Initialize the entity models in the entity manager
		function initialize(metadataStore) {

		    // Patient Observation information
		    var patientObservationType = metadataStore.addEntityType({
		    	shortName: "PatientObservation",
		    	namespace: "Nightingale",
		    	dataProperties: {
		    		id: { dataType: "String", isPartOfKey: true },
		    		observationId: { dataType: "String" },
		    		name: { dataType: "String" },
		    		standard: { dataType: "Boolean" },
		    		typeId: { dataType: "String" },
		    		stateId: { dataType: "String" },
		    		displayId: { dataType: "String" },
		    		patientId: { dataType: "String" },
		    		units: { dataType: "String" },
		    		startDate: { dataType: "String" },
		    		endDate: { dataType: "DateTime" },
		    		groupId: { dataType: "String" },
		    		deleteFlag: { dataType: "Boolean" },
		    		source: { dataType: "String" },
		    		values: { complexTypeName: "ObservationValue:#Nightingale", isScalar: false }
		    	},
		    	navigationProperties: {
		    		type: {
		    			entityTypeName: "ObservationType", isScalar: true,
		    			associationName: "Observation_Type", foreignKeyNames: ["typeId"]
		    		},
		    		observation: {
		    			entityTypeName: "Observation", isScalar: true,
		    			associationName: "PatientObservation_Observation", foreignKeyNames: ["observationId"]
		    		},
		    		patient: {
		    			entityTypeName: "Patient", isScalar: true,
		    			associationName: "Patient_Observations", foreignKeyNames: ["patientId"]
		    		},
		    		state: {
		    			entityTypeName: "ObservationState", isScalar: true,
		    			associationName: "Observation_State", foreignKeyNames: ["stateId"]
		    		},
		    		display: {
		    			entityTypeName: "ObservationDisplay", isScalar: true,
		    			associationName: "Observation_Display", foreignKeyNames: ["displayId"]
		    		}
		    	}
		    });
			
		    // var patientObservationPropertyList = [
                // {
		            // // Short name of property
		            // name: 'startDate',
		            // // Desired display name of property
		            // displayName: 'Start Date',
		            // // Collection of validators
		            // validatorsList: [
						// customValidators.validators.dateValidator({maxDate: 'today'})
                        // //breeze.Validator.required()
						
		            // ]
                // }
		    // ];
			// //patientObservationType.validators.push( customValidators.validators.observationValidator );			
		    // validatorFactory.fixNamesAndRegisterValidators(metadataStore, 'PatientObservation', patientObservationPropertyList);

		    // Observations' value complex type
		    metadataStore.addEntityType({
		    	shortName: "ObservationValue",
		    	namespace: "Nightingale",
		    	isComplexType: true,
		    	dataProperties: {
		    		id: { dataType: "String" },
		    		text: { dataType: "String" },
		    		value: { dataType: "String" },
		    		previousValue: { complexTypeName: "ObservationPreviousValue:#Nightingale", isScalar: true }
		    	}
		    });

		    // Observation values' previous value complex type
		    metadataStore.addEntityType({
		    	shortName: "ObservationPreviousValue",
		    	namespace: "Nightingale",
		    	isComplexType: true,
		    	dataProperties: {
		    		value: { dataType: "String" },
		    		startDate: { dataType: "DateTime" },
		    		unit: { dataType: "String" },
		    		source: { dataType: "String" }
		    	}
		    });

		    // Observation information
		    //  - This is a list of observations to use 
		    //  - for typeahead adding of observations
		    metadataStore.addEntityType({
		    	shortName: "Observation",
		    	namespace: "Nightingale",
		    	dataProperties: {
		    		id: { dataType: "String", isPartOfKey: true },
		    		name: { dataType: "String" },
		    		typeId: { dataType: "String" },
		    		standard: { dataType: "Boolean" }
		    	},
		    	navigationProperties: {
		    		type: {
		    			entityTypeName: "ObservationType", isScalar: true,
		    			associationName: "Observation_Type", foreignKeyNames: ["typeId"]
		    		}
		    	}
		    });
						
		    // Observation Display property
		    metadataStore.addEntityType({
		    	shortName: "ObservationDisplay",
		    	namespace: "Nightingale",
		    	dataProperties: {
		    		id: { dataType: "String", isPartOfKey: true },
		    		name: { dataType: "String" }
		    	}
		    });

		    // Observation State property
		    metadataStore.addEntityType({
		    	shortName: "ObservationState",
		    	namespace: "Nightingale",
		    	dataProperties: {
		    		id: { dataType: "String", isPartOfKey: true },
		    		name: { dataType: "String" },
		            allowedTypeIds: { complexTypeName: "IdName:#Nightingale", isScalar: false }
		    	}
		    });

		    metadataStore.registerEntityTypeCtor(
		    	'Observation', null, observationInitializer);

		    metadataStore.registerEntityTypeCtor(
		    	'PatientObservation', null, patientObservationInitializer);
		    
		    metadataStore.registerEntityTypeCtor(
		    	'ObservationValue', null, observationValueInitializer);

			/**
			*
			* 	@method patientObservationInitializer
			*/			
		    function patientObservationInitializer(patObs) {
                patObs.isNew = ko.observable(false);
		    	patObs.computedValue = ko.computed({
		    		read: function () {
		    			var thisObservationsValues = patObs.values();
		    			var returnValue = null;
		    			if (thisObservationsValues.length === 1) {
		    				returnValue = thisObservationsValues[0];
		    			}
		    			else if (thisObservationsValues.length > 1) {
		    				returnValue = thisObservationsValues;
		    			}
		    			return returnValue;
		    		},
		    		write: function (newValue) {
		                // If attribute is a multiselect,
		                if (patObs.controlType === 2) {
		                    // TODO: Do something
	                  	}
	                  	else {
	                  		patObs.values()[0].value(newValue);
	                	}
	                }
	            });
		    	patObs.computedValueString = ko.computed(function () {
		    		var thisValue = patObs.computedValue();
		    		// If there is a value,
		    		if (thisValue) {
		    			// Check if it is one or more values
		    			if (Array.isArray(thisValue)) {
		    				// Then if it is an array, it is a blood pressure reading
		    				var diastValue = ko.utils.arrayFirst(thisValue, function (val) {
		    					return val.text().indexOf('Diast') !== -1;
		    				});
		    				// Then if it is an array, it is a blood pressure reading
		    				var systValue = ko.utils.arrayFirst(thisValue, function (val) {
		    					return val.text().indexOf('Sys') !== -1;
		    				});
                    		thisValue = (!!diastValue && !!systValue) ? systValue.value() + '/' + diastValue.value() + ' ' + patObs.units() : '';
		    			} else {
		    				// Else it has one value so return it plus units
		    				thisValue = thisValue.value() + ' ' + patObs.units();
		    			}
		    		} else {
		    			thisValue = '-';
		    		}
		    		return thisValue;
		    	});
				patObs.validationErrors = ko.observableArray([]);
				/**
				*	computed. validating the patient observation and updating errors and error messages into patObs.validationErrors. 
				*	returns true for valid observation.
				*	@method isValid 
				*/
			    patObs.isValid = ko.computed( function() {					
					var values = patObs.values();
					var startDate = patObs.startDate();
					var hasErrors = patObs.entityAspect.hasValidationErrors;
					var type = patObs.type()? patObs.type().name() : null;
					switch( type ){
						case 'Assessments':
						
							break;
						case 'Labs':
							hasErrors = validateLab();
							break;
						case 'Problems':
							hasErrors = validateProblem();
							break;
						case 'Risks':
							break;
						case 'Vitals':
							hasErrors = validateLab();
							break;
						case 'Allergies':
							break;
						case 'Medications':
							break;
					}
					
					return !hasErrors;
					
					function validateLab(){
						var hasActualValues = false;
						var obsErrors = [];
						// if( hasErrors ){
							// //breeze validation errors (not in use for now)
							// var errors = patObs.entityAspect.getValidationErrors();
							// ko.utils.arrayForEach(errors, function (error) {
								// obsErrors.push({ PropName: error.propertyName, Message: error.errorMessage});
							// });
						// }
						
						ko.utils.arrayForEach( values, function(value) {
							var propName = 'value';
							if( value.text && value.text() && value.text().indexOf("Systolic") !== -1 ){
								propName = 'systolic';
							}
							else if( value.text && value.text() && value.text().indexOf("Diastolic") !== -1 ){
								propName = 'diastolic';
							}
							if( isNaN(value.value()) ){	//note: this will change when we get alert limits high/low: numeric/decimal with prefix +/-. when no high/low its any string.																
								var name = value.name? value.name() : value.text? value.text() : 'observation ';
								var msg = name + ' has invalid value' ;
								obsErrors.push({ PropName: propName, Message: msg });
								hasErrors = true;
							}
							if( startDate && !value.value() ){
								var name = value.name? value.name() : value.text? value.text() : 'observation ';	//patObs.name()
								var msg = name + ' must have a value';
								obsErrors.push({ PropName: propName, Message: msg });
								hasErrors = true;
							}
							else if( value.value() ){
								hasActualValues = true;
							}
						});
						
						if( !startDate && hasActualValues ){
							obsErrors.push({ PropName: 'startDate', Message: patObs.name() + ' must have a Date' });
							hasErrors = true;
						}	
						var context = {maxDate: 'today'};
						var startDateError = dateHelper.isInvalidDate(startDate, context);
						if( startDateError != null ){
							obsErrors.push({ PropName: 'startDate', Message: patObs.name() + ' Date ' + startDateError.Message});
							hasErrors = true;
						}
						patObs.validationErrors(obsErrors);
						return hasErrors;
					}
					function validateProblem(){
						var obsErrors = [];
						var context = {maxDate: 'today'};
						var startDateError = dateHelper.isInvalidDate(startDate, context);
						if( startDateError != null ){
							obsErrors.push({ PropName: 'startDate', Message: patObs.name() + ' Date ' + startDateError.Message});
							hasErrors = true;
						}
						patObs.validationErrors(obsErrors);
						return hasErrors;
					}
				});
								
			    patObs.canSave = ko.computed(patObs.isValid);
			    
			    // patObs.entityAspect.validationErrorsChanged.subscribe(function (validationChangeArgs) {
			        // var hasErrors = patObs.entityAspect.hasValidationErrors;
			        // var errorsList = [];
			        // if (hasErrors) {
			            // patObs.isValid(false);
			            // var errors = patObs.entityAspect.getValidationErrors();
			            // ko.utils.arrayForEach(errors, function (error) {
			                // errorsList.push(new validationError(error.propertyName, error.errorMessage));
			            // });
			            // patObs.validationErrors(errorsList);
			        // } else {
			            // patObs.validationErrors([]);
			            // patObs.isValid(true);
			        // }
			    // });
				
				/**
				*	computed. tracks for any validation errors and returns a list of the errored property names.
				*	this will be used in the property field css binding condition for invalid styling.
				*	@method patObs.validationErrorsArray
				*/
			    patObs.validationErrorsArray = ko.computed(function () {
			        var thisArray = [];
			        ko.utils.arrayForEach(patObs.validationErrors(), function (error) {
			            thisArray.push(error.PropName);
			        });
			        return thisArray;
			    });
		    }

		    function observationInitializer(addtlObs) {
		    	addtlObs.value = ko.computed(addtlObs.name);
		    }
		    
		    function observationValueInitializer(obsValue) {
		    	obsValue.previousValueString = ko.computed(function () {
		    		var thisString = '';
		            // If there is a last value,
		            if (obsValue.previousValue()) {
		                // Create a string from it
		                var value = obsValue.previousValue().value();
		                var startdate = moment(obsValue.previousValue().startDate()).format('MM/DD/YYYY');
		                var unit = obsValue.previousValue().unit();
		                var source = obsValue.previousValue().source();
		                thisString = value ? value + ' ' + unit : '';                    	
		                thisString = moment(startdate).isValid() ? (thisString ? thisString + ' on ' + startdate : startdate) : thisString;
		                thisString = source ? thisString + ' (' + source + ')' : thisString;
					}
					return thisString;
	            });
		    }
		  }

		  function checkDataContext() {
		  	if (!datacontext) {
		  		datacontext = require('services/datacontext');
		  	}
		  }
		});