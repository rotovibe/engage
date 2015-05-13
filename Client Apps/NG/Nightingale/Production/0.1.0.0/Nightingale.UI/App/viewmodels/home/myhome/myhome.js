//TODO: Inject dependencies
define(['services/session', 'services/datacontext', 'config.services'],
    function (session, datacontext, servicesConfig) {

        function CalendarOptionsModel(events, header, editable, viewDate, defaultView) {
            var self = this;
            // Set the events equal to an observableArray of unwrapped events
            self.events = events;
            self.header = header;
            self.editable = editable;
            self.viewDate = viewDate || ko.observable(new Date());
            self.defaultView = defaultView;
        }

        var initialized = false;
        var selectedCohortToken;
        var maxPatientCount = ko.observable(20);
        var cohortPatientsSkip = ko.observable(0);

        function Event() {
            var self = this;
            self.title = ko.observable();
            self.start = ko.observable();
            self.end = ko.observable();
            self.allDay = ko.observable(false);
        }

        // The end point to use when getting cohorts
        var cohortEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'cohorts', 'Cohort');
        });
        // The end point to use when getting cohorts
        var currentCohortsPatientsEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'cohortpatients', 'Patient', { Skip: cohortPatientsSkip(), Take: maxPatientCount() });
        });

        // Internal properties and functions
        var newEvent = ko.observable(new Event());
        var myEvents = ko.observableArray();
        var myHeader = {
                left: 'title',
                right: 'today agendaDay,month prev,next'
        };
        // The date to start the calendar on
        var thisDate = ko.observable(new Date());

        // List of cohorts for this care manager
        var cohortsList = ko.observableArray();
        // The currently selected cohort
        var selectedCohort = ko.observable();
        // A list of patients, in this cohort
        var patientsList = ko.observableArray();


        // Object containing the options
        var calendarOptions = new CalendarOptionsModel(myEvents, myHeader, false, thisDate, 'agendaDay')

        // Reveal the bindable properties and functions
        var vm = {
            activate: activate,
            attached: attached,
            newEvent: newEvent,
            addEvent: addEvent,
            cohortsList: cohortsList,
            selectedCohort: selectedCohort,
            patientsList: patientsList,
            calendarOptions: calendarOptions,
            title: 'Home'
        };

        return vm;

        function addEvent() {
            var params = {
                id: myEvents().length + 1 + 'aaa',
                title: newEvent().title(),
                start: newEvent().start(),
                end: newEvent().end(),
                allDay: newEvent().allDay()
            };
            
            var thisEvent = datacontext.createCalendarEvent(params);
            getEvents();
        }

        function activate() {
            if (!initialized) {
                initializeViewModel();
                initialized = true;
            }
            return true;
        }

        function attached() {
            $('.calendar').fullCalendar('render');
        }

        function getEvents() {
            datacontext.getEventsByUserId(myEvents, session.currentUser().userId(), false).then(function () { console.log('Got the events - ', myEvents()); });
        }

        function initializeViewModel() {
            // Get the mock events for this user
            //getEvents();
            // Go get a list of cohorts locally
            datacontext.getEntityList(cohortsList, cohortEndPoint().EntityType, cohortEndPoint().ResourcePath, null, null, false, null, 'sName').then(cohortsReturned);
            // On first load show the patients list flyout and open the data column
            // Subscribe to changes on the selected cohort to get an updated patient list when it changes
            selectedCohortToken = selectedCohort.subscribe(function () {
                // Get a list of patients by cohort
                //canLoadMoreCohortPatients(false);
                //cohortPatientsSkip(0);
                // If there is a filter when the cohort changes, clear it
                //if (cohortFilter()) {
                //    cohortFilter(null);
                //}
                patientsList([]);
                getPatientsByCohort();
            });
            //throttledFilterToken = throttledFilter.subscribe(function (val) {
            //    // Get a list of patients by cohort using filter
            //    if (selectedCohort()) {
            //        canLoadMoreCohortPatients(false);
            //        cohortPatientsSkip(0);
            //        patientsList([]);
            //        getPatientsByCohort(val);
            //    }
            //});
            // Set the max patient count to the value of settings.TotalPatientCount, if it exists
            //if (session.currentUser().settings().length !== 0) {
            //    ko.utils.arrayForEach(session.currentUser().settings(), function (setting) {
            //        if (setting.Key === 'TotalPatientCount') {
            //            maxPatientCount(parseInt(setting.Value));
            //        }
            //    });
            //}
            // Set initialized true so we don't accidentally re-initialize the view model
            initialized = true;
            return true;

            function cohortsReturned() {
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

        function choosePatient(patient) {
            // If there is a current patient and it is equal to the patient you are trying to set to current
            if (selectedPatient() && selectedPatient() === patient) {
                // Then do nothing (this is because we don't want to do anything if
                // We have already selected our patient.
                patientsListFlyoutOpen(false);
            }
                // Else check if datacontext exists in the global namespace (It should if datacontext.js has been loaded)
            else if (datacontext) {
                // Else go choose a new patient
                var patientId = ko.unwrap(patient.id);
                // Go get a patient to use as the current patient.  TODO : Remove this when we have a list of patients to select from
                datacontext.getEntityById(selectedPatient, patientId, patientEndPoint().EntityType, patientEndPoint().ResourcePath, true).then(function () { patientsListFlyoutOpen(false); });
                // Go get a list of problems for the currently selected patient
                datacontext.getEntityList(null, patientProblemEndPoint().EntityType, patientProblemEndPoint().ResourcePath + patientId + '/Problems', null, null, true);
                // Go get a list of programs for the currently selected patient
                //datacontext.getEntityList(null, patientProgramsEndPoint().EntityType, patientProgramsEndPoint().ResourcePath, null, patientId, true);
            }
        }

        function calculateSkipTake() {
            //var totalRecordsShowing = patientsList().length;
            //var maxPossibleRecordsShowing = cohortPatientsSkip() === 0 ? maxPatientCount() : cohortPatientsSkip() + maxPatientCount();
            //// If max possible records showing is greater than the total records that are showing,
            //if (maxPossibleRecordsShowing > totalRecordsShowing) {
            //    // Then don't show the load more button
            //    canLoadMoreCohortPatients(false);
            //}
            //else {
            //    // Else, show the load more button
            //    canLoadMoreCohortPatients(true);
            //}
            //// Always reset the skip after getting more records
            //cohortPatientsSkip(maxPossibleRecordsShowing);
        }

    });