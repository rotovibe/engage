// Register all of the models in the entity manager (initialize function) and provide other non-entity models
define(['config.services', 'services/session', 'services/entityserializer', 'services/dataservices/programsservice'],
	function (servicesConfig, session, entitySerializer, programsService) {

	    var datacontext;

	    var responseEndPoint = ko.computed({
	        read: function () {
	            var currentUser = session.currentUser();
	            if (!currentUser) {
	                return '';
	            }
	            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'response', 'Response');
	        },
	        deferEvaluation: true
	    });	        
        
	    self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };

		var DT = breeze.DataType;

		// Expose the model module to the requiring modules
		var stepmodels = {
		    initialize: initialize
		};
		return stepmodels;

		// Initialize the entity models in the entity manager
		function initialize(metadataStore) {

		    // ContractProgram information
		    metadataStore.addEntityType({
		        shortName: "ContractProgram",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            shortName: { dataType: "String" }
		        }
		    });

		    // Program information
		    metadataStore.addEntityType({
		        shortName: "Program",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            patientId: { dataType: "String" },
		            shortName: { dataType: "String" },
		            status: { dataType: "String" },
		            description: { dataType: "String" },
		            enabled: { dataType: "Boolean" },
		            sourceId: { dataType: "String" },
		            programState: { dataType: "Int64" },
		            startDate: { dataType: "DateTime" },
		            elementState: { dataType: "Int64" }
		        },
		        navigationProperties: {
		            modules: {
		                entityTypeName: "Module", isScalar: false,
		                associationName: "Program_Modules"
		            },
		            patient: {
		                entityTypeName: "Patient", isScalar: true,
		                associationName: "Patient_Programs", foreignKeyNames: ["patientId"]
		            },
		            elementStateModel: { 
		                entityTypeName: "ElementState", isScalar: true,
		                associationName: "Program_ElementState", foreignKeyNames: ["elementState"]
		            }
		        }
		    });

		    // Module information
		    metadataStore.addEntityType({
		        shortName: "Module",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            description: { dataType: "String" },
		            programId: { dataType: "String" },
		            sourceId: { dataType: "String" },
		            order: { dataType: "Int64" },
		            completed: { dataType: "Boolean" },
		            enabled: { dataType: "Boolean" },
		            status: { dataType: "Int64" },
		            dateCompleted: { dataType: "DateTime" },
		            next: { dataType: "String" },
		            previous: { dataType: "String" },
		            spawnElement: { complexTypeName: "SpawnElement:#Nightingale", isScalar: false },
		            elementState: { dataType: "Int64" }
		        },
		        navigationProperties: {
		            program: {
		                entityTypeName: "Program", isScalar: true,
		                associationName: "Program_Modules", foreignKeyNames: ["programId"]
		            },
		            actions: {
		                entityTypeName: "Action", isScalar: false,
		                associationName: "Module_Actions"
		            }
		        }
		    });

		    // Action information
		    metadataStore.addEntityType({
		        shortName: "Action",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            moduleId: { dataType: "String" },
		            sourceId: { dataType: "String" },
		            order: { dataType: "Int64" },
		            completed: { dataType: "Boolean" },
		            completedBy: { dataType: "String" },
                    enabled: { dataType: "Boolean" },
                    status: { dataType: "Int64" },
                    elementState: { dataType: "Int64" },
                    dateCompleted: { dataType: "DateTime" },
                    next: { dataType: "String" },
                    previous: { dataType: "String" },
                    spawnElement: { complexTypeName: "SpawnElement:#Nightingale", isScalar: false }
		        },
		        navigationProperties: {
		            module: {
		                entityTypeName: "Module", isScalar: true,
		                associationName: "Module_Actions", foreignKeyNames: ["moduleId"]
		            },
		            steps: {
		                entityTypeName: "Step", isScalar: false,
		                associationName: "Action_Steps"
		            },
		            elementStateModel: {
		                entityTypeName: "ElementState", isScalar: true,
		                associationName: "Action_ElementState", foreignKeyNames: ["elementState"]
		            }
		        }
		    });

		    // Step information
		    metadataStore.addEntityType({
		        shortName: "Step",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            header: { dataType: "String" },
		            description: { dataType: "String" },
		            title: { dataType: "String" },
		            text: { dataType: "String" },
		            question: { dataType: "String" },
		            notes: { dataType: "String" },
		            actionId: { dataType: "String" },
		            sourceId: { dataType: "String" },
		            stepTypeId: { dataType: "String" },
		            selectedResponseId: { dataType: "String" },
		            order: { dataType: "Int64" },
		            controlType: { dataType: "Int64" },
		            selectType: { dataType: "Int64" },
		            completed: { dataType: "Boolean" },
		            enabled: { dataType: "Boolean" },
		            spawnElement: { complexTypeName: "SpawnElement:#Nightingale", isScalar: false },
                    status: { dataType: "Int64" }
		        },
		        navigationProperties: {
		            action: {
		                entityTypeName: "Action", isScalar: true,
		                associationName: "Action_Steps", foreignKeyNames: ["actionId"]
		            },
		            stepType: {
		                entityTypeName: "StepType", isScalar: true,
		                associationName: "Step_StepType", foreignKeyNames: ["stepTypeId"]
		            },
		            responses: {
		                entityTypeName: "Response", isScalar: false,
		                associationName: "Step_Responses"
		            }
		        }
		    });

		    // Response entity
		    metadataStore.addEntityType({
		        shortName: "Response",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            text: { dataType: "String" },
		            required: { dataType: "Boolean" },
		            value: { dataType: "String" },
		            stepId: { dataType: "String" },
		            nominal: { dataType: "Boolean" },
		            order: { dataType: "Int64" },
		            spawnElement: { complexTypeName: "SpawnElement:#Nightingale", isScalar: false },
		            nextStepId: { dataType: "String" }
		        },
		        navigationProperties: {
		            step: {
		                entityTypeName: "Step", isScalar: true,
		                associationName: "Step_Responses", foreignKeyNames: ["stepId"]
		            }
		        }
		    });

		    // Step Type entity
		    metadataStore.addEntityType({
		        shortName: "StepType",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "Int64", isPartOfKey: true },
		            name: { dataType: "String" },
		            path: { dataType: "String" }
		        },
		        navigationProperties: {
		            steps: {
		                entityTypeName: "Step", isScalar: false,
		                associationName: "Step_StepType"
		            }
		        }
		    });

		    // Spawn Element complex type
		    metadataStore.addEntityType({
		        shortName: "SpawnElement",
		        namespace: "Nightingale",
		        isComplexType: true,
		        dataProperties: {
		            elementId: { dataType: "String" },
		            elementType: { dataType: "String" },
                    tag: { dataType: "String" }
		        }
		    });

		    metadataStore.registerEntityTypeCtor('Program', null, programInitializer);
		    metadataStore.registerEntityTypeCtor('Action', null, actionInitializer);
		    metadataStore.registerEntityTypeCtor('Step', null, stepInitializer);

		    function programInitializer(program) {
		        if (!program.elementState()) {
		            program.elementState(1);
		        }
		    };

		    function actionInitializer(action) {
		        action.computedPath = ko.computed({
		            read: function () {
		            	// Make sure the datacontext is set up
		        		checkDataContext();
		            	// At the beginning, set a variable to hold whether an inactive step is found yet
		            	var inactiveStepFound = false;
		                var theseSteps = action.steps().sort(alphabeticalOrderSort);
		                var computedSteps = [];
		                if (theseSteps.length === 0) { return theseSteps; }
		                // Get the first step, and halt the loop when you reach 'Complete'
		                var isFirstStep = true;
		                for (var step = theseSteps[0]; !!step;) {
		                    // If it's the first step,
		                    if (isFirstStep) {
		                        // Set it to active
		                        step.activeStep(true);
		                    }
		                    // Push this step into these steps
		                    computedSteps.push(step);
		                    var theNextStep = findNextStep(step, inactiveStepFound);
		                    // If the next step was returned as inactive,
		                    if (theNextStep && !theNextStep.activeStep()) {
		                    	// Then an inactive step has been found
		                    	inactiveStepFound = true;
		                    }
		                    // If no next step is returned, break out of the loop
		                    if (!theNextStep) { break; }
		                    step = theNextStep;
		                    isFirstStep = false;
		                }
		                return computedSteps;
		            },
		            deferEvaluation: true
		        }).extend({ throttle: 50});
		        action.hasChanges = ko.computed(function () {
		            return action.elementState() === 4;
		        });
		        action.isSaving = ko.observable(false);
		        action.saveAction = function () {
		            setTimeout(function () {
		                action.isSaving(true);
		                saveAction(action);
		            }, 0);
		        };
		    };

		    function stepInitializer(step) {
		        checkDataContext();
		        step.activeStep = ko.observable(false);
		        step.selectedResponse = ko.observable();
		        step.computedResponse = ko.computed({   // function () {
		            read: function () {
                        // If there is no selectedResponseId then return null
		                if (step.selectedResponseId() === 0 || !step.selectedResponseId()) {
		                    return null;
		                }
                        // If not then go get it (have to require datacontext here due to a circular dependency)
		                datacontext.getEntityById(step.selectedResponse, step.selectedResponseId(), 'Response', 'fakePath1', false);
		                return step.selectedResponse();
		            },
		            write: function (newValue) {
		            	if (newValue !== undefined) {
			                step.action().elementState(4);
			                step.selectedResponse(newValue);
			                if (newValue) {
			                	step.selectedResponseId(newValue.id());		                	
			                }		            		
		            	}
		            },
		            deferEvaluation: true
		        });
		    }

		    function findNextStep(step, inactiveStepFound) {

		    	// Anonymous function
		        function getNextStep(stepId, setActive) {
		            // Create an observable to catch the result of getEntityById
		            var nextStepObservable = ko.observable();
                    // Go get the next step
		            datacontext.getEntityById(nextStepObservable, stepId, 'Step', 'fakePath2', false);
                    // If nextStepObservable has a value
		            if (nextStepObservable()) {
                        // Set the step as the value of activestepFound
		                nextStepObservable().activeStep(setActive);
		            }
		            return nextStepObservable();
		        }

		        var nextStep = null;
		        var isCompleteStep = step.stepType().name() === 'Complete';
		        // Set a var equal to the response is not required or it is required and has a value
		        var ifRequiredHasValue = (!!step.computedResponse() && (!step.computedResponse().required() || (!!step.computedResponse().required() && !!step.computedResponse().value())));
		        var ifIsSingleSelectAndRequiredHasValue = (!!step.computedResponse() && (!step.computedResponse().required() || (!!step.computedResponse().required() && (step.stepType().name() === 'SingleSelect' || step.stepType().name() === 'Radio' ))));
		        // If a Response has been selected and it has a next step and it's not required and it's not a complete step,		        
		        if (step.computedResponse() && step.computedResponse().nextStepId() && ifRequiredHasValue && !isCompleteStep) {
		            // Return the next step as the next step 
		            nextStep = getNextStep(step.computedResponse().nextStepId(), !inactiveStepFound);
		        }
		        else if (step.computedResponse() && step.computedResponse().nextStepId() && ifIsSingleSelectAndRequiredHasValue && !isCompleteStep) {
		        	nextStep = getNextStep(step.computedResponse().nextStepId(), !inactiveStepFound);	
		        }
		        // If not and the step.Responses does not have a length of zero and it's not a complete step,
		        else if (step.responses().length !== 0 && !isCompleteStep) {
		            // Iterate through the responses to look for a nominal answer
		            ko.utils.arrayForEach(step.responses(), function (response) {
		                if (response.nominal() === true) {
		                    // If the step's response is required and there is a value,
		                    if (response.required() === true && !response.value()) {
		                    	inactiveStepFound = true;
		                    }
		                    // Set the nominal response as the next step
		                    nextStep = getNextStep(response.nextStepId(), !inactiveStepFound);
		                }
		            });
		        }
		        else {
		            nextStep = null;
		        }
                // If it is a complete step,
		        if (isCompleteStep) {
		            // Tack on a completeAction property, if it doesn't already have one
		            if (!step.completeAction) {
		                step.completeAction = function () {
		                    var action = step.action();
		                    setTimeout(function () {
		                        action.dateCompleted(new moment().toISOString());
		                        action.completed(true);
		                        action.isSaving(true);
		                        saveAction(action);
		                    }, 0);
		                };
		            }
		        }
		        return nextStep;
		    }

		    function saveAction(action) {
		        setTimeout(function () {
		            // Cancel changes for everything not in the computed path
		            programsService.cancelChangesForNonComputedPath(action);

		            // If the action is set to completed,
		            if (action.completed()) {
		            	// Set the completed steps to true
		            	programsService.setCompletedSteps(action);
		            }

		            var serializedAction = entitySerializer.serializeAction(action, datacontext.manager);

		            // Get the id of the patient that this action is for
		            var patientId = action.module().program().patientId();

		            // Get the id of the patient that this action is for
		            var programId = action.module().program().id();

		            return datacontext.saveAction(serializedAction, programId, patientId).then(saveCompleted);
		        }, 50);

		        function saveCompleted() {
		            // If the action has been completed,
		            if (action.completed()) {
		                // Make sure the state is set to 2
		                action.elementState(5);
		            }
		            // Else if the element state is 4 (in progress) when saved,
		            else if (action.elementState() === 4) {
		                // Display an alert that progress was saved
		                var thisAlert = datacontext.createEntity('Alert', { result: 'warning', reason: 'Progress saved' });
		                thisAlert.entityAspect.acceptChanges();
		                datacontext.enums.alerts.push(thisAlert);
		            }
		            // Save the action and all of it's steps and responses
		            action.entityAspect.acceptChanges();

                    // Save changes to all of the steps and responses
		            ko.utils.arrayForEach(action.steps(), function (step) {
		                step.entityAspect.acceptChanges();
		                ko.utils.arrayForEach(step.responses(), function (response) {
		                    response.entityAspect.acceptChanges();
		                });
		            });
		            // Save the action's module
		            action.module().entityAspect.acceptChanges();
		            action.isSaving(false);
		        }
		    }
		}

		function checkDataContext() {
		    if (!datacontext) {
		        datacontext = require('services/datacontext');
		    }
		}
	});