define(['services/session', 'config.services'],
    function (session, servicesConfig) {

        // Create an object to act as the datacontext
        var datacontext;

        // Create an object to use to reveal functions from this module
        var patientsService = {
            //saveBackground: saveBackground,
            saveIndividual: saveIndividual,
            getFullSSN: getFullSSN,
            deleteIndividual: deleteIndividual,
            addPatientToRecentList: addPatientToRecentList,
            initializeIndividual: initializeIndividual,
            savePatientSystems: savePatientSystems,
			deletePatientSystems: deletePatientSystems
        };
        return patientsService;
        
        /**obsolete: use saveIndividual** POST to the server, check the results for entities
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
                return data.httpResponse.data;
            }
        }*/
		
        // POST to the server, check the results for entities
        function saveIndividual(manager, serializedPatient, insert) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            var patientId = serializedPatient.Id;

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/Update', 'Patient');

            // If there is a patient id,
            if (patientId) {

                // Create a payload from the JS object
                var payload = {};

                payload.Patient = serializedPatient;
                payload.Insert = insert;
                if (serializedPatient.InsertDuplicate) {
                    payload.InsertDuplicate = true;
                }
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
                return manager.executeQuery(query).fail(postFailed);
            }

        }

        // POST to the server, check the results for entities
        function deleteIndividual(manager, patient) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            var patientId = patient.id();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + patientId + '/Delete', 'Patient');

            // If there is a patient id,
            if (patientId) {

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
                
                return manager.executeQuery(query).then(saveSucceeded);
            }

            function saveSucceeded(data) {
                var thisAlert = datacontext.createEntity('Alert', { result: 1, reason: 'Individual deleted!' });
                thisAlert.entityAspect.acceptChanges();
                datacontext.alerts.push(thisAlert);
                return data.httpResponse.data;
            }
        }

        function initializeIndividual (manager, observable) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/Initialize');


            // Create a payload from the JS object
            // var payload = {};

            // payload = JSON.stringify(payload);

            // Query to post the results
            var query = breeze.EntityQuery
                .from(endPoint.ResourcePath)
                .toType('Patient');
            
            //return query.execute().then(saveSucceeded).fail(postFailed);
            return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);

            function saveSucceeded(data) {
                observable(data.results[0]);
                return data.httpResponse.data;
            }
        }
		
		function deletePatientSystems(manager, deleteIds, patientId){
			// If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();
			if (deleteIds) {
                 var IdsString = deleteIds.join();	//string - comma separated id's. no payLoad.				                

				// Create an end point to use
				var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + patientId + '/PatientSystems/' + IdsString, 'PatientSystem');

                // Query
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: 'DELETE',
                        $encoding: 'JSON'
                        //$data: payload
                    });

                return manager.executeQuery(query).then(deleteSucceeded).fail(postFailed);
            }

            function deleteSucceeded(data) {
                return data.httpResponse.data;				
            }
		}
		
        // POST(insert)/PUT(update) to the server, check the results for entities
        function savePatientSystems(manager, serializedPatientSystems, isInsert) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();
            
            // If there is a contact card,
            if (serializedPatientSystems && serializedPatientSystems.length) {
				
				var patientId = serializedPatientSystems[0].PatientId;
				// Create an end point to use
				var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + patientId + '/PatientSystems', 'PatientSystem');
	
                // Create a payload from the JS object
                var payload = {};

                payload.PatientSystems = serializedPatientSystems;
				var method = 'PUT'
                if (isInsert) {
                    method = 'POST';
                }
                payload = JSON.stringify(payload);

                // Query to post the results
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: method,
                        $encoding: 'JSON',
                        $data: payload
                    });
				if( isInsert ){
					query = breeze.EntityQuery
                    .from(endPoint.ResourcePath).toType('PatientSystem')
                    .withParameters({
                        $method: method,
                        $encoding: 'JSON',
                        $data: payload
                    });					
				}
                return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);
            }

            function saveSucceeded(data) {
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
                return data.httpResponse.data;
            }
        }

        function postFailed(error) {
            checkDataContext();
            console.log('[PatientService] Error - ', error);
            var thisAlert = datacontext.createEntity('Alert', { result: 0, reason: 'Save failed!' });
            thisAlert.entityAspect.acceptChanges();
            datacontext.alerts.push(thisAlert);
            return error;
        }

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }

        function addPatientToRecentList(patient) {
            var thisUser = session.currentUser();
            var theseRecentIndividuals = thisUser.recentIndividuals();
            // Check if the individual is already in the list
            if (theseRecentIndividuals.indexOf(patient) !== -1) {
                // Move them to the top and shift everyone else down
                thisUser.recentIndividuals.remove(patient);
                thisUser.recentIndividuals.unshift(patient);
            } else {
                // Insert them at the top and shift everyone else down
                thisUser.recentIndividuals.unshift(patient);
            }
        }

    });