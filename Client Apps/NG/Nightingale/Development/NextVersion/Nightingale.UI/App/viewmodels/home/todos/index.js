//TODO: Inject dependencies
define(['services/session', 'services/datacontext', 'config.services', 'models/base', 'services/local.collections', 'viewmodels/home/myhome/myhome'],
    function (session, datacontext, servicesConfig, modelConfig, localCollections, myHomeIndex) {

        var initialized = false;

        var catNotSet = {
            id: ko.observable(''),
            name: ko.observable('Not Set'),
        }

        var filtersHeaderOpen = ko.observable(true);

        // Pointer to add todo in myhome
        var addToDo = myHomeIndex.addToDo;

        // Calculated thirty days ago
        var thirtyDaysAgo = new Date(new Date().setDate(new Date().getDate()-30));
        thirtyDaysAgo = moment(thirtyDaysAgo).format();

        // Which views (filters) are available
        var views = ko.observableArray([
            new View(
                'interventions',
                'Intervention: Assigned to Others',
                [
                    new modelConfig.Parameter('createdById', session.currentUser().userId(), '=='),
                    new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '!='),
                    new modelConfig.Parameter('statusId', '1', '==')
                ],
                ['startdate','description','category','patient','goal'],
                'startDate'
            ),
            new View(
                'interventions',
                'Intervention: Closed by Others',
                [
                    new modelConfig.Parameter('createdById', session.currentUser().userId(), '=='),
                    new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '!='),
                    new modelConfig.Parameter('statusId', '1', '!=')
                ],
                ['status-small','closeddate','description-small','category','patient','goal'],
                'closedDate desc'
            ),
            new View(
                'interventions',
                'Intervention: My Closed List',
                [
                    new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '=='),
                    new modelConfig.Parameter('statusId', '1', '!=')
                ],
                ['status-small','closeddate','description-small','category','patient','goal'],
                'closedDate desc'
            ),
            new View(
                'interventions',
                'Intervention: My Open List',
                [
                    new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '=='),
                    new modelConfig.Parameter('statusId', '1', '==')
                ],
                ['startdate','description','category','patient','goal'],
                'startDate'
            ),
            new View(
                'todos',
                'To Do: Assigned to Others',
                [
                    new modelConfig.Parameter('createdById', session.currentUser().userId(), '=='),
                    new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '!='),	
					new modelConfig.Parameter('assignedToId', '', '!='),	//filter out the unassigned they have their own filter.
                    new modelConfig.Parameter('statusId', '2', '!='),
                    new modelConfig.Parameter('statusId', '4', '!=')
                ],
                ['priority-small','duedate','title-small','category','patient','assignedto']
            ),
            new View(
                'todos',
                'To Do: Closed by Others',
                [
                    new modelConfig.Parameter('createdById', session.currentUser().userId(), '=='),
                    new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '!='),
					new modelConfig.Parameter('assignedToId', '', '!='),	//filter out the unassigned they have their own filter.
                    new modelConfig.Parameter('statusId', '1', '!='),
                    new modelConfig.Parameter('statusId', '3', '!='),
                    new modelConfig.Parameter('closedDate', new Date(thirtyDaysAgo), '>=')
                ],
                ['status-small','closedon','title-small','category','patient','assignedto'],
                'closedDate desc'
            ),
            new View(
                'todos',
                'To Do: My Closed List',
                [
                    new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '=='),
                    new modelConfig.Parameter('statusId', '1', '!='),
                    new modelConfig.Parameter('statusId', '3', '!='),
                    new modelConfig.Parameter('closedDate', new Date(thirtyDaysAgo), '>=')
                ],
                ['status-small','closedon','title-small','category','patient','assignedto'],
                'closedDate desc'
            ),
            new View(
                'todos',
                'To Do: My Open List', 
                [
                    new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '=='),
                    new modelConfig.Parameter('statusId', '2', '!='),
                    new modelConfig.Parameter('statusId', '4', '!=')
                ],
                ['priority','duedate','title','category','patient']
            ), 
			 new View(
                'todos',
                'To Do: Open Unassigned', 
                [
                    new modelConfig.Parameter('assignedToId', '', '=='),
                    new modelConfig.Parameter('statusId', '2', '!='),
                    new modelConfig.Parameter('statusId', '4', '!=')
                ],
                ['priority','duedate','title','category','patient']				
            ),			 
			 new View(
                'todos',
                'To Do: Closed Unassigned', 
                [
                    new modelConfig.Parameter('assignedToId', '', '=='),
                    new modelConfig.Parameter('statusId', '1', '!='),
                    new modelConfig.Parameter('statusId', '3', '!=')
                ],
                ['priority','duedate','title','category','patient']
            )
        ]);

        // Which widget is fullscreen, which is left column
        var fullScreenWidget = ko.observable();
        var leftColumnOpen = ko.observable('todopanel');

        // Summaries to choose from for pie chart
        var availableTodoSummaries = [
            new Summary('By Priority', 'priority', 'levelName'),
            new Summary('By Category', 'category', 'name'),
            new Summary('By Status', 'status', 'name')
        ];
        // Which summary is selected
        var selectedTodoSummary = ko.observable(availableTodoSummaries[1]);

        // Summaries to choose from for pie chart
        var availableInterventionSummaries = [
            new Summary('By Category', 'category', 'name'),
            new Summary('By Status', 'status', 'name')
        ];
        // Which summary is selected
        var selectedInterventionSummary = ko.observable(availableInterventionSummaries[0]);

        // Indicator of processing
        var todosSaving = ko.computed(datacontext.todosSaving);

        // Which priorities are available to filter on
        var priorities = datacontext.enums.priorities;

        // Which view (filters) to apply
        var selectedView = ko.observable();
        // Which view was last used
        var lastSelectedView = ko.observable();
        // Filter down by categories
        var selectedCategories = ko.observableArray();
        // Filter down by priorities
        var selectedPriorities = ko.observableArray();
        // Column to override the default sort
        var selectedSortColumn = ko.observable();
        // Filters showing or not?
        var filtersOpen = ko.observable();

        // Which categories are available to filter on
        var categories = ko.computed(function () {
            var thesetodocats;
            // If there is a selected view and it is of type todos
            if (selectedView() && selectedView().type() === 'todos') {
                // Use the todo categories
                thesetodocats = datacontext.enums.toDoCategories();
            } else {
                // Else use the intervention categories
                thesetodocats = datacontext.enums.interventionCategories();
            }
            // Make a copy of the categories list
            var theseCats = thesetodocats.slice(0, thesetodocats.length);
            // If there is not already a not set option,
            if (theseCats.indexOf(catNotSet) === -1) {
                // Add it also
                theseCats.push(catNotSet);
            }
            return theseCats;
        });

        selectedView.subscribe(function (newValue) {
            // Whenever the selected view changes, clear all filters and sorting
            selectedSortColumn(null);
            if (lastSelectedView() && lastSelectedView().type() !== selectedView().type()) {
                // and if so clear secondary filters
                selectedCategories([]);
                selectedPriorities([]);
            }
            lastSelectedView(newValue);
        });

        // If there is selected categories, we need to filter out all the others
        var activeFilters = ko.computed(function () {
            var selectedcategories = selectedCategories();
            var selectedpriorities = selectedPriorities();
            var returnfilters = [];
            // Check if there are categories selected
            if (selectedcategories.length > 0) {
                // Grab all the other categories
                var othercats = ko.utils.arrayFilter(categories(), function (cat) {
                    // Check if this category is a selected category
                    var match = ko.utils.arrayFirst(selectedcategories, function (selectCat) {
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
                    // Don't return nulls either
                    returnfilters.push(new modelConfig.Parameter('categoryId', '', '!='));
                    returnfilters.push(new modelConfig.Parameter('categoryId', null, '!='));
                }
            }
             // Check if there are priorities selected
            if (selectedpriorities.length > 0) {
                // Grab all the other priorities
                var othercats = ko.utils.arrayFilter(priorities(), function (cat) {
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
                    returnfilters.push(new modelConfig.Parameter('priorityId', null, '!='));
                }
            }
            return returnfilters;
        });

        // Actively showing columns
        var activeColumns = ko.computed(function () {
            var cols = [];
            if (selectedView()) {
                cols = selectedView().columns();
            }
            return cols;
        });

        // My todos
        var myToDos = ko.computed(function () {
            var theseTodos = [];
            //Subscribe to localcollection todos
            var allTodos = localCollections.todos();
            var selectedview = selectedView();
            var params = [];
            var orderString = '';
            if (selectedview && selectedview.type() === 'todos') {
                // Add these parameters to the query
                ko.utils.arrayForEach(selectedview.parameters, function (param) {
                    params.push(param);
                });
                // If there are filters,
                if (activeFilters().length > 0) {
                    // Add them as parameters
                    ko.utils.arrayForEach(activeFilters(), function (param) {
                        params.push(param);
                    });
                }
                // Either sort by the selected sort or the default
                orderString = selectedSortColumn() ? selectedSortColumn() : selectedview.primarySort;
                // Add the second and third orders to the string
                orderString = orderString + ', category.name, title';
                // Go get the todos
                theseTodos = datacontext.getToDosQuery(params, orderString);
                // Filter out the new todos
                theseTodos = ko.utils.arrayFilter(theseTodos, function (todo) {
                    return !todo.isNew();
                });
            }
            return theseTodos;
        }).extend({ throttle: 500 });

        // My interventions
        var myInterventions = ko.computed(function () {
            var theseInterventions = [];
            //Subscribe to localcollection todos
            var allInterventions = localCollections.interventions();
            var selectedview = selectedView();
            var params = [];
            var orderString = '';
            if (selectedview && selectedview.type() === 'interventions') {
                // Add these parameters to the query
                ko.utils.arrayForEach(selectedview.parameters, function (param) {
                    params.push(param);
                });
                // If there are filters,
                if (activeFilters().length > 0) {
                    // Add them as parameters
                    ko.utils.arrayForEach(activeFilters(), function (param) {
                        params.push(param);
                    });
                }
                // Either sort by the selected sort or the default
                orderString = selectedSortColumn() ? selectedSortColumn() : selectedview.primarySort;
                // Add the second and third orders to the string
                var finalOrderString = orderString + ', category.name, description';
                // Go get the todos
                theseInterventions = datacontext.getInterventionsQuery(params, orderString);
            }
            return theseInterventions;
        });

        var myToDosChart = ko.computed(function () {
            var subscribers = myToDos();
            var data = [];
            var finaldata = [];
            var total = 0;
            data.push(new ChartToDo('Not set'));
            ko.utils.arrayForEach(myToDos(), function (todo) {
                var match = ko.utils.arrayFirst(data, function (dt) {
                    if (todo[selectedTodoSummary().mainProperty]()) {
                        return dt.name === todo[selectedTodoSummary().mainProperty]()[selectedTodoSummary().secondaryProperty]();
                    } else {
                        return dt.name === 'Not set';
                    }
                });
                if (match) {
                    match.count += 1;
                } else {
                    var newChart = new ChartToDo(todo[selectedTodoSummary().mainProperty]()[selectedTodoSummary().secondaryProperty]());
                    newChart.count = 1;
                    data.push(newChart);

                }
            });
            // Get total
            ko.utils.arrayForEach(data, function (dt) {
                total += dt.count;
            });
            // Make percentages
            ko.utils.arrayForEach(data, function (dt) {
                // If the count is greater than zero,
                if (dt.count) {
                    // Calculate the percentage and push it in to the graph
                    dt.percentage = dt.count / total;
                    finaldata.push([dt.name, dt.percentage]);                    
                }
            });
            return finaldata;
        });

        var myInterventionsChart = ko.computed(function () {
            var subscribers = myInterventions();
            var data = [];
            var finaldata = [];
            var total = 0;
            data.push(new ChartToDo('Not set'));
            ko.utils.arrayForEach(myInterventions(), function (intervention) {
                var match = ko.utils.arrayFirst(data, function (dt) {
                    if (intervention[selectedInterventionSummary().mainProperty]()) {
                        return dt.name === intervention[selectedInterventionSummary().mainProperty]()[selectedInterventionSummary().secondaryProperty]();
                    } else {
                        return dt.name === 'Not set';
                    }
                });
                if (match) {
                    match.count += 1;
                } else {
                    var newChart = new ChartToDo(intervention[selectedInterventionSummary().mainProperty]()[selectedInterventionSummary().secondaryProperty]());
                    newChart.count = 1;
                    data.push(newChart);

                }
            });
            // Get total
            ko.utils.arrayForEach(data, function (dt) {
                total += dt.count;
            });
            // Make percentages
            ko.utils.arrayForEach(data, function (dt) {
                // If the count is greater than zero,
                if (dt.count) {
                    // Calculate the percentage and push it in to the graph
                    dt.percentage = dt.count / total;
                    finaldata.push([dt.name, dt.percentage]);                    
                }
            });
            return finaldata;
        });

        function ChartToDo (name) {
            var self = this;
            self.name = name;
            self.percentage = 0;
            self.count = 0;
        }

        // The end point to use when getting cohorts
        var myToDosEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'ToDo');
        });

        // Reveal the bindable properties and functions
        var vm = {
            activate: activate,
            title: 'Home',
            todosSaving: todosSaving,
            myToDos: myToDos,
            myInterventions: myInterventions,
            myToDosChart: myToDosChart,
            myInterventionsChart: myInterventionsChart,
            availableTodoSummaries: availableTodoSummaries,
            availableInterventionSummaries: availableInterventionSummaries,
            selectedTodoSummary: selectedTodoSummary,
            selectedInterventionSummary: selectedInterventionSummary,
            filtersOpen: filtersOpen,
            addToDo: addToDo,
            refreshView: refreshView,
            views: views,
            activeColumns: activeColumns,
            priorities: priorities,
            categories: categories,
            selectedPriorities: selectedPriorities,
            selectedCategories: selectedCategories,
            selectedView: selectedView,
            selectedSortColumn: selectedSortColumn,
            resetFilters: resetFilters,
            toggleSort: toggleSort,
            fullScreenWidget: fullScreenWidget,
            leftColumnOpen: leftColumnOpen,
            toggleFullScreen: toggleFullScreen,
            toggleOpenColumn: toggleOpenColumn,
            filtersHeaderOpen: filtersHeaderOpen
        };

        return vm;

        function activate() {
            if (!initialized) {
                initializeViewModel();
                initialized = true;
            }
            return true;
        }

        function initializeViewModel() {
            refreshView();
            selectedView(views()[7]);
            // Go get a list of my todos
            // Don't think we need to do this unless we don't have all of the todos yet
            // datacontext.getEntityList(myToDos, myToDosEndPoint().EntityType, myToDosEndPoint().ResourcePath, null, null, false).then(todosReturned);
            initialized = true;
            return true;

            function todosReturned() {
                
            }
        };

        // Toggle which column is open
        function toggleOpenColumn() {
            leftColumnOpen(!leftColumnOpen());
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

        // Reset all the filters to default state
        function resetFilters () {
            selectedCategories([]);
            selectedPriorities([]);
            filtersOpen(false);
        }

        // Toggle the active sort
        function toggleSort(sender) {
            // If the current column is the one to sort by
            if (selectedSortColumn() && selectedSortColumn().indexOf(sender.sortProperty) !== -1) {
                // If it ends in desc
                if (selectedSortColumn() && selectedSortColumn().substr(selectedSortColumn().length - 4, 4) === 'desc' ) {
                    // Clear the sort column, as it should be undone
                    selectedSortColumn(null);
                } else {
                    // Else set it as the sort column
                    selectedSortColumn(sender.sortProperty + ' desc');
                }
            } else {
                // Else set it as the new sort column
                selectedSortColumn(sender.sortProperty);
            }
        }

        // Force refresh todos from the server
        function refreshView() {
            // Go refresh all possible todos
            datacontext.getToDos(null, { StatusIds: [1,3], AssignedToId: session.currentUser().userId() });
            // Go refresh all possible todos
            datacontext.getToDos(null, { StatusIds: [1,3], CreatedById: session.currentUser().userId() });
            // Go refresh all possible todos
            datacontext.getToDos(null, { StatusIds: [2,4], AssignedToId: session.currentUser().userId(), FromDate: thirtyDaysAgo });
            datacontext.getToDos(null, { StatusIds: [2,4], CreatedById: session.currentUser().userId(), FromDate: thirtyDaysAgo });
			// all open && unassigned, regardless of created by.
			datacontext.getToDos(null, { StatusIds: [1,3], AssignedToId: '-1'});
			// all closed && unassigned, in the last 30 days, regardless of created by.
			datacontext.getToDos(null, { StatusIds: [2,4], AssignedToId: '-1', FromDate: thirtyDaysAgo});
			
            datacontext.getInterventions(null, { StatusIds: [1], AssignedToId: session.currentUser().userId() });
            datacontext.getInterventions(null, { StatusIds: [1], CreatedById: session.currentUser().userId() });
            datacontext.getInterventions(null, { StatusIds: [2,3], AssignedToId: session.currentUser().userId() });
            datacontext.getInterventions(null, { StatusIds: [2,3], CreatedById: session.currentUser().userId() });
        }

        // A view to select
        function View(type, name, params, cols, prisort) {
            var self = this;
            self.type = ko.observable(type);
            self.name = ko.observable(name);
            self.parameters = params;
            self.columns = ko.observableArray(cols);
            self.primarySort = prisort ? prisort : 'dueDate desc';
        }
        
        // Summary object
        function Summary(name, mainprop, secprop) {
            var self = this;
            self.name = name;
            self.mainProperty = mainprop;
            self.secondaryProperty = secprop;
        }

    });