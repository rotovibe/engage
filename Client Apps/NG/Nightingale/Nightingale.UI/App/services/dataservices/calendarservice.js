define(['models/users', 'config.services', 'services/local.collections'],
    function (userModelsConfig, serviceConfig, localCollections) {

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

            //     // Query to post the results
            // var query = breeze.EntityQuery
            //     .from(resource)
            //     .toType('Event');

            // return manager.executeQuery(query).then(querySuccess).fail(queryFailed);

            function querySuccess(data) {
                var events = data.results;
                // Make sure the user has been materialized
                return eventsObservable(events);
            }
        }

        // Create a calendar event
        var createCalendarEvent = function (manager, event) {                                  
            var thisEvent = manager.createEntity('Event', event);
            thisEvent.entityAspect.acceptChanges();
            localCollections.events.push(thisEvent);
            return thisEvent;
        }

        var getEventById = function (manager, eventId) {
            //If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            //from the local cache            
			var p = getLocalById(manager, 'Event', 'Event', 'id', eventId);  //Go check and see if we have this locally already
			if (p.length > 0) {
				return p[0];
			}		
        };
		
        var userservice = {
            getEventsByUserId: getEventsByUserId,
            createCalendarEvent: createCalendarEvent,
			removeCalendarEventById: removeCalendarEventById,
            getEventById: getEventById
        };

        return userservice;

        // Remove a calendar event
        function removeCalendarEvent (event) {
            localCollections.events.remove(event);
			event.entityAspect.setDeleted();
			event.entityAspect.acceptChanges();
        }
		
		function removeCalendarEventById (eventId){			
			var matchedEvent = ko.utils.arrayFirst(localCollections.events(), function (evt) {
                return evt.id() === eventId;
            });
			if(matchedEvent){
				removeCalendarEvent(matchedEvent);
			}
		}

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

        function getLocalById(manager, resource, entityType, idPropertyName, id) {
            var query = breeze.EntityQuery.from(resource)
                .where(idPropertyName, '==', id)
                .toType(entityType);
            return manager.executeQueryLocally(query);
        }

    });