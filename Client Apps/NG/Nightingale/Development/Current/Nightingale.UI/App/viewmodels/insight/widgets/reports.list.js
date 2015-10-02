//TODO: Inject dependencies
define(['config.services', 'services/session', 'services/datacontext', 'models/base', 'viewmodels/shell/shell'],
    function (servicesConfig, session, datacontext, modelConfig, shell) {

        // End point to go grab some cohorts
        var cohortEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'cohorts', 'Cohort');
        });

        // End point to grab the patients from the current cohort
        var currentCohortsPatientsEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'cohortpatients', 'Patient', { Skip: 0, Take: 5000 });
        });

        var cohortsList = ko.observableArray();
        var selectedCohort = ko.observable();
        var cohortFilter = ko.observable();
        var throttledFilter = ko.computed(cohortFilter).extend({ throttle: 500 }).extend({ notify: 'always' });
        var patientsList = ko.observableArray();
        var selectedPatient = ko.observable();

        // Have we initialized this vm already?
        var initialized = false;
        var selectedCohortToken;

        var deleteModalShowing = ko.observable(false);
        var deleteOverride = function () {
            // Prompt the user to confirm deletion
            var result = confirm('You are about to delete an individual.  Press OK to continue, or cancel to return without deleting.');
            // If they press OK,
            if (result === true) {
                patientsList.remove(selectedPatient());
                datacontext.deleteIndividual(selectedPatient());
                // Proceed to navigate away
            }
            else {                    
                return false;
            }
        }
		var modalSettings = {
			title: 'Delete Individual',
			entity: selectedPatient, 
			templatePath: 'templates/patient.delete.html', 
			showing: deleteModalShowing, 
			saveOverride: null, 
			cancelOverride: null, 
			deleteOverride: deleteOverride, 
			classOverride: null
		}
        var modal = new modelConfig.modal(modalSettings);
        modal.canDelete(true);
        // Reveal the bindable properties and functions
        var vm = {
            cohortsList: cohortsList,
            selectedCohort: selectedCohort,
            cohortFilter: cohortFilter,
            patientsList: patientsList,
            activate: activate,
            choosePatient: choosePatient
        };

        return vm;

        function choosePatient (sender) {
            selectedPatient(sender);
            deleteModalShowing(true);
            shell.currentModal(modal);
        }

        function activate() {
            if (!initialized) {
                initializeViewModel();
            }
            return true;
        }

        function initializeViewModel() {
            // Go get a list of cohorts locally
            datacontext.getEntityList(cohortsList, cohortEndPoint().EntityType, cohortEndPoint().ResourcePath, null, null, false, null, 'sName').then(cohortsReturned);
            // Subscribe to changes on the selected cohort to get an updated patient list when it changes
            selectedCohortToken = selectedCohort.subscribe(function () {
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
                    patientsList([]);
                    getPatientsByCohort(val);
                }
            });
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
            datacontext.getEntityList(patientsList, currentCohortsPatientsEndPoint().EntityType, currentCohortsPatientsEndPoint().ResourcePath, null, selectedCohort().iD(), true, parameters);
        }



    });