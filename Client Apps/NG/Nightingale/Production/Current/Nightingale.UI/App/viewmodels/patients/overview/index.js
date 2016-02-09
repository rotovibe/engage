define(['services/session', 'services/datacontext', 'config.services', 'models/base', 'services/local.collections', 'viewmodels/patients/index'],
    function (session, datacontext, servicesConfig, modelConfig, localCollections, patientsIndex) {

        var initialized = ko.observable(false);
        var subscriptionTokens = [];

        // Selected patient
        var selectedPatient = ko.computed(function () {
          return patientsIndex.selectedPatient();
        });

        // Calculated thirty days ago
        var thirtyDaysAgo = new Date(new Date().setDate(new Date().getDate() - 30));
        thirtyDaysAgo = moment(thirtyDaysAgo).format();

        // var todosListOpen = ko.observable(false);
        // var interventionsListOpen = ko.observable(false);

        var todosPanelOpen = ko.observable(true);
        var taskPanelOpen = ko.observable(true);
        var interventionsPanelOpen = ko.observable(true);
        var fullScreenWidget = ko.observable();

        var todoFiltersHeaderOpen = ko.observable(false);
        var interventionFiltersHeaderOpen = ko.observable(false);
        var taskFiltersHeaderOpen = ko.observable(false);

/// Todo region

        var catNotSet = {
            id: ko.observable(''),
            name: ko.observable('Not Set')
        };

        // Which view (filters) to apply
        var selectedTodoView = ko.observable();
        // Keep track of which index it is to keep it between patients
        var selectedTodoViewIndex = ko.observable();

        // Which todoViews (filters) are available
        var todoViews = ko.computed(function () {
            var selectedpatient = selectedPatient();
            var theseviews = [];
            if (selectedpatient && initialized()) {
                theseviews = [
                    new View('Closed To-Do List', [ new modelConfig.Parameter('patientId', selectedpatient.id(), '=='), new modelConfig.Parameter('statusId', '1', '!='), new modelConfig.Parameter('statusId', '3', '!=')], ['status-small','closedon','title','category','assignedto','priority-small'],'closedDate desc'),
                    new View('Open To-Do List', [ new modelConfig.Parameter('patientId', selectedpatient.id(), '=='), new modelConfig.Parameter('statusId', '2', '!='), new modelConfig.Parameter('statusId', '4', '!=')], ['priority','duedate','title','category', 'assignedto'])];

            } else {
            }
            return theseviews;
        }).extend({ throttle: 100 });

        // Clear the view if the selected patient changes
        var tvToken = todoViews.subscribe(function (newValue) {
            setTimeout(function () {
                if (selectedTodoViewIndex() !== null && typeof selectedTodoViewIndex() != "undefined") {
                    selectedTodoView(todoViews()[selectedTodoViewIndex()]);
                } else {
                    selectedTodoView(todoViews()[1]);
                }
            }, 200);
        });
        subscriptionTokens.push(tvToken);

        // Indicator of processing
        var todosSaving = ko.computed(datacontext.todosSaving);

        // Which categories are available to filter on
        var todoCategories = ko.computed(function () {
            var thesetodocats = datacontext.enums.toDoCategories();
            var theseCats = thesetodocats.slice(0, thesetodocats.length);
            // If there is not already a not set option,
            if (theseCats.indexOf(catNotSet) === -1) {
                // Add it also
                theseCats.push(catNotSet);
            }
            return theseCats;
        });

        // Which priorities are available to filter on
        var todoPriorities = datacontext.enums.priorities;

        // Filter down by categories
        var selectedTodoCategories = ko.observableArray();
        // Filter down by priorities
        var selectedTodoPriorities = ko.observableArray();
        // Column to override the default sort
        var selectedTodoSortColumn = ko.observable();
        // Filters showing or not?
        var todoFiltersOpen = ko.observable();

        var stvToken = selectedTodoView.subscribe(function (newValue) {
            // Whenever the selected view changes, clear all filters and sorting
            selectedTodoSortColumn(null);
            if (newValue) {
                selectedTodoViewIndex(todoViews().indexOf(selectedTodoView()));
            }
        });
        subscriptionTokens.push(stvToken);

        // If there is selected categories, we need to filter out all the others
        var activeTodoFilters = ko.computed(function () {
            var selectedcategories = selectedTodoCategories();
            var selectedpriorities = selectedTodoPriorities();
            var returnfilters = [];
            // Check if there are categories selected
            if (selectedcategories.length > 0) {
                // Grab all the other categories
                var othercats = ko.utils.arrayFilter(todoCategories(), function (cat) {
                    // Check if this category is a selected category
                    var matchingcat = ko.utils.arrayFirst(selectedcategories, function (selectCat) {
                        return selectCat.id() === cat.id();
                    });
                    // If there is a match found,
                    if (matchingcat) {
                        // Don't return this category
                        return false;
                    } else {
                        // If not do return it
                        return true;
                    }
                });
                ko.utils.arrayForEach(othercats, function (cat) {
                    returnfilters.push(new modelConfig.Parameter('categoryId', cat.id(), '!='));
                    returnfilters.push(new modelConfig.Parameter('categoryId', null, '!='));
                });
                // Not set is null, so only filter out nulls if that option isn't selected
                var foundselectednull = false;
                ko.utils.arrayForEach(selectedcategories, function (cat) {
                    if (cat.id() === '') {
                        foundselectednull = true;
                    }
                });
                // If we didn't select for nulls
                if (!foundselectednull) {
                    // Don't return nulls either
                    returnfilters.push(new modelConfig.Parameter('categoryId', '', '!='));
                    returnfilters.push(new modelConfig.Parameter('categoryId', null, '!='));
                }
            }
             // Check if there are priorities selected
            if (selectedpriorities.length > 0) {
                // Grab all the other priorities
                var othercats = ko.utils.arrayFilter(todoPriorities(), function (cat) {
                    // Check if this priority is a selected priority
                    var match = ko.utils.arrayFirst(selectedpriorities, function (selectCat) {
                        return selectCat.id() === cat.id();
                    });
                    // If there is a match found,
                    if (match) {
                        // Don't return this category
                        return false;
                    } else {
                        // If not do return it
                        return true;
                    }
                });
                ko.utils.arrayForEach(othercats, function (cat) {
                    returnfilters.push(new modelConfig.Parameter('priorityId', cat.id(), '!='));
                });
                // Not set is null, so only filter out nulls if that option isn't selected
                var foundselectednull = false;
                ko.utils.arrayForEach(selectedpriorities, function (priority) {
                    if (priority.id() === '0') {
                        foundselectednull = true;
                    }
                });
                // If we didn't select for nulls
                if (!foundselectednull) {
                    // Don't return nulls either
                    returnfilters.push(new modelConfig.Parameter('priorityId', '', '!='));   
                }
            }
            return returnfilters;
        });

        // Actively showing columns
        var activeTodoColumns = ko.computed(function () {
            var cols = [];
            if (selectedTodoView()) {
                cols = selectedTodoView().columns();
            }
            return cols;
        });

        // Title 
        var computedTodoTitle = ko.computed(function () {
            var thistitle = '';
            if (selectedTodoView()) {
                thistitle = selectedTodoView().name();
            }
            return thistitle;
        });

        var myToDos = ko.observableArray([]);
        var myToDosUpdater = ko.computed(function () {
            var theseTodos = [];
            //Subscribe to localcollection todos
            var allTodos = localCollections.todos();
            var selectedview = selectedTodoView();
            var params = [];
            var orderString = '';
            // Subscribe to this patient
            var dummySubscription = selectedPatient();
            if (selectedview) {
                // Subscribe to this patient's todos
                var dummySubscription = dummySubscription.todos();
                // Add these parameters to the query
                ko.utils.arrayForEach(selectedview.parameters, function (param) {
                    params.push(param);
                });
                // If there are filters,
                if (activeTodoFilters().length > 0) {
                    // Add them as parameters
                    ko.utils.arrayForEach(activeTodoFilters(), function (param) {
                        params.push(param);
                    });
                }
                // Either sort by the selected sort or the default
                orderString = selectedTodoSortColumn() ? selectedTodoSortColumn() : selectedview.primarySort;
                // Add the second and third orders to the string
                orderString = orderString + ', category.name, title';
                // Go get the todos
                theseTodos = datacontext.getToDosQuery(params, orderString);
                // Filter out the new todos
                theseTodos = ko.utils.arrayFilter(theseTodos, function (todo) {
                    return !todo.isNew() && !todo.deleteFlag();
                });
            }
			myToDos(theseTodos);
            return theseTodos;
        }).extend({ throttle: 50 });

        // Reset all the filters to default state
        function resetTodoFilters () {
            selectedTodoCategories([]);
            selectedTodoPriorities([]);
            todoFiltersOpen(false);
        }

        // Toggle the active sort
        function toggleTodoSort(sender) {
            // If the current column is the one to sort by
            if (selectedTodoSortColumn() && selectedTodoSortColumn().indexOf(sender.sortProperty) !== -1) {
                // If it ends in desc
                if (selectedTodoSortColumn() && selectedTodoSortColumn().substr(selectedTodoSortColumn().length - 4, 4) === 'desc' ) {
                    // Clear the sort column, as it should be undone
                    selectedTodoSortColumn(null);
                } else {
                    // Else set it as the sort column
                    selectedTodoSortColumn(sender.sortProperty + ' desc');
                }
            } else {
                // Else set it as the new sort column
                selectedTodoSortColumn(sender.sortProperty);
            }
        }

        // Force refresh todos from the server
        function refreshTodoView() {			
            // Get the patients' todos
            patientsIndex.getPatientsToDos();
        }

		/**
		*	clear all todos from localCollections and breeze cache.
		*	@method	clearTodosCacheAndLoad
		*/
		function clearTodosCacheAndLoad(){
			//assign empty array so todos wount be referenced from ko data binding of the views that had them showing.			
			myToDos([]);
			var todos = localCollections.todos();
			//empty the collection. the todos should be cleaned out by garbage collector.
			localCollections.todos([]);
			return setTimeout( function(){
				//short delay to allow the ko data binding to release references to these todos, before removing them: 				
				if( todos && todos.length > 0 ){					
					ko.utils.arrayForEach( todos, function(todo){						
						if( todo ){
							//remove from breeze cache:
							todo.entityAspect.setDeleted();
							todo.entityAspect.acceptChanges();							
						}						
					});
				}				
				patientsIndex.getPatientsToDos();
			}, 50);
		}
/// #Todo region





/// Intervention region

        // Which view (filters) to apply
        var selectedInterventionView = ko.observable();
        // Keep track of which index it is to keep it between patients
        var selectedInterventionViewIndex = ko.observable();

        // Which todoViews (filters) are available
        var interventionViews = ko.computed(function () {
            var selectedpatient = selectedPatient();
            var theseviews = [];
            if (selectedpatient && initialized()) {
                theseviews = [
                    new View('Closed Intervention List', [ new modelConfig.Parameter('goal.patientId', selectedpatient.id(), '=='), new modelConfig.Parameter('statusId', '1', '!=')], ['status-small','closeddate','description-small','category','goal','assignedto'], 'closedDate desc'),
                    new View('Open Intervention List', [ new modelConfig.Parameter('goal.patientId', selectedpatient.id(), '=='), new modelConfig.Parameter('statusId', '1', '==')], ['dueDate','description','category','goal','assignedto'],'dueDate desc')
                ]
            } else {
            }
            return theseviews;
        }).extend({ throttle: 10 });

        // Clear the view if the selected patient changes
        var ivToken = interventionViews.subscribe(function (newValue) {
            setTimeout(function () {
                if (selectedInterventionViewIndex()) {                 
                    selectedInterventionView(interventionViews()[selectedInterventionViewIndex()]);
                } else {
                    selectedInterventionView(interventionViews()[1]);
                }
            }, 200);
        });
        subscriptionTokens.push(ivToken);

        // Indicator of processing
        var interventionsSaving = ko.computed(datacontext.interventionsSaving);

        // Which categories are available to filter on
        var interventionCategories = ko.computed(function () {
            var thesetodocats = datacontext.enums.interventionCategories();
            var theseCats = thesetodocats.slice(0, thesetodocats.length);
            // If there is not already a not set option,
            if (theseCats.indexOf(catNotSet) === -1) {
                // Add it also
                theseCats.push(catNotSet);
            }
            return theseCats;
        });

        // Filter down by categories
        var selectedInterventionCategories = ko.observableArray();
        // Column to override the default sort
        var selectedInterventionSortColumn = ko.observable();
        // Filters showing or not?
        var interventionFiltersOpen = ko.observable();

        var sivToken = selectedInterventionView.subscribe(function (newValue) {
            // Whenever the selected view changes, clear all filters and sorting
            selectedInterventionSortColumn(null);
            if (newValue) {
                selectedInterventionViewIndex(interventionViews().indexOf(selectedInterventionView()));
            }
        });
        subscriptionTokens.push(sivToken);

        // If there is selected categories, we need to filter out all the others
        var activeInterventionFilters = ko.computed(function () {
            var selectedcategories = selectedInterventionCategories();
            var returnfilters = [];
            // Check if there are categories selected
            if (selectedcategories.length > 0) {
                // Grab all the other categories
                var othercats = ko.utils.arrayFilter(interventionCategories(), function (cat) {
                    // Check if this category is a selected category
                    var matchingcat = ko.utils.arrayFirst(selectedcategories, function (selectCat) {
                        return selectCat.id() === cat.id();
                    });
                    // If there is a match found,
                    if (matchingcat) {
                        // Don't return this category
                        return false;
                    } else {
                        // If not do return it
                        return true;
                    }
                });
                ko.utils.arrayForEach(othercats, function (cat) {
                    returnfilters.push(new modelConfig.Parameter('categoryId', cat.id(), '!='));
                });
                // Not set is null, so only filter out nulls if that option isn't selected
                var foundselectednull = false;
                ko.utils.arrayForEach(selectedcategories, function (cat) {
                    if (cat.id() === '') {
                        foundselectednull = true;
                    }
                });
                // If we didn't select for nulls
                if (!foundselectednull) {
                    // Don't return string empties or nulls either
                    returnfilters.push(new modelConfig.Parameter('categoryId', '', '!='));
                    returnfilters.push(new modelConfig.Parameter('categoryId', null, '!='));
                }
            }
            return returnfilters;
        });

        // Actively showing columns
        var activeInterventionColumns = ko.computed(function () {
            var cols = [];
            if (selectedInterventionView()) {
                cols = selectedInterventionView().columns();
            }
            return cols;
        });

        // Title 
        var computedInterventionTitle = ko.computed(function () {
            var thistitle = '';
            if (selectedInterventionView()) {
                thistitle = selectedInterventionView().name();
            }
            return thistitle;
        });

        // My todos
        var myInterventions = ko.computed(function () {
            var theseInterventions = [];
            //Subscribe to localcollection todos
            var allInterventions = localCollections.interventions();
            var selectedview = selectedInterventionView();
            var params = [];
            var orderString = '';
            // Subscribe to this patient
            var dummySubscription = selectedPatient();
            if (selectedview) {
                // Subscribe to this patient's todos
                var dummySubscription = dummySubscription.goals();
                // Add these parameters to the query
                ko.utils.arrayForEach(selectedview.parameters, function (param) {
                    params.push(param);
                });
                // If there are filters,
                if (activeInterventionFilters().length > 0) {
                    // Add them as parameters
                    ko.utils.arrayForEach(activeInterventionFilters(), function (param) {
                        params.push(param);
                    });
                }
                // Either sort by the selected sort or the default
                orderString = selectedInterventionSortColumn() ? selectedInterventionSortColumn() : selectedview.primarySort;
                // Add the second and third orders to the string
                orderString = orderString + ', category.name, description';
                // Go get the todos
                theseInterventions = datacontext.getInterventionsQuery(params, orderString);
                // Filter out the new todos
                // theseInterventions = ko.utils.arrayFilter(theseInterventions, function (intervention) {
                //     return !intervention.isNew() && !intervention.deleteFlag();
                // });
            }
            return theseInterventions;
        }).extend({ throttle: 50 });

        // Reset all the filters to default state
        function resetInterventionFilters () {
            selectedInterventionCategories([]);
            interventionFiltersOpen(false);
        }

        // Toggle the active sort
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

        // Force refresh todos from the server
        function refreshInterventionView() {
            // Get the patients' todos
            patientsIndex.getPatientsInterventions();
        }

/// #Intervention region





/// Task region

        // Which view (filters) to apply
        var selectedTaskView = ko.observable();
        // Keep track of which index it is to keep it between patients
        var selectedTaskViewIndex = ko.observable();

        // Which todoViews (filters) are available
        var taskViews = ko.computed(function () {
            var selectedpatient = selectedPatient();
            var theseviews = [];
            if (selectedpatient && initialized()) {
                theseviews = [
                    new View('Closed Task List', [ new modelConfig.Parameter('goal.patientId', selectedpatient.id(), '=='), new modelConfig.Parameter('statusId', '1', '!='), new modelConfig.Parameter('statusId', '3', '!=')], ['status-small','closeddate','targetdate','description-small','targetvalue-small','goal'],'closedDate desc'),
                    new View('Open Task List', [ new modelConfig.Parameter('goal.patientId', selectedpatient.id(), '=='), new modelConfig.Parameter('statusId', '2', '!='), new modelConfig.Parameter('statusId', '4', '!=')], ['startdate','targetdate','description-small','targetvalue','goal'],'targetDate')
                ];
            } else {

            }
            return theseviews;
        }).extend({ throttle: 10 });

        // Clear the view if the selected patient changes
        var tdvToken = taskViews.subscribe(function (newValue) {
            setTimeout(function () {
                if (selectedTaskViewIndex()) {                 
                    selectedTaskView(taskViews()[selectedTaskViewIndex()]);
                } else {
                    selectedTaskView(taskViews()[1]);
                }
            }, 200);
        });
        subscriptionTokens.push(tdvToken);

        // Indicator of processing
        var tasksSaving = ko.computed(datacontext.tasksSaving);

        // Column to override the default sort
        var selectedTaskSortColumn = ko.observable();
        // Filters showing or not?
        var taskFiltersOpen = ko.observable();

        var stvToken = selectedTaskView.subscribe(function (newValue) {
            // Whenever the selected view changes, clear all filters and sorting
            selectedTaskSortColumn(null);
            if (newValue) {
                selectedTaskViewIndex(taskViews().indexOf(selectedTaskView()));
            }
        });
        subscriptionTokens.push(stvToken);

        // If there is selected categories, we need to filter out all the others
        var activeTaskFilters = ko.computed(function () {
            var returnfilters = [];
            // Check if there are categories selected
            return returnfilters;
        });

        // Actively showing columns
        var activeTaskColumns = ko.computed(function () {
            var cols = [];
            if (selectedTaskView()) {
                cols = selectedTaskView().columns();
            }
            return cols;
        });

        // Title 
        var computedTaskTitle = ko.computed(function () {
            var thistitle = '';
            if (selectedTaskView()) {
                thistitle = selectedTaskView().name();
            }
            return thistitle;
        });

        // My todos
        var myTasks = ko.computed(function () {
            var theseTasks = [];
            //Subscribe to localcollection todos
            var allTasks = localCollections.tasks();
            var selectedview = selectedTaskView();
            var params = [];
            var orderString = '';
            // Subscribe to this patient
            var dummySubscription = selectedPatient();
            if (selectedview) {
                // Subscribe to this patient's todos
                var dummySubscription = dummySubscription.goals();
                // Add these parameters to the query
                ko.utils.arrayForEach(selectedview.parameters, function (param) {
                    params.push(param);
                });
                // If there are filters,
                if (activeTaskFilters().length > 0) {
                    // Add them as parameters
                    ko.utils.arrayForEach(activeTaskFilters(), function (param) {
                        params.push(param);
                    });
                }
                // Either sort by the selected sort or the default
                orderString = selectedTaskSortColumn() ? selectedTaskSortColumn() : selectedview.primarySort;
                // Add the second and third orders to the string
                orderString = orderString + ', status.name, description';
                // Go get the todos
                theseTasks = datacontext.getTasksQuery(params, orderString);
            }
            return theseTasks;
        }).extend({ throttle: 50 });

        // Reset all the filters to default state
        function resetTaskFilters () {
            taskFiltersOpen(false);
        }

        // Toggle the active sort
        function toggleTaskSort(sender) {
            // If the current column is the one to sort by
            if (selectedTaskSortColumn() && selectedTaskSortColumn().indexOf(sender.sortProperty) !== -1) {
                // If it ends in desc
                if (selectedTaskSortColumn() && selectedTaskSortColumn().substr(selectedTaskSortColumn().length - 4, 4) === 'desc' ) {
                    // Clear the sort column, as it should be undone
                    selectedTaskSortColumn(null);
                } else {
                    // Else set it as the sort column
                    selectedTaskSortColumn(sender.sortProperty + ' desc');
                }
            } else {
                // Else set it as the new sort column
                selectedTaskSortColumn(sender.sortProperty);
            }
        }

        // Force refresh todos from the server
        function refreshTaskView() {
            // Get the patients' todos
            patientsIndex.getPatientsTasks();
        }

/// #Task region


        // Reveal the bindable properties and functions
        var vm = {
            activate: activate,
            title: 'Overview',
            selectedPatient: selectedPatient,
            toggleWidgetOpen: toggleWidgetOpen,
            todoFiltersHeaderOpen: todoFiltersHeaderOpen,
            interventionFiltersHeaderOpen: interventionFiltersHeaderOpen,
            taskFiltersHeaderOpen: taskFiltersHeaderOpen,
            fullScreenWidget: fullScreenWidget,
            toggleFullScreen: toggleFullScreen,
            toggleHeaderOpen: toggleHeaderOpen,

            todosPanelOpen: todosPanelOpen,
            taskPanelOpen: taskPanelOpen,
            interventionsPanelOpen: interventionsPanelOpen,

            myToDos: myToDos,
            activeTodoColumns: activeTodoColumns,
            todosSaving: todosSaving,
            todoFiltersOpen: todoFiltersOpen,
            resetTodoFilters: resetTodoFilters,
            refreshTodoView: refreshTodoView,
            todoViews: todoViews,
            computedTodoTitle: computedTodoTitle,
            todoPriorities: todoPriorities,
            todoCategories: todoCategories,
            selectedTodoPriorities: selectedTodoPriorities,
            selectedTodoCategories: selectedTodoCategories,
            selectedTodoView: selectedTodoView,
            selectedTodoSortColumn: selectedTodoSortColumn,
            toggleTodoSort: toggleTodoSort,


            myInterventions: myInterventions,
            activeInterventionColumns: activeInterventionColumns,
            interventionsSaving: interventionsSaving,
            interventionFiltersOpen: interventionFiltersOpen,
            resetInterventionFilters: resetInterventionFilters,
            refreshInterventionView: refreshInterventionView,
            interventionViews: interventionViews,
            computedInterventionTitle: computedInterventionTitle,
            interventionCategories: interventionCategories,
            selectedInterventionCategories: selectedInterventionCategories,
            selectedInterventionView: selectedInterventionView,
            selectedInterventionSortColumn: selectedInterventionSortColumn,
            toggleInterventionSort: toggleInterventionSort,


            myTasks: myTasks,
            activeTaskColumns: activeTaskColumns,
            tasksSaving: tasksSaving,
            taskFiltersOpen: taskFiltersOpen,
            resetTaskFilters: resetTaskFilters,
            refreshTaskView: refreshTaskView,
            taskViews: taskViews,
            computedTaskTitle: computedTaskTitle,
            selectedTaskView: selectedTaskView,
            selectedTaskSortColumn: selectedTaskSortColumn,
            toggleTaskSort: toggleTaskSort,

            detached: detached
        };

        return vm;

        function activate() {
            if (!initialized()) {
                initializeViewModel();
                initialized(true);
            }
			if( selectedPatient() ){
				clearTodosCacheAndLoad();
			}
            return true;
        }

        function detached() {
			// remarked ! disposing computeds on this module causes many unpredictable problems.
            // console.log('patients/index detached.');
            // myToDosUpdater.dispose();
            // selectedPatient.dispose();
            // todoViews.dispose();
            // todosSaving.dispose();
            // todoCategories.dispose();
            // activeTodoFilters.dispose();
            // activeTodoColumns.dispose();
            // computedTodoTitle.dispose();
            // myToDos.dispose();
            // interventionViews.dispose();
            // interventionsSaving.dispose();
            // interventionCategories.dispose();
            // activeInterventionFilters.dispose();
            // activeInterventionColumns.dispose();
            // computedInterventionTitle.dispose();
            // myInterventions.dispose();
            // taskViews.dispose();
            // tasksSaving.dispose();
            // activeTaskFilters.dispose();
            // activeTaskColumns.dispose();
            // computedTaskTitle.dispose();
            // myTasks.dispose();
            // ko.utils.arrayForEach(subscriptionTokens, function (token) {
                // token.dispose();
            // });
        }

        function toggleHeaderOpen (sender, widgetOpen) {
            if (widgetOpen()) {
                sender(!sender());    
            }
        }

        // Toggle whether the widget is open or not
        function toggleWidgetOpen(sender) {
            if (sender === todosPanelOpen && (taskPanelOpen() || interventionsPanelOpen())) {
                todosPanelOpen(!todosPanelOpen());
                if (!todosPanelOpen()) {
                    todoFiltersHeaderOpen(false);
                }
            } else if (sender === interventionsPanelOpen && (taskPanelOpen() || todosPanelOpen())) {
                interventionsPanelOpen(!interventionsPanelOpen());
                if (!interventionsPanelOpen()) {
                    interventionFiltersHeaderOpen(false);
                }
            } else if (sender === taskPanelOpen && (interventionsPanelOpen() || todosPanelOpen())) {
                taskPanelOpen(!taskPanelOpen());
                if (!taskPanelOpen()) {
                    taskFiltersHeaderOpen(false);
                }
            }
        }

        // Toggle the widget to / from fullscreen
        function toggleFullScreen(widgetname) {
            // If this widget is already fullscreen,
            if (widgetname === fullScreenWidget()) {
                // Remove full screen widget
                fullScreenWidget(null);
            } else {
                // Else, set it as the full screen widget
                fullScreenWidget(widgetname);
            }
        }

        function initializeViewModel() {
            //refreshView();
            //selectedView(views()[0]);
            //initialized(true);
            return true;

            function todosReturned() {
            }
        }

        // A view to select
        function View(name, params, cols, prisort) {
            var self = this;
            self.name = ko.observable(name);
            self.parameters = params;
            self.columns = ko.observableArray(cols);
            self.primarySort = prisort ? prisort : 'dueDate desc';
        }

    });