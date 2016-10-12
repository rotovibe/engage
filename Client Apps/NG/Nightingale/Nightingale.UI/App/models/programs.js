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
        
	    var alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };

		var DT = breeze.DataType;

		// Expose the model module to the requiring modules
		var stepmodels = {
		    initialize: initialize,
		    extSaveAction: extSaveAction
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
		            eligibilityRequirements: { dataType: "String" },
		            eligibilityStartDate: { dataType: "DateTime" },
		            eligibilityEndDate: { dataType: "DateTime" },
		            enabled: { dataType: "Boolean" },
		            sourceId: { dataType: "String" },
		            programState: { dataType: "Int64" },
		            startDate: { dataType: "DateTime" },
		            endDate: { dataType: "DateTime" },
		            version: { dataType: "String" },
		            templateName: { dataType: "String" },
		            authoredBy: { dataType: "String" },
		            programVersion: { dataType: "String" },
		            templateVersion: { dataType: "String" },
		            programVersionUpdatedOn: { dataType: "DateTime" },
		            elementState: { dataType: "Int64" },
		            stateUpdatedOn: { dataType: "DateTime" },
                    assignDate: { dataType: "DateTime" },
                    assignById: { dataType: "String" },
                    assignToId: { dataType: "String" },
		            attrStartDate: { dataType: "DateTime" },
		            attrEndDate: { dataType: "DateTime" },
		            objectives: { complexTypeName: "Objective:#Nightingale", isScalar: false },
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
		            attributes: {
		                entityTypeName: "Attributes", isScalar: true,
		                associationName: "Program_Attributes"
		            },
		            elementStateModel: { 
		                entityTypeName: "ElementState", isScalar: true,
		                associationName: "Program_ElementState", foreignKeyNames: ["elementState"]
		            },
		            assignBy: {
		                entityTypeName: "CareManager", isScalar: true,
		                associationName: "Program_AssignedBy", foreignKeyNames: ["assignById"]
		            },
		            assignTo: {
		                entityTypeName: "CareManager", isScalar: true,
		                associationName: "Program_AssignedTo", foreignKeyNames: ["assignToId"]
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
		            elementState: { dataType: "Int64" },
		            objectives: { complexTypeName: "Objective:#Nightingale", isScalar: false },
                    assignDate: { dataType: "DateTime" },
                    assignById: { dataType: "String" },
                    assignToId: { dataType: "String" },
		            attrStartDate: { dataType: "DateTime" },
		            attrEndDate: { dataType: "DateTime" },
                    startDate: { dataType: "DateTime" },
                    endDate: { dataType: "DateTime" },
                    stateUpdatedOn: { dataType: "DateTime" },
                    authoredBy: { dataType: "String" },
                    moduleVersion: { dataType: "String" },
                    moduleVersionUpdatedOn: { dataType: "DateTime" }
		        },
		        navigationProperties: {
		            program: {
		                entityTypeName: "Program", isScalar: true,
		                associationName: "Program_Modules", foreignKeyNames: ["programId"]
		            },
		            elementStateModel: {
		                entityTypeName: "ElementState", isScalar: true,
		                associationName: "Module_ElementState", foreignKeyNames: ["elementState"]
		            },
		            actions: {
		                entityTypeName: "Action", isScalar: false,
		                associationName: "Module_Actions"
		            },
		            assignBy: {
		                entityTypeName: "CareManager", isScalar: true,
		                associationName: "Module_AssignedBy", foreignKeyNames: ["assignById"]
		            },
		            assignTo: {
		                entityTypeName: "CareManager", isScalar: true,
		                associationName: "Module_AssignedTo", foreignKeyNames: ["assignToId"]
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
		            description: { dataType: "String" },
		            moduleId: { dataType: "String" },
		            sourceId: { dataType: "String" },
		            order: { dataType: "Int64" },
		            completed: { dataType: "Boolean" },
		            archived: { dataType: "Boolean" },
                    archivedDate: { dataType: "DateTime" },
		            archiveOriginId: { dataType: "String" },
		            completedBy: { dataType: "String" },
                    enabled: { dataType: "Boolean" },
                    status: { dataType: "Int64" },
					deleteFlag: { dataType: "Boolean" },
                    elementState: { dataType: "Int64" },
		            stateUpdatedOn: { dataType: "DateTime" },
                    dateCompleted: { dataType: "DateTime" },
                    next: { dataType: "String" },
                    previous: { dataType: "String" },
                    assignDate: { dataType: "DateTime" },
                    assignById: { dataType: "String" },
                    assignToId: { dataType: "String" },
                    actionVersion: { dataType: "String" },
                    actionVersionUpdatedOn: { dataType: "DateTime" },
                    authoredBy: { dataType: "String" },
		            attrStartDate: { dataType: "DateTime" },
		            attrEndDate: { dataType: "DateTime" },
                    spawnElement: { complexTypeName: "SpawnElement:#Nightingale", isScalar: false },
		            objectives: { complexTypeName: "Objective:#Nightingale", isScalar: false }
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
		            },
		            assignBy: {
		                entityTypeName: "CareManager", isScalar: true,
		                associationName: "Action_AssignedBy", foreignKeyNames: ["assignById"]
		            },
		            assignTo: {
		                entityTypeName: "CareManager", isScalar: true,
		                associationName: "Action_AssignedTo", foreignKeyNames: ["assignToId"]
		            },
		            historicalAction: {
		                entityTypeName: "Action", isScalar: true,
		                associationName: "Action_HistoricalAction", foreignKeyNames: ["archiveOriginId"]
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

		    // Attributes complex type
		    metadataStore.addEntityType({
		        shortName: "Attributes",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            planElementId: { dataType: "String" },
                    authoredBy: { dataType: "String" },
                    ineligibleReason: { dataType: "String" },
                    eligibility: { dataType: "String" },
                    enrollment: { dataType: "String" },
                    graduatedFlag: { dataType: "String" },
                    population: { dataType: "String" },
                    removedReason: { dataType: "String" },
                    didNotEnrollReason: { dataType: "String" },
                    overrideReason: { dataType: "String" },
                    removedReason: { dataType: "String" },
                    completedBy: { dataType: "String" },
                    completed: { dataType: "Int64" },
                    dateCompleted: { dataType: "DateTime" },
                    market: { dataType: "String" }
		        },
		        navigationProperties: {		 
		            // module: {
		            //     entityTypeName: "Module", isScalar: true,
		            //     associationName: "Module_Attributes", foreignKeyNames: ["planElementId"]
		            // },
		            program: {
		                entityTypeName: "Program", isScalar: true,
		                associationName: "Program_Attributes", foreignKeyNames: ["planElementId"]
		            },
		            eligibilityModel: {
		                entityTypeName: "EligibilityStatus", isScalar: true,
		                associationName: "Attributes_Eligibility", foreignKeyNames: ["eligibility"]
		            },
		            enrollmentModel: {
		                entityTypeName: "EnrollmentStatus", isScalar: true,
		                associationName: "Attributes_Enrollment", foreignKeyNames: ["enrollment"]
		            },
		            graduatedFlagModel: {
		                entityTypeName: "YesOrNo", isScalar: true,
		                associationName: "Attributes_GraduatedFlag", foreignKeyNames: ["graduatedFlag"]
		            },
		            completedModel: {
		                entityTypeName: "YesOrNo", isScalar: true,
		                associationName: "Attributes_Completed", foreignKeyNames: ["completed"]
		            }
		        }
		    });

		    // Objective complex type
		    metadataStore.addEntityType({
		        shortName: "Objective",
		        namespace: "Nightingale",
		        isComplexType: true,
		        dataProperties: {
		            id: { dataType: "String" },
		            value: { dataType: "String" },
		            unit: { dataType: "String" },
		            status: { dataType: "Int64" },
		        }
		    });

		    // Objective cate complex type
		    //
		    // This is a look up that unfortunately
		    // matched the objective from above
		    metadataStore.addEntityType({
		        shortName: "ObjectiveLookup",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            categories: { complexTypeName: "IdName:#Nightingale", isScalar: false }
		        }
		    });

		    metadataStore.registerEntityTypeCtor('Program', null, programInitializer);
		    metadataStore.registerEntityTypeCtor('Module', null, moduleInitializer);
		    metadataStore.registerEntityTypeCtor('Objective', null, objectiveInitializer);
		    metadataStore.registerEntityTypeCtor('Action', null, actionInitializer);
		    metadataStore.registerEntityTypeCtor('Step', null, stepInitializer);

		    function programInitializer(program) {
		        if (!program.elementState()) {
		            program.elementState(1);
		        }
		        program.eligibilityDatesStatement = ko.computed(function () {
		        	var startdate = program.eligibilityStartDate();
		        	var enddate = program.eligibilityEndDate();
		        	if (!startdate && !enddate) { 
		        		return '';
		        	}
		        	var thistext = 'Eligibility ';
		        	if (startdate) {
                		startdate = moment(startdate).format('MM/DD/YYYY');
		        		thistext += ' begins on ' + startdate;
		        	}
		        	if (enddate) {
		        		enddate = moment(enddate).format('MM/DD/YYYY');
		        		if (startdate) {
		        			thistext += ' and ends on ' + enddate;
		        		} else {
		        			thistext += ' ends on ' + enddate;		        			
		        		}
		        	}
		        	return thistext;
		        });
		        program.duration = ko.computed(function () {
	                var startDate = moment(program.attrStartDate());
	                var today = moment();
	                var dateDiff = today.diff(startDate, 'days');
	                return dateDiff;
		        });
		        program.allModulesOpen = ko.computed(function () {
	                // Check if any modules aren't opened yet,
	                var firstClosed = ko.utils.arrayFirst(program.modules(), function (module) {
	                    return !module.isOpen();
	                });
	                return !firstClosed;
		        });
				
				/**
				*	concatenating the end date if it is populated to the program name.
				*	@method displayName
				*/
				program.displayName = ko.computed( function(){					
					var name = program.name();
					var endDate = program.attrEndDate();
					var elementState = program.elementState();
					if ( endDate && elementState == 5 || elementState == 6){
						var theMoment = moment( endDate );
						if( theMoment.isValid() ){
							name = name +' - ' + theMoment.format('MM/DD/YYYY');	
						}						
					}
					return name;
				});
                /*
				program.notesDisplayName = ko.observable();
                */
				program.nameByElementState = ko.computed(function () {				    
				    if (program.elementState() != 5 && program.elementState() != 6) {				            
				        return program.name();
				    }
				    //5 or 6
				    else {
				        var today = moment(new Date());
				        var stateUpdatedDate = moment(program.stateUpdatedOn());				            
				        return (program.name() + " - " + moment(program.assignDate())
                            .format('MM/DD/YYYY'));				            			            
				    }
				});
		    };

		    function moduleInitializer(module) {
		    	module.isOpen = ko.observable();
		    	// Set it to false either way for now
		    	if (module.elementState() !== 4 || module.completed()) {
		    		module.isOpen(false);
		    	} else {
		    		module.isOpen(false);
		    	}
		        module.completeActionsCount = ko.computed(function () {
	                var theseActions = module.actions();
	                var count = ko.utils.arrayFilter(theseActions, function (act) {
	                	return act.elementState() === 5 && act.enabled();
	                });
	                return count.length > 0 ? count.length : '-';
		        });
		        module.openActionsCount = ko.computed(function () {
	                var theseActions = module.actions();
	                var count = ko.utils.arrayFilter(theseActions, function (act) {
	                	return act.elementState() === 4 && act.enabled();
	                });
	                return count.length > 0 ? count.length : '-';
		        });
		        module.notStartedActionsCount = ko.computed(function () {
	                var theseActions = module.actions();
	                var count = ko.utils.arrayFilter(theseActions, function (act) {
	                	return act.elementState() === 2 && act.enabled();
	                });
	                return count.length > 0 ? count.length : '-';
		        });
		    };

		    function objectiveInitializer(objective) {
		    	checkDataContext();
				objective.objective = ko.computed(function () {
					var thisObjective;
					if (objective.id()) {
						thisObjective = ko.utils.arrayFirst(datacontext.enums.objectives(), function (obj) {
							return obj.id() === objective.id();
						});
					}
					return thisObjective;
				});
				objective.categoriesString = ko.computed(function () {
					var thisObjective;
					var thisString = '';
					if (objective.id()) {
						// Find the matching objective lookup
						thisObjective = ko.utils.arrayFirst(datacontext.enums.objectives(), function (obj) {
							return obj.id() === objective.id();
						});
						// If there is an objective found and it has categories,
						if (thisObjective && thisObjective.categories()) {
							// Create a category string with it's categories
							ko.utils.arrayForEach(thisObjective.categories(), function (cat) {
								thisString += cat.name() + ', ';
							});
							thisString = thisString.substring(0, thisString.length - 2);
						}
					}
					return thisString;
				});
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
				action.isLoading = ko.observable(false);
		        action.saveAction = function () {
		            setTimeout(function () {
		                action.isSaving(true);
		                saveAction(action);
		            }, 0);
		        };
		        action.formattedCompletedDate = ko.computed(function () {
	                var date = moment(action.dateCompleted());
	                var strDate = date.format('MM/DD/YYYY');
	                return strDate;
		        });
		        // Get all of this actions' history
		        action.history = ko.computed(function () {
		        	// Subscribe to the historical action
		        	var firstHistoricalAction = action.historicalAction();
		        	// Temporary array containing actions history
		        	var tempArray = [];
		        	// Recursively check for historical actions
		        	checkForHistory(action, tempArray);
		        	// Return the list of actions
		        	return tempArray;
		        });

		        function checkForHistory(action, array) {
		        	// Subscribe to the archive origin id property
		        	var subscription = action.archiveOriginId();
		        	if (action.historicalAction()) {
		        		array.push(action.historicalAction());
		        		checkForHistory(action.historicalAction(), array);
		        	}
		        }
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
		            		if (step.action().elementState() !== 4) {
		            			step.action().stateUpdatedOn(new moment().toISOString());
			                	step.action().elementState(4);
		            		}
			                step.selectedResponse(newValue);
			                if (newValue) {
			                	step.selectedResponseId(newValue.id());		                	
			                }		            		
		            	}
		            },
		            deferEvaluation: true
		        });
		        step.hasNoTrueAnswers = ko.computed(function () {
                    // If it has a type and that type is Checkbox,
		            if (step.stepType() && step.stepType().name() === 'Checkbox') {
                        // Try to find a response with value set to true
		                var thisAnswer = ko.utils.arrayFirst(step.responses(), function (rspnse) {
		                    return rspnse.value() === 'true';
		                });
                        // If an answer is found,
		                if (thisAnswer) {
                            // Return false
		                    return false;
		                }
                        // Else return true, meaning it has no true answers
		                return true;
		            }
		            // If we reach here return false
		            return false;
		        }).extend({ throttle: 25 });
		    };

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
		    };

		    function saveAction(action) {
		        checkDataContext();
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

		            return datacontext.saveAction(action, serializedAction, programId, patientId);
		        }, 50);

		        function saveCompleted() {
		        }
		    };
		}

		// Need to refactor all save action stuff to a service and 
		// have datacontext implement it
		function extSaveAction(action) {
	        checkDataContext();
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
	            action.entityAspect.acceptChanges();

	            return datacontext.saveAction(action, serializedAction, programId, patientId).then(saveCompleted);
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
	                datacontext.alerts.push(thisAlert);
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

		function checkDataContext() {
		    if (!datacontext) {
		        datacontext = require('services/datacontext');
		    }
		}
	});