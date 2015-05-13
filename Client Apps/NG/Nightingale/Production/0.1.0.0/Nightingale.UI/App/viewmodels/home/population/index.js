define(['plugins/router', 'services/navigation', 'config.services', 'services/session', 'services/datacontext'],
    function (router, navigation, servicesConfig, session, datacontext) {

        var patients = ko.observableArray();
        var initialized = false;
        var selectedCohorts = ko.observableArray();
        var showingPatients = ko.observableArray();
        var currentUser = session.currentUser();

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
        }

    });