define(['services/session', 'config.services'],
    function (session, servicesConfig) {

        // Create an object to act as the datacontext
        var datacontext;

        // Create an object to use to reveal functions from this module
        var patientsService = {
            saveBackground: saveBackground,
            getFullSSN: getFullSSN
        };
        return patientsService;
        
        // POST to the server, check the results for entities
        function saveBackground(manager, patientId, background) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + patientId + '/Background', 'Background');

            // If there is a patient id,
            if (patientId) {

                // Create a payload from the JS object
                var payload = {};

                payload.PatientId = patientId;
                payload.Background = background;
                payload = JSON.stringify(payload);

                // Query to post the results
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: 'POST',
                        $encoding: 'JSON',
                        $data: payload
                    });
                
                //return query.execute().then(saveSucceeded).fail(postFailed);
                return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);
            }

            function saveSucceeded(data) {
                console.log(data);
                return data.httpResponse.data;
            }
        }

        function getFullSSN(manager, patientId) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + patientId + '/SSN');

            // If there is a patient id,
            if (patientId) {

                // Create a payload from the JS object
                var payload = {};

                payload = JSON.stringify(payload);

                // Query to post the results
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: 'GET',
                        $encoding: 'JSON',
                        $data: payload
                    });
                
                //return query.execute().then(saveSucceeded).fail(postFailed);
                return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);
            }

            function saveSucceeded(data) {
                console.log(data);
                return data.httpResponse.data;
            }
        }

        function postFailed(error) {
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