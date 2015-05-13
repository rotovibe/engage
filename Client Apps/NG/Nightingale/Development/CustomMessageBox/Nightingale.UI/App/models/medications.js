// Register all of the user related models in the entity manager (initialize function) and provide other non-entity models
/**
*	@module	medications.js defines patient medications entities
*	
*/
define(['services/session', 'viewmodels/templates/customMessageBox'],
	function (session, customMessageBox) {

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
		            freqWhenId: { dataType: "String" },
		            categoryId: { dataType: "String" },
		            sourceId: { dataType: "String" },
		            systemName: { dataType: "String" },
		            typeId: { dataType: "String" },
		            prescribedBy: { dataType: "String" },
		            sigCode: { dataType: "String" },
		            reason: { dataType: "String" },		            
					isCreateNewMedication: { dataType: "Boolean" },		            
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
		    	// Shoudl the sig code be calculated
		    	medication.computedSigCode = ko.computed(function () {
		    		if(!medication.dosage() && !medication.freqQuantity() && !medication.strength() && !medication.form() && !medication.freqWhen() && !medication.freqHowOften()){
		    			return '-';
		    		}
		    		var dose = medication.dosage() ? medication.dosage().trim() : '';
		    		var quantity = medication.freqQuantity() ? medication.freqQuantity().trim() : '';
					quantity = quantity ? quantity  + ' times ' : '';
		    		var strength = medication.strength() ? medication.strength().trim() : '';
		    		var form = medication.form() ? medication.form().trim() : '';
		    		var when = medication.freqWhen() ? medication.freqWhen().name().trim() : '';
		    		var howOften = medication.freqHowOften() ? medication.freqHowOften().name().trim() : '';
					if(!dose && !quantity && !strength && !form && !when && !howOften){
						return '-';	//trimmed values may be empty
					}
		    		return dose + ' ' + strength + ' ' + form + ' ' + quantity + ' ' + howOften + ' ' + when;
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
				/**
				*	@method deletePatientMedication - demonstrate usage of customMessageBox for confirmation.
				*
				*/
		        medication.deletePatientMedication = function(){
					var message = 'You are about to delete: ' + medication.name() +' from this individual.  Press OK to continue, or cancel to return without deleting.';                
					//custom modal message box:					
					this.dialog = new customMessageBox(message, 'are you sure?', [
										{text:'Yes', value:true, css:'btn color'},
										{text:'Cancel', value: false, css:'btn cancel'}
										], false);
					this.dialog.show().then(function(response){						
						if(response){
							checkDataContext();
							datacontext.deletePatientMedication(medication).then(deleted);
							function deleted () {
								return true;//self.medications().remove(medication);						                       
							}
						}
						else{
							return false;		
						}	
					});
					//end custom modal message box.
		        }
		    }
		}

		function checkDataContext() {
		    if (!datacontext) {
		        datacontext = require('services/datacontext');
		    }
		}
	});