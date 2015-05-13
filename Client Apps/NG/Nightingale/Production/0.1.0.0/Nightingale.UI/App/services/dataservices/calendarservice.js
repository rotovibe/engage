define(['config.models.user', 'config.services'],
    function (userModelsConfig, serviceConfig) {

        var getEventsByUserId = function (manager, resource, eventsObservable, userId, forceRemote) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // If it is not forceRemote, then try to get this from the local cache
            if (!forceRemote) {
                var p = getLocalList(manager, resource, 'Event', 'userId', userId);  //Go check and see if we have this locally already
                if (p.length > 0) {
                    eventsObservable(p);
                    return Q.resolve();
                }
            }

                // Query to post the results
            var query = breeze.EntityQuery
                .from(resource)
                .toType('Event');

            return manager.executeQuery(query).then(querySuccess).fail(queryFailed);

            function querySuccess(data) {
                var events = data.results;
                // Make sure the user has been materialized
                return eventsObservable(events);

            }
        }

        // Create a calendar event
        var createCalendarEvent = function (manager, userId, params) {
            // Create a user to use as the current user
            
            params.userId = userId;

            // Create and save the user
            var thisEvent = manager.createEntity('Event', params);
            thisEvent.entityAspect.acceptChanges();
            return thisEvent;
        }

        var userservice = {
            getEventsByUserId: getEventsByUserId,
            createCalendarEvent: createCalendarEvent
        };

        return userservice;

        function queryFailed(error) {
            console.log('Error - ', error);
        }

        function getLocalList(manager, resource, entityType, parentPropertyName, parentPropertyId, orderby) {
            var query = breeze.EntityQuery.from(resource)
                .select('id, title, start, end, allDay, url')
                .toType(entityType);
            
            if (parentPropertyName && parentPropertyId) {
                query = query.where(parentPropertyName, '==', parentPropertyId);
            }
            return manager.executeQueryLocally(query);
        }

    });