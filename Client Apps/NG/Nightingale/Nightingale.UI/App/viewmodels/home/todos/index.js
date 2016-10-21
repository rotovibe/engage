//TODO: Inject dependencies
define(['services/session', 'services/datacontext', 'config.services', 'models/base', 'services/local.collections', 'viewmodels/home/myhome/myhome'],
    function (session, datacontext, servicesConfig, modelConfig, localCollections, myHomeIndex) {

        var initialized = false;
		var subscriptionTokens = [];
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
			
		//paging (skip take logic)
		var maxToToDosLoaded = ko.observable(false);
		var maxTodosCount = ko.observable(400); // max todos to load per view / query.
		var todosTake = ko.observable(maxTodosCount()); //set the take for any server query ( one and first page design, without load more )
        var todosTotalCount = ko.observable(0);
		var todosProcessing = ko.observable(false);
        //var interventionsProcessing = ko.observable(false);
		var viewChanged = ko.observable(false);
		var categoryChanged = ko.observable(false);
		var priorityChanged = ko.observable(false);
        // Which views (filters) are available
        var views = ko.observableArray([
            new View(	//0
                'interventions',
                'Intervention: Assigned to Others',
                [
                    new modelConfig.Parameter('createdById', session.currentUser().userId(), '=='),
                    new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '!='),
                    new modelConfig.Parameter('statusId', '1', '==')
                ],
                ['duedate','description','category','patient','goal'],
                'dueDate desc, startDate desc',
                null,
                null,
                1
            ),
            new View(	//1
                'interventions',
                'Intervention: Closed by Others',
                [
                    new modelConfig.Parameter('createdById', session.currentUser().userId(), '=='),
                    new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '!='),
                    new modelConfig.Parameter('statusId', '1', '!=')
                ],
                ['status-small','closeddate','description-small','category','patient','goal'],
                'closedDate desc',
                null,
                null,
                2
            ),
            new View(	//2
                'interventions',
                'Intervention: My Closed List',
                [
                    new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '=='),
                    new modelConfig.Parameter('statusId', '1', '!=')
                ],
                ['status-small','closeddate','description-small','category','patient','goal'],                 
                'closedDate desc',
                null,
                null,
                3               
            ),
            new View(	//3
                'interventions',
                'Intervention: My Open List',
                [
                    new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '=='),
                    new modelConfig.Parameter('statusId', '1', '==')
                ],
                ['duedate','description','category','patient','goal'],
                'dueDate desc, startDate desc',
                null,
                null,
                4
            ),
            new View(	//4
                'todos',
                'To Do: Assigned to Others',
                [
                    new modelConfig.Parameter('createdById', session.currentUser().userId(), '=='),
                    new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '!='),	
					new modelConfig.Parameter('assignedToId', '', '!='),	//filter out the unassigned they have their own filter.
                    new modelConfig.Parameter('statusId', '2', '!='),
                    new modelConfig.Parameter('statusId', '4', '!=')
                ],
                ['priority-small','duedate','title-small','category','patient','assignedto'],
				'dueDate desc',
				[
					new modelConfig.Parameter('StatusIds', [1,3]),
					new modelConfig.Parameter('CreatedById', session.currentUser().userId()),
					new modelConfig.Parameter('NotAssignedToId', session.currentUser().userId())
				],
				'-DueDate'
            ),
            new View(	//5
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
                'closedDate desc',
				[
					new modelConfig.Parameter('StatusIds', [2,4]),
					new modelConfig.Parameter('CreatedById', session.currentUser().userId()),
					new modelConfig.Parameter('FromDate', thirtyDaysAgo),
					new modelConfig.Parameter('NotAssignedToId', session.currentUser().userId())
				],
				'-ClosedDate'
            ),
            new View(	//6
                'todos',
                'To Do: My Closed List',
                [
                    new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '=='),
                    new modelConfig.Parameter('statusId', '1', '!='),
                    new modelConfig.Parameter('statusId', '3', '!='),
                    new modelConfig.Parameter('closedDate', new Date(thirtyDaysAgo), '>=')
                ],
                ['status-small','closedon','title-small','category','patient','assignedto'],
                'closedDate desc',
				[
					new modelConfig.Parameter('StatusIds', [2,4]),
					new modelConfig.Parameter('AssignedToId', session.currentUser().userId()),
					new modelConfig.Parameter('FromDate', thirtyDaysAgo)
				],
				'-ClosedDate'
            ),
            new View(	//7
                'todos',
                'To Do: My Open List', 
                [
                    new modelConfig.Parameter('assignedToId', session.currentUser().userId(), '=='),
                    new modelConfig.Parameter('statusId', '2', '!='),
                    new modelConfig.Parameter('statusId', '4', '!=')
                ],
                ['priority','duedate','title','category','patient'],
				'dueDate desc',
				[ 	
					new modelConfig.Parameter('StatusIds', [1,3]), 
					new modelConfig.Parameter('AssignedToId', session.currentUser().userId())
				],
				'-DueDate'
            ), 
			 new View(	//8
                'todos',
                'To Do: Open Unassigned', 
                [
                    new modelConfig.Parameter('assignedToId', '', '=='),
                    new modelConfig.Parameter('statusId', '2', '!='),
                    new modelConfig.Parameter('statusId', '4', '!=')
                ],
                ['priority','duedate','title','category','patient'],
				'dueDate desc',
				[
					new modelConfig.Parameter('StatusIds', [1,3]),
					new modelConfig.Parameter('AssignedToId', '-1')
				],
				'-DueDate'
            ),			 
			 new View(	//9
                'todos',
                'To Do: Closed Unassigned', 
                [
                    new modelConfig.Parameter('assignedToId', '', '=='),
                    new modelConfig.Parameter('statusId', '1', '!='),
                    new modelConfig.Parameter('statusId', '3', '!=')
                ],
                ['priority','duedate','title','category','patient'],
				'dueDate desc',
				[
					new modelConfig.Parameter('StatusIds', [2,4]),
					new modelConfig.Parameter('AssignedToId', '-1'),
					new modelConfig.Parameter('FromDate', thirtyDaysAgo)
				],
				'-DueDate'
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
		var lastSelectedCategories = ko.observableArray([]);
		var lastSelectedPriorities = ko.observableArray([]);
        // Filter down by priorities
        var selectedPriorities = ko.observableArray();
        // Column to override the default sort
        var selectedSortColumn = ko.observable();
		var backendSort = ko.observable('-DueDate'); //default todo sort
		var lastSort = ko.observable( backendSort() );
		
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

        var selectedViewToken = selectedView.subscribe(function (newValue) {
            // Whenever the selected view changes, clear all filters and sorting
            selectedSortColumn(null);
			backendSort(null);
            if (lastSelectedView() && lastSelectedView().type() !== selectedView().type()) {
                // and if so clear secondary filters
                selectedCategories([]);
                selectedPriorities([]);				
            }
			if(lastSelectedView() && lastSelectedView().name() !== selectedView().name()){
				viewChanged(true);
			}
			lastSelectedView(newValue);
        });
		subscriptionTokens.push( selectedViewToken );
		
        // If there is selected categories, we need to filter out all the others
        var activeFilters = ko.computed(function () {
            var selectedcategories = selectedCategories();
			var selectedpriorities = selectedPriorities();
			var categoryFilterChanged = isFilterSelectionChanged( selectedCategories(), lastSelectedCategories() );            
			var priorityFilterChanged = isFilterSelectionChanged( selectedPriorities(), lastSelectedPriorities() );
			if( categoryFilterChanged ){
				lastSelectedCategories([]);
				ko.utils.arrayForEach( selectedCategories(), function( item ){
					lastSelectedCategories.push( item );
				});
				categoryChanged(true);
			}
			if( priorityFilterChanged ){
				lastSelectedPriorities([]);
				ko.utils.arrayForEach( selectedPriorities(), function( item ){
					lastSelectedPriorities.push( item );
				});
				priorityChanged(true);
			}
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
        }).extend({ throttle: 50 });

        // Actively showing columns
        var activeColumns = ko.computed(function () {
            var cols = [];
            if (selectedView()) {
                cols = selectedView().columns();
            }
            return cols;
        });
		
		function manageTodoPaging( localCount ){
			var totalCount = todosTotalCount();
			if( localCount < maxTodosCount() && localCount < totalCount ){
				maxToToDosLoaded(false);
			}
			else{
				if( localCount >= maxTodosCount() ){
					maxToToDosLoaded(true);
				}
			}			
		}
		
		var todosReloading = ko.computed( function(){
			var todosSaving = todosSaving ? todosSaving(): false;
			var processing = todosProcessing ? todosProcessing() : false;			
			var totalCount = todosTotalCount();
			var todos = myToDos? myToDos(): null;			
			return (todosSaving || processing 
					|| (totalCount > 0 && todos.length == 0));	//reloading has finished, but ko is processing the array and we need to wait a bit
		}).extend({ throttle: 50 });
		
		function getLocalTodos(){
			var theseTodos = [];
			var params = [];
			var orderString = '';
			// Add these parameters to the query
			ko.utils.arrayForEach(selectedView().parameters, function (param) {
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
			orderString = selectedSortColumn() ? selectedSortColumn() : selectedView().primarySort;
			// Add the second and third orders to the string
			orderString = orderString + ', category.name, title';
			// Go get the todos			
			theseTodos = datacontext.getLocalTodos( params, orderString );
			
			return theseTodos;
		}
		
		function isFilterSelectionChanged( selected, lastSelected ){
			//compare the current selections to prev selections
			var changed = false;
			if( selected.length !== lastSelected.length ){
				changed = true;
			}
			else{
				if( selected.length == 0 && lastSelected.length == 0 ){
					changed = false;
				}
				else{
					return false; //any change would always change the number of selected elements.
					
					//the following part may not be needed , based on the assumption that per user clicking speed,
					//	any change would always change the number of selected elements:
					
					/*var selectedFlat = ko.utils.arrayMap(selected(), function(item){
						return item.id();
					});
					var lastSelectedFlat = ko.utils.arrayMap(lastSelected(), function(item){
						return item.id();
					});
					var diffs = ko.utils.compareArrays( selectedFlat, lastSelectedFlat );
					var dups = ko.utils.arrayFilter(diffs, function(d){
						return d.status == 'retained';
					});
					if( diffs.length == dups.length && selectedFlat.length == dups.length && lastSelectedFlat.length == dups.length ){					
						changed = false;
					}
					else{
						console.log('todosfilters selections changed');
						changed = true;
					}*/
				}
			}
			return changed;
		}
		
		var myToDosQueryResult = ko.observableArray([]);
		var myToDos = ko.observableArray([]);
        var myToDosUpdater = ko.computed(function () {
            var theseTodos = [];
			//subscriptions:
			var allTodos = localCollections.todos();
            var selectedview = selectedView();
			var categoryFilterChanged = categoryChanged();
			var priorityFilterChanged = priorityChanged();
			var lastselectedview = lastSelectedView();
            var viewchanged = viewChanged ? viewChanged() : false;
			var bSort = backendSort() ? backendSort() : selectedView().backendSort;           
			var processing = todosProcessing();
			if( !processing ){
				if (selectedview && selectedview.type() === 'todos') {				
					if( ( bSort !== lastSort() && bSort !== null ) || categoryFilterChanged || priorityFilterChanged || viewchanged){						
						lastSort( bSort ); 
						if( categoryFilterChanged ){
							categoryChanged(false);
						}
						if( priorityFilterChanged ){
							priorityChanged(false);
						}
						if( viewchanged ){
							viewChanged(false);
						}
						maxToToDosLoaded(false);						
						clearTodosCacheAndLoad();
						theseTodos = [];
						
					}
					else{
						//refresh from local query					
						theseTodos = getLocalTodos();
						manageTodoPaging(theseTodos.length);
						todosProcessing(false);
					}
				}
			}
			myToDos(theseTodos);
            return theseTodos;
        }).extend({ throttle: 100 });

		/**
		*	clear all todos from localCollections and breeze cache.
		*	@method	clearTodosCacheAndLoad
		*/
		function clearTodosCacheAndLoad(){
			maxToToDosLoaded(false);
			//assign empty array so todos wount be referenced from ko data binding of the views that had them showing.			
			myToDos([]);
			todosProcessing(true);
			var todos = datacontext.getLocalTodos([], null);
			//empty the collection. the todos should be cleaned out by garbage collector.
			localCollections.todos([]);
			todosTotalCount(0);	
			setTimeout( function(){
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
				loadMoreTodos();	//load first block with the new sort
			}, 50);
		}
		
		var todosShowingText = ko.computed( function(){
			var showing = ' showing ';
			var totalCount = todosTotalCount();
			var todos = myToDos? myToDos(): null;
			var reloading = todosReloading ? todosReloading() : false;
			if( reloading ){
				showing = 'Loading...';
			}
			else{
				showing = todos.length + ' showing'
				if( totalCount && todos.length < totalCount ) {
					showing += ' out of ' + totalCount;
				}
			}
			return showing;
		}).extend({ throttle: 50 });
				
        // My interventions
		var myInterventions = ko.observableArray([]);
        var myInterventionsUpdater = ko.computed(function () {            
            var selectedview = selectedView();
            var categoryFilterChanged = categoryChanged();
            
            if (selectedview && selectedview.type() === 'interventions') {                
                if (viewChanged()) {
                    viewChanged(false);
                    loadInterventionsFromServer();
                }
                else if (categoryFilterChanged) {
                    categoryChanged(false);
                    getLocalInterventions();
                }
                else if (selectedSortColumn()) {
                    getLocalInterventions();
                }
            }            
        }).extend({throttle:50});

        function getLocalInterventions(){
            var theseInterventions = [];            
			var params = [];
			var orderString = '';
			// Add these parameters to the query
			ko.utils.arrayForEach(selectedView().parameters, function (param) {
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
			orderString = selectedSortColumn() ? selectedSortColumn() : selectedView().primarySort;
			// Add the second and third orders to the string
			orderString = orderString + ', category.name, description';
			// Go get the intervention			
			theseInterventions = datacontext.getInterventionsQuery(params, orderString);
			myInterventions(theseInterventions);			
        }

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
			detached: detached,
            title: 'Home',
            todosSaving: todosSaving,
            myToDos: myToDos,
			todosShowingText: todosShowingText,		
			maxToToDosLoaded: maxToToDosLoaded,
			todosReloading: todosReloading,
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
            else {                
                refreshView();
			}
            return true;
        }

        function initializeViewModel() {
			if (session.currentUser().settings()) {
				var totalQueryCount = datacontext.getSettingsParam('TotalQueryCount');
				if( totalQueryCount ){
					//this parameter will be the take for any server query ( one and first page design, without load more )
					maxTodosCount( parseInt( totalQueryCount ) );
					todosTake( maxTodosCount() );
				}                
            }
            selectedView(views()[7]);	//open assigned to me
			refreshView();
            initialized = true;
            return true;
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
					backendSort(null);
                } else {
                    // Else set it as the sort column
                    selectedSortColumn(sender.sortProperty + ' desc');
					if( sender.backendSort ){
						backendSort('-'+ sender.backendSort);
					}else{
						backendSort(null);
					}
                }
            } else {
                // Else set it as the new sort column
                selectedSortColumn(sender.sortProperty);
				if( sender.backendSort ){
					backendSort(sender.backendSort);
				}else{
					backendSort(null);
				}
            }
        }

		function loadMoreTodos(){	
			var params = [];
			ko.utils.arrayForEach( selectedView().remoteParams, function( param ) {
				params[param.Property] = param.Value;
			});
			if( selectedCategories().length > 0 ){
				params.CategoryIds = [];
				ko.utils.arrayForEach( selectedCategories(), function (category) {
					params.CategoryIds.push( category.id() );	//sending string category ids
				});
			}
            if( selectedPriorities().length > 0 ){
				params.PriorityIds = [];
				ko.utils.arrayForEach( selectedPriorities(), function (priority) {
					if( !isNaN(priority.id()) ){
						params.PriorityIds.push( parseInt(priority.id()) );	//sending int priority ids
					}
				});
			}			
			params.Skip = 0;
			params.Take = todosTake();
			params.Sort = backendSort() ? backendSort() : selectedView().backendSort;
			datacontext.getToDos( myToDosQueryResult, params, todosTotalCount ).then( todosReturned );
		}
		
		function todosReturned(){
			var returnedCount = myToDosQueryResult()? myToDosQueryResult().length : 0;
			var totalCount = todosTotalCount();						
			if( totalCount < maxTodosCount() ){			
				maxToToDosLoaded(false);
			}
			else{				
				if( returnedCount && returnedCount >= maxTodosCount() ){	
					maxToToDosLoaded(true);
				}
			}
			todosProcessing(false);				
		}
		
		function refreshTodos() {
			maxToToDosLoaded(false);					
			clearTodosCacheAndLoad();
		}
        
		function refreshInterventions() {
		    loadInterventionsFromServer();
		}

        //can be used when implementing paging because we need to clear old page data and push new data
        function clearInterventionsCacheAndLoad(){            
			//assign empty array so interventions wount be referenced from ko data binding of the views that had them showing.			
			myInterventions([]);
			//interventionsProcessing(true);
			var interventions = datacontext.getInterventionsQuery([], null);
			//empty the collection. the interventions should be cleaned out by garbage collector.
			localCollections.interventions([]);				
			setTimeout( function(){
				//short delay to allow the ko data binding to release references to these todos, before removing them: 
				if( interventions && interventions.length > 0 ){										
					ko.utils.arrayForEach( interventions, function(intervention){
						if( intervention ){
							//remove from breeze cache:
							intervention.entityAspect.setDeleted();
							intervention.entityAspect.acceptChanges();															
						}
					});					
				}
				loadInterventionsFromServer();	//load first block with the new sort
			}, 50);
        }
        
        function loadInterventionsFromServer(){	
            var params = {};//server side filtering object
            params.InterventionFilterType = selectedView().interventionFilterType;
            if (params.InterventionFilterType == 1)  {
                params.CreatedById = session.currentUser().userId();
                params.StatusIds = [1];
            }
            else if(params.InterventionFilterType == 2){
                params.CreatedById = session.currentUser().userId();
            }
            else if (params.InterventionFilterType == 3) {
                params.AssignedToId = session.currentUser().userId();                
            }
            else if (params.InterventionFilterType == 4) {
                params.AssignedToId = session.currentUser().userId();
                params.StatusIds = [1];
            }
			datacontext.getInterventions( null, params).then( interventionsReturned );
		}

        function interventionsReturned() {            
            getLocalInterventions();            
		}
		
        // Force refresh from the server
        function refreshView() {
			if( selectedView() && selectedView().type() == 'todos' ){
				refreshTodos();
			}
			else{			    
			    refreshInterventions();
			}			            
        }

        // A view to select
        function View(type, name, params, cols, prisort, remoteParams, backendSort, interventionFilterType) {
            var self = this;
            self.type = ko.observable(type);
            self.name = ko.observable(name);
            //following fields are used for filtering on client side
            self.parameters = params;
            self.columns = ko.observableArray(cols);
            self.primarySort = prisort ? prisort : 'dueDate desc';
			self.remoteParams = remoteParams;
			self.backendSort = backendSort ? backendSort : '-DueDate';
            //following fields are used for filtering on server side
			self.interventionFilterType = interventionFilterType;
        }
        
        // Summary object
        function Summary(name, mainprop, secprop) {
            var self = this;
            self.name = name;
            self.mainProperty = mainprop;
            self.secondaryProperty = secprop;
        }
		
		function detached(){			
		}		
    });