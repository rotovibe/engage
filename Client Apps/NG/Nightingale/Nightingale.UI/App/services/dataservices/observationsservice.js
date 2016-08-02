define(['services/session', 'config.services'],
    function (session, servicesConfig) {

        // Create an object to act as the datacontext
        var datacontext;

        // Create an object to use to reveal functions from this module
        var observationsService = {
            getObservations: getObservations,
            getObservationsByTypeId: getObservationsByTypeId,
            saveObservations: saveObservations,
            initializeObservation: initializeObservation
        };
        return observationsService;
        
        // WARNING : Code smell!  Proceed with caution
        // -------------------------------------------
        // This is a bad way to go get all of the observations
        // that need to be used when searching for observations to create
        // but is necessary because the service was set up to pull the
        // options by type instead of just including the type in each option
        // for filtering purposes

        function getObservations(manager) {
            checkDataContext();
            var theseOptions = ko.observableArray();
            
            // If it is a type that is equal to a problem, append nothign, else append false
            // 
            // Clearly a hack but we are only supposed to send false if it does NOT match
            // the hardcoded type id of problem...
            //ar falseOrNot = typeId !== '533d8278d433231deccaa62d' ? "/MatchLibrary/false" : '/MatchLibrary/';

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), '/Observations', 'Observation');

            return datacontext.getEntityList(theseOptions, endPoint.EntityType, endPoint.ResourcePath, null, null, true);
            //.then(observationsReturned);

            // function observationsReturned() {
            //     // Go through each option,
            //     ko.utils.arrayForEach(theseOptions(), function (option) {
            //         // And set the type id of the observation, since it comes back null
            //         option.typeId(typeId);
            //     });
            //     // Return
            //     return true;
            // }
        }
        function getObservationsByTypeId(manager, typeId) {
            checkDataContext();
            var theseOptions = ko.observableArray();
            
            // If it is a type that is equal to a problem, append nothign, else append false
            // 
            // Clearly a hack but we are only supposed to send false if it does NOT match
            // the hardcoded type id of problem...
            var falseOrNot = typeId !== '533d8278d433231deccaa62d' ? "/MatchLibrary/false" : '/MatchLibrary/';

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), '/Observation/Type/' + typeId + falseOrNot, 'Observation');

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
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + serializedObservations.PatientId + '/Observation/Update', 'PatientObservation');

            // If there is a contact card,
            if (serializedObservations) {

                // Create a payload from the JS object
                var payload = {};

                payload.PatientObservations = serializedObservations;
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
            datacontext.observationsSaving(true);
            var path = 'Patient/' + patientId + '/Observation/' + observationId;
            // If it is a type that is equal to a problem, append nothing, else append false
            // 
            // Clearly a hack but we are only supposed to send the problem thing if it matches
            // the hardcoded type id of problem...
            path += typeId !== '533d8278d433231deccaa62d' ? '' : '/Problem/Initialize';

            // Add initialize onto the end of everything
            //path += '/Initialize';

            var resource = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), path, type);

            var query = breeze.EntityQuery
                .from(resource.ResourcePath)
                .toType(type);

            return manager.executeQuery(query).then(dataReturned).fail(postFailed);

            function dataReturned(data) {
                datacontext.observationsSaving(false);
                observable(data.results[0]);
                return data;
            }
        }

        function postFailed(error) {
            datacontext.observationsSaving(false);
            console.log(error);
        }

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }

    });