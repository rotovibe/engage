define(['services/session', 'config.services', 'services/local.collections'], function (session, servicesConfig, localCollections) {

    var datacontext, patientsIndex;

    var patientProblemEndPoint = ko.computed(function () {
        var currentUser = session.currentUser();
        if (!currentUser) {
            return '';
        }
        return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'PatientObservation');
    });

    var entityFinder = {
        searchForEntities: searchForEntities,
        searchForProblems: searchForProblems,
        searchForLocalCollectionEntities: searchForLocalCollectionEntities
    };
    return entityFinder;

    function checkDataContext() {
        if (!datacontext) {
            datacontext = require('services/datacontext');
            patientsIndex = require('viewmodels/patients/index');
        }
    }

    // Search the data for entities, and materialize them
    function searchForEntities(data) {
        checkDataContext();
        // If there is a result in the data returned, it is an alert,
        if (data.Result) {
            // So create an alert and add it to enums.alerts
            var thisAlert = datacontext.createEntity('Alert', { result: data.Result.Result, reason: data.Result.Reason });
            thisAlert.entityAspect.acceptChanges();
            datacontext.alerts.push(thisAlert);
        }

        // If there is a program in the data returned, it is an patient program,
        if (data.Program || data.Programs) {
            if (data.Program) {
                createProgram(data.Program);
            }
            if (data.Programs) {
                ko.utils.arrayForEach(data.Programs, function (program) {
                    createProgram(program);
                });
            }
        }
        return true;
    }

    // Search the data for entities, and materialize them
    // If you are wondering why this was hard-coded to only search for problems,
    // Please request more information from the 1/23/2014 meeting
    // with Tony D., Mel, Snehal, Tanuja, and Patrick
    function searchForProblems(data) {
        var patientId = data.PatientId;
        checkDataContext();
        // If there is a result in the data returned, it is an alert,
        if (data.RelatedChanges && patientId) {
            ko.utils.arrayForEach(data.RelatedChanges, function (change) {
                if (change === '100') {
                    // Go get a list of observations that are of type problem for the currently selected patient
                    datacontext.getEntityList(null, patientProblemEndPoint().EntityType, patientProblemEndPoint().ResourcePath + patientId + '/Observation/Problems', null, null, true);
                }
                if (change === '200') {
                    // Go refresh the list of patients todos
                    patientsIndex.getPatientsToDos();
                }
                // if (change === '300') {
                //     // Go refresh the list of patients todos
                //     patientsIndex.getPatientsToDos();
                // }
            });
        }
        return true;
    }

    function searchForLocalCollectionEntities (data) {
        var planElems = data[0];
        // Check for tasks not in local collections yet
        if (planElems && planElems.interventions) {
            ko.utils.arrayForEach(planElems.interventions, function (intv) {
                // Check for a matching intervention
                if (localCollections.interventions.indexOf(intv) === -1) {
                    // Add it to the local collection if it wasn't found
                    localCollections.interventions.push(intv);
                }
            });
        }
        // Check for tasks not in local collections yet
        if (planElems && planElems.tasks) {
            ko.utils.arrayForEach(planElems.tasks, function (task) {
                // Check for a matching intervention
                if (localCollections.tasks.indexOf(task) === -1) {
                    // Add it to the local collection if it wasn't found
                    localCollections.tasks.push(task);
                }
            });
        }
    }

    function createProgram(program) {
        var thisProgram = datacontext.createEntity('Program', { id: program.Id, name: program.Name, patientId: program.PatientId, shortName: program.shortName, status: program.Status, enabled: program.Enabled, completed: program.Completed, elementState: program.ElementState });
        thisProgram.entityAspect.acceptChanges();
    }

    function createModule(module) {
        var thisModule = datacontext.createEntity('Module', { id: module.Id, name: module.Name, programId: module.ProgramId, shortName: module.shortName, status: module.Status, enabled: module.Enabled, completed: module.Completed });
        thisModule.entityAspect.acceptChanges();
    }

    function createAction(action) {
        var thisAction = datacontext.createEntity('action', { id: action.Id, name: action.Name, moduleId: action.ModuleId, shortName: action.shortName, status: action.Status, enabled: action.Enabled, completed: action.Completed });
        thisAction.entityAspect.acceptChanges();
    }
});