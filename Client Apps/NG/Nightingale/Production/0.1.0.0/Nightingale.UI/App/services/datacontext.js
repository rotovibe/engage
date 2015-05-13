// Data context is going to be registered in the global namespace
// to provide a service for grabbing data.
// 
// Since the view models are loaded up more or less independent of each other
// it reduces the dependencies on well-defined services.  Instead each method
// in the DataContext provides a service and the end-point definition and
// The unit of work uses a factory-type pattern to provide methods to getting
// the data.

define(['services/session', 'services/jsonResultsAdapter', 'config.models', 'config.services', 'services/dataservices/getentityservice', 'config.models.program', 'config.models.lookups', 'config.models.contacts', 'config.models.goals', 'config.models.notes', 'config.models.observations', 'services/dataservices/programsservice', 'services/entityfinder', 'services/usercontext', 'services/dataservices/contactservice', 'services/entityserializer', 'services/dataservices/lookupsservice', 'services/dataservices/goalsservice', 'services/dataservices/notesservice', 'services/dataservices/observationsservice', 'services/dataservices/caremembersservice', 'services/dataservices/patientsservice'],
    function (session, jsonResultsAdapter, modelConfig, servicesConfig, getEntityService, stepModelConfig, lookupModelConfig, contactModelConfig, goalModelConfig, notesModelConfig, observationsModelConfig, programsService, entityFinder, usercontext, contactService, entitySerializer, lookupsService, goalsService, notesService, observationsService, careMembersService, patientsService) {
        
        // Object to use for the loading messages
        function loadingMessage(message, showing) {
            var self = this;
            self.Message = ko.observable(message);
            self.Showing = ko.observable(showing);
        }

        // Keep track of whether there are changes inside this manager
        var hasChanges = ko.observable(false);
        
        var additionalObservationsLoaded = false;

        // API Token to include in the post / get calls
        var apiToken = ko.computed(function () {
            if (!session.currentUser()) {
                return '527bbc43231e250c4cb93eb6';
            }
            return session.currentUser().aPIToken();
        });
        
        // Array of messages showing what is loading
        var loadingMessages = ko.observableArray();
        // Object determining whether there are loading messages that are showing
        var loadingMessagesShowing = ko.computed(function () {
            var showing = false;
            ko.utils.arrayForEach(loadingMessages(), function (message) {
                if (message().Showing()) {
                    showing = true;
                }
            });
            return showing;
        });
        
        var commModesFiltered = false;
        var enums = {};
        enums.priorities = ko.observableArray();
        enums.alerts = ko.observableArray();
        enums.genders = ko.observableArray([
            new modelConfig.Gender('m', 'M', 'Male'),
            new modelConfig.Gender('f', 'F', 'Female'),
            new modelConfig.Gender('n', 'N', 'Neutral')
        ]);
        enums.daysOfWeek = ko.observableArray([
            new modelConfig.Day('0', 'M', 'Monday'),
            new modelConfig.Day('1', 'Tu', 'Tuesday'),
            new modelConfig.Day('2', 'W', 'Wednesday'),
            new modelConfig.Day('3', 'Th', 'Thursday'),
            new modelConfig.Day('4', 'F', 'Friday'),
            new modelConfig.Day('5', 'Sat', 'Saturday'),
            new modelConfig.Day('6', 'Sun', 'Sunday')
        ]);

        enums.phoneTypes = ko.observableArray();
        enums.emailTypes = ko.observableArray();
        enums.addressTypes = ko.observableArray();
        enums.textTypes = ko.observableArray();
        enums.languages = ko.observableArray();
        enums.states = ko.observableArray();
        enums.timeZones = ko.observableArray();
        enums.timesOfDay = ko.observableArray();
        enums.communicationModes = ko.observableArray();
        enums.communicationTypes = ko.observableArray();
        enums.focusAreas = ko.observableArray();
        enums.sources = ko.observableArray();
        enums.barrierCategories = ko.observableArray();
        enums.interventionCategories = ko.observableArray();
        enums.goalTypes = ko.observableArray();
        enums.goalTaskStatuses = ko.observableArray();
        enums.barrierStatuses = ko.observableArray();
        enums.interventionStatuses = ko.observableArray();
        enums.careManagers = ko.observableArray();
        enums.careMemberTypes = ko.observableArray();
        enums.observationTypes = ko.observableArray();
        
        function configureCustomHeaders() {
            var ajaxAdapter = breeze.config.getAdapterInstance("ajax", "jQuery");
            ajaxAdapter.defaultSettings = {
                headers: {
                    // any headers that you want to specify.
                    "Token": apiToken()
                }
            };
            var otehrAdapter = breeze.config.initializeAdapterInstance("dataService", "webApi", true);
            otehrAdapter.defaultSettings = {
                headers: {
                    // any headers that you want to specify.
                    "Token": apiToken()
                }
            };
            breeze.ajaxpost.configAjaxAdapter(ajaxAdapter);
        }

        var EntityQuery = breeze.EntityQuery;
        // The data service is responsible for telling the queries where to query from
        var ds = new breeze.DataService({
            adapterName: 'webApi',
            serviceName: servicesConfig.remoteServiceName,
            hasServerMetadata: false,
            jsonResultsAdapter: jsonResultsAdapter
        });

        // The manager is where all of the entities are stored
        var manager = configureBreezeManager();
        var metadataStore = manager.metadataStore;
        metadataStore.setEntityTypeForResourceName('ProgramsTest', 'Program');
        
        var getUserByUserToken = usercontext.getUserByUserToken;
        var logOutUserByToken = usercontext.logOutUserByToken;
        var getUserSettings = usercontext.getUserSettings;
        var createUserFromSessionUser = usercontext.createUserFromSessionUser;
        var getEventsByUserId = usercontext.getEventsByUserId;
        var createCalendarMocks = usercontext.createCalendarMocks;
        var createCalendarEvent = usercontext.createCalendarEvent;

        // Get Entity by ID
        //
        // Pass in an end-point and an entity type to get data from that end-point
        // and create an entity in the manager of that type.
        var getEntityById = function (entityObservable, id, entityType, endpoint, forceRemote) {
            var message = queryStarted(entityType, forceRemote);
            return getEntityService.getEntityById(manager, message, entityObservable, id, entityType, endpoint, forceRemote)
                .then(queryCompleted);
        };

        // Get Entity by ID - The way Mel is currently doing it
        //
        // Pass in an end-point and an entity type to get data from that end-point
        // and create an entity in the manager of that type.
        var getMelsEntityById = function (entityObservable, id, entityType, endpoint, forceRemote, params) {
            var message = queryStarted(entityType, forceRemote);
            return getEntityService.getMelsEntityById(manager, message, entityObservable, id, entityType, endpoint, forceRemote, params)
                .then(queryCompleted);
        };

        // Get List of Entities
        //
        // Pass in an end-point and an entity type to get data from that end-point
        // and create a list of entities in the manager of that type.  Optional parameters
        // are parentPropertyName and parentPropertyId.  Parent Property Name is the name
        // of the property and id is the corresponding id.  These are used to find entities
        // from cache by their parent.
        var getEntityList = function (entityObservable, entityType, endpoint, parentPropertyName, parentPropertyId, forceRemote, params, orderBy) {
            var message = queryStarted(entityType, forceRemote);
            return getEntityService.getEntityList(manager, message, entityObservable, entityType, endpoint, parentPropertyName, parentPropertyId, forceRemote, params, orderBy)
                .then(queryCompleted);
        };

        // Create Entity
        //
        // Pass in a JSON object with the new entities constructor properties
        function createEntity (entityType, constructorProperties) {
            return manager.createEntity(entityType, constructorProperties);
        }

        // Create Entity
        //
        // Pass in a JSON object with the new entities constructor properties
        function initializeEntity(observable, entityType, patientId, goalId) {
            return goalsService.initializeEntity(manager, observable, entityType, patientId, goalId);
        }

        // Create Entity
        //
        // Pass in a JSON object with the new entities constructor properties
        function createComplexType(entityType, constructorProperties) {
            var thisEntityType = manager.metadataStore.getEntityType(entityType);
            var thisEntity = thisEntityType.createInstance(constructorProperties);
            return thisEntity;
        }
        
        // Search for entities should be equal to this object
        var searchForEntities = entityFinder.searchForEntities;

        // Subscribe to changes to our entity manager
        manager.hasChangesChanged.subscribe(function (eventArgs) {
            hasChanges(eventArgs.hasChanges);
        });

        var datacontext = {
            manager: manager,
            loadingMessages: loadingMessages,
            loadingMessagesShowing: loadingMessagesShowing,
            getEntityById: getEntityById,
            getMelsEntityById: getMelsEntityById,
            getEntityList: getEntityList,
            createEntity: createEntity,
            initializeEntity: initializeEntity,
            createComplexType: createComplexType,
            getUserByUserToken: getUserByUserToken,
            logOutUserByToken: logOutUserByToken,
            createUserFromSessionUser: createUserFromSessionUser,
            getEventsByUserId: getEventsByUserId,
            createCalendarMocks: createCalendarMocks,
            createCalendarEvent: createCalendarEvent,
            primeData: primeData,
            saveChangesToPatientProperty: saveChangesToPatientProperty,
            saveEntity: saveEntity,
            saveAction: saveAction,
            saveGoal: saveGoal,
            deleteGoal: deleteGoal,
            saveNote: saveNote,
            deleteNote: deleteNote,
            saveCareMember: saveCareMember,
            enums: enums,
            saveContactCard: saveContactCard,
            cancelAllChangesToContactCard: cancelAllChangesToContactCard,
            cancelEntityChanges: cancelEntityChanges,
            getAllChanges: getAllChanges,
            searchForEntities: searchForEntities,
            checkIfAllAdditionalObservationsAreLoadedYet: checkIfAllAdditionalObservationsAreLoadedYet,
            initializeObservation: initializeObservation,
            saveObservations: saveObservations,
            saveBackground: saveBackground,
            getFullSSN: getFullSSN,
            hasChanges: hasChanges
        };

        return datacontext;

        // Go prime the data that will be shared throughout the application
        function primeData() {            
            configureCustomHeaders();
            var promise = Q.all([
                loadUpEnums(),
                getUserSettings(session.currentUser, '/1.0/' + session.currentUser().contracts()[0].number() + '/settings', apiToken()),
                getProblemsLookup(),
                getCohortsLookup(),
                getContractProgramsLookup(),
                getTimesOfDayLookup(),
                getTimeZonesLookup(),
                getAllStatesLookup(),
                getAllLanguagesLookup(),
                getAllCommModesLookup(),
                getAllCommTypesLookup(),
                getGoalLookups(),
                getObservationTypeLookups(),
                getCareMemberTypeLookups(),
                getAllCareManagers(),
                loadUpMocks()
            ]);
            return promise;
        }

        // Get a list of problems lookups
        function getProblemsLookup() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'lookup/problems', 'Problem');
                return getEntityList(null, endPoint.EntityType, endPoint.ResourcePath, null, null, true);
            }
        }

        // Get a list of cohorts lookups
        function getCohortsLookup() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'cohorts', 'Cohort');
                return getEntityList(null, endPoint.EntityType, endPoint.ResourcePath, null, null, true);
            }
        }

        // Get a list of active contract programs (programs for this contract)
        function getContractProgramsLookup() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'programs/active', 'ContractProgram');
                return getEntityList(null, endPoint.EntityType, endPoint.ResourcePath, null, null, true);
            }
        }

        // Get a list of times of day
        function getTimesOfDayLookup() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'lookup/timesofdays', 'TimeOfDay');
                return getEntityList(enums.timesOfDay, endPoint.EntityType, endPoint.ResourcePath, null, null, true);
            }
        }

        // Get a list of time zones
        function getTimeZonesLookup() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'lookup/timezones', 'TimeZone');
                return getEntityList(enums.timeZones, endPoint.EntityType, endPoint.ResourcePath, null, null, true, null, 'name');
            }
        }

        // Get a list of states
        function getAllStatesLookup() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'lookup/states', 'State');
                return getEntityList(enums.states, endPoint.EntityType, endPoint.ResourcePath, null, null, true);
            }
        }

        // Get a list of languages
        function getAllLanguagesLookup() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'lookup/languages', 'Language');
                return getEntityList(enums.languages, endPoint.EntityType, endPoint.ResourcePath, null, null, true);
            }
        }

        // Get a list of communication modes
        function getAllCommModesLookup() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'lookup/commmodes', 'CommunicationMode');
                return getEntityList(enums.communicationModes, endPoint.EntityType, endPoint.ResourcePath, null, null, true).then(filterCommModes);
            }
        }

        // Get a list of communication types
        function getAllCommTypesLookup() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'lookup/commtypes', 'CommunicationType');
                return getEntityList(enums.communicationTypes, endPoint.EntityType, endPoint.ResourcePath, null, null, true).then(filterCommModes);
            }
        }

        // Get a list of communication types
        function getAllCareManagers() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Contact/CareManagers', 'CareManager');
                return getEntityList(enums.careManagers, endPoint.EntityType, endPoint.ResourcePath, null, null, true);
            }

        }

        // Get a list of goal lookups
        function getGoalLookups() {
            if (session.currentUser()) {
                lookupsService.getLookup(manager, 'FocusArea', datacontext.enums.focusAreas, true);
                lookupsService.getLookup(manager, 'Source', datacontext.enums.sources, true);
                lookupsService.getLookup(manager, 'BarrierCategory', datacontext.enums.barrierCategories, true);
                return lookupsService.getLookup(manager, 'InterventionCategory', datacontext.enums.interventionCategories, true);
            }
        }

        // Get a list of goal lookups
        function getObservationTypeLookups() {
            if (session.currentUser()) {
                return lookupsService.getLookup(manager, 'ObservationType', enums.observationTypes, true);
            }
        }

        function checkIfAllAdditionalObservationsAreLoadedYet(patientId) {
            if (!additionalObservationsLoaded) {
                getAllAdditionalObservationsByPatientAndType(patientId);
            }
        }

        // Have to get these by patient and by type even though they are not specific to the patient
        function getAllAdditionalObservationsByPatientAndType(patientId) {
            ko.utils.arrayForEach(enums.observationTypes(), function (type) {
                observationsService.getAdditionalObservationsByTypeIdAndPatientId(manager, type.id(), patientId);
            });
            additionalObservationsLoaded = true;
        }

        // Get a list of goal lookups
        function getCareMemberTypeLookups() {
            if (session.currentUser()) {
                return lookupsService.getLookup(manager, 'CareMemberType', enums.careMemberTypes, true);
            }
        }

        // Filter the communication modes into types
        function filterCommModes() {
            // We need both comm types and comm modes, check for them first
            if (enums.communicationModes().length !== 0 && enums.communicationTypes().length !== 0 && !commModesFiltered) {
                // Create place holders for the id's
                var phoneId = '', emailId = '', addressId = '', textId = '';
                // Go through each communication modes,
                ko.utils.arrayForEach(enums.communicationModes(), function (mode) {
                    // And if the mode's name is phone,
                    if (mode.name() === 'Phone') {
                        // Set the id to the current mode's id
                        phoneId = mode.id();
                    }
                    else if (mode.name() === 'Email') {
                        emailId = mode.id();
                    }
                    else if (mode.name() === 'Text') {
                        textId = mode.id();
                    }
                    else if (mode.name() === 'Mail') {
                        addressId = mode.id();
                    }
                });
                // Go through each communication types,
                ko.utils.arrayForEach(enums.communicationTypes(), function (type) {
                    // Check for phones
                    var thisMode = ko.utils.arrayFirst(type.commModeIds(), function (mode) {
                        return mode.id() === phoneId;
                    });
                    // If there is a mode returned,
                    if (thisMode) {
                        // Add it to the phoneTypes
                        enums.phoneTypes.push(type);
                    }
                    // Check for emails
                    var thisMode = ko.utils.arrayFirst(type.commModeIds(), function (mode) {
                        return mode.id() === emailId;
                    });
                    if (thisMode) {
                        enums.emailTypes.push(type);
                    }
                    // Check for texts
                    var thisMode = ko.utils.arrayFirst(type.commModeIds(), function (mode) {
                        return mode.id() === textId;
                    });
                    if (thisMode) {
                        enums.textTypes.push(type);
                    }
                    // Check for addresses
                    var thisMode = ko.utils.arrayFirst(type.commModeIds(), function (mode) {
                        return mode.id() === addressId;
                    });
                    if (thisMode) {
                        enums.addressTypes.push(type);
                    }
                });
                // Set commmodes filtered to true so we don't filter them again
                commModesFiltered = true;
                return true;
            }
            else { return false; }
        };

        // Go create some mock data for the step model stuff
        function loadUpMocks() {
            goalModelConfig.createMocks(manager);
            var allChangedEntities = getAllChanges();
            saveAllChangesToEntities(allChangedEntities);
        }

        // Go create the enums we will use
        function loadUpEnums() {
            lookupModelConfig.initializeEnums(manager);
            datacontext.getEntityList(datacontext.enums.priorities, 'Priority', 'fakeEndPoint', null, null, false);
            datacontext.getEntityList(datacontext.enums.goalTypes, 'GoalType', 'fakeEndPoint', null, null, false);
            datacontext.getEntityList(datacontext.enums.goalTaskStatuses, 'GoalTaskStatus', 'fakeEndPoint', null, null, false);
            datacontext.getEntityList(datacontext.enums.barrierStatuses, 'BarrierStatus', 'fakeEndPoint', null, null, false);
            datacontext.getEntityList(datacontext.enums.interventionStatuses, 'InterventionStatus', 'fakeEndPoint', null, null, false);
        }

        // Configure the Breeze entity manager to always pass an api key
        function configureBreezeManager() {
            breeze.NamingConvention.camelCase.setAsDefault();            
            breeze.config.initializeAdapterInstance("ajax", "jQuery", true);
            var mgr = new breeze.EntityManager({ dataService: ds });
            // Register the model types in models in the entity manager
            modelConfig.initialize(mgr.metadataStore);
            lookupModelConfig.initialize(mgr.metadataStore);
            stepModelConfig.initialize(mgr.metadataStore);
            contactModelConfig.initialize(mgr.metadataStore);
            goalModelConfig.initialize(mgr.metadataStore);
            notesModelConfig.initialize(mgr.metadataStore);
            observationsModelConfig.initialize(mgr.metadataStore);
            return mgr;
        }

        // If a query completes, clear the isLoading flag
        function queryStarted(object, forceRemote, action) {
            var message;
            // If there is an action, use that as the action, or else default to loading
            var thisAction = action ? action : 'Loading';
            // If it is hitting the server, 
            if (forceRemote) {
                // Show a message
                message = ko.observable(new loadingMessage(thisAction + ' ' + object + 's...', forceRemote));
                addLoadingMessage(message);
            }
            return message;
        }

        // If a query completes, clear the isLoading flag
        function queryCompleted(message) {
            removeLoadingMessage(message);
        }

        // If a query fails, show why
        function queryFailed(error) {
            // TODO : Show an alert here
            console.log('An error occurred - ', error);
        }

        // Add a message to the loading message queue
        function addLoadingMessage(message) {
            loadingMessages.push(message);
        }

        // Remove a message from the loading message queue, if it exists in the message queue
        function removeLoadingMessage(message) {
            if (loadingMessages.indexOf(message) !== -1) {
                setTimeout(function () { loadingMessages.remove(message); }, 500);
            }
        }

        // Save a single properties new value
        function saveChangesToPatientProperty(entity, propertyName, propertyValue, parameters) {
            var message = queryStarted('Patient', true, 'Saving');

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'patient');

            // Unwrap the entity
            var unwrappedPatient = ko.unwrap(entity);

            // Get the patient's Id
            var patientId = unwrappedPatient.id();
            // Create a full url to use
            var fullUrl = servicesConfig.remoteServiceName + '/' + endPoint.ResourcePath + patientId + '/' + propertyName;

            // If there is a propertyValue to append, add it (can be zero also)
            if (propertyValue || propertyValue === 0) {
                // Add a property on to the full url, if there is a value
                fullUrl = fullUrl + '/' + propertyValue;
            }

            var params = { };

            $.each(parameters, function (index, item) {
                params[item.Property] = item.Value;
            });
               
            return $.ajax({
                url: fullUrl,
                cache: false,
                dataType: 'json',
                type: "POST",
                headers: {
                    Token: apiToken()
                },
                data: params,
                success: function (data) {
                    unwrappedPatient.entityAspect.acceptChanges();
                    queryCompleted(message);
                    return entityFinder.searchForEntities(data);
                }
            }).fail(saveFailed);

            function saveFailed(error) {
                console.log('Error - ', error);
                var thisAlert = datacontext.createEntity('Alert', { result: '0', reason: 'Save failed!' });
                thisAlert.entityAspect.acceptChanges();
                datacontext.enums.alerts.push(thisAlert);
            }
        }

        // Save a single entity
        function saveEntity(entity, parameters) {
            // Display a message while saving
            var message = queryStarted(entity.entityType.shortName, true, 'Saving');

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'patient', 'Update');
            //var patientId = entity().id(); 0
            var fullUrl = servicesConfig.remoteServiceName + '/' + endPoint.ResourcePath + 'Update';
            
            var params = { };
            $.each(parameters, function (index, item) {
                params[item.Property] = item.Value || '""';
            });
            var token = apiToken();

            return $.ajax({
                url: fullUrl,
                cache: false,
                dataType: 'json',
                type: "POST",
                headers: {
                    Token: token
                },
                data: params,
                success: function (data) {
                    // If there is a fullSSN
                    if (entity.fullSSN()) {
                        // Set the last four SSN value
                        entity.lastFourSSN(entity.fullSSN().substr(entity.fullSSN().length - 4));    
                        // Clear the full SSN value
                        entity.fullSSN(null);                    
                    }
                    entity.entityAspect.acceptChanges();
                    queryCompleted(message);
                    return true;
                }
            }).fail(saveFailed);

            function saveFailed(error) {
                entity.entityAspect.rejectChanges();
                var thisAlert = datacontext.createEntity('Alert', { result: '0', reason: 'Save failed!' });
                thisAlert.entityAspect.acceptChanges();
                enums.alerts.push(thisAlert);
                console.log(error);
                removeLoadingMessage(message);
            }

        }
        
        // Save a single action
        function saveAction(serializedAction, programId, patientId) {
            // Display a message while saving
            var message = queryStarted('Action', true, 'Saving');

            return programsService.saveActionPost(manager, serializedAction, programId, patientId).then(queryCompleted(message));

        }

        // Save changes to a single contact card
        function saveContactCard(contactCard) {
            // Display a message while saving
            var message = queryStarted('Contact card', true, 'Saving');

            var serializedContactCard;
            setTimeout(function () {
                serializedContactCard = entitySerializer.serializeContactCard(contactCard, manager);
            }, 50);
            setTimeout(function () {
                return contactService.saveContactCard(manager, serializedContactCard).then(saveCompleted);
            }, 50);

            function saveCompleted (data) {
                // If data was returned and has a property called success that is true,
                if (data) {
                    // Go through the data, find any entities that need to have their Id's cleaned up
                    var updatedPhones = data.UpdatedPhone;
                    var updatedEmails = data.UpdatedEmail;
                    var updatedAddresses = data.UpdatedAddress;
                    // Iterate through the updated phones,
                    if (updatedPhones) {
                        ko.utils.arrayForEach(updatedPhones, function (newPhone) {
                            // Find the first matching phone,
                            var phoneToUpdate = ko.utils.arrayFirst(contactCard.phones(), function (oldPhone) {
                                // Where the id equals the id of the 'newPhone' returned from the server
                                return oldPhone.id() === newPhone.OldId;
                            });
                            // If a phone is found,
                            if (phoneToUpdate) {
                                // Update it's id property
                                phoneToUpdate.id(newPhone.NewId);
                            }
                        });
                    }
                    if (updatedEmails) {
                        ko.utils.arrayForEach(updatedEmails, function (newEmail) {
                            // Find the first matching phone,
                            var emailToUpdate = ko.utils.arrayFirst(contactCard.emails(), function (oldEmail) {
                                // Where the id equals the id of the 'newPhone' returned from the server
                                return oldEmail.id() === newEmail.OldId;
                            });
                            // If a phone is found,
                            if (emailToUpdate) {
                                // Update it's id property
                                emailToUpdate.id(newEmail.NewId);
                            }
                        });
                    }
                    if (updatedAddresses) {
                        ko.utils.arrayForEach(updatedAddresses, function (newAddress) {
                            // Find the first matching phone,
                            var addressToUpdate = ko.utils.arrayFirst(contactCard.addresses(), function (oldAddress) {
                                // Where the id equals the id of the 'newPhone' returned from the server
                                return oldAddress.id() === newAddress.OldId;
                            });
                            // If a phone is found,
                            if (addressToUpdate) {
                                // Update it's id property
                                addressToUpdate.id(newAddress.NewId);
                            }
                        });
                    }

                    // Save all of the levels of everything related to a contact card
                    contactCard.entityAspect.acceptChanges();

                    // Finally, clear out the message
                    queryCompleted(message);
                }
            }
        }

        // Save changes to a single contact card
        function saveGoal(goal) {
            // Display a message while saving
            var message = queryStarted('Goal', true, 'Saving');

            var serializedGoal;
            setTimeout(function () {
                serializedGoal = entitySerializer.serializeGoal(goal, manager);
            }, 50);
            setTimeout(function () {
                return goalsService.saveGoal(manager, serializedGoal).then(saveCompleted);
            }, 50);

            function saveCompleted(data) {
                // If data was returned and has a property called success that is true,
                goal.isNew(false);
                // Save all of the levels of everything related to a contact card
                goal.entityAspect.acceptChanges();

                // Finally, clear out the message
                queryCompleted(message);
            }
        }

        // Save changes to a single contact card
        function deleteGoal(goal) {
            // Display a message while saving
            var message = queryStarted('Goal', true, 'Deleting');

            return goalsService.deleteGoal(manager, goal).then(deleteCompleted);

            function deleteCompleted(data) {
                // If data was returned and has a property called success that is true,

                goal.entityAspect.rejectChanges();
                // Save all of the levels of everything related to a contact card
                goal.patientId(null);
                manager.detachEntity(goal);
                // Finally, clear out the message
                queryCompleted(message);
            }
        }

        // Save changes to a single contact card
        function saveNote(note) {
            // Display a message while saving
            var message = queryStarted('Note', true, 'Saving');

            var serializedNote = entitySerializer.serializeNote(note, manager);
            return notesService.saveNote(manager, serializedNote).then(saveCompleted);

            function saveCompleted(data) {
                // If data was returned and has a property called success that is true,
                //note.isNew(false);
                // Save all of the levels of everything related to a contact card
                note.id(data.Id);
                note.entityAspect.acceptChanges();

                // Finally, clear out the message
                queryCompleted(message);
                return true;
            }
        }

        // Save changes to a single contact card
        function deleteNote(note) {
            // Display a message while saving
            var message = queryStarted('Note', true, 'Deleting');

            return notesService.deleteNote(manager, note).then(deleteCompleted);

            function deleteCompleted(data) {
                note.entityAspect.rejectChanges();
                note.patientId(null);
                manager.detachEntity(note);
                // Finally, clear out the message
                queryCompleted(message);
            }
        }

        // Save changes to a single contact card
        function saveCareMember(careMember, saveType) {
            // Display a message while saving
            var message = queryStarted('CareMember', true, 'Saving');

            var serializedCareMember = entitySerializer.serializeCareMember(careMember, manager);
            return careMembersService.saveCareMember(manager, serializedCareMember, saveType).then(saveCompleted);

            function saveCompleted(data) {
                // If data was returned and has a property called success that is true,
                //note.isNew(false);
                // Save all of the levels of everything related to a contact card
                if (data.Id) {
                    careMember.id(data.Id);
                }
                careMember.entityAspect.acceptChanges();

                // Finally, clear out the message
                queryCompleted(message);
                return true;
            }
        }

        function initializeObservation(observable, type, observationId, typeId, patientId) {
            return observationsService.initializeObservation(manager, observable, type, typeId, patientId, observationId).then(initialized);
            function initialized(data) {
                return data;
            }
        }

        // Save changes to a single contact card
        function saveObservations(patientId) {
            var theseObservations = ko.observableArray();
            // Display a message while saving
            var message = queryStarted('Observations', true, 'Saving');
            // Get all observations by patient id
            getEntityList(theseObservations, 'Observation', 'local', 'patientId', patientId, false);
            var serializedObservations = [];
            serializedObservations.PatientId = patientId;
            // Go through the observations,
            ko.utils.arrayForEach(theseObservations(), function (observation) {
                // And if the observation a date...
                if (moment(observation.startDate()).isValid()) {
                    var canSave = true;
                    // Go through each value and ensure a value exists,
                    ko.utils.arrayForEach(observation.values(), function (value) {
                        // If there is no value,
                        if (!value.value()) {
                            // Set the canSave to false
                            canSave = false;
                        }
                    });
                    // If you can save,
                    if (canSave) {
                        // Accept it's changes
                        observation.entityAspect.acceptChanges();
                        // Serialize it
                        var serializedObservation = entitySerializer.serializeObservation(observation, manager);
                        serializedObservations.push(serializedObservation);
                    }
                }
            });
            return observationsService.saveObservations(manager, serializedObservations).then(saveCompleted);

            function saveCompleted(data) {
                // If data was returned and has a property called success that is true,
                //note.isNew(false);
                // Save all of the levels of everything related to a contact card
                // ko.utils.arrayForEach(theseObservations(), function (observation) {
                //     observation.entityAspect.acceptChanges();
                // });
                // Finally, clear out the message
                queryCompleted(message);
                return true;
            }
        }

        // Save changes to a single contact card
        function saveBackground(patient) {
            // Display a message while saving
            var message = queryStarted('Background', true, 'Saving');
            var backgroundString = patient.background();
            var patientId = patient.id();

            return patientsService.saveBackground(manager, patientId, backgroundString).then(saveCompleted);

            function saveCompleted(data) {
                // If data was returned and has a property called success that is true,
                //note.isNew(false);
                // Save all of the levels of everything related to a contact card
                patient.entityAspect.acceptChanges();

                // Finally, clear out the message
                queryCompleted(message);
                return true;
            }
        }

        // Get a patients full SSN for display only
        function getFullSSN(patientId) {
            return patientsService.getFullSSN(manager, patientId).then(dataReturned);

            function dataReturned(data) {
                return data;
            }
        }

        // Cancel changes to a contact card
        function cancelAllChangesToContactCard(contactCard) {
            return contactService.cancelAllChangesToContactCard(contactCard);
        }

        // Cancel changes to a single entity
        function cancelEntityChanges(entity) {
            entity.entityAspect.rejectChanges();
        }

        // Cancel changes to all entities
        function cancelAllChanges() {
            manager.rejectChanges();
        }
        
        // Gets all changes to all entities
        function getAllChanges() {
            return manager.getChanges();
        }

        // Save all changes to each entity in the array
        function saveAllChangesToEntities(entities) {
            ko.utils.arrayForEach(entities, function (entity) {
                entity.entityAspect.acceptChanges();
            });
        }
    });