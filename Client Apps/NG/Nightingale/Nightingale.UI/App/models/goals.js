define(['services/session'],
    function (session) {

        var datacontext;

        var DT = breeze.DataType;

        var goalModels = {
            initialize: initialize,
            createMocks: createMocks
        };
        return goalModels;

        function initialize(metadataStore) {

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
                    customAttributes: { complexTypeName: "Attribute:#Nightingale", isScalar: false },
                    details: { dataType: "String" },
                    newDetails:  { dataType: "String" }
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

            metadataStore.addEntityType({
                shortName: "Value",
                namespace: "Nightingale",
                isComplexType: true,
                dataProperties: {
                    value: { dataType: "String" }
                }
            });

            metadataStore.addEntityType({
                shortName: "Barrier",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    name: { dataType: "String" },
                    categoryId: { dataType: "String" },
                    statusId: { dataType: "String" },
                    patientGoalId: { dataType: "String" },
                    deleteFlag: { dataType: "Boolean" },
                    details: { dataType: "String" },
                    newDetails:  { dataType: "String" }
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

            metadataStore.addEntityType({
                shortName: "Task",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    description: { dataType: "String" },
                    statusId: { dataType: "String" },
                    createdById: { dataType: "String" },
                    targetValue: { dataType: "String" },
                    startDate: { dataType: "DateTime" },
                    targetDate: { dataType: "DateTime" },
                    closedDate: { dataType: "DateTime" },
                    statusDate: { dataType: "DateTime" },
                    goalName: { dataType: "String" },
                    patientGoalId: { dataType: "String" },
                    patientId: { dataType: "String" },
                    barrierIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
                    customAttributes: { complexTypeName: "Attribute:#Nightingale", isScalar: false },
                    deleteFlag: { dataType: "Boolean" },
                    details: { dataType: "String" },
                    newDetails:  { dataType: "String" }
                },
                navigationProperties: {
                    status: {
                        entityTypeName: "GoalTaskStatus", isScalar: true,
                        associationName: "Task_Status", foreignKeyNames: ["statusId"]
                    },
                    goal: {
                        entityTypeName: "Goal", isScalar: true,
                        associationName: "Goal_Tasks", foreignKeyNames: ["patientGoalId"]
                    },
                    patientDetails: {
                        entityTypeName: "ToDoPatient", isScalar: true,
                        associationName: "Task_PatientDetails", foreignKeyNames: ["patientId"]
                    }
                }
            });

            metadataStore.addEntityType({
                shortName: "Intervention",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    description: { dataType: "String" },
                    statusId: { dataType: "String" },
                    createdById: { dataType: "String" },
                    categoryId: { dataType: "String" },
                    startDate: { dataType: "DateTime" },
                    dueDate: { dataType: "DateTime" },
                    closedDate: { dataType: "DateTime" },
                    goalName: { dataType: "String" },
                    patientGoalId: { dataType: "String" },
                    patientId: { dataType: "String" },
                    assignedToId: { dataType: "String" },
                    barrierIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
                    deleteFlag: { dataType: "Boolean" },
                    details: { dataType: "String" },
                    newDetails:  { dataType: "String" }
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
                    },
                    patientDetails: {
                        entityTypeName: "ToDoPatient", isScalar: true,
                        associationName: "Intervention_PatientDetails", foreignKeyNames: ["patientId"]
                    }
                }
            });

            metadataStore.addEntityType({
                shortName: "PatientDetails",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    firstName: { dataType: "String" },
                    lastName: { dataType: "String" },
                    middleName: { dataType: "String" },
                    suffix: { dataType: "String" },
                    preferredName: { dataType: "String" }
                }
            });

            metadataStore.addEntityType({
                shortName: "IdName",
                namespace: "Nightingale",
                isComplexType: true,
                dataProperties: {
                    id: { dataType: "String" },
                    name: { dataType: "String" }
                }
            });

            metadataStore.addEntityType({
                shortName: "ValueDisplay",
                namespace: "Nightingale",
                isComplexType: true,
                dataProperties: {
                    value: { dataType: "String" },
                    display: { dataType: "String" }
                }
            });

            metadataStore.addEntityType({
                shortName: "FocusArea",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    name: { dataType: "String" }
                }
            });

            metadataStore.addEntityType({
                shortName: "Source",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    name: { dataType: "String" }
                }
            });

            metadataStore.addEntityType({
                shortName: "BarrierCategory",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    name: { dataType: "String" }
                }
            });

            metadataStore.addEntityType({
                shortName: "InterventionCategory",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    name: { dataType: "String" }
                }
            });

            metadataStore.addEntityType({
                shortName: "InterventionStatus",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    name: { dataType: "String" },
                    iconClass: { dataType: "String" },
                    textClass: { dataType: "String" }
                }
            });

            metadataStore.addEntityType({
                shortName: "BarrierStatus",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    name: { dataType: "String" },
                    iconClass: { dataType: "String" },
                    textClass: { dataType: "String" }
                }
            });

            metadataStore.addEntityType({
                shortName: "GoalTaskStatus",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    name: { dataType: "String" },
                    order: { dataType: "Int64" },
                    iconClass: { dataType: "String" },
                    textClass: { dataType: "String" }
                }
            });

            metadataStore.addEntityType({
                shortName: "GoalType",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    name: { dataType: "String" }
                }
            });

            metadataStore.addEntityType({
                shortName: "CareManager",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    userId: { dataType: "String" },
                    preferredName: { dataType: "String" },
                    firstName: { dataType: "String" },
                    lastName: { dataType: "String" }
                }
            });

            metadataStore.registerEntityTypeCtor(
                'Goal', null, goalInitializer);
            metadataStore.registerEntityTypeCtor(
                'Barrier', null, barrierInitializer);
            metadataStore.registerEntityTypeCtor(
                'Intervention', null, interventionInitializer);
            metadataStore.registerEntityTypeCtor(
                'Task', null, taskInitializer);
            metadataStore.registerEntityTypeCtor(
                'Attribute', null, attributeInitializer);
            metadataStore.registerEntityTypeCtor(
                'CareManager', null, careManagerInitializer);

            function barrierInitializer(barrier) {
                barrier.isNew = ko.observable(false);
                barrier.checkAppend = function () {
                    appendNewDetails( barrier.newDetails, barrier.details );
                };

                barrier.relatedInterventions = ko.computed(function () {
                    checkDataContext();
                    var interventionList = [];
                    if (barrier.goal()) {
                        ko.utils.arrayForEach(barrier.goal().interventions(), function (intervention) {
                            var thisBarrierId = ko.utils.arrayFirst(intervention.barrierIds(), function (baEnum) {
                                return baEnum.id() === barrier.id();
                            });
                            if (thisBarrierId) {
                                interventionList.push(intervention);
                            }
                        });
                    }
                    return interventionList;
                });

                barrier.relatedTasks = ko.computed(function () {
                    checkDataContext();
                    var taskList = [];
                    if (barrier.goal()) {
                        ko.utils.arrayForEach(barrier.goal().tasks(), function (task) {
                            var thisBarrierId = ko.utils.arrayFirst(task.barrierIds(), function (baEnum) {
                                return baEnum.id() === barrier.id();
                            });
                            if (thisBarrierId) {
                                taskList.push(task);
                            }
                        });
                    }
                    return taskList;
                });

                barrier.taskListString = ko.computed(function () {
                    checkDataContext();
                    var thisString = '';
                    var tasks = barrier.relatedTasks();
                    ko.utils.arrayForEach(tasks, function (task) {
                        thisString += task ? task.description() + ', ' : '';
                    });
                    if (thisString.length > 0) {
                        thisString = thisString.substr(0, thisString.length - 2);
                    } else if (thisString.length === 0) {
                        thisString = 'None';
                    }
                    return thisString;
                });

                barrier.interventionListString = ko.computed(function () {
                    checkDataContext();
                    var thisString = '';
                    var interventions = barrier.relatedInterventions();
                    ko.utils.arrayForEach(interventions, function (intervention) {
                        thisString += intervention ? intervention.description() + ', ' : '';
                    });
                    if (thisString.length > 0) {
                        thisString = thisString.substr(0, thisString.length - 2);
                    } else if (thisString.length === 0) {
                        thisString = 'None';
                    }
                    return thisString;
                });

                barrier.validationErrors = ko.observableArray([]);
                barrier.isValid = ko.computed( function() {
                    var hasErrors = false;
                    var barrierErrors = [];
                    var name = barrier.name();
                    if( !name ){
                        barrierErrors.push({ PropName: 'name', Message: 'Description is required' });
                        hasErrors = true;
                    }
                    barrier.validationErrors(barrierErrors);
                    return !hasErrors;
                });
                barrier.validationErrorsArray = ko.computed(function () {
                    var thisArray = [];
                    ko.utils.arrayForEach(barrier.validationErrors(), function (error) {
                        thisArray.push(error.PropName);
                    });
                    return thisArray;
                });

            }

            function goalInitializer(goal) {
                goal.isNew = ko.observable(false);
                goal.checkAppend = function () {
                    appendNewDetails( goal.newDetails, goal.details );
                };
                goal.focusAreaString = ko.computed(function () {
                    checkDataContext();
                    var thisString = '';
                    ko.utils.arrayForEach(goal.focusAreaIds(), function (focusArea) {
                        var thisFocusArea = ko.utils.arrayFirst(datacontext.enums.focusAreas(), function (faEnum) {
                            return faEnum.id() === focusArea.id();
                        });
                        thisString += thisFocusArea.name() + ', ';
                    });
                    if (thisString.length > 0) {
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
                            thisString += thisProgram ? thisProgram.name() + ', ' : '';
                        });
                        if (thisString.length > 0) {
                            thisString = thisString.substr(0, thisString.length - 2);
                        }
                    }
                    if (thisString.length === 0) {
                        thisString = 'None';
                    }
                    return thisString;
                });
                goal.isLoaded = false;
                goal.validationErrors = ko.observableArray([]);
                goal.isValid = ko.computed( function() {
                    var hasErrors = false;
                    var goalErrors = [];
                    var name = goal.name();
                    if( !name ){
                        goalErrors.push({ PropName: 'name', Message: 'Description is required' });
                        hasErrors = true;
                    }
                    goal.validationErrors(goalErrors);
                    return !hasErrors;
                });
                goal.validationErrorsArray = ko.computed(function () {
                    var thisArray = [];
                    ko.utils.arrayForEach(goal.validationErrors(), function (error) {
                        thisArray.push(error.PropName);
                    });
                    return thisArray;
                });
                goal.sortedInterventions = ko.computed(function () {
                    return goal.interventions().sort(function (l, r) { return (l.description() == r.description()) ? (l.description() < r.description() ? -1 : 1) : (l.description() < r.description() ? -1 : 1) });
                });
                goal.sortedTasks = ko.computed(function () {
                    return goal.tasks().sort(function (l, r) { return (l.description() == r.description()) ? (l.description() < r.description() ? -1 : 1) : (l.description() < r.description() ? -1 : 1) });
                });
            }

            function interventionInitializer(intervention) {
                intervention.isNew = ko.observable(false);
                intervention.checkAppend = function () {
                    appendNewDetails( intervention.newDetails, intervention.details );
                };
                intervention.barrierString = ko.computed(function () {
                    checkDataContext();
                    var thisString = '';
                    if (intervention.goal()) {
                        ko.utils.arrayForEach(intervention.barrierIds(), function (barrier) {
                            var thisBarrier = ko.utils.arrayFirst(intervention.goal().barriers(), function (baEnum) {
                                return baEnum.id() === barrier.id();
                            });
                            if (thisBarrier) {
                                thisString += thisBarrier.name() + ', ';
                            }
                        });
                        if (thisString.length > 0) {
                            thisString = thisString.substr(0, thisString.length - 2);
                        }
                    }
                    if (thisString.length === 0) {
                        thisString = 'None';
                    }
                    return thisString;
                });

                intervention.relatedBarriers = ko.computed(function () {
                    checkDataContext();
                    var barriersList = [];
                    if (intervention.goal()) {
                        ko.utils.arrayForEach(intervention.barrierIds(), function (barrier) {
                            var thisBarrier = ko.utils.arrayFirst(intervention.goal().barriers(), function (baEnum) {
                                return baEnum.id() === barrier.id();
                            });
                            if (thisBarrier) {
                                barriersList.push(thisBarrier);
                            }
                        });
                    }
                    return barriersList;
                });

                intervention.computedGoalName = ko.computed(function () {
                    var returnString = '';
                    if (intervention.goal()) {
                        returnString = intervention.goal().name();
                    } else if (intervention.goalName()) {
                        returnString = intervention.goalName();
                    }
                    return returnString;
                });
                intervention.computedPatient = ko.computed(function () {
                    var returnPatient = '';
                    if (intervention.goal() && intervention.goal().patient()) {
                        returnPatient = intervention.goal().patient();
                    } else if (intervention.goalName()) {
                        returnPatient = intervention.patientDetails();
                    }
                    return returnPatient;
                });
                intervention.isDirty = ko.observable(false);
                intervention.clearDirty = function(){
                    intervention.isDirty(false);
                };
                intervention.watchDirty = function () {
                    var propToken = intervention.entityAspect.propertyChanged.subscribe(function (newValue) {
                        intervention.isDirty(true);
                        propToken.dispose();
                    });
                    var barriersToken = intervention.barrierIds.subscribe(function (newValue) {
                        intervention.isDirty(true);
                        barriersToken.dispose();
                    });
                };
                intervention.dueDateErrors = ko.observableArray([]);
                intervention.startDateErrors = ko.observableArray([]);
                intervention.validationErrors = ko.observableArray([]);
                intervention.isValid = ko.computed( function() {
                    var hasErrors = false;
                    var description = intervention.description();
                    var startDate = intervention.startDate();
                    var dueDate = intervention.dueDate();
                    var interventionErrors = [];
                    var dueDateErrors = intervention.dueDateErrors();
                    var startDateErrors = intervention.startDateErrors();
                    var hasChanges = intervention.isDirty();
                    if( !description ){
                        hasErrors = true;
                        if( hasChanges || !intervention.isNew() ){
                            interventionErrors.push({ PropName: 'description', Message: 'Description is required' });
                        }
                    }
                    if( startDateErrors.length > 0 ){
                        ko.utils.arrayForEach( startDateErrors, function(error){
                            interventionErrors.push({ PropName: 'startDate', Message: 'Start Date ' + error.Message});
                            hasErrors = true;
                        });
                    }
                    if( dueDateErrors.length > 0 ){
                        ko.utils.arrayForEach( dueDateErrors, function(error){
                            interventionErrors.push({ PropName: 'dueDate', Message: 'Due Date ' + error.Message});
                            hasErrors = true;
                        });
                    }

                    intervention.validationErrors(interventionErrors)
                    return !hasErrors;
                });

                intervention.validationErrorsArray = ko.computed(function () {
                    var thisArray = [];
                    ko.utils.arrayForEach(intervention.validationErrors(), function (error) {
                        thisArray.push(error.PropName);
                    });
                    return thisArray;
                });
            }

            function taskInitializer(task) {
                task.checkAppend = function () {
                    appendNewDetails( task.newDetails, task.details );
                };
                task.barrierString = ko.computed(function () {
                    checkDataContext();
                    var thisString = '';
                    if (task.goal()) {
                        ko.utils.arrayForEach(task.barrierIds(), function (barrier) {
                            var thisBarrier = ko.utils.arrayFirst(task.goal().barriers(), function (baEnum) {
                                return baEnum.id() === barrier.id();
                            });
                            if (thisBarrier) {
                                thisString += thisBarrier.name() + ', ';
                            }
                        });
                        if (thisString.length > 0) {
                            thisString = thisString.substr(0, thisString.length - 2);
                        }
                    }
                    if (thisString.length === 0) {
                        thisString = 'None';
                    }
                    return thisString;
                });
                task.relatedBarriers = ko.computed(function () {
                    checkDataContext();
                    var barriersList = [];
                    if (task.goal()) {
                        ko.utils.arrayForEach(task.barrierIds(), function (barrier) {
                            var thisBarrier = ko.utils.arrayFirst(task.goal().barriers(), function (baEnum) {
                                return baEnum.id() === barrier.id();
                            });
                            if (thisBarrier) {
                                barriersList.push(thisBarrier);
                            }
                        });
                    }
                    return barriersList;
                });
                task.computedGoalName = ko.computed(function () {
                    var returnString = '';
                    if (task.goal()) {
                        returnString = task.goal().name();
                    } else if (task.goalName()) {
                        returnString = task.goalName();
                    }
                    return returnString;
                });
                task.validationErrors = ko.observableArray([]);
                task.isValid = ko.computed( function() {
                    var hasErrors = false;
                    var taskErrors = [];
                    var description = task.description();
                    if( !description ){
                        taskErrors.push({ PropName: 'description', Message: 'Description is required' });
                        hasErrors = true;
                    }
                    task.validationErrors(taskErrors);
                    return !hasErrors;
                });
                task.validationErrorsArray = ko.computed(function () {
                    var thisArray = [];
                    ko.utils.arrayForEach(task.validationErrors(), function (error) {
                        thisArray.push(error.PropName);
                    });
                    return thisArray;
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
                        if (attribute.controlType === 2) {

                        } else {
                            attribute.values()[0].value(newValue);
                        }
                    }
                });
                attribute.displayValue = ko.computed(function () {
                    var returnValue = null;
                    if (attribute.values().length === 1) {
                        returnValue = ko.utils.arrayFirst(attribute.options(), function (option) {
                            return attribute.values()[0].value() === option.value();
                        });
                    }
                    else if (attribute.values().length > 1) {
                        returnValue = attribute.values();
                    }
                    return returnValue? returnValue.display() : '';
                });
            }

            function careManagerInitializer(careManager) {
                careManager.fullName = ko.computed(function () {
                    var fn = careManager.firstName();
                    var ln = careManager.lastName();
                    return fn + ' ' + ln;
                }).extend({ throttle: 100 });
				
				careManager.firstLastOrPreferredName = ko.computed( function(){
					var preferred = careManager.preferredName();
					var firstName = careManager.firstName();
					if( !firstName ) {
						firstName = '';
					}
					var lastName = careManager.lastName();
					if( !lastName ) {
						lastName = '';
					}
					return preferred? preferred : (firstName + ' ' + lastName);
				}).extend({throttle: 100});
				
            }
        }

        function appendNewDetails( newDetails, details ){
            if( newDetails() ){
                var append = '';
                if( details() && details().length ){
                    append = '\n';
                }
                checkDataContext();
                append += moment().format('MM-DD-YYYY h:mm A') + ' ';
                append += (' ' + datacontext.getUsercareManagerName());
                append += (' - ' + newDetails());
                details(details() ? details() + append : append);
                newDetails('');
            }
        }

        function createMocks(manager) {

        }

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }
    });
