define(['services/session', 'config.services'],
    function (session, servicesConfig) {

        // The end point to use when getting cohorts
        var myToDosEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Scheduling/ToDos', 'ToDo', null);
        });

        // Create an object to act as the datacontext
        var datacontext;

        // Create an object to use to reveal functions from this module
        var contactService = {
            saveNote: saveNote,
						getNote: getNote,
            deleteNote: deleteNote,
            saveToDo: saveToDo,
            getToDos: getToDos,
            getToDosQuery: getToDosQuery
        };
        return contactService;

        // POST to the server, check the results for entities
        function saveNote(manager, serializedNote, type) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }
			var receivingEntityType = '';
            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
			var endPoint;
			var method = 'POST'
			var isInsert = false;
			if( !isNaN(serializedNote.Id) && Number(serializedNote.Id) < 1 ){
				isInsert = true;
				if( type && type.toLowerCase() === 'utilization' ){
					serializedNote.Id = null;
					//different endpoint for utilization note type
					endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + serializedNote.PatientId + '/Notes/Utilizations', 'Utilization');
					receivingEntityType = 'Note';
				}
				else{
					endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + serializedNote.PatientId + '/Note/Insert', 'Note');
					receivingEntityType = 'Note';
				}
			}
			else{
				//update:
				if( type && type.toLowerCase() === 'utilization' ){
					//different endpoint for utilization note type
					endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + serializedNote.PatientId + '/Notes/Utilizations/' + String(serializedNote.Id) , 'Utilization');
					receivingEntityType = 'Note';
				}
				else{
					endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + serializedNote.PatientId + '/Note/' + String(serializedNote.Id) , 'PatientNote');
					receivingEntityType = 'Note';
				}
				method = 'PUT';
			}
            
			function saveSucceeded(data) {
                return data.httpResponse.data;
            }

            // If there is a contact card,
            if (serializedNote) {

                // Create a payload from the JS object
                var payload = {};

                payload[endPoint.EntityType] = serializedNote;	//insert Note: "Note" ; update Note: "PatientNote" ; Utilization- insert/update: "Utilization"
                payload = JSON.stringify(payload);

                // Query to post the results
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath).toType('Note')
                    .withParameters({
                        $method: method,
                        $encoding: 'JSON',
                        $data: payload
                    });

				//	breeze comment: this call is done for 'Utilization, 'Note' and 'PatientNote' endpoints.
				//	'Note' and 'PatientNote' endpoints are not returning the whole object. only the id.
				//	this is Y in this case we do not delete the observable used to hold the new "note" entity
				//	and replace it by the returned (.toType).

                //return query.execute().then(saveSucceeded).fail(postFailed);
                return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);
            }
        }

        function deleteNote(manager, note) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

			var method = 'POST';
			var endPoint = null;
            // Create an end point to use
			if( note.type() && note.type().name().toLowerCase() === 'utilization' ){
				//different endpoint for utilization note type
				endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + note.PatientId + '/Notes/Utilizations/' + note.id(), 'Note');
				method = 'DELETE';
			}
			else{
				endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + note.patientId() + '/Note/' + note.id() + '/Delete', 'Note');
			}

            // Create a payload from the JS object
            var payload = {};

            payload = JSON.stringify(payload);

            // Query to post the results
            var query = breeze.EntityQuery
                .from(endPoint.ResourcePath)
                .withParameters({
                    $method: method,
                    $encoding: 'JSON',
                    $data: payload
                });

            return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);

            function saveSucceeded(data) {
                return data.httpResponse.data;
            }
        }
		/**
		*	get note by id. initialy intended to load the full object of utilization type note only.
		*	as notes list in history are retrieved through the note endpoint, they are retrieving only some of the utilization props.
		*	@method getNote
		*	@param manager datacontext manager
		*	@param id {string}
		*	@param type {string} the note type name (examples: utilization, general touchpoint etc. )
		*	@param observable optional. to keep the result
		*/
		function getNote( manager, id, patientId, type, observable ){
			if( type && type.toLowerCase() === 'utilization' ){
				checkDataContext();
				// If there is no manager, we can't query using breeze
				if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

				var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(),
									'Patient/' + patientId + '/Notes/Utilizations/' + id, 'Note');

				// Query to post the results
				var query = breeze.EntityQuery
					.from(endPoint.ResourcePath)
					.withParameters({
						$method: 'GET',
						$encoding: 'JSON'
					})
					.toType('Note');

				return manager.executeQuery(query).then(querySucceeded).fail(queryFailed);
			}

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

        // POST to the server, check the results for entities
        function saveToDo(manager, serializedToDo, verb) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Scheduling/ToDo/' + verb, 'ToDo');

            // If there is a contact card,
            if (serializedToDo) {

                // Create a payload from the JS object
                var payload = {};

                payload.ToDo = serializedToDo;
                payload = JSON.stringify(payload);

                // Query to post the results
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .toType('ToDo')
                    .withParameters({
                        $method: 'POST',
                        $encoding: 'JSON',
                        $data: payload
                    });

                //return query.execute().then(saveSucceeded).fail(postFailed);
                return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);
            }

            function saveSucceeded(data) {
                if (data.results && data.results.length > 0) {
                    return data.results[0];
                } else {
                    return data.httpResponse.data;
                }
            }
        }

        function postFailed(error) {
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
		
        function getToDos (manager, observable, params, observableTotalCount) {
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

            payload.PatientId = params.PatientId;
            payload.AssignedToId = params.AssignedToId;
			payload.NotAssignedToId = params.NotAssignedToId;	//=> where AssignedToId != value && AssignedToId != null
            payload.CreatedById = params.CreatedById;
            payload.FromDate = params.FromDate;
            payload.StatusIds = params.StatusIds;
			payload.CategoryIds = params.CategoryIds;
			payload.PriorityIds = params.PriorityIds;
			payload.Skip = params.Skip;
			payload.Take = params.Take;
			payload.Sort = params.Sort;
            payload = JSON.stringify(payload);

            // Query to post the results
            var query = breeze.EntityQuery
                .from(myToDosEndPoint().ResourcePath)
                .withParameters({
                    $method: 'POST',
                    $encoding: 'JSON',
                    $data: payload
                })
                .toType('ToDo');

            return manager.executeQuery(query).then(querySucceeded).fail(queryFailed);

            function querySucceeded(data) {
                var s = data.results;
				if(observableTotalCount){					
					var count = data.httpResponse.data.TotalCount;					
					if( count != undefined ){
						observableTotalCount(count);						
					}					
				}
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

        function getToDosQuery (manager, params, orderString) {
            // Make sure the datacontext has been loaded
            checkDataContext();
            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Create a base query
            var query = breeze.EntityQuery.from('ToDo')
                .toType('ToDo');

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