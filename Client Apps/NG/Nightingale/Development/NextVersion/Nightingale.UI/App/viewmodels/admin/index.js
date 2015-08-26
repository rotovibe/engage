//TODO: Inject dependencies
define(['services/navigation', 'services/session', 'config.services', 'services/datacontext'],
    function (navigation, session, servicesConfig, datacontext) {
        
        var vm = {
            activate: activate,
            navigation: navigation,
            title: 'Admin'
        };

        return vm;
        
        function activate() {
            console.log('Loaded admin index')
        }

    });