define(['models/base'], function (modelConfig) {

  // This is a module to hold collections that are shared resources throughout the application
  // We can't have a two way reference to datacontext, so it requires this module
  var datacontext;
  //
  //   This augments the datacontext and breeze by keeping commonly used collections in one place.
  //
  //   Enums is a list of enums that is used around the datacontext

  var enums = {};
  enums.priorities = ko.observableArray();
  enums.genders = ko.observableArray([
    new modelConfig.Gender('m', 'M', 'Male'),
    new modelConfig.Gender('f', 'F', 'Female'),
    new modelConfig.Gender('n', 'N', 'Neutral')
  ]);
	enums.patientStatuses = ko.observableArray();
	enums.patientStatusReasons = ko.observableArray();
	//patient multi id status
	enums.patientSystemStatus =  ko.observableArray();
	enums.systemStatus =  ko.observableArray();
	enums.systems = ko.observableArray();
  enums.daysOfWeek = ko.observableArray([
    new modelConfig.Day('0', 'M', 'Monday'),
    new modelConfig.Day('1', 'Tu', 'Tuesday'),
    new modelConfig.Day('2', 'W', 'Wednesday'),
    new modelConfig.Day('3', 'Th', 'Thursday'),
    new modelConfig.Day('4', 'F', 'Friday'),
    new modelConfig.Day('5', 'Sat', 'Saturday'),
    new modelConfig.Day('6', 'Sun', 'Sunday')
  ]);
	enums.userAssociatedTypes = ko.observableArray([
		new modelConfig.SomeType('0', 'Assigned to me'),
		new modelConfig.SomeType('1', 'Unassigned')
	]);
	enums.maritalStatuses = ko.observableArray();
	enums.deceasedStatuses = ko.observableArray();
	
	enums.contactStatuses = ko.observableArray([
		new modelConfig.SomeType('1', 'Active'),
		new modelConfig.SomeType('2', 'Inactive'),
		new modelConfig.SomeType('3', 'Archived')
	]);
	enums.contactLookUpGroupType = ko.observableArray([
		new modelConfig.SomeType('0', 'Unknown'),
		new modelConfig.SomeType('1', 'ContactType'),
		new modelConfig.SomeType('2', 'CareTeam')
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
  enums.toDoCategories = ko.observableArray();
  enums.interventionCategories = ko.observableArray();
  enums.goalTypes = ko.observableArray();
  enums.goalTaskStatuses = ko.observableArray();
  enums.barrierStatuses = ko.observableArray();
  enums.interventionStatuses = ko.observableArray();
  enums.careManagers = ko.observableArray();
  enums.careMemberTypes = ko.observableArray();
  enums.careMemberFrequency = ko.observableArray();
  enums.observationTypes = ko.observableArray();
  enums.observationStates = ko.observableArray();
  enums.observationDisplays = ko.observableArray();
  enums.objectives = ko.observableArray();
  // Note specific lookups
  enums.noteTypes = ko.observableArray();
  enums.noteMethods = ko.observableArray();
  enums.noteWhos = ko.observableArray();
  enums.noteSources = ko.observableArray();
  enums.noteOutcomes = ko.observableArray();
	// Note utilization type lookups
	enums.visitTypes = ko.observableArray();
	enums.utilizationSources = ko.observableArray();
	enums.dispositions = ko.observableArray();
	enums.utilizationLocations = ko.observableArray();
  // Allergy lookups
  enums.allergyTypes = ko.observableArray();
  enums.severities = ko.observableArray();
  enums.reactions = ko.observableArray();
  enums.allergySources = ko.observableArray();
  enums.allergyStatuses = ko.observableArray();
  // Medication lookups
  enums.medicationStatuses = ko.observableArray();
  enums.medicationCategories = ko.observableArray();
  enums.freqHowOftens = ko.observableArray();
	enums.frequency = ko.observableArray();	//a new lookup to replace freqHowOftens and freqWhens (ENG 969)
  enums.medSuppTypes = ko.observableArray();
  enums.freqWhens = ko.observableArray();

    //
    //   ToDos are shared between a bunch of views so we keep a collection here as well
    var todos = ko.observableArray();

	var contacts = ko.observableArray();
    //
    //   Interventions are shared between a bunch of views so we keep a collection here as well
    var interventions = ko.observableArray();

    //
    //   Tasks are shared between a bunch of views so we keep a collection here as well
    var tasks = ko.observableArray();

    //
    //   Events are shared between a bunch of views so we keep a collection here as well
    var events = ko.observableArray();

    var alerts = ko.observableArray();
	
	var contactTypesTree = ko.observableArray();
	var contactTypes = ko.observableArray();
    var localcollections = {
        enums: enums,
        todos: todos,
		contacts: contacts,
        tasks: tasks,
        interventions: interventions,
        events: events,
        alerts: alerts,
		contactTypesTree: contactTypesTree,
		contactTypes: contactTypes,
        refreshToDos: refreshToDos,
        refreshInterventions: refreshInterventions
    }
    return localcollections;

    function refreshToDos() {
        checkDataContext();
        datacontext.getEntityList(todos, 'ToDo', 'fakeTodoEndPoint', null, null, false);
    }

    function refreshInterventions() {
        checkDataContext();
        datacontext.getEntityList(interventions, 'Intervention', 'fakeInterventionEndPoint', null, null, false);
    }

    function checkDataContext() {
        if (!datacontext) {
            datacontext = require('services/datacontext');
        }
    }


}); 