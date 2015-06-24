/** Data context is going to be registered in the global namespace
 * to provide a service for grabbing data.
 * 
 * Since the view models are loaded up more or less independent of each other
 * it reduces the dependencies on well-defined services.  Instead each method
 * in the DataContext provides a service and the end-point definition and
 * The unit of work uses a factory-type pattern to provide methods to getting
 * the data.
 *
 * @module datacontext
 */
 
 /**
 *	@class datacontext
 *
 */
define(['services/session', 'services/jsonResultsAdapter', 'models/base', 'config.services', 'services/dataservices/getentityservice', 'models/programs', 'models/lookups', 'models/contacts', 'models/goals', 'models/notes', 'models/observations', 'models/allergies', 'models/medications', 'services/dataservices/programsservice', 'services/entityfinder', 'services/usercontext', 'services/dataservices/contactservice', 'services/entityserializer', 'services/dataservices/lookupsservice', 'services/dataservices/goalsservice', 'services/dataservices/notesservice', 'services/dataservices/observationsservice', 'services/dataservices/caremembersservice', 'services/dataservices/patientsservice', 'services/dataservices/allergiesservice', 'services/dataservices/medicationsservice', 'services/local.collections'],
    function (session, jsonResultsAdapter, modelConfig, servicesConfig, getEntityService, stepModelConfig, lookupModelConfig, contactModelConfig, goalModelConfig, notesModelConfig, observationsModelConfig, allergyModelConfig, medicationModelConfig, programsService, entityFinder, usercontext, contactService, entitySerializer, lookupsService, goalsService, notesService, observationsService, careMembersService, patientsService, allergiesService, medicationsService, localCollections) {

        // Object to use for the loading messages
        function loadingMessage(message, showing) {
            var self = this;
            self.Message = ko.observable(message);
            self.Showing = ko.observable(showing);
        }

        // Keep track of whether there are changes inside this manager
        var hasChanges = ko.observable(false);
        
        var observationsLoaded = false;

        // Monitors whether programs are currently saving to lock functionality
        var programsSaving = ko.observable(false);
        // Monitors whether observations are currently saving to lock functionality
        var observationsSaving = ko.observable(false);
        // Monitors whether todos are currently saving to lock functionality
        var todosSaving = ko.observable(false);
        // Monitors whether interventions are currently saving to lock functionality
        var interventionsSaving = ko.observable(false);
        // Monitors whether tasks are currently saving to lock functionality
        var tasksSaving = ko.observable(false);
        // Monitors whether tasks are currently saving to lock functionality
        var allergySaving = ko.observable(false);
        // Monitors whether tasks are currently saving to lock functionality
        var medicationSaving = ko.observable(false);


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
            breeze.ajaxpost();
            //breeze.ajaxpost.configAjaxAdapter(ajaxAdapter);
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
        var getEventById = usercontext.getEventById;
        var createCalendarMocks = usercontext.createCalendarMocks;
        var createCalendarEvent = usercontext.createCalendarEvent;
		var removeCalendarEventById = usercontext.removeCalendarEventById;
		
        // Get Entity by ID
        //
        // Pass in an end-point and an entity type to get data from that end-point
        // and create an entity in the manager of that type.
        var getEntityById = function (entityObservable, id, entityType, endpoint, forceRemote) {
            // If it is a patient, call it an individual in the message
            entityTypeName = (entityType && entityType === 'Patient') ? 'Individual' : entityType
            var message = queryStarted(entityTypeName, forceRemote);
            return getEntityService.getEntityById(manager, message, entityObservable, id, entityType, endpoint, forceRemote)
                .then(queryCompleted);
        };

        // Check to see if we have this entity locally yet
        var checkForEntityLocally = function (entityObservable, id, entityType) {            
            var query = breeze.EntityQuery.from(entityType + 's')
                .where('id', '==', id)
                .toType(entityType);
            var p = manager.executeQueryLocally(query);
            if (p.length > 0) {
                entityObservable(p[0]);
            }
        }

        // Get Entity by ID - The way Mel is currently doing it
        //
        // Pass in an end-point and an entity type to get data from that end-point
        // and create an entity in the manager of that type.
        var getMelsEntityById = function (entityObservable, id, entityType, endpoint, forceRemote, params) {
            // If it is a patient, call it an individual in the message
            entityTypeName = (entityType && entityType === 'Patient') ? 'Individual' : entityType
            var message = queryStarted(entityTypeName, forceRemote);
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
        var getEntityList = function (entityObservable, entityType, endpoint, parentPropertyName, parentPropertyId, forceRemote, params, orderBy, skipMerge) {
            // If it is a patient, call it an individual in the message
            entityTypeName = (entityType && entityType === 'Patient') ? 'Individual' : entityType
            var message = queryStarted(entityTypeName, forceRemote);
            return getEntityService.getEntityList(manager, message, entityObservable, entityType, endpoint, parentPropertyName, parentPropertyId, forceRemote, params, orderBy, skipMerge)
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
            checkForEntityLocally: checkForEntityLocally,
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
			syncCalendarEvents: syncCalendarEvents,
            getEventById: getEventById,
			getCalendarEvents: getCalendarEvents,
            primeData: primeData,
            saveChangesToPatientProperty: saveChangesToPatientProperty,
            deleteIndividual: deleteIndividual,
            saveIndividual: saveIndividual,
            initializePatient: initializePatient,
            saveAction: saveAction,
            repeatAction: repeatAction,
            getRepeatedAction: getRepeatedAction,
            removeProgram: removeProgram,
            savePlanElemAttr: savePlanElemAttr,
            saveGoal: saveGoal,
            saveIntervention: saveIntervention,
            saveTask: saveTask,
            saveBarrier: saveBarrier,
            deleteGoal: deleteGoal,
            saveNote: saveNote,
            deleteNote: deleteNote,
            saveCareMember: saveCareMember,
            enums: localCollections.enums,
            alerts: localCollections.alerts,
            saveContactCard: saveContactCard,
            cancelAllChangesToContactCard: cancelAllChangesToContactCard,
            cancelEntityChanges: cancelEntityChanges,
            getAllChanges: getAllChanges,
            searchForEntities: searchForEntities,
            checkIfAllObservationsAreLoadedYet: checkIfAllObservationsAreLoadedYet,
            initializeObservation: initializeObservation,
            saveObservations: saveObservations,
            savePatientSystems: savePatientSystems,
            saveBackground: saveBackground,
            getFullSSN: getFullSSN,
            addPatientToRecentList: addPatientToRecentList,
            hasChanges: hasChanges,
            programsSaving: programsSaving,
            observationsSaving: observationsSaving,
            todosSaving: todosSaving,
            interventionsSaving: interventionsSaving,
            tasksSaving: tasksSaving,
            allergySaving: allergySaving,
            medicationSaving: medicationSaving,
            createAlert: createAlert,
            getToDos: getToDos,
            getToDosQuery: getToDosQuery,
            getInterventions: getInterventions,
            getInterventionsQuery: getInterventionsQuery,
            getTasks: getTasks,
            getTasksQuery: getTasksQuery,
            saveToDo: saveToDo,
            detachEntity: detachEntity,
            initializeNewMedication: initializeNewMedication,
            initializeNewPatientMedication: initializeNewPatientMedication,
            getPatientMedications: getPatientMedications,
			getPatientFrequencies: getPatientFrequencies,
            getRemoteMedicationFields: getRemoteMedicationFields,
            getPatientMedicationsQuery: getPatientMedicationsQuery,
            saveMedication: saveMedication,
			deletePatientMedication: deletePatientMedication,
            initializeAllergy: initializeAllergy,
            initializeNewAllergy: initializeNewAllergy,
            getRemoteAllergies: getRemoteAllergies,
            saveAllergies: saveAllergies,
            getPatientAllergies: getPatientAllergies,
			deletePatientAllergy: deletePatientAllergy,
            getPatientAllergiesQuery: getPatientAllergiesQuery,
            singleSort: singleSort
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
                getNoteLookups(),
                getGoalLookups(),
                getMedicationLookups(),
                getObjectivesLookup(),
                getObservationTypeLookups(),
                getCareMemberTypeLookups(),
                getAllergyLookups(),
                getAllCareManagers(),
                getRecentIndividuals(),
                loadUpMocks()
            ]).then(processLookpus);
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
                return getEntityList(datacontext.enums.timesOfDay, endPoint.EntityType, endPoint.ResourcePath, null, null, true);
            }
        }

        // Get a list of time zones
        function getTimeZonesLookup() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'lookup/timezones', 'TimeZone');
                return getEntityList(datacontext.enums.timeZones, endPoint.EntityType, endPoint.ResourcePath, null, null, true, null, 'name');
            }
        }

        // Get a list of time zones
        function getObjectivesLookup() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'lookup/objectives', 'ObjectiveLookup');
                return getEntityList(datacontext.enums.objectives, endPoint.EntityType, endPoint.ResourcePath, null, null, true);
            }
        }

        // Get a list of states
        function getAllStatesLookup() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'lookup/states', 'State');
                return getEntityList(datacontext.enums.states, endPoint.EntityType, endPoint.ResourcePath, null, null, true);
            }
        }

        // Get a list of languages
        function getAllLanguagesLookup() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'lookup/languages', 'Language');
                return getEntityList(datacontext.enums.languages, endPoint.EntityType, endPoint.ResourcePath, null, null, true);
            }
        }

        // Get a list of communication modes
        function getAllCommModesLookup() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'lookup/commmodes', 'CommunicationMode');
                return getEntityList(datacontext.enums.communicationModes, endPoint.EntityType, endPoint.ResourcePath, null, null, true).then(filterCommModes);
            }
        }

        // Get a list of communication types
        function getAllCommTypesLookup() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'lookup/commtypes', 'CommunicationType');
                return getEntityList(datacontext.enums.communicationTypes, endPoint.EntityType, endPoint.ResourcePath, null, null, true).then(filterCommModes);
            }
        }

        // Get a list of communication types
        function getAllCareManagers() {
            if (session.currentUser() && session.currentUser().contracts().length !== 0) {
                var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Contact/CareManagers', 'CareManager');
                return getEntityList(datacontext.enums.careManagers, endPoint.EntityType, endPoint.ResourcePath, null, null, true);
            }
        }

        // Get a list of note lookups
        function getNoteLookups() {
            if (session.currentUser()) {
                lookupsService.getNoteLookup(manager, 'NoteMethod', datacontext.enums.noteMethods, true);
                lookupsService.getNoteLookup(manager, 'NoteOutcome', datacontext.enums.noteOutcomes, true);
                lookupsService.getNoteLookup(manager, 'NoteWho', datacontext.enums.noteWhos, true);
                lookupsService.getNoteLookup(manager, 'NoteSource', datacontext.enums.noteSources, true);
                lookupsService.getLookup(manager, 'NoteType', datacontext.enums.noteTypes, true);
                return lookupsService.getNoteLookup(manager, 'NoteDuration', datacontext.enums.noteDurations, true);
            }
        }

        // Get a list of goal lookups
        function getGoalLookups() {
            if (session.currentUser()) {
                lookupsService.getLookup(manager, 'FocusArea', datacontext.enums.focusAreas, true);
                lookupsService.getLookup(manager, 'Source', datacontext.enums.sources, true);
                lookupsService.getLookup(manager, 'BarrierCategory', datacontext.enums.barrierCategories, true);
                lookupsService.getLookup(manager, 'ToDoCategory', datacontext.enums.toDoCategories, true);
                return lookupsService.getLookup(manager, 'InterventionCategory', datacontext.enums.interventionCategories, true);
            }
        }

        // Get a list of medication lookups
		function getMedicationLookups() {
            if (session.currentUser()) {				 
				lookupsService.getLookup(manager, 'MedSuppType', datacontext.enums.medSuppTypes, true);
				lookupsService.getLookup(manager, 'FreqHowOften', datacontext.enums.freqHowOftens, true);							
				lookupsService.getLookup(manager, 'Frequency', datacontext.enums.frequency, true);							
				lookupsService.getLookup(manager, 'FreqWhen', datacontext.enums.freqWhens, true);				
            }			
        }
		/**
		*	perform operations after lookups are loaded.
		*	@method processLookpus 
		*
		*/
		function processLookpus(){
			var promise = Q.all([
				processFrequencyLookup()
			]);
			return promise;
		}
		
		/**
		*			consolidate frequency lookups ("Frequency") into PatientMedicationFrequency entities.
		*			the Frequency lookups will be the global part of the list, common to all patients.
		*			each patient may have their own specific PatientMedicationFrequency entities - we will read them when selecting a patient (getPatientFrequencies).
		*
		*	@method processFrequencyLookup 
		*/
		function processFrequencyLookup(){
			//consolidate frequency lookups into PatientMedicationFrequency entities:
			ko.utils.arrayForEach(datacontext.enums.frequency(), function(frequency){
				manager.createEntity('PatientMedicationFrequency', {id: frequency.id(), name: frequency.name() , sortOrder: 0}, breeze.EntityState.Unchanged);
			});
			//add a nullo frequency option that will be the "add new" option: make sure it will be sorted last
			manager.createEntity('PatientMedicationFrequency', {id: -1, name: '-Add New-', sortOrder: 1}, breeze.EntityState.Unchanged); 
		}
		
        function getRecentIndividuals () {
            var theseRecentIndividuals = ko.observableArray();
            var recentIndividualsEndPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Contact/' + session.currentUser().userId() + '/RecentPatients', 'Patient');
            datacontext.getEntityList(theseRecentIndividuals, recentIndividualsEndPoint.EntityType, recentIndividualsEndPoint.ResourcePath, null, null, false).then(querySucceeded);
            
            function querySucceeded () {
                session.currentUser().recentIndividuals([]);
                ko.utils.arrayForEach(theseRecentIndividuals(), function (individual) {
                    session.currentUser().recentIndividuals.push(individual);
                });
            }
        }

        // Get a list of goal lookups
        function getObservationTypeLookups() {
            if (session.currentUser()) {
                return lookupsService.getLookup(manager, 'ObservationType', localCollections.enums.observationTypes, true).then(function () {
                    // Load up the observations
                    checkIfAllObservationsAreLoadedYet();
                    // Get observation states from the server
                    var thisEndPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Observation/States', 'ObservationState');
                    return getEntityList(localCollections.enums.observationStates, thisEndPoint.EntityType, thisEndPoint.ResourcePath, null, null, true);
                });
            }
        }

        function checkIfAllObservationsAreLoadedYet() {
            if (!observationsLoaded) {
                getAllObservations();
            }
        }

        // Have to get these by patient and by type even though they are not specific to the patient
        function getAllObservations() {
            observationsService.getObservations(manager);
            // ko.utils.arrayForEach(enums.observationTypes(), function (type) {
            //     observationsService.getObservationsByTypeId(manager, type.id());
            // });
            observationsLoaded = true;
        }

        // Get a list of goal lookups
        function getCareMemberTypeLookups() {
            if (session.currentUser()) {
                return lookupsService.getLookup(manager, 'CareMemberType', localCollections.enums.careMemberTypes, true);
            }
        }

        // Get a list of goal lookups
        // TODO: Remove the False 
        function getAllergyLookups() {
            if (session.currentUser()) {
                lookupsService.getLookup(manager, 'AllergyType', localCollections.enums.allergyTypes, true);
                lookupsService.getLookup(manager, 'Severity', localCollections.enums.severities, true);
                lookupsService.getNoteLookup(manager, 'AllergySource', localCollections.enums.allergySources, true);
                return lookupsService.getLookup(manager, 'Reaction', localCollections.enums.reactions, true);
            }
        }

        // Filter the communication modes into types
        function filterCommModes() {
            // We need both comm types and comm modes, check for them first
            if (localCollections.enums.communicationModes().length !== 0 && localCollections.enums.communicationTypes().length !== 0 && !commModesFiltered) {
                // Create place holders for the id's
                var phoneId = '', emailId = '', addressId = '', textId = '';
                // Go through each communication modes,
                ko.utils.arrayForEach(localCollections.enums.communicationModes(), function (mode) {
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
                ko.utils.arrayForEach(localCollections.enums.communicationTypes(), function (type) {
                    // Check for phones
                    var thisMode = ko.utils.arrayFirst(type.commModeIds(), function (mode) {
                        return mode.id() === phoneId;
                    });
                    // If there is a mode returned,
                    if (thisMode) {
                        // Add it to the phoneTypes
                        localCollections.enums.phoneTypes.push(type);
                    }
                    // Check for emails
                    var thisMode = ko.utils.arrayFirst(type.commModeIds(), function (mode) {
                        return mode.id() === emailId;
                    });
                    if (thisMode) {
                        localCollections.enums.emailTypes.push(type);
                    }
                    // Check for texts
                    var thisMode = ko.utils.arrayFirst(type.commModeIds(), function (mode) {
                        return mode.id() === textId;
                    });
                    if (thisMode) {
                        localCollections.enums.textTypes.push(type);
                    }
                    // Check for addresses
                    var thisMode = ko.utils.arrayFirst(type.commModeIds(), function (mode) {
                        return mode.id() === addressId;
                    });
                    if (thisMode) {
                        localCollections.enums.addressTypes.push(type);
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
            usercontext.initializeEnums(manager);
            // Set the collections into the enums namespace
            datacontext.getEntityList(datacontext.enums.priorities, 'Priority', 'fakeEndPoint', null, null, false);
            datacontext.getEntityList(datacontext.enums.goalTypes, 'GoalType', 'fakeEndPoint', null, null, false);
            datacontext.getEntityList(datacontext.enums.goalTaskStatuses, 'GoalTaskStatus', 'fakeEndPoint', null, null, false);
            datacontext.getEntityList(datacontext.enums.barrierStatuses, 'BarrierStatus', 'fakeEndPoint', null, null, false);
            datacontext.getEntityList(datacontext.enums.interventionStatuses, 'InterventionStatus', 'fakeEndPoint', null, null, false);
            // Observation stuff (first one is temporary)
            //datacontext.getEntityList(enums.observationStates, 'ObservationState', 'fakeEndPoint', null, null, false);
            datacontext.getEntityList(localCollections.enums.observationDisplays, 'ObservationDisplay', 'fakeEndPoint', null, null, false);
            // datacontext.getEntityList(localCollections.enums.noteTypes, 'NoteType', 'fakeEndPoint', null, null, false);
            datacontext.getEntityList(datacontext.enums.allergyStatuses, 'AllergyStatus', 'fakeEndPoint', null, null, false);
            datacontext.getEntityList(datacontext.enums.medicationStatuses, 'MedicationStatus', 'fakeEndPoint', null, null, false);
            datacontext.getEntityList(datacontext.enums.medicationCategories, 'MedicationCategory', 'fakeEndPoint', null, null, false);
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
            allergyModelConfig.initialize(mgr.metadataStore);
            medicationModelConfig.initialize(mgr.metadataStore);
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
                message = ko.observable(new loadingMessage(thisAction + ' ' + object + '...', forceRemote));
                addLoadingMessage(message);
            }
            return message;
        }

        // If a query completes, clear the isLoading flag
        function queryCompleted(message, programFlag) {
            if (programFlag) {
                programsSaving(false);
            }
            removeLoadingMessage(message);
        }

        // If a query fails, show why
        function queryFailed(error) {
            
            checkForFourOhOne(error);
            // TODO : Show an alert here
            throw new Error('Could not complete query');
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
            var message = queryStarted('Individual', true, 'Saving');

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
                checkForFourOhOne(error);
                var thisAlert = datacontext.createEntity('Alert', { result: '0', reason: 'Save failed!' });
                thisAlert.entityAspect.acceptChanges();
                localCollections.alerts.push(thisAlert);
                removeLoadingMessage(message);
            }
        }

        function deleteIndividual (entity) {
            var message = queryStarted('Individual', true, 'Deleting');
            patientsService.deleteIndividual(manager, entity).then(saveCompleted).fail(saveFailed);

            function saveCompleted(data) {
                // Remove the person from cache completely
                //entity.entityAspect.setDetached();
                manager.detachEntity(entity);
                //entity.entityAspect.acceptChanges();

                // Finally, clear out the message
                queryCompleted(message);
                setTimeout(function () { location.reload(); }, 2000);
                return true;
            }

            function saveFailed(error) {
                var thisAlert = datacontext.createEntity('Alert', { result: 0, reason: 'Delete failed!' });
                thisAlert.entityAspect.acceptChanges();
                localCollections.alerts.push(thisAlert);
                checkForFourOhOne(error);
                removeLoadingMessage(message);
                throw new Error('Delete failed');
            }
        }

        // Save a single entity
        function saveIndividual(patient) {
            // Display a message while saving
            // var message = queryStarted(entity.entityType.shortName, true, 'Saving');

            // // Create an end point to use
            // var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'patient', 'Update');
            //var patientId = entity().id(); 0
            //var fullUrl = servicesConfig.remoteServiceName + '/' + endPoint.ResourcePath + 'Update';

            // Display a message while saving
            var message = queryStarted('Individual', true, 'Saving');

            // Should the individual be inserted or just updated?
            var insert = patient.isNew();

            var serializedIndividual;
            serializedIndividual = entitySerializer.serializeIndividual(patient, manager);
            return patientsService.saveIndividual(manager, serializedIndividual, insert).then(saveCompleted).fail(saveFailed);

            function saveCompleted(data) {
                // If there is an outcome returned and it equals zero,
                if (data.httpResponse.data.Outcome && data.httpResponse.data.Outcome.Result == 0) {
                    // Set patient as a possible duplicate
                    patient.isDuplicate(true);
                } else {
                    patient.isDuplicate(false);
                    // Else save it all
                    patient.isNew(false);
                    // Save all of the levels of everything related to a patient
                    if (patient.fullSSN()) {
                        // Set the last four SSN value
                        patient.lastFourSSN(patient.fullSSN().substr(patient.fullSSN().length - 4));
                        // Clear the full SSN value
                        patient.fullSSN(null);
                    }
                    patient.entityAspect.acceptChanges();
                    updateTodoPatient(patient);
                    queryCompleted(message);
                    return true;
                }
                // Finally, clear out the message
                queryCompleted(message);
            }

            function saveFailed(error) {
                patient.entityAspect.rejectChanges();
                checkForFourOhOne(error);
                var thisAlert = datacontext.createEntity('Alert', { result: '0', reason: 'Save failed!' });
                thisAlert.entityAspect.acceptChanges();
                localCollections.alerts.push(thisAlert);
                console.log(error);
                removeLoadingMessage(message);
            }
        }

        function initializePatient(observable) {
            return patientsService.initializeIndividual(manager, observable).then(initialized);
            function initialized(data) {
                return data;
            }
        }
        
        function removeProgram(program, reason) {
            var message = queryStarted('Program', true, 'Removing');

            return programsService.removeProgram(manager, program.id(), program.name(), program.patientId(), reason).then(queryCompleted(message)).fail(queryFailed);
        }

        // Save a single action
        function saveAction(action, serializedAction, programId, patientId) {
            // Display a message while saving
            var message = queryStarted('Action', true, 'Saving');
            programsSaving(true);
            action.entityAspect.acceptChanges();
            return programsService.saveActionPost(manager, serializedAction, programId, patientId).then(saveActionCompleted);

            function saveActionCompleted(data) {
                // Find the action and make sure completedBy didn't change
                if (data.httpResponse.data && data.httpResponse.data.PlanElems && data.httpResponse.data.PlanElems.Actions) {
                    var thisAction = ko.utils.arrayFirst(data.httpResponse.data.PlanElems.Actions, function (act) {
                        return act.Id === action.id();
                    });
                    action.completedBy(thisAction.CompletedBy);
                }
                queryCompleted(message);
                entityFinder.searchForProblems(data.httpResponse.data);
                // entityFinder.searchForAnything(data.httpResponse.data);
                if (data.results) {
                    entityFinder.searchForLocalCollectionEntities(data.results);
                }
                programsSaving(false);

                // Save the action and all of it's steps and responses
                action.entityAspect.acceptChanges();
                // If the action has been completed,
                if (action.completed() && action.elementState() !== 5) {
                    // Make sure the state is set to 5
                    action.elementState(5);
                }
                // Else if the element state is 4 (in progress) when saved,
                else if (action.elementState() === 4) {
                    // Display an alert that progress was saved
                    var thisAlert = datacontext.createEntity('Alert', { result: 'warning', reason: 'Progress saved' });
                    thisAlert.entityAspect.acceptChanges();
                    localCollections.alerts.push(thisAlert);
                }
                // Save changes to all of the steps and responses
                ko.utils.arrayForEach(action.steps(), function (step) {
                    step.entityAspect.acceptChanges();
                    ko.utils.arrayForEach(step.responses(), function (response) {
                        response.entityAspect.acceptChanges();
                    });
                });
                // Save the action's module
                action.module().entityAspect.acceptChanges();
                action.isSaving(false);
            }
        }

        // Save a single action
        function repeatAction(action) {
            // Display a message while saving
            var message = queryStarted('Action', true, 'Saving');
            programsSaving(true);

            var serializedAction = entitySerializer.serializeAction(action, datacontext.manager);

                // Get the id of the patient that this action is for
            var patientId = action.module().program().patientId();

            // Get the id of the patient that this action is for
            var programId = action.module().program().id();

            return programsService.repeatAction(manager, serializedAction, programId, patientId).then(repeatActionCompleted);

            function repeatActionCompleted(data) {
                queryCompleted(message);
                programsSaving(false);
                createAlert('warning', 'Action has been repeated!');
                action.isSaving(false);
            }
        }

        // Get an action by which it was repeated from
        function getRepeatedAction(action) {
            var repeatedAction = programsService.getRepeatedAction(manager, action);
            return repeatedAction;
        }

        function savePlanElemAttr(planElement, programId, patientId) {
            var message = queryStarted('Assignment', true, 'Saving');

            var planElem = { Id: planElement.id(), AssignToId: planElement.assignToId() }
            return programsService.savePlanElemAttrs(manager, planElem, programId, patientId).then(queryCompleted(message));
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

            // Save all of the levels of everything related to a contact card
            goal.entityAspect.acceptChanges();
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
                // Accept the changes to everything related to a goal
                ko.utils.arrayForEach(goal.tasks(), function (task) {
                    task.entityAspect.acceptChanges();
                });
                ko.utils.arrayForEach(goal.interventions(), function (intervention) {
                    intervention.entityAspect.acceptChanges();
                });
                ko.utils.arrayForEach(goal.barriers(), function (barrier) {
                    barrier.isNew(false);
                    barrier.entityAspect.acceptChanges();
                });
                // Finally, clear out the message
                queryCompleted(message);
            }
        }

        // Save a type of intervention
        function saveIntervention(intervention) {
            // Display a message while saving
            var message = queryStarted('Intervention', true, 'Saving');

            // Save intervention changes so new ones returned are accepted
            intervention.entityAspect.acceptChanges();
            var serializedIntervention;
            serializedIntervention = entitySerializer.serializeIntervention(intervention, manager);
            return goalsService.saveIntervention(manager, serializedIntervention, intervention.goal().patientId()).then(saveCompleted);

            function saveCompleted(data) {
                // Check if we have already added the intervention to the local collection
                if (localCollections.interventions.indexOf(intervention) === -1) {
                    // If not, add it in
                    localCollections.interventions.push(intervention);
                }
                // Save all of the levels of everything related to a contact card
                intervention.entityAspect.acceptChanges();
                // Finally, clear out the message
                queryCompleted(message);
            }    
        }

        // Save a type of intervention
        function saveTask(task) {
            // Display a message while saving
            var message = queryStarted('Task', true, 'Saving');
            // Get the patient id from the goal
            var patientId = task.goal().patientId();
            // Save all of the levels of everything related to a contact card
            task.entityAspect.acceptChanges();
            var serializedTask;
            serializedTask = entitySerializer.serializeTask(task, manager);
            return goalsService.saveTask(manager, serializedTask, patientId).then(saveCompleted);

            function saveCompleted(data) {
                // Check if we have already added the intervention to the local collection
                if (localCollections.tasks.indexOf(task) === -1) {
                    // If not, add it in
                    localCollections.tasks.push(task);
                }
                // Save all of the levels of everything related to a contact card
                //task.entityAspect.acceptChanges();
                // Finally, clear out the message
                queryCompleted(message);
            }
        }

        // Save a type of barrier
        function saveBarrier(barrier) {
            // Display a message while saving
            var message = queryStarted('Barrier', true, 'Saving');
            // Get the patient id from the goal
            var patientId = barrier.goal().patientId();

            // Save barrier changes so new ones returned are accepted
            barrier.entityAspect.acceptChanges();
            var serializedBarrier;
            serializedBarrier = entitySerializer.serializeBarrier(barrier, manager);
            return goalsService.saveBarrier(manager, serializedBarrier, patientId).then(saveCompleted);

            function saveCompleted(data) {
                // Save all of the levels of everything related to a contact card
                barrier.entityAspect.acceptChanges();
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
                // Clear out the message
                queryCompleted(message);
                manager.detachEntity(goal);
            }
        }

        // Save changes to a single contact card
        function saveNote(note) {
            // Display a message while saving
            var message = queryStarted('Note', true, 'Saving');
            var serializedNote = entitySerializer.serializeNote(note, manager);
            return notesService.saveNote(manager, serializedNote).then(saveCompleted);

            function saveCompleted(data) {
                // Replace the id of the note since we had a negative number there
                note.id(data.Id);
                // Accept changes
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
            getEntityList(theseObservations, 'PatientObservation', 'local', 'patientId', patientId, false);
            var serializedObservations = [];
            serializedObservations.PatientId = patientId;
            // Go through the observations,
            ko.utils.arrayForEach(theseObservations(), function (observation) {				
				if (observation.needToSave()) {
					// Accept it's changes (if modified)
					observation.entityAspect.acceptChanges();
					// Serialize it
					var serializedObservation = entitySerializer.serializeObservation(observation, manager);
					serializedObservations.push(serializedObservation);
				} else{
					observation.entityAspect.rejectChanges();
				}
            });
            if (serializedObservations.length > 0) {
                observationsSaving(true);
                return observationsService.saveObservations(manager, serializedObservations).then(saveCompleted);
            } else {				
				queryCompleted(message);
				observationsSaving(false);
				return Q();	//return a resolved promise.			
            }

            function saveCompleted(data) {
                observationsSaving(false);
                queryCompleted(message);
                return true;
            }
        }

        function savePatientSystems(patientSystems) {
            // This is all going to have to be refactored
            // When we go to the multiple patient id scenario
            // Since the save call only takes 1 at a time
            var theseObservations = ko.observableArray();
            // Is this an insert or update?
            var isInsert = false;
            // Display a message while saving
            var message = queryStarted('System Ids', true, 'Saving');
            var serializedPatientSystems = [];
            ko.utils.arrayForEach(patientSystems, function (patSys) {
                // And if the patSys a date and has changes...
                if (patSys.systemId() && (patSys.entityAspect.entityState.isAdded() || patSys.entityAspect.entityState.isModified())) {
                    var canSave = true;
                    // If you can save,
                    if (canSave) {
                        // Accept it's changes
                        patSys.entityAspect.acceptChanges();
                        // Serialize it
                        var serializedPatientSystem = entitySerializer.serializePatientSystem(patSys, manager);
                        isInsert = serializedPatientSystem.Id < 0;
                        serializedPatientSystems.push(serializedPatientSystem);
                    }
                }
            });
            if (serializedPatientSystems.length > 0) {
                ko.utils.arrayForEach(serializedPatientSystems, function (patSystem) {
                    return patientsService.savePatientSystem(manager, patSystem, isInsert).then(saveCompleted);
                });                
            }
            function saveCompleted(data) {
                queryCompleted(message);
                var thisPatient = patientSystems[0].patient();
                // Trigger a refresh on anything watching the state of the system id
                thisPatient.patientSystems.valueHasMutated();
                thisPatient.displaySystemId(patientSystems[0].systemId());
                // If it was an insert,
                if (isInsert) {
                    // Find the first patient system that is new
                    ko.utils.arrayForEach(patientSystems, function (patSys) {
                        // Set to the returned id
                        if (patSys.id() < 0) {
                            patSys.id(data.PatientSystemId);
                            patSys.entityAspect.acceptChanges();
                        }
                    });
                }
                thisPatient.entityAspect.acceptChanges();
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

        /**		
		*		note: breeze rejectChanges has a known issue with array of complex type:
		*			regardless of the entityState, all items of the array will be removed, 
		*			even if there were no changes. (https://github.com/Breeze/breeze.js/issues/47)
		*	 	workaround options:
		*	 	1. reload the entity from the server right after rejectChanges.
		*	 	2. keep the original pristine state to go back to. (example - see todo.panel todo.programIds restored by originalProgramIds).
		*	
		*	@method cancelEntityChanges
		*		
		*	@param entity - a single entity to cancel changes if any.		
		*/		
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

        function checkForFourOhOne(error) {
            // Check if the error status code is a 401
            if (error.status && error.status === 401) {
                // Log the user out
                session.logOff();
            }
        }

        function addPatientToRecentList (patient) {
            patientsService.addPatientToRecentList(patient);
        }

        function createAlert(result, reason) {
            // We use the name result and reason because that is the backend convention, not our choice
            // Create an alert
            var thisAlert = datacontext.createEntity('Alert', { result: result, reason: reason });
            // Accept changes since it doesn't get persisted anyway
            thisAlert.entityAspect.acceptChanges();
            // Add it to the enums of alerts
            localCollections.alerts.push(thisAlert);
        }

        function getToDos (observable, params) {
            var message = queryStarted('ToDos', true, 'Loading');
            todosSaving(true);
            return notesService.getToDos(manager, observable, params).then(todosReturned);

            function todosReturned(todos) {
                // Finally, clear out the message
                queryCompleted(message);
                // Make sure each of the todos are in the collection locally
                ko.utils.arrayForEach(todos, function (todo) {
                    if (localCollections.todos.indexOf(todo) === -1) {
                        // Add it in
                        localCollections.todos.push(todo);
                    }
                });
                todosSaving(false);
            }
        }

        function getToDosQuery (params, orderstring) {
            return notesService.getToDosQuery(manager, params, orderstring);
        }

        function getInterventions (observable, params) {
            var message = queryStarted('Interventions', true, 'Loading');
            interventionsSaving(true);
            return goalsService.getInterventions(manager, observable, params).then(interventionsReturned);

            function interventionsReturned(interventions) {
                // Finally, clear out the message
                queryCompleted(message);
                // Make sure each of the interventions are in the collection locally
                ko.utils.arrayForEach(interventions, function (intervention) {
                    if (localCollections.interventions.indexOf(intervention) === -1) {
                        // Add it in
                        localCollections.interventions.push(intervention);
                    }
                });
                interventionsSaving(false);
            }
        }

        function getInterventionsQuery (params, orderstring) {
            return goalsService.getInterventionsQuery(manager, params, orderstring);
        }

        function getTasks (observable, params) {
            var message = queryStarted('Tasks', true, 'Loading');
            tasksSaving(true);
            return goalsService.getTasks(manager, observable, params).then(tasksReturned);

            function tasksReturned(tasks) {
                // Finally, clear out the message
                queryCompleted(message);
                // Make sure each of the tasks are in the collection locally
                ko.utils.arrayForEach(tasks, function (task) {
                    if (localCollections.tasks.indexOf(task) === -1) {
                        // Add it in
                        localCollections.tasks.push(task);
                    }
                });
                tasksSaving(false);
            }
        }

        function getTasksQuery (params, orderstring) {
            return goalsService.getTasksQuery(manager, params, orderstring);
        }

        // Save changes to a single contact card
        function saveToDo(todo, action) {
            // Display a message while saving
            var message = queryStarted('ToDo', true, 'Saving');
            todosSaving(true);
            todo.entityAspect.acceptChanges();
            var serializedTodo = entitySerializer.serializeToDo(todo, manager);
            return notesService.saveToDo(manager, serializedTodo, action).then(saveCompleted);

            function saveCompleted(data) {
                // If it is a new todo,
                if (todo && todo.id() < 0) {
                    // Remove it so the replacement gets set
                    manager.detachEntity(todo);
                }
                if (localCollections.todos.indexOf(data) < 0) {
                    localCollections.todos.push(data);                    
                }
                // Finally, clear out the message
                queryCompleted(message);
                todosSaving(false);
                return data;
            }
        }

		
		function getUsercareManagerName(){
			var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
						return caremanager.id() === session.currentUser().userId();
			});
			return thisMatchedCareManager.preferredName();
		}
		
		function getCalendarEvents( theseTodos ){
			//convert todos to calendar events:
			var userEvents = [];
			var careManagerName = getUsercareManagerName();
			ko.utils.arrayForEach(theseTodos, function (todo) {
				var isEvent = isTodoEvent(todo);
				if(isEvent){
					var event = {	//fullcalendar event - plain object	//getNewEvent();
						id: todo.id(),
						title: getEventTitle(todo),
						start: todo.dueDate(),
						allDay: true,
						patientId: todo.patientId(),
						patientName: getEventPatientName(todo),
						assignedToName: careManagerName,
						userId: todo.assignedToId(),
						typeId: 2
					}
					userEvents.push(event);
				}
			});
			return userEvents;
		}
		
		/**
		*	synchronize Event entities based on the given todos.
		*	the event entities are on client only and they reflect todos and interventions.
		*	in this function we reflect the given todos on Event entities by creating / updating / deleting accirdingly.
		*	@method syncCalendarEvents 		
		*	@param theseTodos: the current user todos - expected todos assigned to the current user and that are not deleted.
		*
		*/
		function syncCalendarEvents( theseTodos ){
            // Add /update an event entity for each todo
			var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
						return caremanager.id() === session.currentUser().userId();
			});
            ko.utils.arrayForEach(theseTodos, function (todo) {				
				syncEventFromTodo(todo);			
            });	

			function syncEventFromTodo(todo){
				var isEvent = isTodoEvent(todo);
				if(isEvent){
					//the todo should be represented by a calendar event:
					if(!updateCalendarEventFromTodo(todo)){
						insertCalendarEventFromTodo(todo);
					}					
				}
				else{					
					removeCalendarEventById(todo.id());	
				}
			}
			
			function insertCalendarEventFromTodo(todo){
				var newEvent = {
					id: todo.id(),
					title: getEventTitle(todo),
					start: todo.dueDate(),
					allDay: true,
					patientId: todo.patientId(),
					patientName: getEventPatientName(todo),
					assignedToName: thisMatchedCareManager.preferredName(),
					userId: todo.assignedToId(),
					typeId: 2
				};
				createCalendarEvent(newEvent);
			}
	
			function updateCalendarEventFromTodo(todo){
				var existingEvent = getEventById(todo.id());
				if(existingEvent){
					existingEvent.title(getEventTitle(todo));
					existingEvent.start(todo.dueDate());
					existingEvent.patientId(todo.patientId());
					existingEvent.patientName(getEventPatientName(todo));
					existingEvent.assignedToName(thisMatchedCareManager.preferredName());
					existingEvent.userId(todo.assignedToId());
					existingEvent.entityAspect.acceptChanges();
					return true;
				}
				return false;
			}			
		}
		
		function getEventTitle(todo){
			return (todo.patientDetails() ? todo.patientDetails().fullLastName() + ', ' + todo.patientDetails().fullFirstName() + ' - ' : '') + todo.title();
		};
		function getEventPatientName(todo){
			return todo.patientDetails() ? todo.patientDetails().fullLastName() + ', ' + todo.patientDetails().fullFirstName() : '-';
		};
		function isTodoEvent(todo){
					//does this todo need to be represented by a calendar event:
					//	- assigned to current user
					// 	- not deleted
					//	- open
					return (todo && todo.assignedToId() && todo.assignedToId() === session.currentUser().userId() 
						&& !todo.deleteFlag() && moment(todo.dueDate()).isValid()
						&& (todo.statusId() === 1 || todo.statusId() === 3));
		};
		
		// function getNewEvent(){
			// function Event() {
				// var self = this;
				// self.id = ko.observable();
				// self.title = ko.observable();
				// self.start = ko.observable();
				// self.allDay = ko.observable(false);
				// self.url = ko.observable('');
				// self.patientId = ko.observable('');
				// self.patientName = ko.observable('');
				// self.assignedToName = ko.observable('');
				// self.typeId = ko.observable('');
				// self.userId = ko.observable();
			// }
			// return new Event();
		// }
		
        // Update a todo patient's information
        function updateTodoPatient(patient) {
            var thisTodoPatient = ko.observable();
            // Check to see if there is a matching todo patient dto
            checkForEntityLocally(thisTodoPatient, patient.id(), 'ToDoPatient');
            // If a matching todo patient was found,
            if (thisTodoPatient()) {
                // Update all of the properties
                thisTodoPatient().firstName(patient.firstName());
                thisTodoPatient().lastName(patient.lastName());
                thisTodoPatient().middleName(patient.middleName());
                thisTodoPatient().suffix(patient.suffix());
                thisTodoPatient().preferredName(patient.preferredName());
            }
        }

        // Remove an entity from cache
        function detachEntity(entity) {
            manager.detachEntity(entity);
        }

        function initializeNewPatientMedication(patient, name) {
            // Create a fake medications length
            var medId = (patient.medications().length + 1) * -1;
            // Get the default source
            var defaultSource = ko.utils.arrayFirst(datacontext.enums.allergySources(), function (src) {
                return src.isDefault();
            });
            // Get the default type to associate
            var defaultType = ko.utils.arrayFirst(datacontext.enums.medSuppTypes(), function (type) {
                return type.name() === 'Prescribed';
            });
            var patMed = createEntity('PatientMedication', { id: medId, patientId: patient.id(), categoryId: 1, statusId: 1, sourceId: defaultSource.id(), type: defaultType });
            patMed.isNew(true);
            patMed.isEditing(true);
            patMed.name(name);
			patMed.isCreateNewMedication(true);
            return patMed;
        }
		
		/**
		*			read medication frequencies from 'Frequency' lookup merged with patient specific medication frequencies ('PatientMedFrequency' - as in back end).
		*			the cache entity for this merge is : PatientMedicationFrequency.
		*			we need to return frequencies (PatientMedicationFrequency entities) 
		*			with patientId = null OR patientId = -1 OR patientId = given patientId.
		*
		*	@method getPatientFrequencies		
		*/
		function getPatientFrequencies(observable, patientId, forceRemote){
			return medicationsService.getPatientFrequencies(manager, observable, patientId, forceRemote);
		}
		
        function getPatientMedications (observable, params, patientId) {
            var message = queryStarted('Medications', true, 'Loading');
            medicationSaving(true);
            return medicationsService.getPatientMedications(manager, observable, params, patientId).then(medicationReturned);

            function medicationReturned(medication) {
                // Finally, clear out the message
                queryCompleted(message);
                medicationSaving(false);
            }
        }

        function getRemoteMedicationFields (medication) {			
            return medicationsService.getRemoteMedicationFields(manager, medication).then(dataReturned);

            function dataReturned (data) {
                return data;
            }
        }

        function getPatientMedicationsQuery (params, orderstring) {
            return medicationsService.getPatientMedicationsQuery(manager, params, orderstring);
        }

        function initializeNewMedication(name){
            return medicationsService.initializeNewMedication(manager, name);
        }
		
        // Save changes to a list of medications
        function saveMedication(medication) {
            var message = queryStarted('Medication', true, 'Saving');			
			trimNewMedicationFields(medication);
			if(medication.customFrequency()){
				//a custom frequency is added - save it first
				return saveCustomFrequency(medication).then(handleCreateNew);
			}
			else return handleCreateNew(medication);
			
			function handleCreateNew(medication){
				//continue saving:
				if(medication.isCreateNewMedication() === true){
					console.log('datacontext-saveMedication: isCreateNewMedication = true');				
					//initialize medicationMap record and link by setting its id in familyId before saving this patient medication:
					return medicationsService.initializeNewMedication(manager, name)
						.then(function(data){
							if(data && data.MedicationMap && data.MedicationMap.Id){ 
								console.log('datacontext-saveMedication: isCreateNewMedication = true => got a familyId ' + data.MedicationMap.Id);
								medication.familyId(data.MedicationMap.Id);                							
								return saveIt(medication);							               
							}
						});
				}
				else{
					console.log('datacontext-saveMedication: isCreateNewMedication = false');
					return saveIt(medication);
				}
			}
						
			function saveCustomFrequency(medication){				
				return medicationsService.saveCustomFrequency(manager, medication.customFrequency(), medication.patientId())				
				.then(
					function(data){
						//when it is returned: populate the new created frequencyId in medication.frequencyId
						if(data && data.Id){
							medication.frequencyId(data.Id);
						}
						return medication;
					});
			}
			
			function trimNewMedicationFields(medication){				
				if(medication.name())
					medication.name(medication.name().trim());
				if(medication.route())
					medication.route(medication.route().trim());
				if(medication.form())
					medication.form(medication.form().trim());
				if(medication.strength())
					medication.strength(medication.strength().trim());
				if(medication.reason())
					medication.reason(medication.reason().trim());				
				if(medication.notes())
					medication.notes(medication.notes().trim());				
				if(medication.prescribedBy())
					medication.prescribedBy(medication.prescribedBy().trim());				 				
				if(medication.freqQuantity())
					medication.freqQuantity(medication.freqQuantity().trim());				
				if(medication.dosage())
					medication.dosage(medication.dosage().trim());
				if(medication.customFrequency())
					medication.customFrequency(medication.customFrequency().trim());
			}
			
			function saveIt(medication){
				console.log('datacontext-saveMedication: saveIt recalculateNDC=' + medication.recalculateNDC());
				medication.entityAspect.acceptChanges();
				// Serialize it
				var serializedMedication = entitySerializer.serializePatientMedication(medication, manager);
				if (serializedMedication) {
					medicationSaving(true);
					return medicationsService.saveMedication(manager, serializedMedication, medication.isNew()).then(saveCompleted);
				} else {
					return Q.promise(function () {
						queryCompleted(message);
						return true;
					}).then(saveCompleted);
				}
				
				function saveCompleted(data) {
					// If it was a newly created med,
					if (medication.isNew()) {
						// Remove it so the newly returned med is only one shown
						setTimeout(function () {
							// Wait half a second to remove it so it can be properly disposed of
							medication.entityAspect.rejectChanges();
							medication.patientId(null);
							manager.detachEntity(medication);
						}, 500);
					}
					// Remove it so the newly returned med is only one shown
					medication.isEditing(false);
					medicationSaving(false);
					queryCompleted(message);
					return data.results[0];
				}
			}            
        }

		function deletePatientMedication(medication){						
			var message = queryStarted('Medication', true, 'Deleting');
            return medicationsService.deletePatientMedication(manager, medication).then(deleted).fail(deleteFailed);

            function deleted(data) {
                // Remove the medication from cache completely                
                manager.detachEntity(medication);                

                // Finally, clear out the message
                queryCompleted(message);
                //setTimeout(function () { location.reload(); }, 2000);
                return true;
            }

            function deleteFailed(error) {
                var thisAlert = datacontext.createEntity('Alert', { result: 0, reason: 'Delete failed!' });
                thisAlert.entityAspect.acceptChanges();
                localCollections.alerts.push(thisAlert);
                checkForFourOhOne(error);
                removeLoadingMessage(message);
                throw new Error('Delete failed');
            }
		}
		
        function initializeAllergy(observable, type, allergyId, patientId, isNewAllergy) {
            return allergiesService.initializeAllergy(manager, observable, type, patientId, allergyId).then(initialized);
            // var newAllergy = createEntity('PatientAllergy', { id: -1, name: 'Fake Allergy', patientId: patientId });
            // return Q.promise(function () {
            //     return observable(newAllergy);
            // });
            function initialized(allergy) {
                // Find the default allergy source
                var defaultSource = ko.utils.arrayFirst(datacontext.enums.allergySources(), function (src) {
                    return src.isDefault();
                });
                // Set default properties on the allergy
                ko.unwrap(allergy).source(defaultSource);
                ko.unwrap(allergy).statusId(1);
                ko.unwrap(allergy).isUserCreated(isNewAllergy);
                return allergy;
            }
        }

        function initializeNewAllergy(allergyName) {
            allergySaving(true);
            return allergiesService.initializeNewAllergy(manager, allergyName);
        }

        // Save changes to a list of allergies
        function saveAllergies(allergies) {
            var serializedAllergies = [];
            var message = queryStarted('Allergy', true, 'Saving');
            // serializedAllergies.PatientId = patientId;
            // Go through the observations,
            ko.utils.arrayForEach(allergies, function (allergy) {
                allergy.entityAspect.acceptChanges();
                // Serialize it
                var serializedAllergy = entitySerializer.serializePatientAllergy(allergy, manager);
                serializedAllergies.push(serializedAllergy);
            });
            if (serializedAllergies.length > 0) {
                allergySaving(true);
                return allergiesService.saveAllergies(manager, serializedAllergies).then(saveCompleted);
            } else {
                return Q.promise(function () {
                    queryCompleted(message);
                    return true;
                }).then(saveCompleted);
            }

            function saveCompleted(data) {
                // After saving, remove the isNew flag
                ko.utils.arrayForEach(allergies, function (allergy) {
                    allergy.isNew(false);
                });
                allergySaving(false);
                queryCompleted(message);
                return data.results;
            }
        }

		function deletePatientAllergy(allergy){
			var message = queryStarted('Allergy', true, 'Deleting');
            return allergiesService.deletePatientAllergy(manager, allergy).then(deleted).fail(deleteFailed);

            function deleted(data) {
                // Remove the allergy from cache completely                
                manager.detachEntity(allergy);                

                // Finally, clear out the message
                queryCompleted(message);
                //setTimeout(function () { location.reload(); }, 2000);
                return true;
            }

            function deleteFailed(error) {
                var thisAlert = datacontext.createEntity('Alert', { result: 0, reason: 'Delete failed!' });
                thisAlert.entityAspect.acceptChanges();
                localCollections.alerts.push(thisAlert);
                checkForFourOhOne(error);
                removeLoadingMessage(message);
                throw new Error('Delete failed');
            }	
		}
		
        function getPatientAllergies (observable, params, patientId) {
            var message = queryStarted('Allergies', true, 'Loading');
            allergySaving(true);
            return allergiesService.getPatientAllergies(manager, observable, params, patientId).then(allergiesReturned);

            function allergiesReturned(allergies) {
                // Finally, clear out the message
                queryCompleted(message);
                allergySaving(false);
            }
        }

        function getPatientAllergiesQuery (params, orderstring) {
            return allergiesService.getPatientAllergiesQuery(manager, params, orderstring);
        }

        function getRemoteAllergies(searchterm) {
            return allergiesService.getRemoteAllergies(manager, searchterm);
        }

        function singleSort (patientId, breezeEntities, type, prop) {
            var query = breeze.EntityQuery
                .from(type)
                .toType(type)
                .where('patientId', '==', patientId)
                .orderBy(prop);

            return manager.executeQueryLocally(query);
        }

    });