define(['services/session', 'config.services', 'services/entityfinder'],
    function (session, servicesConfig, entityFinder) {
        
        // The end point to use when getting cohorts
        var myInterventionsEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Goal/Interventions', 'Intervention', null);
        });

        // The end point to use when getting cohorts
        var myTasksEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Goal/Tasks', 'Task', null);
        });

        // Create an object to act as the datacontext
        var datacontext;

        // Create an object to use to reveal functions from this module
        var contactService = {
            initializeEntity: initializeEntity,
            saveGoal: saveGoal,
            saveIntervention: saveIntervention,
            saveTask: saveTask,
            saveBarrier: saveBarrier,
            deleteGoal: deleteGoal,
            getInterventions: getInterventions,
            getInterventionsQuery: getInterventionsQuery,
            getTasks: getTasks,
            getTasksQuery: getTasksQuery
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
                    })
                    .toType('Goal');

                return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);
            }

            function saveSucceeded(data) {
                return data.httpResponse.data;
            }
        }

        function saveIntervention(manager, serializedIntervention, patientId) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + patientId + '/Goal/' + serializedIntervention.PatientGoalId + '/Intervention/' + serializedIntervention.Id + '/Update', 'Goal');

            // If there is a contact card,
            if (serializedIntervention) {

                // Create a payload from the JS object
                var payload = {};

                payload.Intervention = serializedIntervention;
                payload = JSON.stringify(payload);

                // Query to post the results
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: 'POST',
                        $encoding: 'JSON',
                        $data: payload
                    })
                    .toType('Intervention');

                return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);
            }

            function saveSucceeded(data) {
                var results = data.results[0];
                return data.httpResponse.data;
            }
        }

        function saveTask(manager, serializedTask, patientId) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + patientId + '/Goal/' + serializedTask.PatientGoalId + '/Task/' + serializedTask.Id + '/Update', 'Goal');

            // If there is a contact card,
            if (serializedTask) {

                // Create a payload from the JS object
                var payload = {};

                payload.Task = serializedTask;
                payload = JSON.stringify(payload);

                // Query to post the results
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: 'POST',
                        $encoding: 'JSON',
                        $data: payload
                    })
                    .toType('Task');

                return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);
            }

            function saveSucceeded(data) {
                var results = data.results[0];
                return data.httpResponse.data;
            }
        }

        function saveBarrier(manager, serializedBarrier, patientId) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + patientId + '/Goal/' + serializedBarrier.PatientGoalId + '/Barrier/' + serializedBarrier.Id + '/Update', 'Goal');

            // If there is a contact card,
            if (serializedBarrier) {

                // Create a payload from the JS object
                var payload = {};

                payload.Barrier = serializedBarrier;
                payload = JSON.stringify(payload);

                // Query to post the results
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: 'POST',
                        $encoding: 'JSON',
                        $data: payload
                    })
                    .toType('Barrier');

                return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);
            }

            function saveSucceeded(data) {
                var results = data.results[0];
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
            datacontext.alerts.push(thisAlert);
        }

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }

        function getInterventions (manager, observable, params) {
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
                .from(myInterventionsEndPoint().ResourcePath)
                .withParameters({
                    $method: 'POST',
                    $encoding: 'JSON',
                    $data: payload
                })
                .toType('Intervention');
            
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

        function getInterventionsQuery (manager, params, orderString) {
            // Make sure the datacontext has been loaded
            checkDataContext();
            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Create a base query
            var query = breeze.EntityQuery.from('Interventions')
                .toType('Intervention');

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

        function getTasks (manager, observable, params) {
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
                .from(myTasksEndPoint().ResourcePath)
                .withParameters({
                    $method: 'POST',
                    $encoding: 'JSON',
                    $data: payload
                })
                .toType('Task');

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

        function getTasksQuery (manager, params, orderString) {
            // Make sure the datacontext has been loaded
            checkDataContext();
            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Create a base query
            var query = breeze.EntityQuery.from('Tasks')
                .toType('Task');

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