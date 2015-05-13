//TODO: Inject dependencies
define(['services/navigation', 'services/session', 'config.services', 'services/datacontext'],
    function (navigation, session, servicesConfig, datacontext) {
        
        var vm = {
            activate: activate,
            navigation: navigation,
            title: 'Insight'
        };

        return vm;
        
        function activate() {
            console.log('Loaded Insight index');
        }

    });