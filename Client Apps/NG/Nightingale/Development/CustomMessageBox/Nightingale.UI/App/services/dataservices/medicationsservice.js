/**
*	@module medicationservice
*
*/
define(['services/session', 'config.services', 'services/entityfinder'],
    function (session, servicesConfig, entityFinder) {

        var medicationSearchEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'ContactCard');
        });

        // The end point to use when getting cohorts
        var myPatientMedicationsEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'PatientMedSupp', 'PatientMedication', null);
        });

        // Create an object to act as the datacontext
        var datacontext;

        // Create an object to use to reveal functions from this module
        var medicationsService = {
            getRemoteMedications: getRemoteMedications,
            saveMedication: saveMedication,
            initializeMedication: initializeMedication,
            initializeNewMedication: initializeNewMedication,
            getPatientMedications: getPatientMedications,
            getRemoteMedicationFields: getRemoteMedicationFields,
            getPatientMedicationsQuery: getPatientMedicationsQuery,
			deletePatientMedication: deletePatientMedication
        };
        return medicationsService;

        // POST to the server, check the results for entities
        function getRemoteMedications(manager, searchString) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();
            
            // Create an end point to use
            var endpoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Search/meds', 'IdName');
            
            // Create a payload from the JS object
            var payload = {};

            payload.SearchTerm = searchString;
            payload.Take = 15;
            payload = JSON.stringify(payload);
            
            var query = breeze.EntityQuery
                .from(endpoint.ResourcePath)
                .withParameters(payload)
                .toType('MedicationSearch');

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

        // POST to the server, check the results for entities
        function getRemoteMedicationFields(manager, medicationName, route, form, strength) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();
            
            // Create an end point to use
            var endpoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Search/meds/fields', 'MedicationSearch');
            
            // Create a payload from the JS object
            var payload = {};

            payload.Name = medicationName;
            // if (medication.type()) {
            //     payload.Type = medication.type().name();
            // }
            if (route) {
                payload.Route = route;
            }
            if (form) {
                payload.Form = form;
            }
            if (strength) {
                // Try to only get the value and strip out units
                // Per discussion with Mel and Snehal that there
                // will absolutely only be a value followed by a space
                payload.Strength = strength.split(' ')[0];
            }
            
            var query = breeze.EntityQuery
                .from(endpoint.ResourcePath)
                .withParameters(payload)
                .toType('MedicationSearch');

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
                return data.httpResponse.data;
            }
        }

        // Initialize a new observation
        function initializeMedication(manager, observable, type, patientId, medicationId) {
            checkDataContext();
            datacontext.medicationSaving(true);
            var path = 'PatientMedication/Initialize';

            // Add initialize onto the end of everything
            //path += '/Initialize';

            // Create a payload from the JS object
            var payload = {};

            payload.PatientId = patientId;
            payload.MedicationId = medicationId;

            var resource = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), path, type);

            // Query to post the results
            var query = breeze.EntityQuery
                .from(resource.ResourcePath)
                .withParameters({
                    $method: 'POST',
                    $encoding: 'JSON',
                    $data: payload
                })
                .toType('PatientMedication');

            // var query = breeze.EntityQuery
            //     .from(resource.ResourcePath)
                // .toType(type);

            return manager.executeQuery(query).then(dataReturned).fail(saveFailed);

            function dataReturned(data) {
                datacontext.medicationSaving(false);
                observable(data.results[0]);
                return observable;
            }
        }

        function initializeNewMedication(manager, medicationName) {
            checkDataContext();
            var path = 'MedicationMap/Initialize';

            // Create a payload from the JS object
            var payload = {
                MedicationMap: {
                    FullName: medicationName    
                }
            };
            
            var resource = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), path);

            // Query to post the results
            var query = breeze.EntityQuery
                .from(resource.ResourcePath)
                .withParameters({
                    $method: 'POST',
                    $encoding: 'JSON',
                    $data: payload
                });

            return manager.executeQuery(query).then(dataReturned).fail(saveFailed);

            function dataReturned(data) {
                return data.httpResponse.data;                
            }
        }

        // POST to the server, check the results for entities
        function saveMedication(manager, serializedMedication, isInsert) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'PatientMedSupp/Save');

            // If there is as action
            if (serializedMedication) {

                // Create a payload from the JS object
                var payload = {};

                payload.PatientMedSupp = serializedMedication;

                if (isInsert) {
                    payload.Insert = true;
                }

                if (serializedMedication.RecalculateNDC && !isInsert) {
                    payload.RecalculateNDC = true;
                }
                
                payload = JSON.stringify(payload);
                
                // Query to post the results
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: 'POST',
                        $encoding: 'JSON',
                        $data: payload
                    })
                    .toType('PatientMedication');

                return manager.executeQuery(query).fail(saveFailed);
            }

            function saveSucceeded(data) {
                //entityFinder.searchForProblems(data.httpResponse.data);
                //datacontext.allergySaving(false);
                return data.results;
            }
        }
        
        /**
		*	@method deletePatientMedication
		*	
		*/
		function deletePatientMedication(manager, medication){
			// If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();
						
			var medicationId  = medication.id();
			if(medicationId){				
				//a REST delete endpoint
				var endpoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient');
				endpoint.ResourcePath += medication.patientId() + '/PatientMedSupp/' + medicationId;
				
				var query = breeze.EntityQuery
					.from(endpoint.ResourcePath)
					.withParameters({
                        $method: 'DELETE',
                        $encoding: 'JSON'                        
                    });
				return manager.executeQuery(query).then(deleted);	
			}
            
			function deleted(data){
				var thisAlert = datacontext.createEntity('Alert', { result: 1, reason: 'Medication deleted!' });
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

        function getPatientMedications (manager, observable, params, patientId) {
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
            payload.CategoryIds = params.CategoryIds;
            payload = JSON.stringify(payload);

            // Query to post the results
            var query = breeze.EntityQuery
                .from(myPatientMedicationsEndPoint().ResourcePath + patientId)
                .withParameters({
                    $method: 'POST',
                    $encoding: 'JSON',
                    $data: payload
                })
                .toType('PatientMedication');

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

        function getPatientMedicationsQuery (manager, params, orderString) {
            // Make sure the datacontext has been loaded
            checkDataContext();
            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Create a base query
            var query = breeze.EntityQuery.from('PatientMedication')
                .toType('PatientMedication');

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