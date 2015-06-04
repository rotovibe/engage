/**
*	@module medicationservice
*	@class medicationservice
*/
define(['services/session', 'config.services', 'services/entityfinder', 'services/dataservices/getentityservice'],
    function (session, servicesConfig, entityFinder, getEntityService) {

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

		var medicationFrequencyEndpoint  = ko.computed(function () {
			var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
			return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'PatientMedSupp/Frequency', 'PatientMedicationFrequency');
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
			getPatientFrequencies: getPatientFrequencies,
			saveCustomFrequency: saveCustomFrequency,
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
		
		/**
		*	@method getLocalFrequencyById
		*
		*/
		function getLocalFrequencyById(manager, id){
			return getEntityService.getLocalById(manager, medicationFrequencyEndpoint().ResourcePath + id, 'PatientMedicationFrequency', 'id', id);
		}
		/**
		*	saving a frequency lookup for a specific patient.
		*	@method saveCustomFrequency
		*	@param	customFrequency - string mandatory
		*	@param	patientId - string mandatory
		*/
		function saveCustomFrequency(manager, customFrequency, patientId){
			if(customFrequency && patientId){
				checkDataContext();
				// If there is no manager, we can't query using breeze
				if (!manager) { throw new Error("[manager] cannot be a null parameter"); }
				
				// Create a payload from the JS object
				var payload = {
					PatientMedFrequency: {
						PatientId: patientId,
						Name: customFrequency
					}
				};
				payload = JSON.stringify(payload);

				// Query to post the results
				var query = breeze.EntityQuery
					.from(medicationFrequencyEndpoint().ResourcePath + 'Insert')	//PatientMedSupp/Frequency/Insert
					.withParameters({
						$method: 'POST',
						$encoding: 'JSON',
						$data: payload
					})
					.toType('PatientMedicationFrequency');
				
				return manager.executeQuery(query).then(saveSucceeded).fail(saveFailed);

				function saveSucceeded(data) {
					if(data && data.httpResponse && data.httpResponse.data){
						var result = data.httpResponse.data
						if(result.Id){
							//in case the user tries to create a duplicate(case is insensitive!), backend will return existing frequency id:
							var existingFrequency = getLocalFrequencyById(manager, result.Id);
							if(existingFrequency.length === 0){
								manager.createEntity('PatientMedicationFrequency', {id: result.Id, name: customFrequency, patientId: patientId, sortOrder: 0}, breeze.EntityState.Unchanged);
							}
						}
						return result;                
					}
					return data;
				}
			}
		}
		
		/**
		*			get patient specific medication frequencies.
		*			the frequency lookup dropdown will need to show general frequency values and these patient specific values (if any)
		*			the dropdown needs to show a merged list of the lookup values (Frequency entity/lookup) and patient frequencies from PatientMedSupp/Frequency/<patientId>.
		*			the entity: PatientMedicationFrequency is on the client cache only and it represent that list, with added patientId property.
		*			the global frequencies taken from the lookup "Frequency" are pushed into this collection with patientId null.		
		*	@method getPatientFrequencies 
		*/
		function getPatientFrequencies(manager, observable, patientId, forceRemote){
			checkDataContext();
            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }
			
			if(forceRemote){
				//query remotely: get patient medication frequencies for this patient only:
				var query = breeze.EntityQuery
					.from(medicationFrequencyEndpoint().ResourcePath + patientId)	//PatientMedSupp/Frequency/<patientId>                
					.toType('PatientMedicationFrequency');//Frequency
					
				return manager.executeQuery(query).then(remoteQuerySucceeded).fail(queryFailed);
			}
			else return getLocalFrequencies(observable, patientId);
			
			/**
			*	once we got this specific patient frequencies or we assume we have it in the cache (forceRemote=null or undefined), 
			*		query locally to get a merged frequency list of this patient + the static part of the frequencies list (patientId=null or empty)
			*	note: to query by patientId it must be in the properties of the metadata of the entity.			
			*	@method getLocalFrequencies
			*
			*/
			function getLocalFrequencies(observable, patientId){
				var pred = breeze.Predicate.create("patientId", breeze.FilterQueryOp.Equals, null)
							.or("patientId", breeze.FilterQueryOp.Equals, '')
							.or("patientId", breeze.FilterQueryOp.Equals, patientId)
							.or("patientId", breeze.FilterQueryOp.Equals, "-1");
				var localQuery = breeze.EntityQuery
					.from(medicationFrequencyEndpoint().ResourcePath + patientId)	//PatientMedSupp/Frequency/<patientId>                
					.toType('PatientMedicationFrequency')
					.where(pred)
					.orderBy('sortOrder, name');
				localQuery = localQuery.using(breeze.FetchStrategy.FromLocalCache);
				return manager.executeQuery(localQuery).then(localQuerySucceeded).fail(queryFailed);	//async local query
					
				function localQuerySucceeded(data){
					var list = data.results;
					if (observable) {						
						return observable(list);
					} else {
						return list;
					}	
				}
			}
			
            function remoteQuerySucceeded(data) {
				if(patientId){					
					//note the server return only the patient specific frequencies (if any) and this is only part of the list required in the dropdown,
					//	since we also need to merge the global (patientId=null, patientId=-1) frequencies
					ko.utils.arrayForEach(data.results ,function(frequency){	
						//add the patientId as the server does not return it, but we maintain this property:
						frequency.patientId(patientId);
						frequency.sortOrder(0);
						frequency.entityAspect.acceptChanges();
					});
				}
				//query locally: in order to get the full list of frequencies of this patient including the global frequencies:
				return getLocalFrequencies(observable, patientId);
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