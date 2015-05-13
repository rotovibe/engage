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
		            imageSource: { dataType: "String" }
		        },
		        navigationProperties: {
		            patients: {
		                entityTypeName: "Patient", isScalar: false,
		                associationName: "Priority_Patients"
		            }
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
		    manager.createEntity('Priority', { id: 0, levelName: 'Not Set', imageSource: '/NightingaleUI/Content/images/priority_empty.png' }).entityAspect.acceptChanges();
		    manager.createEntity('Priority', { id: 1, levelName: 'Low', imageSource: '/NightingaleUI/Content/images/priority_low.png' }).entityAspect.acceptChanges();
		    manager.createEntity('Priority', { id: 2, levelName: 'Medium', imageSource: '/NightingaleUI/Content/images/priority_medium.png' }).entityAspect.acceptChanges();
		    manager.createEntity('Priority', { id: 3, levelName: 'High', imageSource: '/NightingaleUI/Content/images/priority_high.png' }).entityAspect.acceptChanges();

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
		    manager.createEntity('GoalTaskStatus', { id: 1, name: 'Open' }).entityAspect.acceptChanges();
		    manager.createEntity('GoalTaskStatus', { id: 2, name: 'Met' }).entityAspect.acceptChanges();
		    manager.createEntity('GoalTaskStatus', { id: 3, name: 'Not Met' }).entityAspect.acceptChanges();
		    manager.createEntity('GoalTaskStatus', { id: 4, name: 'Abandoned' }).entityAspect.acceptChanges();

		    // Barrier status enums
		    manager.createEntity('BarrierStatus', { id: 1, name: 'Open' }).entityAspect.acceptChanges();
		    manager.createEntity('BarrierStatus', { id: 2, name: 'Resolved' }).entityAspect.acceptChanges();

		    // Intervention status enums
		    manager.createEntity('InterventionStatus', { id: 1, name: 'Open' }).entityAspect.acceptChanges();
		    manager.createEntity('InterventionStatus', { id: 2, name: 'Completed' }).entityAspect.acceptChanges();
		    manager.createEntity('InterventionStatus', { id: 3, name: 'Removed' }).entityAspect.acceptChanges();

		    // Goal type enums
		    manager.createEntity('GoalType', { id: 1, name: 'Long-Term' }).entityAspect.acceptChanges();
		    manager.createEntity('GoalType', { id: 2, name: 'Short-Term' }).entityAspect.acceptChanges();

		}

		function checkDataContext() {
		    if (!datacontext) {
		        datacontext = require('services/datacontext');
		    }
		}
	});