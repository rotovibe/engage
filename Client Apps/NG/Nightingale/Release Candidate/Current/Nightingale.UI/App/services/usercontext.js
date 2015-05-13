define(['services/session', 'models/base', 'config.services', 'models/users', 'services/dataservices/usersservice', 'services/dataservices/calendarservice'],
    function (session, modelConfig, servicesConfig, userModelConfig, userService, calendarService) {
        
        // API Token to include in the post / get calls
        // May have to leave this one for the log out service
        var apiToken = ko.computed(function () {
            if (!session.currentUser()) {
                return 'null';
            }
            return session.currentUser().aPIToken();
        });

        // Current user id
        var currentUserId = ko.computed(function () {
            if (!session.currentUser()) {
                return 'nothing';
            }
            return session.currentUser().userId();
        });

        // The data service is responsible for telling the queries where to query from
        // This is different from the datacontext because it is hitting the security domain
        var ds = new breeze.DataService({
            serviceName: servicesConfig.securityServiceName,
            hasServerMetadata: false
        });

        // The manager is where all of the entities are stored
        var manager = configureBreezeManager();
        var metadataStore = manager.metadataStore;
        
        var getUserByUserToken = function (userObservable, tokenId, forceRemote) { return Q(userService.getSessionUser(manager, '1.0/login', userObservable, tokenId, forceRemote)); };
        var logOutUserByToken = function (endpoint) { return Q(userService.deleteSessionToken(apiToken(), endpoint)); };
        var getUserSettings = function (userObservable, endpoint, apiToken) { return Q(userService.getUserSettings(endpoint, userObservable, apiToken)); };
        var createUserFromSessionUser = function (userData) { return userService.createUserFromSessionUser(manager, userData); };

        var createCalendarMocks = function () { return userModelConfig.createCalendarMocks(manager, currentUserId()); };
        var initializeEnums = function () { return userModelConfig.initializeEnums(manager); };

        // Go get calendar events for the user id specified
        var getEventsByUserId = function (eventsObservable, userId, forceRemote) {
            // Need to create the resource here
            // createResource(Endpoint)
            var resource = 'events';

            return calendarService.getEventsByUserId(manager, resource, eventsObservable, userId, forceRemote);
        };

        // Get calendar event by event id
        var getEventById = function (eventId) {
            return calendarService.getEventById(manager, eventId);
        }

        // Go create calendar event for this user
        var createCalendarEvent = function (event) {
            return calendarService.createCalendarEvent(manager, event);
        };
		
		var removeCalendarEventById = function (eventId){
			calendarService.removeCalendarEventById(eventId);			
		};
		
        // Create Entity
        //
        // Pass in a JSON object with the new entities constructor properties
        var createEntity = function (entityType, constructorProperties) {
            return manager.createEntity(entityType, constructorProperties);
        }

        var usercontext = {
            createEntity: createEntity,
            getUserByUserToken: getUserByUserToken,
            logOutUserByToken: logOutUserByToken,
            getUserSettings: getUserSettings,
            createUserFromSessionUser: createUserFromSessionUser,
            getEventsByUserId: getEventsByUserId,
            createCalendarMocks: createCalendarMocks,
            createCalendarEvent: createCalendarEvent,
			removeCalendarEventById: removeCalendarEventById,
            getEventById: getEventById,
            initializeEnums: initializeEnums
        };

        return usercontext;
        
        // Configure the Breeze entity manager to always pass an api key
        function configureBreezeManager() {
            breeze.NamingConvention.camelCase.setAsDefault();
            var mgr = new breeze.EntityManager({ dataService: ds });
            // Register the model types in models in the entity manager
            userModelConfig.initialize(mgr.metadataStore);
            return mgr;
        }
        
        // If a query fails, show why
        function queryFailed(error) {
            console.log('An error occurred - ', error);
        }

    });