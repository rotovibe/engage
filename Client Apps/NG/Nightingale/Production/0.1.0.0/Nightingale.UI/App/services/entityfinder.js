define(['services/session', 'config.services'], function (session, servicesConfig) {

    var datacontext;

    var patientProblemEndPoint = ko.computed(function () {
        var currentUser = session.currentUser();
        if (!currentUser) {
            return '';
        }
        return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'PatientProblem');
    });

    var entityFinder = {
        searchForEntities: searchForEntities,
        searchForProblems: searchForProblems
    };
    return entityFinder;

    function checkDataContext() {
        if (!datacontext) {
            datacontext = require('services/datacontext');
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
            datacontext.enums.alerts.push(thisAlert);
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

        // If there is a module in the data returned, it is an patient program module,
        if (data.Module || data.Modules) {
            if (data.Module) {
                createModule(data.Module);
            }
            if (data.Modules) {
                ko.utils.arrayForEach(data.Modules, function (module) {
                    createModule(module);
                });
            }
        }

        // If there is a module in the data returned, it is an patient program module,
        if (data.Action || data.Actions) {
            if (data.Action) {
                createAction(data.Action);
            }
            if (data.Actions) {
                ko.utils.arrayForEach(data.Actions, function (action) {
                    createAction(action);
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
                if (change === 'Problems') {
                    datacontext.getEntityList(null, patientProblemEndPoint().EntityType, patientProblemEndPoint().ResourcePath + patientId + '/Problems', null, null, true);
                }
            });
        }

        return true;
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