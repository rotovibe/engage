// Register all of the models in the entity manager (initialize function) and provide other non-entity models
define(['services/session'],
	function (session) {

	    var datacontext;

		var DT = breeze.DataType;

		// Expose the model module to the requiring modules
		var stepmodels = {
		    initialize: initialize,
		    initializeEnums: initializeEnums
		};
		return stepmodels;

		// Initialize the entity models in the entity manager
		function initialize(metadataStore) {

		    // Time of Day information
		    metadataStore.addEntityType({
		        shortName: "TimeOfDay",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // Time Zone information
		    metadataStore.addEntityType({
		        shortName: "TimeZone",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            preferred: { dataType: "Boolean" }
		        }
		    });

		    // Language information
		    metadataStore.addEntityType({
		        shortName: "Language",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            code: { dataType: "String" },
		            name: { dataType: "String" }
		        }
		    });

		    // State information
		    metadataStore.addEntityType({
		        shortName: "State",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            code: { dataType: "String" },
		            name: { dataType: "String" }
		        }
		    });

		    // Communication Mode
		    metadataStore.addEntityType({
		        shortName: "CommunicationMode",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // Communication Type
		    metadataStore.addEntityType({
		        shortName: "CommunicationType",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            commModeIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false }
		        }
		    });

		    // Priority
		    metadataStore.addEntityType({
		        shortName: "Priority",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            levelName: { dataType: "String" },
		            imageSource: { dataType: "String" },
		            iconClass: { dataType: "String" }
		        },
		        navigationProperties: {
		            patients: {
		                entityTypeName: "Patient", isScalar: false,
		                associationName: "Priority_Patients"
		            }
		        }
		    });

			//individual status - enum
			metadataStore.addEntityType({
		        shortName: "PatientStatus",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

			//contact status - enum
			metadataStore.addEntityType({
		        shortName: "ContactStatus",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

			//care member status - enum
			metadataStore.addEntityType({
		        shortName: "CareMemberStatus",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
					isDefault: { dataType: "Boolean" }
		        }
		    });

			//care member frequency
			metadataStore.addEntityType({
		        shortName: "CareTeamFrequency",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

			//system status - enum
			metadataStore.addEntityType({
				shortName: "SystemStatus",
				namespace: "Nightingale",
				dataProperties:{
					id: { dataType: "String", isPartOfKey: true },
					name: { dataType: "String" }
				}
			});

			//patient system status - enum (multi id = patient system)
			metadataStore.addEntityType({
				shortName: "PatientSystemStatus",
				namespace: "Nightingale",
				dataProperties:{
					id: { dataType: "String", isPartOfKey: true },
					name: { dataType: "String" }
				}
			});

			//individual status reason - lookup
			metadataStore.addEntityType({
		        shortName: "PatientStatusReason",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // Observation Type
		    metadataStore.addEntityType({
		        shortName: "ObservationType",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // Care Member Type
		    metadataStore.addEntityType({
		        shortName: "CareMemberType",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // Element State
		    metadataStore.addEntityType({
		        shortName: "ElementState",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
                    order: { dataType: "Int64" },
		            name: { dataType: "String" }
		        }
		    });

		    // Eligibility State
		    metadataStore.addEntityType({
		        shortName: "EligibilityStatus",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // Enrollment Status State
		    metadataStore.addEntityType({
		        shortName: "EnrollmentStatus",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // YesOrNo State
		    metadataStore.addEntityType({
		        shortName: "YesOrNo",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // TodoCategory
		    metadataStore.addEntityType({
		        shortName: "ToDoCategory",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // NoteMethod
		    metadataStore.addEntityType({
		        shortName: "NoteMethod",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            isDefault: { dataType: "Boolean" }
		        }
		    });

		    // NoteOutcome
		    metadataStore.addEntityType({
		        shortName: "NoteOutcome",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            isDefault: { dataType: "Boolean" }
		        }
		    });

		    // NoteWho
		    metadataStore.addEntityType({
		        shortName: "NoteWho",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            isDefault: { dataType: "Boolean" }
		        }
		    });

		    // NoteSource
		    metadataStore.addEntityType({
		        shortName: "NoteSource",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            isDefault: { dataType: "Boolean" }
		        }
		    });

		    // NoteType
		    metadataStore.addEntityType({
		        shortName: "NoteType",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            isDefault: { dataType: "String" }
		        }
		    });
			//utilization note:	VisitType
			metadataStore.addEntityType({
		        shortName: "VisitType",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            isDefault: { dataType: "Boolean" }
		        }
		    });
			//utilization note:	UtilizationSource
			metadataStore.addEntityType({
		        shortName: "UtilizationSource",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            isDefault: { dataType: "Boolean" }
		        }
		    });
			//utilization note:	Disposition
			metadataStore.addEntityType({
		        shortName: "Disposition",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            isDefault: { dataType: "Boolean" }
		        }
		    });
			//utilization note: UtilizationLocation
			metadataStore.addEntityType({
		        shortName: "UtilizationLocation",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            isDefault: { dataType: "Boolean" }
		        }
		    });

		    // AllergyType
		    metadataStore.addEntityType({
		        shortName: "AllergyType",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // Severity
		    metadataStore.addEntityType({
		        shortName: "Severity",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // Reaction
		    metadataStore.addEntityType({
		        shortName: "Reaction",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // AllergySource
		    metadataStore.addEntityType({
		        shortName: "AllergySource",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            isDefault: { dataType: "Boolean" }
		        }
		    });

		    // TypeName
		    metadataStore.addEntityType({
		        shortName: "MedSuppType",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });
		    // FreqHowOften
		    metadataStore.addEntityType({
		        shortName: "FreqHowOften",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });
		    // FreqWhen
		    metadataStore.addEntityType({
		        shortName: "FreqWhen",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });
		    // Frequency
		    metadataStore.addEntityType({
		        shortName: "Frequency",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });
		    // MedicationStatus
		    metadataStore.addEntityType({
		        shortName: "MedicationStatus",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // MedicationCategory
		    metadataStore.addEntityType({
		        shortName: "MedicationCategory",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

            // DurationUnit
            metadataStore.addEntityType({
                shortName: "DurationUnit",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    name: { dataType: "String" }
                }
            });

				// Marital Status for a Patient
				metadataStore.addEntityType({
						shortName: "MaritalStatus",
						namespace: "Nightingale",
						dataProperties: {
								id: { dataType: "String", isPartOfKey: true },
								name: { dataType: "String" }
						}
				});

				// Deceased State for a Patient
				metadataStore.addEntityType({
						shortName: "Deceased",
						namespace: "Nightingale",
						dataProperties: {
								id: { dataType: "String", isPartOfKey: true },
								name: { dataType: "String" }
						}
				});

		}

	    // Initialize the entity models in the entity manager
		function initializeEnums(manager) {

		    // Create the enums to use for element states so that they can be shared throughout the application
		    manager.createEntity('ElementState', { id: 1, order: 6, name: 'Removed' }).entityAspect.acceptChanges();
		    manager.createEntity('ElementState', { id: 2, order: 3, name: 'Not Started' }).entityAspect.acceptChanges();
		    manager.createEntity('ElementState', { id: 3, order: 1, name: 'Started' }).entityAspect.acceptChanges();
		    manager.createEntity('ElementState', { id: 5, order: 4, name: 'Completed' }).entityAspect.acceptChanges();
		    manager.createEntity('ElementState', { id: 4, order: 2, name: 'In Progress' }).entityAspect.acceptChanges();
		    manager.createEntity('ElementState', { id: 6, order: 5, name: 'Closed' }).entityAspect.acceptChanges();

		    // Create the enums to re-use and save their changes so they don't show up as having changes
		    manager.createEntity('Priority', { id: 0, levelName: 'Not Set', imageSource: '/NightingaleUI/Content/images/priority_empty.png', iconClass: 'icon-priority-empty' }).entityAspect.acceptChanges();
		    manager.createEntity('Priority', { id: 1, levelName: 'Low', imageSource: '/NightingaleUI/Content/images/priority_low.png', iconClass: 'icon-priority grey' }).entityAspect.acceptChanges();
		    manager.createEntity('Priority', { id: 2, levelName: 'Medium', imageSource: '/NightingaleUI/Content/images/priority_medium.png', iconClass: 'icon-priority yellow' }).entityAspect.acceptChanges();
		    manager.createEntity('Priority', { id: 3, levelName: 'High', imageSource: '/NightingaleUI/Content/images/priority_high.png', iconClass: 'icon-priority red' }).entityAspect.acceptChanges();

			//enums.patientStatus
		    manager.createEntity('PatientStatus', { id: 1, name: 'Active'   }).entityAspect.acceptChanges();
		    manager.createEntity('PatientStatus', { id: 2, name: 'Inactive' }).entityAspect.acceptChanges();
			manager.createEntity('PatientStatus', { id: 3, name: 'Archived' }).entityAspect.acceptChanges();

			//enums.contactStatus
		    manager.createEntity('ContactStatus', { id: 1, name: 'Active'   }).entityAspect.acceptChanges();
		    manager.createEntity('ContactStatus', { id: 2, name: 'Inactive' }).entityAspect.acceptChanges();
			manager.createEntity('ContactStatus', { id: 3, name: 'Archived' }).entityAspect.acceptChanges();

			//care member status
			manager.createEntity('CareMemberStatus', { id: 1, name: 'Active' }).entityAspect.acceptChanges();
			manager.createEntity('CareMemberStatus', { id: 2, name: 'Inactive' }).entityAspect.acceptChanges();
			manager.createEntity('CareMemberStatus', { id: 3, name: 'Invalid' }).entityAspect.acceptChanges();

			//enums.systemStatus
			manager.createEntity('SystemStatus', {id: 1, name: 'Active'		}).entityAspect.acceptChanges();
			manager.createEntity('SystemStatus', {id: 2, name: 'Inactive'	}).entityAspect.acceptChanges();

			//enums.patientSystemStatus
			manager.createEntity('PatientSystemStatus',  {id: 1, name: 'Active'		}).entityAspect.acceptChanges();
			manager.createEntity('PatientSystemStatus',  {id: 2, name: 'Inactive'	}).entityAspect.acceptChanges();

            // Types of steps enums
		    manager.createEntity('StepType', { id: 1, name: 'Radio', path: 'programdesigner/questiontypes/radio.html' }).entityAspect.acceptChanges();
		    manager.createEntity('StepType', { id: 2, name: 'Text', path: 'programdesigner/questiontypes/label.html' }).entityAspect.acceptChanges();
		    manager.createEntity('StepType', { id: 3, name: 'Textbox', path: 'programdesigner/questiontypes/textbox.html' }).entityAspect.acceptChanges();
		    manager.createEntity('StepType', { id: 10, name: 'TextArea', path: 'programdesigner/questiontypes/textarea.html' }).entityAspect.acceptChanges();
		    manager.createEntity('StepType', { id: 4, name: 'SingleSelect', path: 'programdesigner/questiontypes/singleselect.html' }).entityAspect.acceptChanges();
		    manager.createEntity('StepType', { id: 7, name: 'Complete', path: 'programdesigner/questiontypes/button.html' }).entityAspect.acceptChanges();
		    manager.createEntity('StepType', { id: 6, name: 'DatePicker', path: 'programdesigner/questiontypes/datepicker.html' }).entityAspect.acceptChanges();
		    manager.createEntity('StepType', { id: 8, name: 'TimePicker', path: 'programdesigner/questiontypes/timepicker.html' }).entityAspect.acceptChanges();
		    manager.createEntity('StepType', { id: 9, name: 'DateTimePicker', path: 'programdesigner/questiontypes/datetimepicker.html' }).entityAspect.acceptChanges();
		    manager.createEntity('StepType', { id: 5, name: 'Checkbox', path: 'programdesigner/questiontypes/checkbox.html' }).entityAspect.acceptChanges();
		    manager.createEntity('StepType', { id: 11, name: 'Modal', path: 'programdesigner/questiontypes/modal.html' }).entityAspect.acceptChanges();

		    // Goal task status enums
		    manager.createEntity('GoalTaskStatus', { id: 1, name: 'Open', order: 1, iconClass: 'icon-priority-empty orange', textClass: '' }).entityAspect.acceptChanges();
		    manager.createEntity('GoalTaskStatus', { id: 2, name: 'Met', order: 4, iconClass: 'icon-check darkgreen', textClass: 'grey' }).entityAspect.acceptChanges();
		    manager.createEntity('GoalTaskStatus', { id: 3, name: 'Not Met', order: 2, iconClass: 'icon-warning-o red', textClass: 'grey' }).entityAspect.acceptChanges();
		    manager.createEntity('GoalTaskStatus', { id: 4, name: 'Abandoned', order: 3, iconClass: 'icon-warning red', textClass: 'grey' }).entityAspect.acceptChanges();

		    // Barrier status enums
		    manager.createEntity('BarrierStatus', { id: 1, name: 'Open', iconClass: 'icon-priority-empty orange', textClass: '' }).entityAspect.acceptChanges();
		    manager.createEntity('BarrierStatus', { id: 2, name: 'Resolved', iconClass: 'icon-check darkgreen', textClass: 'grey' }).entityAspect.acceptChanges();

		    // Intervention status enums
		    manager.createEntity('InterventionStatus', { id: 1, name: 'Open', iconClass: 'icon-priority-empty orange', textClass: '' }).entityAspect.acceptChanges();
		    manager.createEntity('InterventionStatus', { id: 2, name: 'Completed', iconClass: 'icon-check darkgreen', textClass: 'grey' }).entityAspect.acceptChanges();
		    manager.createEntity('InterventionStatus', { id: 3, name: 'Removed', iconClass: 'icon-warning-o red', textClass: 'grey' }).entityAspect.acceptChanges();

		    // Goal type enums
		    manager.createEntity('GoalType', { id: 1, name: 'Long-Term' }).entityAspect.acceptChanges();
		    manager.createEntity('GoalType', { id: 2, name: 'Short-Term' }).entityAspect.acceptChanges();

		    // Observation Display enums
		    manager.createEntity('ObservationDisplay', { id: -1, name: 'None' }).entityAspect.acceptChanges();
		    manager.createEntity('ObservationDisplay', { id: 1, name: 'Primary' }).entityAspect.acceptChanges();
		    manager.createEntity('ObservationDisplay', { id: 2, name: 'Secondary' }).entityAspect.acceptChanges();

		    // Eligibility enums
		    manager.createEntity('EligibilityStatus', { id: 1, name: 'Not Eligible' }).entityAspect.acceptChanges();
		    manager.createEntity('EligibilityStatus', { id: 2, name: 'Eligible' }).entityAspect.acceptChanges();
		    manager.createEntity('EligibilityStatus', { id: 3, name: 'Pending' }).entityAspect.acceptChanges();

		    // Enrollment Status enums
		    manager.createEntity('EnrollmentStatus', { id: 1, name: 'Enrolled' }).entityAspect.acceptChanges();
		    manager.createEntity('EnrollmentStatus', { id: 2, name: 'Pending' }).entityAspect.acceptChanges();
		    manager.createEntity('EnrollmentStatus', { id: 3, name: 'Did Not Enroll' }).entityAspect.acceptChanges();
		    manager.createEntity('EnrollmentStatus', { id: 4, name: 'Disenrolled' }).entityAspect.acceptChanges();
		    manager.createEntity('EnrollmentStatus', { id: 5, name: 'Completed Program' }).entityAspect.acceptChanges();

		    // Enrollment Status enums
		    manager.createEntity('YesOrNo', { id: 1, name: 'No' }).entityAspect.acceptChanges();
		    manager.createEntity('YesOrNo', { id: 2, name: 'Yes' }).entityAspect.acceptChanges();

		    // Note Types enums
		    // manager.createEntity('NoteType', { id: 1, name: 'General' }).entityAspect.acceptChanges();
		    // manager.createEntity('NoteType', { id: 2, name: 'Touchpoint' }).entityAspect.acceptChanges();

		    // Allergy status enums
		    manager.createEntity('AllergyStatus', { id: 1, name: 'Active' }).entityAspect.acceptChanges();
		    manager.createEntity('AllergyStatus', { id: 2, name: 'Inactive' }).entityAspect.acceptChanges();

		    // Medication status enums
            manager.createEntity('MedicationStatus', { id: 1, name: 'Active' }).entityAspect.acceptChanges();
            manager.createEntity('MedicationStatus', { id: 2, name: 'Inactive' }).entityAspect.acceptChanges();
            manager.createEntity('MedicationStatus', { id: 3, name: 'Refused' }).entityAspect.acceptChanges();
            manager.createEntity('MedicationStatus', { id: 4, name: 'NotDoneMedical' }).entityAspect.acceptChanges();
            manager.createEntity('MedicationStatus', { id: 5, name: 'Unknown' }).entityAspect.acceptChanges();
            manager.createEntity('MedicationStatus', { id: 6, name: 'Invalid' }).entityAspect.acceptChanges();
            manager.createEntity('MedicationStatus', { id: 7, name: 'Duplicate' }).entityAspect.acceptChanges();

		    // Medication category enums
		    manager.createEntity('MedicationCategory', { id: 1, name: 'Medication' }).entityAspect.acceptChanges();
		    // manager.createEntity('MedicationCategory', { id: 2, name: 'Supplement' }).entityAspect.acceptChanges();

		    // Deceased enums
		    manager.createEntity('Deceased', { id: 1, name: 'Yes' }).entityAspect.acceptChanges();
		    manager.createEntity('Deceased', { id: 2, name: 'No' }).entityAspect.acceptChanges();
		}

		function checkDataContext() {
		    if (!datacontext) {
		        datacontext = require('services/datacontext');
		    }
		}
	});