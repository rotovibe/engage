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
            deleteNote: deleteNote,
            saveToDo: saveToDo,
            getToDos: getToDos,
            getToDosQuery: getToDosQuery
        };
        return contactService;
        
        // POST to the server, check the results for entities
        function saveNote(manager, serializedNote) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
			var endPoint;
			var method = 'POST'
			if( !isNaN(serializedNote.Id) && Number(serializedNote.Id) < 1 ){
				//insert: (new notes have negative int id's)
				endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + serializedNote.PatientId + '/Note/Insert', 'Note');					
			}
			else{
				//update:
				endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + serializedNote.PatientId + '/Note/' + String(serializedNote.Id) , 'PatientNote');	
				method = 'PUT'; 
			}
            

            // If there is a contact card,
            if (serializedNote) {

                // Create a payload from the JS object
                var payload = {};
				
                payload[endPoint.EntityType] = serializedNote;	//insert: "Note" ; update: "PatientNote"
                payload = JSON.stringify(payload);

                // Query to post the results
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: method,
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

        function deleteNote(manager, note) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + note.patientId() + '/Note/' + note.id() + '/Delete', 'Note');

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

        function getToDos (manager, observable, params) {
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
            payload.CreatedById = params.CreatedById;
            payload.FromDate = params.FromDate;
            payload.StatusIds = params.StatusIds;
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