define(['plugins/router', 'services/navigation', 'config.services', 'services/session', 'services/datacontext', 'viewmodels/patients/index'],
    function (router, navigation, servicesConfig, session, datacontext, patientsIndex) {

        var selectedPatient = ko.computed(function () {
            return patientsIndex.selectedPatient();
        });

        var widgets = ko.observableArray();
        var activeGoal = ko.observable();
        var sliderOpen = ko.observable(true);
        var initialized = false;
        var isEditing = ko.observable(false);
        var newGoal = ko.observable();

        var goalsEndPoint = ko.computed(function() {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'goal', 'Goal');
        });

        function widget(data, column) {
            var self = this;
            self.name = ko.observable(data.name);
            self.path = ko.observable(data.path);
            self.isOpen = ko.observable(data.open);
            self.column = column;
            self.isFullScreen = ko.observable(false);
        }

        function column(name, open, widgets) {
            var self = this;
            self.name = ko.observable(name);
            self.widgets = ko.observableArray();
            self.isOpen = ko.observable(open);
            $.each(widgets, function (index, item) {
                self.widgets.push(new widget(item, self))
            });
        }

        var columns = ko.observableArray([
            new column('goals', true, [{ name: 'Goals', path: 'patients/patientswidgets/goalswidget.html', open: true }]),
            new column('goalDetails', false, [{ name: 'Details', path: 'patients/patientswidgets/goaldetailwidget.html', open: false}])
        ]);

        var vm = {
            activate: activate,
            attached: attached,
            activeGoal: activeGoal,
            isEditing: isEditing,
            sliderOpen: sliderOpen,
            columns: columns,
            widgets: widgets,
            selectedPatient: selectedPatient,
            closeSlider: closeSlider,
            addGoal: addGoal,
            title: 'Goals'
        };

        return vm;

        function addGoal() {
            datacontext.initializeEntity(newGoal, 'Goal', selectedPatient().id()).then(goalReturned);
            isEditing(true);

            function goalReturned(data) {
                var thisGoal = data.results[0];
                thisGoal.patientId(selectedPatient().id());
                thisGoal.statusId(1);
                thisGoal.startDate(new Date());
                thisGoal.isNew(true);
                activeGoal(thisGoal);
            }
        };

        function closeSlider() {
            activeGoal(null);
            sliderOpen(false);
            isEditing(false);
        }

        function activate() {
            // Set a local instance of selectedPatient equal to the injected patient
            selectedPatient.subscribe(function (newValue) {
                // Need to go get programs for the selected patient whenever selected patient changes
                //getGoalsForSelectedPatient();

                // Clear out the active showing action column
                activeGoal(null);
                sliderOpen(false);
                isEditing(false);
            });
        }
        
        function attached() {
            if (!initialized) {
                initialized = true;
            }
        }
        
        function getGoalsForSelectedPatient() {
            datacontext.getEntityList(null, goalsEndPoint().EntityType, goalsEndPoint().ResourcePath, null, null, false);
        }

    });