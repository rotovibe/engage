/**
*	@module allergiesservice
*	@class allergiesservice
*/
define(['services/session', 'config.services', 'services/entityfinder'],
    function (session, servicesConfig, entityFinder) {

        var allergySearchEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'ContactCard');
        });

        // The end point to use when getting cohorts
        var myPatientAllergiesEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'PatientAllergy', 'PatientAllergy', null);
        });

        // Create an object to act as the datacontext
        var datacontext;

        // Create an object to use to reveal functions from this module
        var allergiesService = {
            getRemoteAllergies: getRemoteAllergies,
            saveAllergies: saveAllergies,
            initializeAllergy: initializeAllergy,
            initializeNewAllergy: initializeNewAllergy,
            getPatientAllergies: getPatientAllergies,
            getPatientAllergiesQuery: getPatientAllergiesQuery,
			deletePatientAllergy: deletePatientAllergy
        };
        return allergiesService;

        // POST to the server, check the results for entities
        function getRemoteAllergies(manager, searchString) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();
            
            // Create an end point to use
            var endpoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Search/allergy', 'IdName');
            
            // Create a payload from the JS object
            var payload = {};

            payload.SearchTerm = searchString;
            payload.Take = 15;
            payload = JSON.stringify(payload);
            
            var query = breeze.EntityQuery
                .from(endpoint.ResourcePath)
                .withParameters(payload)
                .toType('AllergySearch');

            // Query to post the results
            // var query = breeze.EntityQuery
            //     .from(endPoint.ResourcePath)
            //     .withParameters({
            //         $method: 'GET',
            //         $encoding: 'JSON',
            //         $data: payload
            //     });

            return manager.executeQuery(query).then(saveSucceeded).fail(saveFailed);

            function saveSucceeded(data) {
                return data.results;
            }
        }

        // Initialize a new observation
        function initializeAllergy(manager, observable, type, patientId, allergyId) {
            checkDataContext();
            datacontext.allergySaving(true);
            var path = 'PatientAllergy/Initialize';

            // Add initialize onto the end of everything
            //path += '/Initialize';

            // Create a payload from the JS object
            var payload = {};

            payload.PatientId = patientId;
            payload.AllergyId = allergyId;

            var resource = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), path, type);

            // Query to post the results
            var query = breeze.EntityQuery
                .from(resource.ResourcePath)
                .withParameters({
                    $method: 'POST',
                    $encoding: 'JSON',
                    $data: payload
                })
                .toType('PatientAllergy');

            // var query = breeze.EntityQuery
            //     .from(resource.ResourcePath)
                // .toType(type);

            return manager.executeQuery(query).then(dataReturned).fail(saveFailed);

            function dataReturned(data) {
                datacontext.allergySaving(false);
                observable(data.results[0]);
                return observable;
            }
        }

        function initializeNewAllergy(manager, allergyName) {
            checkDataContext();
            var path = 'Allergy/Initialize';

            // Add initialize onto the end of everything
            //path += '/Initialize';

            // Create a payload from the JS object
            var payload = {};

            payload.AllergyName = allergyName;

            var resource = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), path, 'AllergySearch');

            // Query to post the results
            var query = breeze.EntityQuery
                .from(resource.ResourcePath)
                .withParameters({
                    $method: 'POST',
                    $encoding: 'JSON',
                    $data: payload
                })
                .toType('AllergySearch');

            // var query = breeze.EntityQuery
            //     .from(resource.ResourcePath)
                // .toType(type);

            return manager.executeQuery(query).fail(saveFailed);

            function dataReturned(data) {
                return data.results[0];
            }
        }

        // POST to the server, check the results for entities
        function saveAllergies(manager, serializedAllergies) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'PatientAllergy/Update');

            // If there is as action
            if (serializedAllergies) {

                // Create a payload from the JS object
                var payload = {};

                payload.PatientAllergies = serializedAllergies;
                payload = JSON.stringify(payload);
                
                // Query to post the results
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: 'POST',
                        $encoding: 'JSON',
                        $data: payload
                    })
                    .toType('PatientAllergy');

                return manager.executeQuery(query).fail(saveFailed);
            }

            function saveSucceeded(data) {
                //entityFinder.searchForProblems(data.httpResponse.data);
                //datacontext.allergySaving(false);
                return data.results;
            }
        }
		
        /**
		*	@method deletePatientAllergy
		*	
		*/
		function deletePatientAllergy(manager, allergy){
			// If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();
			
			var patientAllergyId  = allergy.id();
			if(patientAllergyId){
				//REST delete endpoint:
				var endpoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient');
				endpoint.ResourcePath += allergy.patientId() + '/PatientAllergy/' + patientAllergyId;
				
				var query = breeze.EntityQuery
					.from(endpoint.ResourcePath)
					.withParameters({
                        $method: 'DELETE',
                        $encoding: 'JSON'                        
                    });
				return manager.executeQuery(query).then(deleted);	
			}
            
			function deleted(data){
				var thisAlert = datacontext.createEntity('Alert', { result: 1, reason: 'Allergy deleted!' });
                thisAlert.entityAspect.acceptChanges();
                datacontext.alerts.push(thisAlert);
                return data.httpResponse.data;
			}	
		}
		
        function saveFailed(error) {
            checkDataContext();
            console.log('Error - ', error);            
            var thisAlert = datacontext.createEntity('Alert', { result: 0, reason: 'Save failed!' });
            thisAlert.entityAspect.acceptChanges();
            datacontext.alerts.push(thisAlert);
        }

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }

        function getPatientAllergies (manager, observable, params, patientId) {
            checkDataContext();
            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Create a parameters object and assign a dynamic string query onto it, if those were passed in
            var parameters = {};
            if (params) {
                parameters = params;
            } else {
                var parameters = {
                };
            }
            // Create a payload from the JS object
            var payload = {};
            payload.StatusIds = params.StatusIds;
            payload = JSON.stringify(payload);

            // Query to post the results
            var query = breeze.EntityQuery
                .from(myPatientAllergiesEndPoint().ResourcePath + patientId)
                .withParameters({
                    $method: 'POST',
                    $encoding: 'JSON',
                    $data: payload
                })
                .toType('PatientAllergy');

            return manager.executeQuery(query).then(querySucceeded).fail(queryFailed);

            function querySucceeded(data) {
                var s = data.results;
                if (observable) {
                    return observable(s);
                } else {
                    return s;
                }
            }

            function queryFailed(error) {
                console.log(error);
                checkDataContext();
                checkForFourOhOne(error);
                var messager = 'Failed to load ' + entityType + ' from server.';
                var thisAlert = datacontext.createEntity('Alert', { result: 0, reason: messager });
                thisAlert.entityAspect.acceptChanges();
                datacontext.alerts.push(thisAlert);
                return false;
            }
        }

        function getPatientAllergiesQuery (manager, params, orderString) {
            // Make sure the datacontext has been loaded
            checkDataContext();
            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Create a base query
            var query = breeze.EntityQuery.from('PatientAllergy')
                .toType('PatientAllergy');

            // If there is an orderBy
            if (orderString) {
                // Add it
                query = query.orderBy(orderString);
            }

            // Create a predicate array
            var preds = [];

            // For each of the params,
            ko.utils.arrayForEach(params, function (param) {
                // Create a predicate
                var thispred = breeze.Predicate.create(param.Property, param.Operator, param.Value);
                // Add the predicate to the array of predicates
                preds.push(thispred);
            });

            // If there are predicates in the array,
            if (preds) {
                // Create a list of them
                var pred = breeze.Predicate.and(preds);
                query = query.where(pred);
            }

            return manager.executeQueryLocally(query);
        }

    });