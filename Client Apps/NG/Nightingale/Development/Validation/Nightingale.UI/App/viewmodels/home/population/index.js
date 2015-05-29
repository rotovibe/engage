define(['plugins/router', 'services/navigation', 'config.services', 'services/session', 'services/datacontext'],
    function (router, navigation, servicesConfig, session, datacontext) {

        var patients = ko.observableArray();
        var initialized = false;
        var selectedCohorts = ko.observableArray();
        var showingPatients = ko.observableArray();
        var currentUser = session.currentUser();


        var thesePatients = {
            patients: [
                { 
                    "id": 1,
                    "name": "Billy Condo",
                    "overdue": false,
                    "conditions": [6],
                    "care_gaps": 3
                },
                { 
                    "id": 2,
                    "name": "Johnny Hairband",
                    "overdue": false,
                    "conditions": [5],
                    "care_gaps": 2
                }
            ]
        };

        //var patientsEndPoint = ko.computed(function() {
        //    if (!currentUser()) {
        //        return '';
        //    }
        //    return new servicesConfig.createEndPoint('1.0', currentUser().contracts()[0].number(), 'patient', 'Patient');
        //});
        
        var vm = {
            patients: patients,
            activate: activate,
            attached: attached,
            currentUser: currentUser,
            title: 'index'
        };

        return vm;
        
        function activate() {
        }

        function attached() {
            if (!initialized) {
                initialized = true;
            }
            createVenn();
        }

        function createVenn () {
            var thisVisual = new Visualization('#visualizer', thesePatients.patients, 'care_gaps');
        }

    });