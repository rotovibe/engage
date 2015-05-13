define(['services/session', 'config.services', 'services/entityfinder'],
    function (session, servicesConfig, entityFinder) {
        
        // Create an object to act as the datacontext
        var datacontext;

        // Create an object to use to reveal functions from this module
        var contactService = {
            initializeEntity: initializeEntity,
            saveGoal: saveGoal,
            deleteGoal: deleteGoal
        };
        return contactService;
        
        function initializeEntity(manager, observable, type, patientId, goalId) {
            checkDataContext();
            var path = 'Patient/' + patientId + '/Goal';

            // If a goalId was passed in, 
            if (goalId) {
                // Add it and the type
                path += '/' + goalId + '/' + type
            }
            // Add initialize onto the end of everything
            path += '/Initialize';

            var resource = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), path, type);
            
            var payload = { };
            ///{Version}/{ContractNumber}/Patient/{PatientId}/Goal/{PatientGoalId}/Barrier/Initialize
            ///{Version}/{ContractNumber}/Patient/{PatientId}/Goal/Initialize

            var query = breeze.EntityQuery
                .from(resource.ResourcePath)
                .toType(type)
                .withParameters(payload);
            
            return manager.executeQuery(query).fail(postFailed);
        }

        // POST to the server, check the results for entities
        function saveGoal(manager, serializedGoal) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + serializedGoal.PatientId + '/Goal/' + serializedGoal.Id + '/Update', 'Goal');

            // If there is a contact card,
            if (serializedGoal) {

                // Create a payload from the JS object
                var payload = {};

                payload.Goal = serializedGoal;
                payload = JSON.stringify(payload);

                // Query to post the results
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: 'POST',
                        $encoding: 'JSON',
                        $data: payload
                    });

                return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);
            }

            function saveSucceeded(data) {
                return data.httpResponse.data;
            }
        }

        function deleteGoal(manager, goal) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + goal.patientId() + '/Goal/' + goal.id() + '/Delete', 'Goal');

            // Create a payload from the JS object
            var payload = {};

            payload = JSON.stringify(payload);

            // Query to post the results
            var query = breeze.EntityQuery
                .from(endPoint.ResourcePath)
                .withParameters({
                    $method: 'POST',
                    $encoding: 'JSON',
                    $data: payload
                });

            return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);

            function saveSucceeded(data) {
                return data.httpResponse.data;
            }
        }

        function postFailed(message) {
            checkDataContext();
            console.log('Error - ', error);            
            var thisAlert = datacontext.createEntity('Alert', { result: 0, reason: 'Save failed!' });
            thisAlert.entityAspect.acceptChanges();
            datacontext.enums.alerts.push(thisAlert);
        }

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }

    });