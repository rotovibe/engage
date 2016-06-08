define(['services/navigation', 'services/datacontext', 'services/session', 'config.services', 'plugins/router'],
    function (navigation, datacontext, session, servicesConfig, router) {

        //#region Local Variables

        var firstTime = false;

        var maxPatientCount = ko.observable(50);

        var throttledFilterToken = {};

		var noResultsMessage =  'No records meet your search criteria';
		var subscriptionTokens = [];

       // var isComposed = ko.observable(false);

        // Track whether the user can leave the patients pages and whether they are
        var leaving = ko.observable(false);
        var canLeave = ko.observable(true);

        //#region cohortspanel

        var cohortEndPoint = ko.computed(function () {
            if (!session.currentUser()) {
                return false;
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'cohorts', 'Cohort');
        });
        // How many patients to skip when doing a take
        var cohortPatientsSkip = ko.observable(0);
        // Flag whether additional patients can be loaded in cohorts flyout
        var canLoadMoreCohortPatients = ko.observable(false);
		var showNoResultsMessage = ko.observable(false);
        var currentCohortsPatientsEndPoint = ko.computed(function () {
            if (!session.currentUser()) {
                return false;
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
            if (!session.currentUser()) {
                return false;
            }
            // TODO: Update this end point
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'PatientObservation');
        });

        var patientSystemEndPoint = ko.computed(function () {
            if (!session.currentUser()) {
                return false;
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'PatientSystem');
        });

        var patientAllergyEndPoint = ko.computed(function () {
            if (!session.currentUser()) {
                return false;
            }
            // TODO: Update this end point
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'PatientAllergy', 'PatientAllergy');
        });

        // Endpoint to use for getting the current patient's programs
        var patientProgramEndPoint = ko.computed(function () {
            if (!session.currentUser()) {
                return false;
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Program');
        });

        //#endregion

        var contactCardEndPoint = ko.computed(function () {
            if (!session.currentUser()) {
                return false;
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'ContactCard');
        });

        var goalEndPoint = ko.computed(function () {
            if (!session.currentUser()) {
                return false;
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Goal');
        });

        var noteEndPoint = ko.computed(function () {
            if (!session.currentUser()) {
                return false;
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Note');
        });

        var careMemberEndPoint = ko.computed(function () {
            if (!session.currentUser()) {
                return false;
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'CareMember');
        });

        var ObservationsEndPoint = ko.computed(function () {
            if (!session.currentUser()) {
                return false;
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'PatientObservation');
        });

        var entityType = 'Patient';
        var patientEndPoint = ko.computed(function () {
            if (!session.currentUser()) {
                return false;
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

        var patientsRoute = ko.utils.arrayFirst(navigation.navRoutes(), function (route) {
            return route.title === 'Individual';
        });
        var isComposed = ko.computed(function () {
            return patientsRoute && patientsRoute.isActive();
        });

        var routerInstructionToken = router.activeInstruction.subscribe(function (oldvalue) {
            var oldRoute = oldvalue && oldvalue.fragment ? oldvalue.fragment.substr(0, 8) : '';
            if (hasChanges() && oldRoute === 'patients') {
                // If they can leave but aren't already,
                if (canLeave() && !leaving()) {
                    // Check if they have changes and want to leave
                    var canleave = checkForAnyChanges();
                    if (!canleave) {
                        leaving(false);
                        canLeave(false);
                        return false;
                    } else {
                        leaving(true);
                        canLeave(true);
                        return true;
                    }
                } else if (!canLeave() && !leaving()) {
                    // If they already canclled leaving and aren't leaving
                    // Reset the application states
                    leaving(false);
                    canLeave(true);
                } else if (canLeave() && leaving()) {
                    canLeave(true);
                    leaving(false);
                }
            } else {
                canLeave(true);
                leaving(false);
            }
        }, null, 'beforeChange');

		subscriptionTokens.push( routerInstructionToken );

        var vm = {
            patientsListFlyoutOpen: patientsListFlyoutOpen,
            patientDataColumnOpen: patientDataColumnOpen,
            canLoadMoreCohortPatients: canLoadMoreCohortPatients,
			showNoResultsMessage: showNoResultsMessage,
			noResultsMessage: noResultsMessage,
            cohortFilter: cohortFilter,
			clearCohortFilter: clearCohortFilter,
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
			detached: detached,
            navigation: navigation,
            title: 'Individual',
            canDeactivate: canDeactivate,
            hasChanges: hasChanges,
            attached: attached,
            isComposed: isComposed,
            leaving: leaving,
            getPatientsToDos: getPatientsToDos,
            getPatientsInterventions: getPatientsInterventions,
            getPatientsTasks: getPatientsTasks,
            getPatientsAllergies: getPatientsAllergies,
            getPatientMedications: getPatientMedications,
			getPatientFrequencies: getPatientFrequencies
        };
        return vm;

        //#endregion

        //#region Local Functions

        function attached() {
            //isComposed(true);
        }

        function initializeViewModel() {
            // Go get a list of cohorts locally
            datacontext.getEntityList(cohortsList, cohortEndPoint().EntityType, cohortEndPoint().ResourcePath, null, null, false, null, 'sName').then(cohortsReturned);
            // On first load show the patients list flyout and open the data column
            //patientsListFlyoutOpen(true);
            patientDataColumnOpen(true);
            // Subscribe to changes on the selected cohort to get an updated patient list when it changes
            selectedCohortToken = selectedCohort.subscribe(function () {
                // Get a list of patients by cohort
                canLoadMoreCohortPatients(false);
				showNoResultsMessage(false);
                cohortPatientsSkip(0);
                // If there is a filter when the cohort changes, clear it
                if (cohortFilter()) {
                    cohortFilter(null);
                }
                patientsList([]);
                getPatientsByCohort();
            });
			subscriptionTokens.push( selectedCohortToken );
            throttledFilterToken = throttledFilter.subscribe(function (val) {
                // Get a list of patients by cohort using filter
                if (selectedCohort()) {
                    canLoadMoreCohortPatients(false);
					showNoResultsMessage(false);
                    cohortPatientsSkip(0);
                    patientsList([]);
                    getPatientsByCohort(val);
                }
            });
			subscriptionTokens.push( throttledFilterToken );

            // Set the max patient count to the value of settings.TotalPatientCount, if it exists
            if (session.currentUser().settings()) {
				var totalPatientCount = datacontext.getSettingsParam('TotalPatientCount');
				if( totalPatientCount ){
					maxPatientCount( parseInt( totalPatientCount ) );
				}
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
            } else { parameters.SearchFilter = null; }

			showNoResultsMessage(false);
            // TODO : Add Skip and Take to the endpoint and pass it down as params
            // TODO : Make sure the service is checking locally first before going out to the server to get these patients
            datacontext.getEntityList(patientsList, currentCohortsPatientsEndPoint().EntityType, currentCohortsPatientsEndPoint().ResourcePath, null, selectedCohort().iD(), true, parameters).then(calculateSkipTake);
        }

        function calculateSkipTake() {
            var totalRecordsShowing = patientsList().length;
			if( totalRecordsShowing == 0 ){
				showNoResultsMessage(true);
			}
            var maxPossibleRecordsShowing = cohortPatientsSkip() === 0 ? maxPatientCount() : cohortPatientsSkip() + maxPatientCount();
            // If max possible records showing is greater than the total records that are showing,
            if (maxPossibleRecordsShowing > totalRecordsShowing) {
                // Then don't show the load more button
                canLoadMoreCohortPatients(false);
            } else {
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
            var todosHaveChanges = false;
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
                        if (note.entityAspect.entityState.isAddedModifiedOrDeleted()) {
							switch( note.type().name().toLowerCase() ){
								case 'utilization':{
									var defaultVisitType = ko.utils.arrayFirst( datacontext.enums.visitTypes(), function (visitType) {
										return visitType.isDefault();
									});
									if( note.visitType() && !defaultVisitType ){
										notesHaveChanges = true;
									}
									break;
								}
								default:{
									if( note.text() ){
										notesHaveChanges = true;
									}
								}
							}
                        }
                    });
                }
                if (selectedPatient().todos.peek()) {
                    ko.utils.arrayForEach(selectedPatient().todos.peek(), function (todo) {
                        if (todo.entityAspect.entityState.isAddedModifiedOrDeleted() && todo.title()) {
                            todosHaveChanges = true;
                        }
                    });
                }
                // If the patient's actions have changes,
                if (actionsHaveChanges || notesHaveChanges || goalsHaveChanges || todosHaveChanges) {
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
                    if (todosHaveChanges) {
                        message += '\n-To Do(s)';
                    }
                    message += '\nPress OK to continue, or cancel to return to the patient.';
                    // Prompt the user to confirm leaving
                    var result = confirm(message);
                    // If they press OK,
                    if (result === true) {
                        return true;
                        // Proceed to navigate away
                    } else {
                        // Cancel selecting a patient
                        patientsListFlyoutOpen(false);
                        return false;
                    }
                }
            }
            return true;
        }

		function clearCohortFilter(){
			cohortFilter(null);
		}

        function choosePatient(patient) {
            // Check if there are changes to this patient's actions before proceeding
            // And if override parameter is set to true ignore checking for changes
            if (hasChanges()) {
                var canleave = checkForAnyChanges();
                if (!canleave) {
                    return false;
                }
            }
            var patientId;
			patientsListFlyoutOpen(false);
            // If there is a current patient and it is equal to the patient you are trying to set to current
            if ( selectedPatient() && selectedPatient() === patient ) {
                // Then do nothing (this is because we don't want to do anything if
                // We have already selected our patient.
				if(datacontext){
					getPatientsToDos();	//always reload since we clean the todos cache in other views when resorting or filtering
				}
            } else if (datacontext) {
                if (patient.id) {
                    // Else go choose a new patient
                    patientId = ko.unwrap(patient.id);
                } else {
                    patientId = patient;
                }

				var allPatientPromises = [];
					// Go select a patient by their Id.
					allPatientPromises.push( datacontext.getEntityById(selectedPatient, patientId, patientEndPoint().EntityType, patientEndPoint().ResourcePath, true));
					// Go get a list of goals for the currently selected patient
					allPatientPromises.push( datacontext.getEntityList(null, goalEndPoint().EntityType, goalEndPoint().ResourcePath + patientId + '/Goals', null, null, true, null, null, true));
					// Go get a list of observations for the currently selected patient
					allPatientPromises.push( datacontext.getEntityList(null, ObservationsEndPoint().EntityType, ObservationsEndPoint().ResourcePath + patientId + '/Observations/Current', null, null, true));
					// Go get a list of notes for the currently selected patient
					allPatientPromises.push( datacontext.getEntityList(null, noteEndPoint().EntityType, noteEndPoint().ResourcePath + patientId + '/Notes/100', null, null, true));

                    //note: this is the old call and will be deprecated:
					//allPatientPromises.push( datacontext.getEntityList(null, careMemberEndPoint().EntityType, careMemberEndPoint().ResourcePath + patientId + '/CareMembers', null, null, true));

				Q.all( allPatientPromises ).then( patientReturned );
            }

            function patientReturned() {
				var allPatientPromises = [];
                // If we don't have the patients' programs yet,
                if (selectedPatient() && selectedPatient().programs() && selectedPatient().programs().length === 0) {
                    // Go get a list of programs for the currently selected patient
                    allPatientPromises.push( datacontext.getEntityList(null, patientProgramEndPoint().EntityType, patientProgramEndPoint().ResourcePath + patientId + '/Programs', null, null, true) );
                }
                // Go get a list of patients todos
                allPatientPromises.push( getPatientsToDos() );
                // Go get a list of patients' interventions
                allPatientPromises.push( getPatientsInterventions() );
                // Go get a list of patients' tasks
                allPatientPromises.push( getPatientsTasks() );
                // Add the patient to the recent individuals list
                allPatientPromises.push( datacontext.addPatientToRecentList(selectedPatient()) );
                // Go get a list of contact cards for the currently selected patient
                allPatientPromises.push( datacontext.getEntityList(null, contactCardEndPoint().EntityType, contactCardEndPoint().ResourcePath + patientId + '/Contact', null, null, true) );
                // Go get a list of observations that are of type problem for the currently selected patient
                allPatientPromises.push( datacontext.getEntityList(null, patientProblemEndPoint().EntityType, patientProblemEndPoint().ResourcePath + patientId + '/Observation/Problems', null, null, true) );
                // Go get a list of allergies for the currently selected patient
                allPatientPromises.push( getPatientsAllergies() );
                // Get a list of the patients various systems
                allPatientPromises.push( datacontext.getEntityList(null, patientSystemEndPoint().EntityType, patientSystemEndPoint().ResourcePath + patientId + '/PatientSystems', null, null, true) );

                // Go get a list of medications for the currently selected patient
                allPatientPromises.push( getPatientMedications() );
				allPatientPromises.push( getPatientFrequencies() );

				//get the care team (new api)
				allPatientPromises.push( datacontext.getCareTeam( null, selectedPatient().contactId() ).then( careTeamReturned ) );
				Q.all( allPatientPromises ).then( patientFullyLoaded );

                router.navigate('#patients/' + patientId, false);
            }
        }

		function careTeamReturned( team ){			
		}
		/**
		*	after all patient related calls are completed, mark the patient as loaded (they are synched with Q.all ).
		*	@method patientFullyLoaded
		*/
		function patientFullyLoaded(){
			selectedPatient().isLoaded(true);
		}

        function getPatientsToDos() {
            // Calculated thirty days ago
            var thirtyDaysAgo = new Date(new Date().setDate(new Date().getDate()-30));
            thirtyDaysAgo = moment(thirtyDaysAgo).format();

			var todosPromises = [];
            // Get all open todos
            todosPromises.push( datacontext.getToDos(null, { StatusIds: [1,3], PatientId: selectedPatient().id() }) );
            // Get recently closed todos
            todosPromises.push( datacontext.getToDos(null, { StatusIds: [2,4], PatientId: selectedPatient().id() })); //FromDate thirtyDaysAgo is missing ?! (, FromDate: thirtyDaysAgo)
			return Q.all( todosPromises );
        }

        function getPatientsInterventions() {
            // Calculated thirty days ago
            // var thirtyDaysAgo = new Date(new Date().setDate(new Date().getDate()-30));
            // thirtyDaysAgo = moment(thirtyDaysAgo).format();
            // Get all open todos
            return datacontext.getInterventions(null, { StatusIds: [1,2,3], PatientId: selectedPatient().id() });
        }

        function getPatientsTasks() {
            // Get all open todos
            return datacontext.getTasks(null, { StatusIds: [1,2,3,4], PatientId: selectedPatient().id() });
        }

        function getPatientsAllergies() {
            // Get all open allergies
            var patientId = selectedPatient().id();
            return datacontext.getPatientAllergies(null, { StatusIds: [1,2,3,4,5,6,7] }, patientId);
        }

        function getPatientMedications() {
            // Get all open medications
            var patientId = selectedPatient().id();
            return datacontext.getPatientMedications(null, { StatusIds: [1,2,3,4,5,6,7], CategoryIds: [1,2] }, patientId);
        }

		function getPatientFrequencies() {
            // Get all patient specific medication frequencies
            var patientId = selectedPatient().id();
			var freqPromise = Q();
			if(!selectedPatient().gotMedicationFrequencies){
				//remotely load this patient's custom medication frequencies:
				freqPromise = datacontext.getPatientFrequencies(null, patientId, true);
				selectedPatient().gotMedicationFrequencies = ko.observable(true);
			}
			return freqPromise;
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
            // isComposed(false);
            if (patientId) {
                choosePatient(patientId);
            } else if (!selectedPatient()) {
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
            return canLeave();
            // If there are changes,
            if (hasChanges()) {
                var canleave = checkForAnyChanges();
                if (!canleave) {
                    return false;
                }
            }
            return true;
        }

        function deactivate() {
            patientDataColumnOpen(false);
            patientsListFlyoutOpen(false);
            // isComposed(false);
            leaving(false);
        }

        function toggleEditing() {
            // This method is not currently being used
            isEditing(!isEditing());
            if (isEditing()) {
                patientDataColumnOpen(true);
                patientsListFlyoutOpen(false);
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
            } else { parameters.SearchFilter = null; }
            var newPatients = ko.observableArray();
            datacontext.getEntityList(newPatients, currentCohortsPatientsEndPoint().EntityType, currentCohortsPatientsEndPoint().ResourcePath, null, selectedCohort().iD(), true, parameters)
                .then(function () {
                    patientsList.push.apply(patientsList, newPatients());
                    calculateSkipTake();
                });
        }

        //#endregion

		function detached(){
			// remarked ! disposing computeds on this module causes many unpredictable problems.
			// console.log('patients/index detached.');
			// ko.utils.arrayForEach(subscriptionTokens, function (token) {
                // token.dispose();
            // });
		}
    });