define(['services/session', 'config.services'],
    function (session, servicesConfig) {

        // Create an object to act as the datacontext
        var datacontext;

        // Create an object to use to reveal functions from this module
        var observationsService = {
            getAdditionalObservationsByTypeIdAndPatientId: getAdditionalObservationsByTypeIdAndPatientId,
            saveObservations: saveObservations,
            initializeObservation: initializeObservation
        };
        return observationsService;
        
        // WARNING : Code smell!  Proceed with caution
        // -------------------------------------------
        // This is a bad way to go get all of the additional observations
        // that need to be used when searching for observations to create
        // but is necessary because the service was set up to pull the
        // options by type instead of just including the type in each option
        // for filtering purposes

        function getAdditionalObservationsByTypeIdAndPatientId(manager, typeId, patientId) {
            checkDataContext();
            var theseOptions = ko.observableArray();
            
            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'patient/' + patientId + '/Observation/Type/' + typeId + '/MatchLibrary/', 'AdditionalObservation');

            return datacontext.getEntityList(theseOptions, endPoint.EntityType, endPoint.ResourcePath, null, null, true).then(observationsReturned);

            function observationsReturned() {
                // Go through each option,
                ko.utils.arrayForEach(theseOptions(), function (option) {
                    // And set the type id of the observation, since it comes back null
                    option.typeId(typeId);
                });
                // Return
                return true;
            }
        }

        // POST to the server, check the results for entities
        function saveObservations(manager, serializedObservations) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + serializedObservations.PatientId + '/Observation/Update', 'Observation');

            // If there is a contact card,
            if (serializedObservations) {

                // Create a payload from the JS object
                var payload = {};

                payload.Observations = serializedObservations;
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

        // Initialize a new observation
        function initializeObservation(manager, observable, type, typeId, patientId, observationId) {
            checkDataContext();
            var path = 'Patient/' + patientId + '/Observation/' + observationId;

            // Add initialize onto the end of everything
            //path += '/Initialize';

            var resource = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), path, type);

            var query = breeze.EntityQuery
                .from(resource.ResourcePath)
                .toType(type);

            return manager.executeQuery(query).then(dataReturned).fail(postFailed);

            function dataReturned(data) {
                observable(data.results[0]);
                return data;
            }
        }

        function postFailed(error) {
            console.log(error);
        }

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }

    });