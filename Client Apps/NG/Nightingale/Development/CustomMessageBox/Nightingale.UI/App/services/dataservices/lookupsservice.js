define(['models/base', 'config.services', 'services/session'],
    function (modelConfig, servicesConfig, session) {
        
        var datacontext;

        function getLookup (manager, type, observable, forceRemote) {
            
            var resource;

            if (session && session.currentUser) {
                resource = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Lookup/' + type, type);
            }

            // Query to post the results
            var query = breeze.EntityQuery
                .from(resource.ResourcePath)
                .toType(resource.EntityType);

            return manager.executeQuery(query).then(saveSucceeded).fail(loginFailed);

            function saveSucceeded(data) {
                var s = data.results;
                // Make sure the user has been materialized
                return observable(s);
            }
        }

        // Get note lookups
        // TODO: Rename this method
        function getNoteLookup (manager, type, observable, forceRemote) {
            
            var resource;

            if (session && session.currentUser) {
                resource = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Lookup/Details/' + type, type);
            }

            // Query to post the results
            var query = breeze.EntityQuery
                .from(resource.ResourcePath)
                .toType(resource.EntityType);

            return manager.executeQuery(query).then(saveSucceeded).fail(loginFailed);

            function saveSucceeded(data) {
                var s = data.results;
                // Make sure the user has been materialized
                return observable(s);
            }
        }

        var userservice = {
            getLookup: getLookup,
            getNoteLookup: getNoteLookup
        };

        return userservice;

        function loginFailed(error) {
            checkDataContext();
            console.log('Error - ', error);
            var thisAlert = datacontext.createEntity('Alert', { result: 0, reason: 'Load lookups failed!' });
            thisAlert.entityAspect.acceptChanges();
            datacontext.alerts.push(thisAlert);
        }

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }
    });