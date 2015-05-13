// Register all of the user related models in the entity manager (initialize function) and provide other non-entity models
define(['config.services', 'services/session'],
	function (servicesConfig, session) {

	    var datacontext;

		var DT = breeze.DataType;

		// Expose the model module to the requiring modules
		var usermodels = {
		    initialize: initialize,
		    createCalendarMocks: createCalendarMocks
		};
		return usermodels;

		// Initialize the entity models in the entity manager
		function initialize(metadataStore) {

		    // User information
		    metadataStore.addEntityType({
		        shortName: "User",
		        namespace: "NightingaleUser",
		        dataProperties: {
		            userId: { dataType: "String", isPartOfKey: true },
		            firstName: { dataType: "String" },
		            lastName: { dataType: "String" },
		            userName: { dataType: "String" },
		            aPIToken: { dataType: "String" },
		            contracts: { complexTypeName: "Contract:#NightingaleUser", isScalar: false }
		            //settings: { complexTypeName: "Setting:#NightingaleUser", isScalar: false }
		        }
		    });

            // Contract complex type
		    metadataStore.addEntityType({
		        shortName: "Contract",
		        namespace: "NightingaleUser",
		        isComplexType: true,
		        dataProperties: {
		            id: { dataType: "Int64" },
		            name: { dataType: "String" },
		            number: { dataType: "String" }
		        }
		    });

		    // Setting complex type
		    //metadataStore.addEntityType({
		    //    shortName: "Setting",
		    //    namespace: "NightingaleUser",
		    //    isComplexType: true,
		    //    dataProperties: {
		    //        key: { dataType: "String" },
		    //        value: { dataType: "String" }
		    //    }
		    //});

		    // Calendar Event information
		    metadataStore.addEntityType({
		        shortName: "Event",
		        namespace: "NightingaleUser",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            userId: { dataType: "String" },
		            title: { dataType: "String" },
		            start: { dataType: "DateTime" },
		            end: { dataType: "DateTime" },
                    allDay: { dataType: "Boolean" },
		            url: { dataType: "String" }
		        }
		    });
            
		    metadataStore.registerEntityTypeCtor(
				'User', null, userInitializer);

		    function userInitializer(user) {
		        user.settings = ko.observableArray();
		    }

		}

		function createCalendarMocks(manager, userId) {

            // Grab todays date
		    var date = new Date();
		    var d = date.getDate();
		    var m = date.getMonth();
		    var y = date.getFullYear();

            // Mock up some fake calendar events
		    var eventOne = manager.createEntity('Event', { id: 'eventone', title: 'Diabetes Appointment', userId: userId, start: new Date(y, m, d+4),end: new Date(y, m, d+4), allDay: true });
		    var eventTwo = manager.createEntity('Event', { id: 'eventtwo', title: 'Phone call', userId: userId, start: new Date(y, m, d - 5), end: new Date(y, m, d - 2) });
		    var eventThree = manager.createEntity('Event', { id: 'eventthree', title: 'Something else', userId: userId, start: new Date(y, m, d - 5), end: new Date(y, m, d - 2) });
		    var eventFour = manager.createEntity('Event', { id: 'eventfour', title: 'Goto google.com', userId: userId, start: new Date(y, m, 1), allDay: true, url: 'http://www.google.com' });
		}

		function checkDataContext() {
		    if (!datacontext) {
		        datacontext = require('services/datacontext');
		    }
		}
	});