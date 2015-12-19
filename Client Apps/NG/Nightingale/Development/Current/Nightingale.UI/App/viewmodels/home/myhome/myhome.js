﻿define(['services/session', 'services/datacontext', 'config.services', 'viewmodels/shell/shell', 'models/base', 'services/local.collections', 'services/navigation', 'plugins/router'],
    function (session, datacontext, servicesConfig, shell, modelConfig, localCollections, navigation, router) {

        function CalendarOptionsModel(events, header, editable, viewDate, defaultView) {
            var self = this;
            // Set the events equal to an observableArray of unwrapped events
            self.events = events;			
            self.header = header;
            self.editable = editable;
            self.viewDate = viewDate || ko.observable(new Date());
            self.defaultView = defaultView;
            self.eventClick = function (event) {
                // If we are already in the context of the patient
                if (navigation.currentRoute() && navigation.currentRoute().config.title === 'Individual') {
                    // Get the goals subroute
                    var thisSubRoute = ko.utils.arrayFirst(navigation.currentRoute().config.settings.pages, function (page) {
                        return page.title === 'goals';
                    });
                    // And set the new sub route
                    navigation.setSubRoute(thisSubRoute);
                } else {
                    // Get the event
                    var thisEvent = datacontext.getEventById(event.id);
                    // Show the event details modal
                    eventModalEntity().event(thisEvent);
                    shell.currentModal(eventModal);
                    eventModalShowing(true);
                }
            };
        }

        var newTodo = ko.observable();

        var initialized = false;
        var selectedCohortToken;
        var maxPatientCount = ko.observable(20);
        var cohortPatientsSkip = ko.observable(0);
		//new paging
		var maxTodosInCache = 400;	//TODO: new parameter?
		var todosTop = ko.observable(0);
		var todosBottom = ko.observable(0);
		var todosTake = ko.observable(100); //TODO: new parameter?
		var todosTotalCount = ko.observable(0);
		var canLoadMoreTodos = ko.observable(false);
		//var canLoadPrevTodos = ko.observable(false);
		var maxToToDosLoaded = ko.observable(false);
		var lockTodos = ko.observable(false);
		
        var fullScreenWidget = ko.observable();

        var leftColumnOpen = ko.observable();
        var myToDoListOpen = ko.observable(true);
        var myInterventionListOpen = ko.observable(true);
        var myWorkListOpen = ko.observable(false);

        var modalShowing = ko.observable(false);
        var modalEntity = ko.observable(new ModalEntity(modalShowing));

        // Event details modal
        var eventModalShowing = ko.observable(false);
        var eventModalEntity = ko.observable(new EventModalEntity(eventModalShowing));
        function dummyFunction () { console.log('something was done'); }
		var eventModalSettings = {
			title: 'Event Details',
			entity: eventModalEntity, 
			templatePath: 'viewmodels/templates/event.details', 
			showing: eventModalShowing, 
			saveOverride: dummyFunction, 
			cancelOverride: dummyFunction, 
			deleteOverride: null, 
			classOverride: null
		}
        var eventModal = new modelConfig.modal(eventModalSettings);

        // Columns to override the default sorts
        var selectedTodoSortColumn = ko.observable();
		var backendSort = ko.observable('-DueDate'); //default todo sort
		var lastSort = ko.observable( backendSort() );
        var selectedInterventionSortColumn = ko.observable();

        function saveOverride () {
            datacontext.saveToDo(newTodo(), 'Insert').then(saveCompleted);

            function saveCompleted(todo) {
                // todo.isNew(false);
                // localCollections.todos.push(newTodo());
				var dummy = myToDos().length;
				todo.clearDirty();
            }
        };
        function cancelOverride () {
            datacontext.cancelEntityChanges(modalEntity().todo());
			modalEntity().todo().clearDirty();
        };
		var modalSettings = {
			title: 'Create To Do',
			entity: modalEntity, 
			templatePath: 'viewmodels/templates/todo.edit', 
			showing: modalShowing, 
			saveOverride: saveOverride, 
			cancelOverride: cancelOverride, 
			deleteOverride: null, 
			classOverride: null
		}
        var modal = new modelConfig.modal(modalSettings);

        function Event() {
            var self = this;
            self.id = ko.observable();
            self.title = ko.observable();
            self.start = ko.observable();
            self.allDay = ko.observable(false);
            self.url = ko.observable('');
            self.patientId = ko.observable('');
            self.patientName = ko.observable('');
            self.assignedToName = ko.observable('');
            self.typeId = ko.observable('');
        }

        // Toggle the active sort.
        //  note: the 3'rd click on a sort field is clearing the sort (!)
        function toggleTodoSort(sender) {
            // If the current column is the one to sort by
            if (selectedTodoSortColumn() && selectedTodoSortColumn().indexOf(sender.sortProperty) !== -1) {
                // If it ends in desc
                if (selectedTodoSortColumn() && selectedTodoSortColumn().substr(selectedTodoSortColumn().length - 4, 4) === 'desc' ) {
                    // Clear the sort column, as it should be undone
                    selectedTodoSortColumn(null);
					backendSort(null);
                } else {
                    // Else set it as the sort column
                    selectedTodoSortColumn(sender.sortProperty + ' desc');
					if( sender.backendSort ){
						backendSort('-'+ sender.backendSort);
					}else{
						backendSort(null);
					}
                }
            } else {
                // Else set it as the new sort column
                selectedTodoSortColumn(sender.sortProperty);
				if( sender.backendSort ){
					backendSort(sender.backendSort);
				}else{
					backendSort(null);
				}
            }
        }

        // Toggle the active intervention sort
        function toggleInterventionSort(sender) {            
            // If the current column is the one to sort by
            if (selectedInterventionSortColumn() && selectedInterventionSortColumn().indexOf(sender.sortProperty) !== -1) {
                // If it ends in desc
                if (selectedInterventionSortColumn() && selectedInterventionSortColumn().substr(selectedInterventionSortColumn().length - 4, 4) === 'desc' ) {
                    // Clear the sort column, as it should be undone
                    selectedInterventionSortColumn(null);
                } else {
                    // Else set it as the sort column
                    selectedInterventionSortColumn(sender.sortProperty + ' desc');
                }
            } else {
                // Else set it as the new sort column
                selectedInterventionSortColumn(sender.sortProperty);
            }
        }

        // The end point to use when getting cohorts
        var cohortEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'cohorts', 'Cohort');
        });
        // The end point to use when getting cohorts
        var currentCohortsPatientsEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'cohortpatients', 'Patient', { Skip: cohortPatientsSkip(), Take: maxPatientCount() });
        });

        // Internal properties and functions
        var newEvent = ko.observable(new Event());
        var myEvents = ko.observableArray();
        var myHeader = {
                left: 'title',
                right: 'today agendaWeek,month prev,next'
        };
        // The date to start the calendar on
        var thisDate = ko.observable(new Date());

        // List of cohorts for this care manager
        var cohortsList = ko.observableArray();
        // The currently selected cohort
        var selectedCohort = ko.observable();
        // A list of patients, in this cohort
        var patientsList = ko.observableArray();
		
		var updateCalendarEvents = function( theseTodos ){			
            //TODO: separate the events from todos as they need to be sorted by dueDate range per calendar view.
			myEvents.removeAll();
			var events = datacontext.getCalendarEvents(theseTodos);	//translate todos to events
			ko.utils.arrayPushAll(myEvents, events);
			myEvents.valueHasMutated();
			datacontext.syncCalendarEvents(theseTodos);	//sync Event entities from given todos
		};
		
		function manageTodoPaging( localCount ){
			//localCount - the number of Current User ToDos on local cache
			if( localCount > todosTop() ){
				todosTop( localCount );
			}
			if( localCount < maxTodosInCache ){
				canLoadMoreTodos(true);
				maxToToDosLoaded(false);
			}
			else{
				canLoadMoreTodos(false);
				maxToToDosLoaded(true);
			}
		}
		
		var refreshMyTodos = function(){						           
            // Either sort by the selected sort or the default
			var selectedSortCol = selectedTodoSortColumn();					
            var orderString = selectedSortCol ? selectedSortCol : 'dueDate desc';			
            // Add the second and third orders to the string
            orderString = orderString + ', category.name, title';
            // Go get the todos locally
			var theseTodos = getCurrentUserToDos( true, orderString);
            updateCalendarEvents(theseTodos);                       
            return theseTodos;			
		};
		
        // My todos
        var myToDos = ko.computed(function () {
            var theseTodos = [];
            //Subscribe to localcollection todos: will trigger the refresh when a todo is deleted\updated
            var allTodos = localCollections.todos();
			//Subscribe to sorting
			var selectedSortCol = selectedTodoSortColumn();			
			var bSort = backendSort? backendSort() : null;
			var locked = lockTodos();
			if( locked ){
				return [];
			}
			if( backendSort() !== lastSort() ){
				console.log(' sort has changed:' + backendSort() );
				lastSort( backendSort() );
				todosTop(0);
				todosBottom(0);
				canLoadMoreTodos(false);
				//canLoadPrevTodos(false);				
				maxToToDosLoaded(false);
				lockTodos(true);
				clearTodosCacheAndLoad();
				printDebugCache('after clearTodosCacheAndLoad():');
				theseTodos = [];
			}
			else{
				//refresh from local query
				theseTodos = refreshMyTodos();
				manageTodoPaging(theseTodos.length);	
			}			
            return theseTodos;
			
        }).extend({ throttle: 50 });

		var todosShowingText = ko.computed( function(){
			var showing = ' showing ';
			var totalCount = todosTotalCount();
			var todos = myToDos? myToDos(): null;
			showing = todos.length + ' showing'
			if( todos.length < totalCount ) {
				showing += ' out of ' + totalCount;
			}
			return showing;
		});
		
        // My interventions
        var myInterventions = ko.computed(function () {
            var theseInterventions = [];
            //Subscribe to localcollection todos
            var allInterventions = localCollections.interventions();
            var params = [];
            var orderString = '';
            // Add these parameters to the query
            params.push(new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '=='));
            params.push(new modelConfig.Parameter('statusId', '2', '!='));
            params.push(new modelConfig.Parameter('statusId', '3', '!='));
            // Either sort by the selected sort or the default
            orderString = selectedInterventionSortColumn() ? selectedInterventionSortColumn() : 'dueDate desc, startDate desc';
            // Add the second and third orders to the string
            var finalOrderString = orderString + ', category.name, description';
            // Go get the interventions
            theseInterventions = datacontext.getInterventionsQuery(params, finalOrderString);
            return theseInterventions;
        }).extend({ throttle: 50 });

        // List of columns currently showing
        var activeTodoColumns = ko.observableArray(['priority','duedate','title','category','patient']);

        // List of columns currently showing
        var activeInterventionColumns = ko.observableArray(['dueDate','description','category','patient','goal']);

        // Object containing the options
        var calendarOptions = new CalendarOptionsModel(myEvents, myHeader, false, thisDate, 'agendaWeek')

        // Reveal the bindable properties and functions
        var vm = {
            activate: activate,
            attached: attached,
            newEvent: newEvent,
            addEvent: addEvent,
            fullScreenWidget: fullScreenWidget,
            toggleFullScreen: toggleFullScreen,
            cohortsList: cohortsList,
            selectedCohort: selectedCohort,
            patientsList: patientsList,
            calendarOptions: calendarOptions,
            title: 'Home',
            myToDos: myToDos,
			todosShowingText: todosShowingText,
			canLoadMoreTodos: canLoadMoreTodos,
			loadMoreTodos: loadMoreTodos,
			//canLoadPrevTodos: canLoadPrevTodos,
			//loadPrevTodos: loadPrevTodos,
			maxToToDosLoaded: maxToToDosLoaded,
            myInterventions: myInterventions,
            activeTodoColumns: activeTodoColumns,
            activeInterventionColumns: activeInterventionColumns,
            editToDo: editToDo,
            addToDo: addToDo,
            toggleOpenColumn: toggleOpenColumn,
            leftColumnOpen: leftColumnOpen,
            toggleTodoSort: toggleTodoSort,
            toggleInterventionSort: toggleInterventionSort,
            selectedTodoSortColumn: selectedTodoSortColumn,
            selectedInterventionSortColumn: selectedInterventionSortColumn,
            myToDoListOpen: myToDoListOpen,
            myWorkListOpen: myWorkListOpen,
            myInterventionListOpen: myInterventionListOpen,
            toggleWidgetOpen: toggleWidgetOpen
        };

        return vm;

        // Toggle whether the widget is open or not
        function toggleWidgetOpen(sender) {
            if (sender === myToDoListOpen && (myWorkListOpen() || myInterventionListOpen())) {
                myToDoListOpen(!myToDoListOpen());
            } else if (sender === myInterventionListOpen && (myWorkListOpen() || myToDoListOpen())) {
                myInterventionListOpen(!myInterventionListOpen());
            } else if (sender === myWorkListOpen && (myInterventionListOpen() || myToDoListOpen())) {
                myWorkListOpen(!myWorkListOpen());
            }
        }

        function addEvent(event) {

            // Check if the event already exists
            var matchedEvent = ko.utils.arrayFirst(localCollections.events(), function (evt) {
                return evt.id() === event.id();
            });

            if (!matchedEvent) {
                var params = {
                    id: event.id(),
                    title: event.title(),
                    start: event.start(),
                    allDay: event.allDay(),
                    patientId: event.patientId(),
                    patientName: event.patientName(),
                    assignedToName: event.assignedToName(),
                    typeId: event.typeId()
                };

                var thisEvent = datacontext.createCalendarEvent(params);   
            }
        }

        function activate() {
            if (!initialized) {
                initializeViewModel();
                initialized = true;
            }
            return true;
        }

        function attached() {
            $('.calendar').fullCalendar('render');
        }

        function getEvents() {
            datacontext.getEventsByUserId(myEvents, session.currentUser().userId(), false);
        }

        function toggleOpenColumn() {
            leftColumnOpen(!leftColumnOpen());
            resizeCalendar();
        }

        function toggleFullScreen(widgetname) {
            // If this widget is already fullscreen,
            if (widgetname === fullScreenWidget()) {
                // Remove full screen widget
                fullScreenWidget(null);
            } else {
                // Else, set it as the full screen widget
                fullScreenWidget(widgetname);
            }
            resizeCalendar();
        }

        function resizeCalendar() {
            // Wait 1 second for animations to finish then trigger calendar redraw
            setTimeout(function () {
                $('.calendar').fullCalendar('render');
            }, 500);
        }

        function initializeViewModel() {
            // BUNCH OF LOGIC HERE WE WILL PROBABLY REUSE LATER
            // Get the mock events for this user
            //getEvents();
            // Go get a list of cohorts locally
            datacontext.getEntityList(cohortsList, cohortEndPoint().EntityType, cohortEndPoint().ResourcePath, null, null, false, null, 'sName').then(cohortsReturned);
            // Get my todos
			todosTop(0);
			todosBottom(0);
			canLoadMoreTodos(false);
			//canLoadPrevTodos(false);
			maxToToDosLoaded(false);
			lockTodos(false);
            getCurrentUserToDos().then(generateCalendarEvents);			
			
            // Get my Interventions
            getCurrentUserInterventions();
            // On first load show the patients list flyout and open the data column
            // Subscribe to changes on the selected cohort to get an updated patient list when it changes
            selectedCohortToken = selectedCohort.subscribe(function () {
                patientsList([]);
                getPatientsByCohort();
            });
            // Set initialized true so we don't accidentally re-initialize the view model
            initialized = true;
            return true;

            function cohortsReturned() {
            }
        };

        function getPatientsByCohort(searchValue) {
            var parameters = {};
            // Create an object to hold the parameters
            var parameters = currentCohortsPatientsEndPoint().Parameters;
            // If a search value is passed in
            if (searchValue) {
                // Add a filter parameter onto parameters
                parameters.SearchFilter = searchValue;
            }
            else { parameters.SearchFilter = null; }
            // TODO : Add Skip and Take to the endpoint and pass it down as params
            // TODO : Make sure the service is checking locally first before going out to the server to get these patients
            datacontext.getEntityList(patientsList, currentCohortsPatientsEndPoint().EntityType, currentCohortsPatientsEndPoint().ResourcePath, null, selectedCohort().iD(), true, parameters).then(calculateSkipTake);
        }

        function choosePatient(patient) {
            // If there is a current patient and it is equal to the patient you are trying to set to current
            if (selectedPatient() && selectedPatient() === patient) {
                // Then do nothing (this is because we don't want to do anything if
                // We have already selected our patient.
                patientsListFlyoutOpen(false);
            }
                // Else check if datacontext exists in the global namespace (It should if datacontext.js has been loaded)
            else if (datacontext) {
                // Else go choose a new patient
                var patientId = ko.unwrap(patient.id);
                // Go get a patient to use as the current patient.  TODO : Remove this when we have a list of patients to select from
                datacontext.getEntityById(selectedPatient, patientId, patientEndPoint().EntityType, patientEndPoint().ResourcePath, true).then(function () { patientsListFlyoutOpen(false); });
                // Go get a list of problems for the currently selected patient
                datacontext.getEntityList(null, patientProblemEndPoint().EntityType, patientProblemEndPoint().ResourcePath + patientId + '/Problems', null, null, true);
                // Go get a list of programs for the currently selected patient
                //datacontext.getEntityList(null, patientProgramsEndPoint().EntityType, patientProgramsEndPoint().ResourcePath, null, patientId, true);
            }
        }

        function calculateSkipTake() {
        }

        function editToDo (sender) {
        }

        function addToDo () {
            newTodo(datacontext.createEntity('ToDo', { id: -1, statusId: 1, priorityId: 0, createdById: session.currentUser().userId(), assignedToId: session.currentUser().userId() }));
            newTodo().isNew(true);
			newTodo().watchDirty();
            modalEntity().todo(newTodo());
            shell.currentModal(modal);
            modalShowing(true);
        }

        function getCurrentUserToDos(local, orderString) {
            // Go get a list of my todos			
			if(local){				
				var params = []
				params.push( new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '==') );
                params.push( new modelConfig.Parameter('statusId', '2', '!=') );
                params.push( new modelConfig.Parameter('statusId', '4', '!=') );
					
				var theseTodos = datacontext.getToDosQuery( params, orderString);	
				// Filter out the new todos
				theseTodos = ko.utils.arrayFilter(theseTodos, function (todo) {
					return !todo.isNew();
				});				
				return theseTodos;
			}
			else{
				return loadMoreTodos()				
			}            
        }
		
		//TODO in next stories - manage the cache
		// function checkTrimLowerCache(){
			// if( myToDos().length + todosTake() > maxTodosInCache ){				
				// var removeTodos = [];
				// var trimmingPoint = todosBottom() + todosTake();
				// for( var i = todosBottom(); i < trimmingPoint ; i++ ){
					// if( !todo.isNew() ){
						// removeTodos.push( myToDos()[i] );
					// }
				// }	
				// todosBottom( todosBottom() + removeTodos.length );	
				// ko.utils.arrayForEach( removeTodos, function(todo){										
					// datacontext.detachEntity( todo );								
				// });
			// }
		// }
		
		function clearTodosCacheAndLoad(){
			var todos = getCurrentUserToDos(true);
			setTimeout( function(){
				//localCollections.todos.removeAll();
				ko.utils.arrayForEach( todos, function(todo){
					localCollections.todos.remove(todo);
					// todo.entityAspect.setDeleted();
					// todo.entityAspect.acceptChanges();
					datacontext.detachEntity(todo);
				});
				lockTodos(false);
				loadMoreTodos();	//load first block with the new sort
			}, 200);						
						
		}
		
		// function checkTrimUpperCache(){
			// if( myToDos().length + todosTake() > maxTodosInCache ){				
				// var removeTodos = [];
				// var trimmingPoint = myToDos().length - todosTake() -1;
				// for( var i = myToDos().length-1 ; i > trimmingPoint ; i-- ){
					// if( !todo.isNew() ){
						// removeTodos.push( myToDos()[i] );
					// }
				// }	
				// todosTop( todosTop() - removeTodos.length );
				// ko.utils.arrayForEach( removeTodos, function(todo){										
					// datacontext.detachEntity( todo );								
				// });					
			// }
		// }
		
		function loadMoreTodos(){			
			//printDebugCache('loadMoreTodos: start:');
			//checkTrimLowerCache(); TODO
			//printDebugCache('loadMoreTodos: after trim:');
			var params = { 
						StatusIds: [1,3], 
						AssignedToId: session.currentUser().userId(), 
						Skip: todosTop(), 
						Take: todosTake(), 
						Sort: backendSort() 
			};
			return datacontext.getToDos(null, params, todosTotalCount).then( calculateTopSkipTake );
		}
		
		function printDebugCache( funcStep ){
			console.log( funcStep + ' todosTop=' + todosTop() + ' todosBottom=' + todosBottom() + ' myToDos().length= '+myToDos().length + 'total todos cache: ' + localCollections.todos().length);
		}
		
		//TODO in next stories - manage the cache
		// function loadPrevTodos(){
			// printDebugCache('loadPrevTodos: start:');			
			// checkTrimUpperCache();
			// printDebugCache('loadPrevTodos: after trim:');
			// if(todosBottom() - todosTake() < 0){ console.log('error: todosBottom() - todosTake() < 0'); }
			// var params = { 
						// StatusIds: [1,3], 
						// AssignedToId: session.currentUser().userId(), 
						// Skip: todosBottom() - todosTake(), 
						// Take: todosTake(), 
						// Sort: backendSort() 
			// };
			// return datacontext.getToDos(null, params, todosTotalCount).then( calculateBottomSkipTake );		
		// }
		
		// function calculateBottomSkipTake(){			
			// var nextBottom = todosBottom() - todosTake();
			// todosBottom( nextBottom );
			// if( nextBottom > 0 ){
				// todosTop(nextSkip);
				// //canLoadPrevTodos(true);
			// }
			// else{
				// todosBottom(0);
				// //canLoadPrevTodos(false);
			// }
			// printDebugCache('calculateBottomSkipTake:');
		// }
		
		function calculateTopSkipTake(){
			var skipped = todosTop();
			var nextSkip = skipped + todosTake();
			if( nextSkip < todosTotalCount() && nextSkip < maxTodosInCache ){
				todosTop(nextSkip);		
				canLoadMoreTodos(true);
			}	
			else{
				canLoadMoreTodos(false);
			}				
			if( nextSkip < maxTodosInCache ){			
				maxToToDosLoaded(false);
			}
			else{				
				maxToToDosLoaded(true);
			}			
			printDebugCache( 'calculateTopSkipTake:' );
		}
		
		function generateCalendarEvents(){
			//after the current user todos loaded: get them locally
			//TODO: will have to get todos by DueDate date range			
			var theseTodos = getCurrentUserToDos( true );
			//create calendar event entities to reflect these todos:
            datacontext.syncCalendarEvents(theseTodos);
		}

        function getCurrentUserInterventions() {
            // Go get a list of my todos
            datacontext.getInterventions(null, { StatusIds: [1], AssignedToId: session.currentUser().userId() });
        }

        function ModalEntity(modalShowing) {
            var self = this;
            self.todo = ko.observable();
            self.canSaveObservable = ko.observable(true);
            self.canSave = ko.computed({
                read: function () {
                    var todook = false;
                    if (self.todo()) {                        
						todook = self.todo().isValid();
                    }
                    return todook && self.canSaveObservable();
                },
                write: function (newValue) {
                    self.canSaveObservable(newValue);
                }
            });
            // Object containing parameters to pass to the modal
            self.activationData = { todo: self.todo, canSave: self.canSave, showing: modalShowing  };
        }

        function EventModalEntity(modalShowing) {
            var self = this;
            self.event = ko.observable();
            self.canSaveObservable = ko.observable(true);
            self.canSave = ko.computed({
                read: function () {
                    var todook = false;
                    if (self.event()) {
                        // var todotitle = !!self.todo().title();
                        // var todostatus = !!self.todo().status();
                        // todook = todotitle && todostatus;
                    }
                    // return todook && self.canSaveObservable();
                    return false;
                },
                write: function (newValue) {
                    self.canSaveObservable(newValue);
                }
            });
            // Object containing parameters to pass to the modal
            self.activationData = { event: self.event, canSave: self.canSave, showing: modalShowing  };
        }

    });