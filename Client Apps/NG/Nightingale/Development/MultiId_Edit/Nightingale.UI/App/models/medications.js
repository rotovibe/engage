// Register all of the user related models in the entity manager (initialize function) and provide other non-entity models
define(['services/session', 'services/dateHelper'],
	function (session, dateHelper) {

	    var datacontext;

		var DT = breeze.DataType;

		// Expose the model module to the requiring modules
		var medicationModels = {
		    initialize: initialize
		};
		return medicationModels;

		// Initialize the entity models in the entity manager
		function initialize(metadataStore) {

		    // Patient Medication information
		    metadataStore.addEntityType({
		        shortName: "PatientMedication",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            patientId: { dataType: "String" },
		            dosage: { dataType: "String" },
		            strength: { dataType: "String" },
		            route: { dataType: "String" },
		            form: { dataType: "String" },
		            deleteFlag: { dataType: "Boolean" },
					startDate: { dataType: "DateTime" },
					endDate: { dataType: "DateTime" },
					createdOn: { dataType: "DateTime" },
					updatedOn: { dataType: "DateTime" },
		            statusId: { dataType: "String" },
		            freqQuantity: { dataType: "String" },
		            freqHowOftenId: { dataType: "String" },
					frequencyId:  { dataType: "String" },	//a new lookup to replace freqHowOftens and freqWhens (ENG 969)
		            freqWhenId: { dataType: "String" },
		            categoryId: { dataType: "String" },
		            sourceId: { dataType: "String" },
		            systemName: { dataType: "String" },
		            typeId: { dataType: "String" },
		            prescribedBy: { dataType: "String" },
		            sigCode: { dataType: "String" },
		            reason: { dataType: "String" },		            
					isCreateNewMedication: { dataType: "Boolean" },	
					customFrequency:  { dataType: "String" },
		            isDuplicate: { dataType: "Boolean" },
		            familyId: { dataType: "String" },
		            notes: { dataType: "String" },
		            nDCs: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
		            pharmClasses: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
		        },
		        navigationProperties: {
		            patient: {
		                entityTypeName: "Patient", isScalar: true,
		                associationName: "Patient_Medications", foreignKeyNames: ["patientId"]
		            },
		            status: {
		                entityTypeName: "MedicationStatus", isScalar: true,
		                associationName: "Medication_Status", foreignKeyNames: ["statusId"]
		            },
		            category: {
		                entityTypeName: "MedicationCategory", isScalar: true,
		                associationName: "Medication_Category", foreignKeyNames: ["categoryId"]
		            },
		            type: {
		                entityTypeName: "MedSuppType", isScalar: true,
		                associationName: "Medication_Type", foreignKeyNames: ["typeId"]
		            },
		            source: {
		                entityTypeName: "AllergySource", isScalar: true,
		                associationName: "Medication_Source", foreignKeyNames: ["sourceId"]
		            },
		            freqHowOften: {
		                entityTypeName: "FreqHowOften", isScalar: true,
		                associationName: "Medication_FreqHowOften", foreignKeyNames: ["freqHowOftenId"]
		            },
		            freqWhen: {
		                entityTypeName: "FreqWhen", isScalar: true,
		                associationName: "Medication_FreqWhen", foreignKeyNames: ["freqWhenId"]
		            },
					frequency: {	//a new lookup/entity to replace freqHowOftens and freqWhens (ENG 969)
						entityTypeName: "PatientMedicationFrequency", isScalar: true,
		                associationName: "Medication_Frequency", foreignKeyNames: ["frequencyId"]
					}
		        }
		    });

		    metadataStore.registerEntityTypeCtor(
				'PatientMedication', null, medicationInitializer);

		    function medicationInitializer(medication) {
		    	// Triggers the save method to perform insert instead update
		    	medication.isNew = ko.observable(false);

		    	//custom form/strength/route values - familyId logic:	
				medication.isCreateNewMedication = ko.observable(false);
				
				medication.customFrequency = ko.observable();				
		    	medication.isDuplicate = ko.observable(false);		    	
		    	// for creating new medications: this is the link from patient medication to the new MedicationMap id:
		    	medication.familyId = ko.observable();
		    	// Is the medication valid for a save?
		    	medication.canSave = ko.observable(false);
		    	// Notify when save is complete
		    	medication.isEditing = ko.observable(false);
		    	// Triggers the save method to let API know to recalculate NDC
		    	medication.recalculateNDC = ko.observable(false);
		    	// Should the medication save and flag deletion
		    	medication.deleteFlag(false);
		    	// Should the sig code be calculated
		    	medication.computedSigCode = ko.computed(function () {					
		    		var strDateRange = '';
					if(medication.startDate() && medication.endDate()){
						var startDate = moment(medication.startDate());					
						var startDate = startDate && startDate.isValid() ? startDate : null;
						var endDate = moment(medication.endDate());					
						var endDate = endDate && endDate.isValid() ? endDate : null;					
						if(startDate && endDate){
							var days = endDate.diff(startDate, 'days');
							if (days){
								strDateRange = 'for ' + days + (days==1 ? ' day' : ' days');	
							}						
						}	
					}
					if(!medication.freqQuantity() && !medication.strength() && !medication.form() && !medication.route() &&  !medication.frequency() && !strDateRange){	//&& !medication.freqHowOften() && !medication.freqWhen() 
		    			return '-';
		    		}
		    		//var dose = medication.dosage() ? medication.dosage().trim() : '';		    		
		    		var strength = medication.strength() ? medication.strength().trim() : '';
		    		var form = medication.form() ? medication.form().trim() : '';
					var route = medication.route() ? medication.route().trim() : '';
					//frequency:
					var quantity = medication.freqQuantity() ? medication.freqQuantity().trim() : '';
					quantity = quantity ? quantity: '';		    		
		    		var howOften = medication.frequency() ? medication.frequency().name().trim() : '';
					//var when = medication.freqWhen() ? medication.freqWhen().name().trim() : '';
										
					//format sig str:		
					if(!quantity && !strength && !form && !route && !howOften && !strDateRange){
						return '-';	//trimmed values may be empty
					}
		    		return quantity + ' ' + strength + ' ' + form + ' ' + route + ' ' + howOften + ' ' + strDateRange;
		    	});
				
				medication.setStatus = function(statusId, doneBannerMessage){
		        	checkDataContext();
		        	medication.statusId(statusId);
	                // Save it
	                datacontext.saveMedication(medication).then(saveCompleted);

	                function saveCompleted() {
	                    medication.isNew(false);
	                    medication.entityAspect.acceptChanges();
	                    datacontext.createAlert('warning', doneBannerMessage);
	                }
				}
				// Inactivate this medication
		        medication.inactivate = function () {
					medication.setStatus(2, 'Medication has been deactivated!');
		        }
				medication.activatePatientMedication = function(){
					medication.setStatus(1, 'Medication has been activated!');					
				}
		        medication.deletePatientMedication = function(){
					var message = 'You are about to delete: ' + medication.name() +' from this individual.  Press OK to continue, or cancel to return without deleting.';                
					var result = confirm(message);				
					if (result === true) {
						checkDataContext();
						datacontext.deletePatientMedication(medication).then(deleted);
						function deleted () {
							return true;
						}
					}
					else {                    
						return false;
					}
		        }
				medication.startDateErrors = ko.observableArray([]);	//datetimepicker validation errors
				medication.endDateErrors = ko.observableArray([]);	//datetimepicker validation errors 
				medication.validationErrors = ko.observableArray([]);
				medication.isValid = ko.computed( function() {
					var hasErrors = false;
					var medicationErrors = [];
					var startDate = medication.startDate();
					var endDate = medication.endDate();
					var startDateErrors = medication.startDateErrors();
					var endDateErrors = medication.endDateErrors();
					if( startDateErrors.length > 0 ){
						//datetimepicker validation errors: 
						ko.utils.arrayForEach( startDateErrors, function(error){
							medicationErrors.push({ PropName: 'startDate', Message: medication.name() + ' Start Date ' + error.Message});							
							hasErrors = true;
						});						
					}
					if( endDate ){
						if( endDateErrors.length > 0 ){	
							//datetimepicker validation errors: 
							ko.utils.arrayForEach( endDateErrors, function(error){
								medicationErrors.push({ PropName: 'endDate', Message: medication.name() + ' End Date ' + error.Message});
								hasErrors = true;
							});
						}						
						if( startDate && !hasErrors ){
							//startDate - endDate range: both dates exist and valid:
							if( moment(startDate).isAfter( moment( endDate ) ) ){
								medicationErrors.push({ PropName: 'endDate', Message: medication.name() + ' End Date must be on or after: ' + moment( startDate ).format("MM/DD/YYYY") });
								medicationErrors.push({ PropName: 'startDate', Message: medication.name() + ' Start Date must be on or before: ' + moment( endDate ).format("MM/DD/YYYY") });
								hasErrors = true;
							}
						}
					}
					medication.validationErrors(medicationErrors);
					return medication.canSave() && !hasErrors;
				});
				
				medication.needToSave = function(){
					var result = (medication.isNew() && medication.name() && medication.type() && medication.category() && medication.canSave());	
					result = result || ( medication.isEditing() && medication.entityAspect.entityState.isModified() );
					return result;
				}
				/**
				*	computed. tracks for any validation errors and returns a list of the errored property names.
				*	this will be used in the property field css binding condition for invalid styling.
				*	@method medication.validationErrorsArray
				*/
			    medication.validationErrorsArray = ko.computed(function () {
			        var thisArray = [];
			        ko.utils.arrayForEach(medication.validationErrors(), function (error) {
			            thisArray.push(error.PropName);
			        });
			        return thisArray;
			    });
				
		    } //medicationInitializer ends
			
			metadataStore.addEntityType({
			    shortName: "PatientMedicationFrequency",
			    namespace: "Nightingale",
			    dataProperties: {
			        id: { dataType: "String", isPartOfKey: true },
			        name: { dataType: "String" },
					patientId: { dataType: "String", isNullable: true },
					sortOrder:	{ dataType: "Int64" }
			    }
			});
			
			metadataStore.registerEntityTypeCtor(
				'PatientMedicationFrequency', null, medFrequencyInitializer);

		    function medFrequencyInitializer(frequency) {
				//add properties that dont exist on server endpoint response:
				if(frequency.patientId === undefined){
					frequency.patientId = ko.observable(null);						
				}				
				if(frequency.sortOrder === undefined){
					frequency.sortOrder = ko.observable(0);
				}
			}
		}

		function checkDataContext() {
		    if (!datacontext) {
		        datacontext = require('services/datacontext');
		    }
		}
	});