define([],
    function () {

        var datacontext;

        var getEntityList = function (manager, message, entityObservable, entityType, endpoint, parentPropertyName, parentPropertyId, forceRemote, params, orderBy) {
            checkDataContext();
            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // If it is not forceRemote, then try to get this from the local cache
            if (!forceRemote) {
                var p = getLocalList(manager, endpoint, entityType, parentPropertyName, parentPropertyId, orderBy);  //Go check and see if we have this locally already
                if (p.length > 0) {
                    entityObservable(p);
                    return Q.resolve(message);
                }
            }

            // If there is a parentPropertyId but not a name then just tack the ID onto the end of the resource path
            if (parentPropertyId && !parentPropertyName) {
                endpoint = endpoint + parentPropertyId;
            }

            // Create a parameters object and assign a dynamic string query onto it, if those were passed in
            var parameters = {};
            if (params) {
                parameters = params;
            } else {
                var parameters = {
                };
            }

            if (parentPropertyId && parentPropertyName) {
                parameters[parentPropertyName] = parentPropertyId;
            }

            var query = breeze.EntityQuery
                .from(endpoint)
                .withParameters(parameters)
                .toType(entityType);
            
            return manager.executeQuery(query).then(querySucceeded).fail(queryFailed);

            function querySucceeded(data) {
                
                var s = data.results;
                if (entityObservable) {
                    entityObservable(s)
                }
                if (orderBy) {
                    orderResults(entityObservable, orderBy);
                }
                return message;
            }

            function queryFailed(error) {
                checkDataContext();
                console.log(error);
                var messager = 'Failed to load ' + entityType + ' from server.';
                var thisAlert = datacontext.createEntity('Alert', { result: 0, reason: messager });
                thisAlert.entityAspect.acceptChanges();
                datacontext.enums.alerts.push(thisAlert);
                return message;
            }
        };

        var getEntityById = function (manager, message, entityObservable, id, entityType, endpoint, forceRemote) {

            checkDataContext();

            var fullEndPoint = endpoint + id;
                        
            // Create a parameters object and assign a dynamic string query onto it, if those were passed in
            var parameters = {
            };

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // If it is not forceRemote, then try to get this from the local cache
            if (!forceRemote) {
                // entityType + - removed that from the iD parameter
                var p = getLocalById(manager, fullEndPoint, entityType, 'id', id);  //Go check and see if we have this locally already
                if (p.length > 0) {
                    entityObservable(p[0]);
                    return Q.resolve(message);  //Resolve any promises that are waiting on a resolution
                }
            }

            var query = breeze.EntityQuery
                .from(fullEndPoint)
                .withParameters(parameters)
                .toType(entityType);

            return manager.executeQuery(query).then(querySucceeded).fail(queryFailed);

            function querySucceeded(data) {
                var s = data.results[0];
                if (entityObservable) {
                    entityObservable(s);
                }
                return message;
            }

            function queryFailed(error) {
                checkDataContext();
                console.log(error);
                var messager = 'Failed to load ' + entityType + ' from server.';
                var thisAlert = datacontext.createEntity('Alert', { result: 0, reason: messager });
                thisAlert.entityAspect.acceptChanges();
                datacontext.enums.alerts.push(thisAlert);
                return message;
            }
        };

        var getMelsEntityById = function (manager, message, entityObservable, id, entityType, endpoint, forceRemote, params) {

            checkDataContext();

            var fullEndPoint = endpoint;

            if (id) {
                fullEndPoint = fullEndPoint + id;
            }

            // Create a parameters object and assign a dynamic string query onto it, if those were passed in
            var parameters = {};
            if (params) {
                parameters = params;
            } else {
                var parameters = {
                };
            }

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // If it is not forceRemote, then try to get this from the local cache
            if (!forceRemote) {
                // entityType + - removed that from the iD parameter
                var p = getLocalById(manager, fullEndPoint, entityType, 'id', id);  //Go check and see if we have this locally already
                if (p.length > 0) {
                    entityObservable(p[0]);
                    return Q.resolve(message);  //Resolve any promises that are waiting on a resolution
                }
            }

            var query = breeze.EntityQuery
                .from(fullEndPoint)
                .withParameters(parameters)
                .toType(entityType);

            return manager.executeQuery(query).then(querySucceeded).fail(queryFailed);

            function querySucceeded(data) {
                var s = data.results[0];
                if (entityObservable) {
                    entityObservable(s);
                }
                return message;
            }

            function queryFailed(error) {
                checkDataContext();
                console.log(error);
                var messager = 'Failed to load ' + entityType + ' from server.';
                var thisAlert = datacontext.createEntity('Alert', { result: 0, reason: messager });
                thisAlert.entityAspect.acceptChanges();
                datacontext.enums.alerts.push(thisAlert);
                return message;
            }
        };

        var service = {
            getEntityList: getEntityList,
            getEntityById: getEntityById,
            getMelsEntityById: getMelsEntityById
        };
        return service;

        function getLocalList(manager, resource, entityType, parentPropertyName, parentPropertyId, orderby) {
            var query = breeze.EntityQuery.from(resource)
                .toType(entityType);

            if (orderby) {
                query = query.orderBy(orderby);
            }

            if (parentPropertyName && parentPropertyId) {                
                query = query.where(parentPropertyName, '==', parentPropertyId);
            }
            return manager.executeQueryLocally(query);
        }

        function getLocalById(manager, resource, entityType, idPropertyName, id) {
            var query = breeze.EntityQuery.from(resource)
                .where(idPropertyName, '==', id)
                .toType(entityType);
            return manager.executeQueryLocally(query);
        }

        function orderResults(data, orderBy) {
            data.sort(function (l, r) { return l[orderBy]() > r[orderBy]() ? 1 : -1 })
        }

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }
    });