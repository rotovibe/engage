// Register all of the user related models in the entity manager (initialize function) and provide other non-entity models
define(['config.services', 'services/session'],
	function (servicesConfig, session) {

	    var datacontext;

		var DT = breeze.DataType;

		// Expose the model module to the requiring modules
		var usermodels = {
		    initialize: initialize,
		    createCalendarMocks: createCalendarMocks,
		    initializeEnums: initializeEnums
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
		            url: { dataType: "String" },
		            patientId: { dataType: "String" },
		            typeId: { dataType: "String" },
		            patientName: { dataType: "String" },
		            assignedToName: { dataType: "String" }
		        },
				navigationProperties: {
					type: {
						entityTypeName: "EventType", isScalar: true,
						associationName: "Event_Type", foreignKeyNames: ["typeId"]
					}
				}
		    });

		    // Event type information
		    metadataStore.addEntityType({
		        shortName: "EventType",
		        namespace: "NightingaleUser",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" }
		        }
		    });
            
		    metadataStore.registerEntityTypeCtor(
				'User', null, userInitializer);
			metadataStore.registerEntityTypeCtor(
				'Event', null, eventInitializer);
				
		    function userInitializer(user) {
		        user.settings = ko.observable();//ko.observableArray();
		        user.recentIndividuals = ko.observableArray();
		        user.computedRecentIndividuals = ko.computed(function () {
		        	var theseIndividuals = user.recentIndividuals().slice(0, 5);
		        	return theseIndividuals;
		        }).extend({ throttle: 25 });
		    }
			
			function eventInitializer(event){
				event.hasTimes = ko.computed( function(){
					return moment( event.end() ).isValid() && moment( event.start() ).isValid();
				});
				event.timeString = function(){				
					var start = moment( event.start() );
					var strDate = start.isValid ? start.format("MM/DD/YYYY") : '-';
					if( event.end() ){					
						var end = moment( event.end() );					
						if( end.isValid() && start.isValid() ){
							var strDate = start.format("MM/DD/YYYY h:mm A");
							if( start.isSame( end, 'day') ){
								var endTime = end.format("h:mm A");
								strDate += ' - ' + endTime;
							} else{
								var endTime = end.format("MM/DD/YYYY h:mm A");
								strDate += ' - ' + endTime;
							}
						}					
					}
					return strDate;
				};
			}

		}

	    // Initialize the entity models in the entity manager
		function initializeEnums(manager) {

		    // Create the enums to use for element states so that they can be shared throughout the application
		    manager.createEntity('EventType', { id: 1, name: 'Intervention' }).entityAspect.acceptChanges();
		    manager.createEntity('EventType', { id: 2, name: 'To Do' }).entityAspect.acceptChanges();

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