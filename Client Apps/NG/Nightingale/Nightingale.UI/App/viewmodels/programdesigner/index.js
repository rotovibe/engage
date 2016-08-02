define(['services/session', 'services/navigation'],
    function (session, navigation) {
        
        // Create an entity type and service end point to use when getting that type
        var entityType = 'Program';
        // Create an endPoint to pass to the data context to decide where to get your data from
        var endPoint = ko.computed(function () {
            // Get the current user
            var currentUser = session.currentUser();
            // Check if it has a value
            if (!currentUser) {
                // If not, return nothing
                return '';
            }
            return 'NG/1.0/Contract/' + session.currentUser().contracts()[0].number() + '/program/';
        });
        // List of patients to bind to the left side flyout on patients pages
        var modulesList = ko.observableArray();
        // Who is the currently viewed patient
        var currentModule = ko.observable();
        var initialized = false;

        var modulesListFlyoutOpen = ko.observable(false);

        var vm = {
            modulesListFlyoutOpen: modulesListFlyoutOpen,
            modulesList: modulesList,
            currentModule: currentModule,
            navigation: navigation,
            activate: activate,
            title: 'Program Designer'
        };

        return vm;

        function chooseModule(module) {
            // TODO : Check if patient was passed in
            if (!module.iD) {
                // If not, create an ID property to use to find a patient
                module.iD = ko.observable('');
            }
            // Check if dataContext exists in the global namespace (It should if datacontext.js has been loaded)
            if (datacontext) {
                var programId = ko.unwrap(module.iD);
                datacontext.getEntityById(currentModule, programId, entityType, endPoint(), true).then(getEntityList);
            }
            else {
            }
        }

        function getEntityList() {
            // Get a list of programs (false means don't get from server)
            datacontext.getEntityList(modulesList, entityType, endPoint(), null, null, false);
        }

        function activate() {
            // If the view model has not been initialized, 
            if (!initialized) {
                // Then initialize the view model
                initializeViewModel();
            }
            return true;
        }

        function initializeViewModel() {
            // then Initialize the View Model
            return true;
        };

    });