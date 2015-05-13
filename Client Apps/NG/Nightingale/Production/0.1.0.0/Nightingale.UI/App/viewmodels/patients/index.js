define(['services/navigation', 'services/datacontext', 'services/session', 'config.services', 'plugins/router'],
    function (navigation, datacontext, session, servicesConfig, router) {

        //#region Local Variables

        var firstTime = false;

        var maxPatientCount = ko.observable(50);

        //#region cohortspanel

        var cohortEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'cohorts', 'Cohort');
        });
        // How many patients to skip when doing a take
        var cohortPatientsSkip = ko.observable(0);
        // Flag whether additional patients can be loaded in cohorts flyout
        var canLoadMoreCohortPatients = ko.observable(false);
        var currentCohortsPatientsEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'cohortpatients', 'Patient', { Skip: cohortPatientsSkip(), Take: maxPatientCount() });
        });
        var cohortsList = ko.observableArray();
        var selectedCohort = ko.observable();
        var cohortFilter = ko.observable();
        var throttledFilter = ko.computed(cohortFilter).extend({ throttle: 500 }).extend({ notify: 'always' });
        var patientsList = ko.observableArray();

        //#endregion

        //#region problems

        var patientProblemEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'PatientProblem');
        });

        // Endpoint to use for getting the current patient's programs
        var patientProgramEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Program');
        });

        //#endregion
        
        var contactCardEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'ContactCard');
        });

        var goalEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Goal');
        });

        var noteEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Note');
        });

        var careMemberEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'CareMember');
        });

        var entityType = 'Patient';
        var patientEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'patient', 'Patient');
        });
        var initialized = false;
        var selectedCohortToken;

        //#endregion

        //#region Exposed Variables

        var selectedPatient = ko.observable();
        var isEditing = ko.observable(false);
        var patientsListFlyoutOpen = ko.observable(true);
        var patientDataColumnOpen = ko.observable(true);
        var hasChanges = ko.computed(datacontext.hasChanges);

        //#endregion
        
        //#region Reveal the object and properties

        var vm = {
            patientsListFlyoutOpen: patientsListFlyoutOpen,
            patientDataColumnOpen: patientDataColumnOpen,
            canLoadMoreCohortPatients: canLoadMoreCohortPatients,
            cohortFilter: cohortFilter,
            togglePatientsColumn: togglePatientsColumn,
            minimizePatientsDataColumn: minimizePatientsDataColumn,
            maximizePatientsDataColumn: maximizePatientsDataColumn,
            minimizePatientsListFlyoutColumn: minimizePatientsListFlyoutColumn,
            maximizePatientsListFlyoutColumn: maximizePatientsListFlyoutColumn,
            choosePatient: choosePatient,
            patientsList: patientsList,
            selectedPatient: selectedPatient,
            isEditing: isEditing,
            save: save,
            loadMoreCohortPatients: loadMoreCohortPatients,
            toggleEditing: toggleEditing,
            cohortsList: cohortsList,
            selectedCohort: selectedCohort,
            activate: activate,
            deactivate: deactivate,
            navigation: navigation,
            title: 'Patients',
            canDeactivate: canDeactivate,
            hasChanges: hasChanges
        };
        return vm;

        //#endregion

        //#region Local Functions

        function initializeViewModel() {
            // Go get a list of cohorts locally
            datacontext.getEntityList(cohortsList, cohortEndPoint().EntityType, cohortEndPoint().ResourcePath, null, null, false, null, 'sName').then(cohortsReturned);
            // On first load show the patients list flyout and open the data column
            patientsListFlyoutOpen(true);
            patientDataColumnOpen(true);
            // Subscribe to changes on the selected cohort to get an updated patient list when it changes
            selectedCohortToken = selectedCohort.subscribe(function () {
                // Get a list of patients by cohort
                canLoadMoreCohortPatients(false);
                cohortPatientsSkip(0);
                // If there is a filter when the cohort changes, clear it
                if (cohortFilter()) {
                    cohortFilter(null);
                }
                patientsList([]);
                getPatientsByCohort();
            });
            throttledFilterToken = throttledFilter.subscribe(function (val) {
                // Get a list of patients by cohort using filter
                if (selectedCohort()) {
                    canLoadMoreCohortPatients(false);
                    cohortPatientsSkip(0);
                    patientsList([]);
                    getPatientsByCohort(val);
                }
            });
            // Set the max patient count to the value of settings.TotalPatientCount, if it exists
            if (session.currentUser().settings().length !== 0) {
                ko.utils.arrayForEach(session.currentUser().settings(), function (setting) {
                    if (setting.Key === 'TotalPatientCount') {
                        maxPatientCount(parseInt(setting.Value));
                    }
                });
            }
            // Set initialized true so we don't accidentally re-initialize the view model
            initialized = true;
            return true;
            function cohortsReturned() {
                // Load a default cohort when cohorts are returned from the server
                //selectedCohort(cohortsList()[0]);
            }
        };

        function getPatientsByCohort(searchValue) {
            var parameters = {};
            // Create an object to hold the parameters
            var parameters = currentCohortsPatientsEndPoint().Parameters;
            // If a search value is passed in
            if (searchValue) {
                // Add a filter parameter onto parameters
                parameters.SearchFilter = searchValue;
            }
            else { parameters.SearchFilter = null; }
            // TODO : Add Skip and Take to the endpoint and pass it down as params
            // TODO : Make sure the service is checking locally first before going out to the server to get these patients
            datacontext.getEntityList(patientsList, currentCohortsPatientsEndPoint().EntityType, currentCohortsPatientsEndPoint().ResourcePath, null, selectedCohort().iD(), true, parameters).then(calculateSkipTake);
        }

        function calculateSkipTake() {
            var totalRecordsShowing = patientsList().length;
            var maxPossibleRecordsShowing = cohortPatientsSkip() === 0 ? maxPatientCount() : cohortPatientsSkip() + maxPatientCount();
            // If max possible records showing is greater than the total records that are showing,
            if (maxPossibleRecordsShowing > totalRecordsShowing) {
                // Then don't show the load more button
                canLoadMoreCohortPatients(false);
            }
            else {
                // Else, show the load more button
                canLoadMoreCohortPatients(true);
            }
            // Always reset the skip after getting more records
            cohortPatientsSkip(maxPossibleRecordsShowing);
        }

        //#endregion

        //#region External Functions
        
        function checkForAnyChanges(patient) {
            // Variable to check for changes to actions on this patient
            var actionsHaveChanges = false;
            var notesHaveChanges = false;
            var goalsHaveChanges = false;
            if (selectedPatient()) {
                // Check if the current patient has actions with unsaved changes
                ko.utils.arrayForEach(selectedPatient().programs.peek(), function (program) {
                    ko.utils.arrayForEach(program.modules.peek(), function (module) {
                        ko.utils.arrayForEach(module.actions.peek(), function (action) {
                            if (action.entityAspect.entityState.isAddedModifiedOrDeleted()) {
                                actionsHaveChanges = true;
                            }
                            ko.utils.arrayForEach(action.steps.peek(), function (step) {
                                if (step.entityAspect.entityState.isAddedModifiedOrDeleted()) {
                                    actionsHaveChanges = true;
                                }
                                ko.utils.arrayForEach(step.responses.peek(), function (response) {
                                    if (response.entityAspect.entityState.isAddedModifiedOrDeleted()) {
                                        actionsHaveChanges = true;
                                    }
                                });
                            });
                        });
                    });
                });
                if (selectedPatient().goals.peek()) {
                    ko.utils.arrayForEach(selectedPatient().goals.peek(), function (goal) {
                        if (goal.entityAspect.entityState.isAddedModifiedOrDeleted()) {
                            goalsHaveChanges = true;
                        }
                        ko.utils.arrayForEach(goal.tasks.peek(), function (task) {
                            if (task.entityAspect.entityState.isAddedModifiedOrDeleted()) {
                                goalsHaveChanges = true;
                            }
                        });
                        ko.utils.arrayForEach(goal.barriers.peek(), function (barrier) {
                            if (barrier.entityAspect.entityState.isAddedModifiedOrDeleted()) {
                                goalsHaveChanges = true;
                            }
                        });
                        ko.utils.arrayForEach(goal.interventions.peek(), function (intervention) {
                            if (intervention.entityAspect.entityState.isAddedModifiedOrDeleted()) {
                                goalsHaveChanges = true;
                            }
                        });
                    });
                }
                if (selectedPatient().notes.peek()) {
                    ko.utils.arrayForEach(selectedPatient().notes.peek(), function (note) {
                        if (note.entityAspect.entityState.isAddedModifiedOrDeleted() && note.text()) {
                            notesHaveChanges = true;
                        }
                    });
                }
                // If the patient's actions have changes,
                if (actionsHaveChanges || notesHaveChanges || goalsHaveChanges) {
                    var message = 'You have unsaved - ';
                    if (actionsHaveChanges) {
                        message += '\n-Action(s)';
                    }
                    if (notesHaveChanges) {
                        message += '\n-Note(s)';
                    }
                    if (goalsHaveChanges) {
                        message += '\n-Goal(s)';
                    }
                    message += '\nPress OK to continue, or cancel to return to the patient.';
                    // Prompt the user to confirm leaving
                    var result = confirm(message);
                    // If they press OK,
                    if (result === true) {
                        return true;
                        // Proceed to navigate away
                    }
                    else {
                        // Cancel selecting a patient
                        patientsListFlyoutOpen(false);
                        return false;
                    }
                }
            }
            return true;
        }

        function choosePatient(patient) {
            // Check if there are changes to this patient's actions before proceeding
            if (hasChanges()) {
                var canLeave = checkForAnyChanges();
                if (!canLeave) {
                    return false;
                }
            }
            var patientId;
            // If there is a current patient and it is equal to the patient you are trying to set to current
            if (selectedPatient() && selectedPatient() === patient) {
                // Then do nothing (this is because we don't want to do anything if
                // We have already selected our patient.
                patientsListFlyoutOpen(false);
            }
                // Else check if datacontext exists in the global namespace (It should if datacontext.js has been loaded)
            else if (datacontext) {
                if (patient.id) {
                    // Else go choose a new patient
                    patientId = ko.unwrap(patient.id);
                }
                else {
                    patientId = patient;
                }
                // Go get a patient to use as the current patient.  TODO : Remove this when we have a list of patients to select from
                datacontext.getEntityById(selectedPatient, patientId, patientEndPoint().EntityType, patientEndPoint().ResourcePath, true).then(patientReturned);
                // Go get a list of problems for the currently selected patient
                datacontext.getEntityList(null, patientProblemEndPoint().EntityType, patientProblemEndPoint().ResourcePath + patientId + '/Problems', null, null, true);
                // Go get a list of programs for the currently selected patient
                datacontext.getEntityList(null, patientProgramEndPoint().EntityType, patientProgramEndPoint().ResourcePath + patientId + '/Programs', null, null, true);
                // Go get a list of contact cards for the currently selected patient
                datacontext.getEntityList(null, contactCardEndPoint().EntityType, contactCardEndPoint().ResourcePath + patientId + '/Contact', null, null, true);
                // Go get a list of goals for the currently selected patient
                datacontext.getEntityList(null, goalEndPoint().EntityType, goalEndPoint().ResourcePath + patientId + '/Goals', null, null, true);
                // Go get a list of notes for the currently selected patient
                datacontext.getEntityList(null, noteEndPoint().EntityType, noteEndPoint().ResourcePath + patientId + '/Notes/25', null, null, true);
                // Go get a list of care members for the currently selected patient
                datacontext.getEntityList(null, careMemberEndPoint().EntityType, careMemberEndPoint().ResourcePath + patientId + '/CareMembers', null, null, true);
            }

            function patientReturned() {
                patientsListFlyoutOpen(false);
                router.navigate('#patients/' + patientId, false);
            }
        }

        function togglePatientsColumn() {
            patientDataColumnOpen(!patientDataColumnOpen());
        }

        function minimizePatientsDataColumn() {
            patientDataColumnOpen(false);
        };

        function maximizePatientsDataColumn() {
            patientDataColumnOpen(true);
        };

        function minimizePatientsListFlyoutColumn() {
            patientsListFlyoutOpen(false);
        };

        function maximizePatientsListFlyoutColumn() {
            patientsListFlyoutOpen(true);
        };

        function activate(patientId) {
            if (patientId) {
                choosePatient(patientId);
            }
            else if (!selectedPatient()) {
                patientsListFlyoutOpen(true);
                patientDataColumnOpen(true);
            }
            // If the view model has not been initialized,
            if (!initialized) {
                // then Initialize the View Model
                initializeViewModel();
            }
            return true;
        }

        function canDeactivate() {
            // If there are changes,
            if (hasChanges()) {
                var canLeave = checkForAnyChanges();
                if (!canLeave) {
                    return false;
                }
            }
            return true;
        }

        function deactivate() {
            patientDataColumnOpen(false);
            patientsListFlyoutOpen(false);
            // Should I dispose of these every time it deactivates?
            //selectedCohortToken.dispose();
            //throttledFilterToken.dispose();
        }

        function toggleEditing() {
            // This method is not currently being used
            isEditing(!isEditing());
            if (isEditing()) {
                patientDataColumnOpen(true);
                patientsListFlyoutOpen(false);
            }
            if (!isEditing()) {
            }
        }

        function save(patient) {
            // TODO : Go save this patient information
            isEditing(false);
        }

        function loadMoreCohortPatients() {
            var parameters = {};
            var filter = cohortFilter();
            // Create an object to hold the parameters
            var parameters = currentCohortsPatientsEndPoint().Parameters;
            // If a search value is passed in
            if (filter) {
                // Add a filter parameter onto parameters
                parameters.SearchFilter = filter;
            }
            else { parameters.SearchFilter = null; }
            var newPatients = ko.observableArray();
            datacontext.getEntityList(newPatients, currentCohortsPatientsEndPoint().EntityType, currentCohortsPatientsEndPoint().ResourcePath, null, selectedCohort().iD(), true, parameters)
                .then(function () {
                    patientsList.push.apply(patientsList, newPatients());
                    calculateSkipTake();
                });
        }
        
        //#endregion

    });