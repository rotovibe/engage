//TODO: Inject dependencies
define(['config.services', 'services/session','services/datacontext', 'services/report.context'],
    function (servicesConfig, session, datacontext, reportContext) {

        function programLoading(name) {
            var self = this;
            self.name = name;
        }

        // End point to go grab some cohorts
        var cohortEndPoint = ko.computed(function () {
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'cohorts', 'Cohort');
        });

        var programsLoading = ko.observableArray();
        var actionsLoading = ko.observableArray();
        var careMembersLoading = ko.observableArray();
        var goalsLoading = ko.observableArray();

        // End point to grab the patients from the current cohort
        var currentCohortsPatientsEndPoint = ko.computed(function () {
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'cohortpatients', 'Patient', { Skip: 0, Take: 5000 });
        });

        var goalEndPoint = ko.computed(function () {
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Goal');
        });

        var careMemberEndPoint = ko.computed(function () {
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'CareMember');
        });

        // Endpoint to use for getting the current patient's programs
        var patientProgramEndPoint = ko.computed(function () {
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Program');
        });

        var actionEndPoint = ko.computed(function() {
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Action');
        });

        var cohortsList = ko.observableArray();
        var elementStateList = ko.observableArray();
        var selectedProgramStateIds = ko.observableArray();
        var selectedActionStateIds = ko.observableArray();
        var selectedCohort = ko.observable();

        var patientsList = ko.observableArray();
        var haveCareManagerPatientsList = ko.computed(function () {
            var returnList = [];
            if (careMembersLoading().length === 0) {
                var thesePatients = patientsList();
                returnList = ko.utils.arrayFilter(thesePatients, function (thisPatient) {
                    var thisMatchedCareManager = ko.utils.arrayFirst(thisPatient.careMembers(), function (cm) {
                        return cm.type().name() === 'Care Manager';
                    });
                    return !!thisMatchedCareManager;
                });
            }
            return returnList;
        });
        var haveProgramPatientsList = ko.computed(function () {
            var thesePatients = patientsList();
            var returnList = ko.utils.arrayFilter(thesePatients, function (thisPatient) {
                return thisPatient.programs().length > 0;
            });
            return returnList;
        });
        var haveGoalPatientsList = ko.computed(function () {
            var thesePatients = patientsList();
            var returnList = ko.utils.arrayFilter(thesePatients, function (thisPatient) {
                return thisPatient.goals().length > 0;
            });
            return returnList;
        });

        var goalsList = ko.observableArray();
        var careMembersList = ko.observableArray();

        var programsList = ko.observableArray();
        var inProgressProgramsList = ko.computed(function () {
            var thesePrograms = programsList();
            var returnList = ko.utils.arrayFilter(thesePrograms, function (thisProgram) {
                return thisProgram.elementStateModel().name() === 'In Progress';
            });
            return returnList;
        });
        var completedProgramsList = ko.computed(function () {
            var thesePrograms = programsList();
            var returnList = ko.utils.arrayFilter(thesePrograms, function (thisProgram) {
                return thisProgram.elementStateModel().name() === 'Completed';
            });
            return returnList;
        });
        var closedProgramsList = ko.computed(function () {
            var thesePrograms = programsList();
            var returnList = ko.utils.arrayFilter(thesePrograms, function (thisProgram) {
                return thisProgram.elementStateModel().name() === 'Closed';
            });
            return returnList;
        });
        var notStartedProgramsList = ko.computed(function () {
            var thesePrograms = programsList();
            var returnList = ko.utils.arrayFilter(thesePrograms, function (thisProgram) {
                return thisProgram.elementStateModel().name() === 'Not Started';
            });
            return returnList;
        });

        var actionsList = ko.observableArray();
        var inProgressActionsList = ko.computed(function () {
            var theseActions = actionsList();
            var returnList = ko.utils.arrayFilter(theseActions, function (thisAction) {
                return thisAction.elementStateModel().name() === 'In Progress';
            });
            return returnList;
        });
        var completedActionsList = ko.computed(function () {
            var theseActions = actionsList();
            var returnList = ko.utils.arrayFilter(theseActions, function (thisAction) {
                return thisAction.elementStateModel().name() === 'Completed';
            });
            return returnList;
        });
        var closedActionsList = ko.computed(function () {
            var theseActions = actionsList();
            var returnList = ko.utils.arrayFilter(theseActions, function (thisAction) {
                return thisAction.elementStateModel().name() === 'Closed';
            });
            return returnList;
        });
        var notStartedActionsList = ko.computed(function () {
            var theseActions = actionsList();
            var returnList = ko.utils.arrayFilter(theseActions, function (thisAction) {
                return thisAction.elementStateModel().name() === 'Not Started';
            });
            return returnList;
        });

        // Have we initialized this vm already?
        var initialized = false;
        var selectedCohortToken;
        var programsByCohortLoaded = ko.observable(false);
        var actionsByCohortLoaded = ko.observable(false);
        var careMembersByCohortLoaded = ko.observable(false);
        var goalsByCohortLoaded = ko.observable(false);

        var canShowProgramReport = ko.computed(function() {
            var pbcl = programsByCohortLoaded();
            var pl = programsLoading();
            return (programsByCohortLoaded() && pl.length === 0);
        });

        var canShowActionReport = ko.computed(function() {
            var pbcl = actionsByCohortLoaded();
            var pl = actionsLoading();
            return (actionsByCohortLoaded() && pl.length === 0);
        });

        var canShowCareManagerReport = ko.computed(function() {
            var pl = careMembersLoading();
            return (careMembersByCohortLoaded() && pl.length === 0);
        });

        var activeSecondColumn = ko.observable();
        activeSecondColumn.data = ko.observableArray();
        var programLength = ko.observable();

        var canLoadProgramData = ko.computed(function () {
            var pbcl = !programsByCohortLoaded();
            var sc = selectedCohort();
            return pbcl && !!sc;
        });
        var canLoadActionData = ko.computed(function () {
            var pbcl = !actionsByCohortLoaded();
            var sc = selectedCohort();
            var cspr = canShowProgramReport();
            return pbcl && cspr && !!sc;
        });
        var canLoadGoalData = ko.computed(function () {
            var gbcl = !goalsByCohortLoaded();
            var sc = selectedCohort();
            return gbcl && !!sc;
        });
        var canLoadCareMemberData = ko.computed(function () {
            var cmbcl = !careMembersByCohortLoaded();
            var sc = selectedCohort();
            return cmbcl && !!sc;
        });
        var programInfoVisible = ko.observable(true);
        var goalInfoVisible = ko.observable(true);
        var overviewInfoVisible = ko.observable(true);
        var careMemberInfoVisible = ko.observable(true);
        var dynamicReportVisible = ko.observable(true);
        var dynamicReportName = ko.observable();
        var dynamicReportDescription = ko.observable();
        var dynamicReportQueryJSON = ko.observable();
        var dynamicReportColumns = ko.observable();

        // Reveal the bindable properties and functions
        var vm = {
            patientsList: patientsList,
            haveCareManagerPatientsList: haveCareManagerPatientsList,
            cohortsList: cohortsList,
            goalsList: goalsList,
            elementStateList: elementStateList,
            selectedActionStateIds: selectedActionStateIds,
            selectedProgramStateIds: selectedProgramStateIds,
            careMembersList: careMembersList,
            programsList: programsList,

            actionsList: actionsList,
            inProgressActionsList: inProgressActionsList,
            completedActionsList: completedActionsList,
            closedActionsList: closedActionsList,
            notStartedActionsList: notStartedActionsList,

            programInfoVisible: programInfoVisible,
            goalInfoVisible: goalInfoVisible,
            overviewInfoVisible: overviewInfoVisible,
            careMemberInfoVisible: careMemberInfoVisible,
            dynamicReportVisible: dynamicReportVisible,
            notStartedProgramsList: notStartedProgramsList,
            inProgressProgramsList: inProgressProgramsList,
            completedProgramsList: completedProgramsList,
            closedProgramsList: closedProgramsList,
            haveProgramPatientsList: haveProgramPatientsList,
            haveGoalPatientsList: haveGoalPatientsList,
            selectedCohort: selectedCohort,
            programsByCohortLoaded: programsByCohortLoaded,
            careMembersByCohortLoaded: careMembersByCohortLoaded,
            goalsByCohortLoaded: goalsByCohortLoaded,
            programsLoading: programsLoading,
            actionsLoading: actionsLoading,
            goalsLoading: goalsLoading,
            careMembersLoading: careMembersLoading,
            activate: activate,
            activeSecondColumn: activeSecondColumn,
            canLoadProgramData: canLoadProgramData,
            canLoadActionData: canLoadActionData,
            canLoadGoalData: canLoadGoalData,
            canLoadCareMemberData: canLoadCareMemberData,
            canShowProgramReport: canShowProgramReport,
            canShowActionReport: canShowActionReport,
            programLength: programLength,
            runProgramReport: runProgramReport,
            runActionReport: runActionReport,
            runCareMemberReport: runCareMemberReport,
            runGoalReport: runGoalReport,
            showProgramResults: showProgramResults,

            canShowCareManagerReport: canShowCareManagerReport,
            getPatientsByCareManagerReport: getPatientsByCareManagerReport,

            notStartedProgReport: notStartedProgReport,
            inProgressProgReport: inProgressProgReport,
            completedProgReport: completedProgReport,
            closedProgReport: closedProgReport,

            notStartedActionReport: notStartedActionReport,
            inProgressActionReport: inProgressActionReport,
            completedActionReport: completedActionReport,
            closedActionReport: closedActionReport,

            haveGoalPatReport: haveGoalPatReport,
            withProgPatReport: withProgPatReport,
            showPatientList: showPatientList,

            dynamicReportName: dynamicReportName,
            dynamicReportDescription: dynamicReportDescription,
            dynamicReportColumns: dynamicReportColumns,
            dynamicReportQueryJSON: dynamicReportQueryJSON,

            actionCountReport: actionCountReport,
            dynamicReport: dynamicReport,
            getJSONResults: getJSONResults
        };

        return vm;
        
        function runProgramReport () {
            // Load all programs into cache
            getAllPrograms();
        }

        function runActionReport () {
            // Load all programs into cache
            getAllActions();
        }

        function runCareMemberReport () {
            // Load all programs into cache
            getAllCareMembers();
        }

        function runGoalReport () {
            // Load all programs into cache
            getAllGoals();
        }

        function haveGoalPatReport () {
            if (goalsList().length > 0) {
                showPatientList(haveGoalPatientsList());
            }
        }

        function withProgPatReport () {
            if (programsList().length > 0) {             
                showPatientList(haveProgramPatientsList());   
            }
        }

        function showPatientList (thesePatients) {
            activeSecondColumn.data(thesePatients);
            activeSecondColumn('viewmodels/admin/reports/matching.patients');
        }

        function notStartedProgReport () {
            if (programsList().length > 0) {
                showProgramList(notStartedProgramsList());
            }
        }

        function inProgressProgReport () {
            if (programsList().length > 0) {
                showProgramList(inProgressProgramsList());
            }
        }

        function completedProgReport () {
            if (programsList().length > 0) {
                showProgramList(completedProgramsList());   
            }
        }

        function closedProgReport () {
            if (programsList().length > 0) {
                showProgramList(closedProgramsList());
            }
        }



        function notStartedActionReport () {
            if (actionsList().length > 0) {
                showActionList(notStartedActionsList());
            }
        }

        function inProgressActionReport () {
            if (actionsList().length > 0) {
                showActionList(inProgressActionsList());
            }
        }

        function completedActionReport () {
            if (actionsList().length > 0) {
                showActionList(completedActionsList());   
            }
        }

        function closedActionReport () {
            if (actionsList().length > 0) {
                showActionList(closedActionsList());
            }
        }

        function showActionList (theseActions) {
            activeSecondColumn.data(theseActions);
            activeSecondColumn('viewmodels/admin/reports/matching.actions');
        }

        function showProgramList (thesePrograms) {
            activeSecondColumn.data(thesePrograms);
            activeSecondColumn('viewmodels/admin/reports/matching.programs');
        }

        function showProgramResults () {
            var thisLength = programLength();
            var thesePrograms = ko.observableArray();
            // Get all the programs
            datacontext.getEntityList(thesePrograms, patientProgramEndPoint().EntityType, 'gettingLocally', null, null, false);
            var matchingPrograms = ko.utils.arrayFilter(thesePrograms(), function (prog) {
                if (prog.duration() > thisLength) {
                    return true;
                }
            });
            activeSecondColumn.data(matchingPrograms);
            activeSecondColumn('viewmodels/admin/reports/matching.programs');
        }

        function getAllPrograms () {
            // If we have not already loaded this cohorts programs,
            if (!programsByCohortLoaded()) {
                // For eaech individual,
                var progCount = 0;
                var selectedIds = [];
                ko.utils.arrayForEach(selectedProgramStateIds(), function (progstateid) {
                    selectedIds.push(parseInt(progstateid.id()));
                });

                ko.utils.arrayForEach(patientsList(), function (patient) {
                    var thisPatient = new programLoading(patient.id());
                    programsLoading.push(thisPatient);
                    var thesePatientsPrograms = ko.observableArray();
                    // Go get programs from the server


                    datacontext.getEntityList(thesePatientsPrograms, patientProgramEndPoint().EntityType, patientProgramEndPoint().ResourcePath + patient.id() + '/Programs', null, null, true).then(patientProgramsReturned); 

                    function patientProgramsReturned() {
                        ko.utils.arrayForEach(thesePatientsPrograms(), function (patprog) {
                            // If the program matches a selected state,
                            if (selectedIds.indexOf(patprog.elementState()) !== -1) {
                                var thisPatProgLoading = new programLoading(patprog.id());
                                programsLoading.push(thisPatProgLoading);
                                // Go get the programs details
                                var thisEndPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + patprog.patientId() + '/Program/' + patprog.id(), 'Program');
                                datacontext.getMelsEntityById(null, null, thisEndPoint.EntityType, thisEndPoint.ResourcePath, true, null).then(programReturned);
                                programsList.push(patprog);
                                function programReturned () {
                                    programsLoading.remove(thisPatProgLoading);
                                }   
                            }
                        });
                        programsLoading.remove(thisPatient);
                    }
                });
                programsByCohortLoaded(true);
            } else {
                alert('Program data already loaded');
            }
        }

        function getAllActions () {
            // If we have not already loaded this cohorts programs,
            if (!actionsByCohortLoaded()) {
                // For eaech individual,
                var progCount = 0;
                var selectedIds = [];
                ko.utils.arrayForEach(selectedActionStateIds(), function (actionstateid) {
                    selectedIds.push(parseInt(actionstateid.id()));
                });

                ko.utils.arrayForEach(programsList(), function (program) {
                    var thisProgram = new programLoading(program.id());
                    actionsLoading.push(thisProgram);
                    var theseActions = ko.observableArray();
                    // Go get programs from the server

                    // This HSOULD GET ACTOINS NOT PROGRAMS
                    //datacontext.getEntityList(theseActions, patientProgramEndPoint().EntityType, patientProgramEndPoint().ResourcePath + patient.id() + '/Programs', null, null, true).then(patientProgramsReturned); 

                    // For each programs modules
                    ko.utils.arrayForEach(program.modules(), function (mod) {
                        // For each action
                        ko.utils.arrayForEach(mod.actions(), function (action) {
                            // If the program matches a selected state,
                            if (selectedIds.indexOf(action.elementState()) !== -1) {
                                var thisActionLoading = new programLoading(action.id());
                                actionsLoading.push(thisActionLoading);

                                // Go get the action details from the server
                                datacontext.getEntityList(theseActions, actionEndPoint().EntityType, actionEndPoint().ResourcePath + '/' + program.patientId() + '/Program/' + program.id() + '/Module/' + mod.id() + '/Action/' + action.id(), null, null, true).then(actionsReturned);


                                // THIS IS THE ACTION DETAILS
                                //var thisEndPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + action.patientId() + '/Program/' + patprog.id(), 'Program');
                                //datacontext.getMelsEntityById(null, null, thisEndPoint.EntityType, thisEndPoint.ResourcePath, true, null).then(actionReturned);
                                actionsList.push(action);

                                function actionsReturned () {
                                    actionsLoading.remove(thisActionLoading);
                                }   
                            }
                        });
                        actionsLoading.remove(thisProgram);                        
                    });
                });
                actionsByCohortLoaded(true);
            } else {
                alert('Action data already loaded');
            }
        }

        function getAllGoals () {
            // If we have not already loaded this cohorts programs,
            if (!goalsByCohortLoaded()) {
                // For eaech individual,
                ko.utils.arrayForEach(patientsList(), function (patient) {
                    var thisPatient = new programLoading(patient.id());
                    goalsLoading.push(thisPatient);
                    var theseGoals = ko.observableArray();
                    // Go get care members from the server
                    datacontext.getEntityList(theseGoals, goalEndPoint().EntityType, goalEndPoint().ResourcePath + patient.id() + '/Goals', null, null, true).then(goalsReturned);

                    function goalsReturned() {
                        ko.utils.arrayForEach(theseGoals(), function (thisgoal) {
                            goalsList.push(thisgoal);
                        });                        
                        // ko.utils.arrayForEach(thesePatientsPrograms(), function (patprog) {
                        //     var thisPatProgLoading = new programLoading(patprog.id());
                        //     careMembersLoading.push(thisPatProgLoading);
                        //     // Go get the programs details
                        //     var thisEndPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + patprog.patientId() + '/Program/' + patprog.id(), 'Program');
                        //     datacontext.getMelsEntityById(null, null, thisEndPoint.EntityType, thisEndPoint.ResourcePath, true, null).then(programReturned);
                        //     programsList.push(patprog);
                        //     function programReturned () {
                        //         careMembersLoading.remove(thisPatProgLoading);
                        //     }
                        // });
                        goalsLoading.remove(thisPatient);
                    }
                });
                goalsByCohortLoaded(true);
            } else {
                alert('Goal data already loaded');
            }
        }

        function getAllCareMembers () {
            // If we have not already loaded this cohorts programs,
            if (!careMembersByCohortLoaded()) {
                // For eaech individual,
                ko.utils.arrayForEach(patientsList(), function (patient) {
                    var thisPatient = new programLoading(patient.id());
                    careMembersLoading.push(thisPatient);
                    var theseCareMembers = ko.observableArray();
                    // Go get care members from the server
                    datacontext.getEntityList(theseCareMembers, careMemberEndPoint().EntityType, careMemberEndPoint().ResourcePath + patient.id() + '/CareMembers', null, null, true).then(careMembersReturned);

                    function careMembersReturned() {
                        ko.utils.arrayForEach(theseCareMembers(), function (thisCM) {
                            careMembersList.push(thisCM);
                        });
                        careMembersLoading.remove(thisPatient);
                    }
                });
                careMembersByCohortLoaded(true);
            } else {
                alert('Care Member data already loaded');
            }
        }

        function getProgramsByPatient() {
            datacontext.getEntityList(null, programEndPoint().EntityType, programEndPoint().ResourcePath, null, null, false);
        }

        function activate() {
            if (!initialized) {
                initializeViewModel();
            }
            return true;
        }

        function initializeViewModel() {
            datacontext.getEntityList(elementStateList, 'ElementState', 'fakeEndpoint', null, null, false, null, 'id');
            // Go get a list of cohorts locally
            datacontext.getEntityList(cohortsList, cohortEndPoint().EntityType, cohortEndPoint().ResourcePath, null, null, false, null, 'sName').then(cohortsReturned);
            // Subscribe to changes on the selected cohort to get an updated patient list when it changes
            selectedCohortToken = selectedCohort.subscribe(function () {
                patientsList([]);
                getPatientsByCohort();
                programsByCohortLoaded(false);
            });
            // Set initialized true so we don't accidentally re-initialize the view model
            initialized = true;
            return true;

            function cohortsReturned() {
                // Load a default cohort when cohorts are returned from the server
                //selectedCohort(cohortsList()[0]);
                var thisCohortMatch = ko.utils.arrayFirst(cohortsList(), function (cohort) {
                    return cohort.sName() === "All individuals";
                });
                selectedCohort(thisCohortMatch);
            }
        };

        function getPatientsByCohort(searchValue) {
            var parameters = {};
            // Create an object to hold the parameters
            var parameters = currentCohortsPatientsEndPoint().Parameters;
            parameters.SearchFilter = null;
            // TODO : Add Skip and Take to the endpoint and pass it down as params
            // TODO : Make sure the service is checking locally first before going out to the server to get these patients
            datacontext.getEntityList(patientsList, currentCohortsPatientsEndPoint().EntityType, currentCohortsPatientsEndPoint().ResourcePath, null, selectedCohort().iD(), true, parameters).then(dataReturned);

            function dataReturned (data) {
            }
        }

        function getPatientSelection () {
            var thisReport = $.parseJSON("{ 'entityType': 'Patient', 'columns': ['Id', 'FirstName', 'LastName'], 'childEntities': [ { 'entityType': 'Program', 'columns': [ 'Id', 'Name'], 'childEntities': [{ 'entityType': 'Module', 'columns': [ 'Id', 'Name' ], 'childEntities': [{ 'entityType': 'Action', 'columns': [ 'Id', 'Name' ], 'childEntities': []}]}]}, { 'entityType': 'Goal', 'columns': [ 'Id', 'Name' ], 'childEntities': []}] }");
            var thesePatients = reportContext.runReport(thisReport);
        }

        function getPatientsByCareManagerReport () {
            var thisReport = new reportContext.report();
            thisReport.name = "Individuals by Care Manager";
            thisReport.description = "List of Individuals with Care Manager and Programs";
            thisReport.columns = [new column('id', 'Id'), new column('firstName', 'First Name'),new column('lastName', 'Last Name'), new column('CareMember.preferredName', 'Care Manager'), new column('Programs.length', '# Programs')]
            thisReport.queryJson = $.parseJSON('{ "entityType": "Patient", "columns": ["Id", "FirstName", "LastName"], "childEntities": [ { "entityType": "Program", "columns": [ "Id", "Name"], "childEntities": [{ "entityType": "Module", "columns": [ "Id", "Name" ], "childEntities": [{ "entityType": "Action", "columns": [ "Id", "Name" ], "childEntities": []}]}]}, { "entityType": "CareMember", "columns": [ "Id", "PreferredName" ], "childEntities": [], "onlyFirst": true}] }');
            
            //var thisReport = $.parseJSON("{ 'entityType': 'Patient', 'columns': ['Id', 'FirstName', 'LastName'], 'childEntities': [ { 'entityType': 'Program', 'columns': [ 'Id', 'Name'], 'childEntities': [{ 'entityType': 'Module', 'columns': [ 'Id', 'Name' ], 'childEntities': [{ 'entityType': 'Action', 'columns': [ 'Id', 'Name' ], 'childEntities': []}]}]}, { 'entityType': 'CareMember', 'columns': [ 'Id', 'Name' ], 'childEntities': [], 'onlyFirst': true}] }");
            var thesePatients = reportContext.runNewReport(thisReport.queryJson);
            thisReport.entities(thesePatients);
            showDynamicReport(thisReport);
        }

        function actionCountReport () {
            var thisReport = new reportContext.report();
            thisReport.name = "Action Count by Individual";
            thisReport.description = "Action Counts by Individual with Care Manager and Programs";
            thisReport.columns = [new column('id', 'Id'), new column('preferredName', 'Name'), new column('CareMemberType.name', 'Care Manager'), new column('Patient.firstName', 'Patients First Name')]
            thisReport.queryJson = $.parseJSON('{ "entityType": "CareMember", "columns": ["Id", "PreferredName", "TypeId"], "navProperties": [ { "entityType": "CareMemberType", "propName": "typeId", "columns": [ "Name" ], "childEntities": [] }, { "entityType": "Patient", "propName": "patientId", "columns": [ "FirstName", "LastName" ], "childEntities": [] } ], "childEntities": [ ] }');
            //var thisReport = $.parseJSON("{ 'entityType': 'Patient', 'columns': ['Id', 'FirstName', 'LastName'], 'childEntities': [ { 'entityType': 'Program', 'columns': [ 'Id', 'Name'], 'childEntities': [{ 'entityType': 'Module', 'columns': [ 'Id', 'Name' ], 'childEntities': [{ 'entityType': 'Action', 'columns': [ 'Id', 'Name' ], 'childEntities': []}]}]}, { 'entityType': 'CareMember', 'columns': [ 'Id', 'Name' ], 'childEntities': [], 'onlyFirst': true}] }");
            var thesePatients = reportContext.runNewReport(thisReport.queryJson);
            thisReport.entities(thesePatients);
            showDynamicReport(thisReport);
        }

        function dynamicReport () {
            var thisReport = new reportContext.report();
            thisReport.name = dynamicReportName();
            thisReport.description = dynamicReportDescription();
            thisReport.columns = $.parseJSON(dynamicReportColumns());
            thisReport.queryJson = $.parseJSON(dynamicReportQueryJSON());
            var theseEntities = getJSONResults(thisReport);
            thisReport.entities(theseEntities);
            showDynamicReport(thisReport);
        }

        function getJSONResults(report) {
            var thisReport = report;
            if (!thisReport.queryJson) {
                thisReport = new reportContext.report();
                thisReport.name = dynamicReportName();
                thisReport.description = dynamicReportDescription();
                thisReport.columns = $.parseJSON(dynamicReportColumns());
                //thisReport.columns = [new column('id', 'Id'), new column('preferredName', 'Name'), new column('CareMemberType.name', 'Care Manager'), new column('Patient.firstName', 'Patients First Name')]
                thisReport.queryJson = $.parseJSON(dynamicReportQueryJSON());
            }
            //'{ "entityType": "CareMember", "columns": ["Id", "PreferredName", "TypeId"], "navProperties": [ { "entityType": "CareMemberType", "propName": "typeId", "columns": [ "Name" ], "childEntities": [] }, { "entityType": "Patient", "propName": "patientId", "columns": [ "FirstName", "LastName" ], "childEntities": [] } ], "childEntities": [ ] }');
            //var thisReport = $.parseJSON("{ 'entityType': 'Patient', 'columns': ['Id', 'FirstName', 'LastName'], 'childEntities': [ { 'entityType': 'Program', 'columns': [ 'Id', 'Name'], 'childEntities': [{ 'entityType': 'Module', 'columns': [ 'Id', 'Name' ], 'childEntities': [{ 'entityType': 'Action', 'columns': [ 'Id', 'Name' ], 'childEntities': []}]}]}, { 'entityType': 'CareMember', 'columns': [ 'Id', 'Name' ], 'childEntities': [], 'onlyFirst': true}] }");
            var theseEntities = reportContext.runNewReport(thisReport.queryJson);
            return theseEntities;
        }

        function showDynamicReport(report) {
            activeSecondColumn.data(report);
            activeSecondColumn('viewmodels/admin/reports/dynamic.report');
        }

        function column(name, displayname) {
            var self = this;
            self.name = name;
            self.displayName = displayname;
        }
    });