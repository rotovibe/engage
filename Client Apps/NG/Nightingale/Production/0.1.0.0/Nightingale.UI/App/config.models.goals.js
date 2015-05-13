// Register all of the user related models in the entity manager (initialize function) and provide other non-entity models
define(['services/session'],
	function (session) {

	    var datacontext;

		var DT = breeze.DataType;

		// Expose the model module to the requiring modules
		var goalModels = {
		    initialize: initialize,
		    createMocks: createMocks
		};
		return goalModels;

		// Initialize the entity models in the entity manager
		function initialize(metadataStore) {

		    // Goal information
		    metadataStore.addEntityType({
		        shortName: "Goal",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            sourceId: { dataType: "String" },
		            statusId: { dataType: "String" },
		            typeId: { dataType: "String" },
		            patientId: { dataType: "String" },
		            targetValue: { dataType: "String" },
		            startDate: { dataType: "DateTime" },
		            endDate: { dataType: "DateTime" },
		            targetDate: { dataType: "DateTime" },
		            focusAreaIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
		            programIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
		            customAttributes: { complexTypeName: "Attribute:#Nightingale", isScalar: false }
		        },
		        navigationProperties: {
		            source: {
		                entityTypeName: "Source", isScalar: true,
		                associationName: "Goal_Source", foreignKeyNames: ["sourceId"]
		            },
		            type: {
		                entityTypeName: "GoalType", isScalar: true,
		                associationName: "Goal_Type", foreignKeyNames: ["typeId"]
		            },
		            status: {
		                entityTypeName: "GoalTaskStatus", isScalar: true,
		                associationName: "Goal_Status", foreignKeyNames: ["statusId"]
		            },
		            patient: {
		                entityTypeName: "Patient", isScalar: true,
		                associationName: "Patient_Goals", foreignKeyNames: ["patientId"]
		            },
		            tasks: {
		                entityTypeName: "Task", isScalar: false,
		                associationName: "Goal_Tasks"
		            },
		            barriers: {
		                entityTypeName: "Barrier", isScalar: false,
		                associationName: "Goal_Barriers"
		            },
		            interventions: {
		                entityTypeName: "Intervention", isScalar: false,
		                associationName: "Goal_Interventions"
		            }
		        }
		    });

		    // Attribute complex type
		    metadataStore.addEntityType({
		        shortName: "Attribute",
		        namespace: "Nightingale",
		        isComplexType: true,
		        dataProperties: {
		            id: { dataType: "String" },
		            name: { dataType: "String" },
		            values: { complexTypeName: "Value:#Nightingale", isScalar: false },
		            controlType: { dataType: "Int64" },
		            order: { dataType: "Int64" },
		            options: { complexTypeName: "ValueDisplay:#Nightingale", isScalar: false }
		        }
		    });

		    // Value complex type
		    metadataStore.addEntityType({
		        shortName: "Value",
		        namespace: "Nightingale",
		        isComplexType: true,
		        dataProperties: {
		            value: { dataType: "String" }
		        }
		    });

		    // Barrier information
		    metadataStore.addEntityType({
		        shortName: "Barrier",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            categoryId: { dataType: "String" },
		            statusId: { dataType: "String" },
		            patientGoalId: { dataType: "String" }
		        },
		        navigationProperties: {
		            category: {
		                entityTypeName: "BarrierCategory", isScalar: true,
		                associationName: "Barrier_Category", foreignKeyNames: ["categoryId"]
		            },
		            status: {
		                entityTypeName: "BarrierStatus", isScalar: true,
		                associationName: "Barrier_Status", foreignKeyNames: ["statusId"]
		            },
		            goal: {
		                entityTypeName: "Goal", isScalar: true,
		                associationName: "Goal_Barriers", foreignKeyNames: ["patientGoalId"]
		            }
		        }
		    });

		    // Task information
		    metadataStore.addEntityType({
		        shortName: "Task",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            description: { dataType: "String" },
		            statusId: { dataType: "String" },
		            targetValue: { dataType: "String" },
		            startDate: { dataType: "DateTime" },
		            targetDate: { dataType: "DateTime" },
		            patientGoalId: { dataType: "String" },
		            barrierIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
		            customAttributes: { complexTypeName: "Attribute:#Nightingale", isScalar: false }
		        },
		        navigationProperties: {
		            status: {
		                entityTypeName: "GoalTaskStatus", isScalar: true,
		                associationName: "Task_Status", foreignKeyNames: ["statusId"]
		            },
		            goal: {
		                entityTypeName: "Goal", isScalar: true,
		                associationName: "Goal_Tasks", foreignKeyNames: ["patientGoalId"]
		            }
		        }
		    });

		    // Intervention information
		    metadataStore.addEntityType({
		        shortName: "Intervention",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            description: { dataType: "String" },
		            statusId: { dataType: "String" },
		            categoryId: { dataType: "String" },
		            startDate: { dataType: "DateTime" },
		            patientGoalId: { dataType: "String" },
                    assignedToId: { dataType: "String" },
		            barrierIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false }
		        },
		        navigationProperties: {
		            status: {
		                entityTypeName: "InterventionStatus", isScalar: true,
		                associationName: "Intervention_Status", foreignKeyNames: ["statusId"]
		            },
		            category: {
		                entityTypeName: "InterventionCategory", isScalar: true,
		                associationName: "Intervention_Status", foreignKeyNames: ["categoryId"]
		            },
		            goal: {
		                entityTypeName: "Goal", isScalar: true,
		                associationName: "Goal_Interventions", foreignKeyNames: ["patientGoalId"]
		            },
		            assignedTo: {
		                entityTypeName: "CareManager", isScalar: true,
		                associationName: "Intervention_CareManager", foreignKeyNames: ["assignedToId"]
		            }
		        }
		    });

		    // Id / Name pair
		    metadataStore.addEntityType({
		        shortName: "IdName",
		        namespace: "Nightingale",
		        isComplexType: true,
		        dataProperties: {
		            id: { dataType: "String" },
		            name: { dataType: "String" }
		        }
		    });

		    // Value / Display pair
		    metadataStore.addEntityType({
		        shortName: "ValueDisplay",
		        namespace: "Nightingale",
		        isComplexType: true,
		        dataProperties: {
		            value: { dataType: "String" },
		            display: { dataType: "String" }
		        }
		    });

		    // Focus Area information
		    metadataStore.addEntityType({
		        shortName: "FocusArea",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // Source information
		    metadataStore.addEntityType({
		        shortName: "Source",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // Barrier category information
		    metadataStore.addEntityType({
		        shortName: "BarrierCategory",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // Intervention category information
		    metadataStore.addEntityType({
		        shortName: "InterventionCategory",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });
            
		    // Intervention status information
		    metadataStore.addEntityType({
		        shortName: "InterventionStatus",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // Barrier status information
		    metadataStore.addEntityType({
		        shortName: "BarrierStatus",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // Goal Task status information
		    metadataStore.addEntityType({
		        shortName: "GoalTaskStatus",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // Goal Type information
		    metadataStore.addEntityType({
		        shortName: "GoalType",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });

		    // Care manager information
		    metadataStore.addEntityType({
		        shortName: "CareManager",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            userId: { dataType: "String", isPartOfKey: true },
		            preferredName: { dataType: "String" }
		        }
		    });

		    metadataStore.registerEntityTypeCtor(
				'Goal', null, goalInitializer);
            metadataStore.registerEntityTypeCtor(
				'Intervention', null, interventionInitializer);
            metadataStore.registerEntityTypeCtor(
				'Task', null, taskInitializer);
            metadataStore.registerEntityTypeCtor(
				'Attribute', null, attributeInitializer);

            function goalInitializer(goal) {
                goal.isNew = ko.observable(false);
		        goal.focusAreaString = ko.computed(function () {
		            checkDataContext();
		            var thisString = '';
		            ko.utils.arrayForEach(goal.focusAreaIds(), function (focusArea) {
		                var thisFocusArea = ko.utils.arrayFirst(datacontext.enums.focusAreas(), function (faEnum) {
		                    return faEnum.id() === focusArea.id();
		                });
		                thisString += thisFocusArea.name() + ', ';
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
		        goal.programString = ko.computed(function () {
		            checkDataContext();
		            var thisString = '';
		            var theseProgramIds = goal.programIds();
		            if (goal.patient() && goal.patient().programs()) {
		                var thesePrograms = goal.patient().programs();
		                ko.utils.arrayForEach(theseProgramIds, function (program) {
		                    var thisProgram = ko.utils.arrayFirst(thesePrograms, function (programEnum) {
		                        return programEnum.id() === program.id();
		                    });
		                    thisString += thisProgram.name() + ', ';
		                });
		                // If the string is longer than zero,
		                if (thisString.length > 0) {
		                    // Remove the trailing comma and space
		                    thisString = thisString.substr(0, thisString.length - 2);
		                }
		            }
		            if (thisString.length === 0) {
		                thisString = 'None';
		            }
		            return thisString;
		        });
		    }

		    function interventionInitializer(intervention) {
		        intervention.barrierString = ko.computed(function () {
		            checkDataContext();
		            var thisString = '';
		            if (intervention.goal()) {
		                ko.utils.arrayForEach(intervention.barrierIds(), function (barrier) {
		                    var thisBarrier = ko.utils.arrayFirst(intervention.goal().barriers(), function (baEnum) {
		                        return baEnum.id() === barrier.id();
		                    });
		                    thisString += thisBarrier.name() + ', ';
		                });
		                // If the string is longer than zero,
		                if (thisString.length > 0) {
		                    // Remove the trailing comma and space
		                    thisString = thisString.substr(0, thisString.length - 2);
		                }
		            }
		            if (thisString.length === 0) {
		                thisString = 'None';
		            }
		            return thisString;
		        });
		    }

		    function taskInitializer(task) {
		        task.barrierString = ko.computed(function () {
		            checkDataContext();
		            var thisString = '';
		            if (task.goal()) {
		                ko.utils.arrayForEach(task.barrierIds(), function (barrier) {
		                    var thisBarrier = ko.utils.arrayFirst(task.goal().barriers(), function (baEnum) {
		                        return baEnum.id() === barrier.id();
		                    });
		                    thisString += thisBarrier.name() + ', ';
		                });
		                // If the string is longer than zero,
		                if (thisString.length > 0) {
		                    // Remove the trailing comma and space
		                    thisString = thisString.substr(0, thisString.length - 2);
		                }
		            }
		            if (thisString.length === 0) {
		                thisString = 'None';
		            }
		            return thisString;
		        });
		    }

		    function attributeInitializer(attribute) {
		        checkDataContext();
		        attribute.path = ko.computed(function () {
		            var thisControlType = attribute.controlType();
		            var thisPath = '';
		            if (thisControlType) {
		                if (thisControlType === 1) {
		                    thisPath = 'programdesigner/customattributes/singleselect.edit.html';
		                }
		                else if (thisControlType === 2) {
		                    thisPath = 'programdesigner/customattributes/multiselect.edit.html';
		                }
		                else if (thisControlType === 3) {
		                    thisPath = 'programdesigner/customattributes/datepicker.edit.html';
		                }
		                else if (thisControlType === 4) {
		                    thisPath = 'programdesigner/customattributes/datetimepicker.edit.html';
		                }
		                else if (thisControlType === 5) {
		                    thisPath = 'programdesigner/customattributes/input.edit.html';
		                }
		            }
		            return thisPath;
		        });
		        attribute.displayPath = ko.computed(function () {
		            var thisControlType = attribute.controlType();
		            var thisPath = '';
		            if (thisControlType) {
		                if (thisControlType === 1) {
		                    thisPath = 'programdesigner/customattributes/singleselect.html';
		                }
		                else if (thisControlType === 2) {
		                    thisPath = 'programdesigner/customattributes/multiselect.html';
		                }
		                else if (thisControlType === 3) {
		                    thisPath = 'programdesigner/customattributes/datepicker.html';
		                }
		                else if (thisControlType === 4) {
		                    thisPath = 'programdesigner/customattributes/datetimepicker.html';
		                }
		                else if (thisControlType === 5) {
		                    thisPath = 'programdesigner/customattributes/input.html';
		                }
		            }
		            return thisPath;
		        });
		        if (attribute.values().length === 0) {
		            var thisValue = datacontext.createComplexType('Value', {});
		            attribute.values.push(thisValue);
		        }
		        attribute.computedValue = ko.computed({
		            read: function () {
		                var returnValue = null;
		                if (attribute.values().length === 1) {
		                    returnValue = attribute.values()[0];
		                }
		                else if (attribute.values().length > 1) {
		                    returnValue = attribute.values();
		                }
		                return returnValue;
		            },
		            write: function (newValue) {
                        // If attribute is a multiselect,
		                if (attribute.controlType === 2) {
                            // TODO: Do something
		                }
		                else {
		                    attribute.values()[0].value(newValue);
		                }
		            }
		        });
		    }
		}

		function createMocks(manager) {
		    //var goalOne = manager.createEntity('Goal', { id: 'goal1', name: 'Improve HDL', patientId: '52f55873072ef709f84e6810' });
		}

		function checkDataContext() {
		    if (!datacontext) {
		        datacontext = require('services/datacontext');
		    }
		}
	});