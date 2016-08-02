/**
*	displays todo grid with edit/delete	and opens edit modal.
*	@module todo.panel	
*	@class todo.panel
*/
define(['services/session', 'services/datacontext', 'config.services', 'viewmodels/shell/shell', 'models/base'],
    function (session, datacontext, servicesConfig, shell, modelConfig) {

        var ctor = function () {
    		var self = this;
    		self.descendingDateSort = function (l, r) { return (l.dueDate() == r.dueDate()) ? (l.dueDate() < r.dueDate() ? 1 : -1) : (l.dueDate() < r.dueDate() ? 1 : -1) };
	        self.todoSort = function (l, r) {
	            // Primary sort property
	            var p1 = l.category() ? l.category().name() : '';
	            var p2 = r.category() ? r.category().name() : '';

	            // Secondary sort property
	            var o1 = l.title();
	            var o2 = r.title();
	            
	            if (p1 != p2) {
	                if (p1 < p2) return 1;
	                if (p1 > p2) return -1;
	                return 0;
	            }
	            if (o1 < o2) return 1;
	            if (o1 > o2) return -1;
	            return 0;
	        };
            self.modalShowing = ko.observable(false);
            self.modalEntity = ko.observable(new ModalEntity(self.modalShowing));
			
			/**
			*	used for keeping a snapshot of the original selected program ids
			*	for this todo. this is part of a workaround for restoring the values when the user cancel to close the todo edit modal. 
			*	@property originalProgramIds {array}				
			*			
			*/
			self.originalProgramIds = ko.observableArray([]);
    	}
		
        // All the available columns
    	var allColumns = [
            new Column('priority', 'Priority', 'span2', 'priority.id', 'Priority'),
    		new Column('status', 'Status', 'span2', 'status.id'),
            new Column('priority-small', 'Priority', 'span1', 'priority.id', 'Priority'),
            new Column('status-small', 'Status', 'span1', 'status.id'),
    		new Column('patient', 'Individual','span2', 'patientDetails.lastName', true),
    		new Column('category', 'Category','span2', 'category.name', true ),
            new Column('category-small', 'Category','span1', 'category.name', true ),
    		new Column('title', 'Title','span4', 'title', 'Title'),
            new Column('title-small', 'Title','span3', 'title', 'Title'),
    		new Column('duedate', 'Due Date','span2', 'dueDate', 'DueDate'),
    		new Column('assignedto', 'Assigned To','span2', 'assignedTo.preferredName', true),
            new Column('closedon', 'Date','span2', 'closedDate', 'ClosedDate'),
            new Column('closedon-small', 'Date','span1', 'closedDate', 'ClosedDate'),
            new Column('updatedon', 'Date','span2', 'updatedOn', 'UpdatedOn'),
            new Column('updatedon-small', 'Date', 'span1', 'updatedOn', 'UpdatedOn')
    	];
        
        var patientEndPoint = ko.computed(function () {
            if(!session.currentUser()) {
                return false;
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'patient', 'Patient');
        });

        // Endpoint to use for getting the current patient's programs
        var patientProgramEndPoint = ko.computed(function () {
            if(!session.currentUser()) {
                return false;
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Program');
        });    	
		
        ctor.prototype.activate = function (data) {
    		var self = this;
    		self.todos = data.todos;
            self.selectedSortColumn = data.selectedSortColumn;
            self.toggleSort = data.toggleSort;
            self.canSort = data.canSort ? data.canSort : false;
			self.isBackendSort = data.isBackendSort ? data.isBackendSort : false;
			self.todosReloading = data.todosReloading ? data.todosReloading : ko.observable(false);
			
			//dont allow sorting on category name and individual name since this view's data has backend sorting. 
			//	for now - backend sorting cannot sort on related collections properties as category name in a todo query.
			var column = findColumnByName('assignedto');
			if( column ){
				column.backendSort = !self.isBackendSort;
			}
			column = findColumnByName('patient');
			if( column ){
				column.backendSort = !self.isBackendSort;;
			}
			column = findColumnByName('category');
			if( column ){
				column.backendSort = !self.isBackendSort;;
			}
			column = findColumnByName('category-small');
			if( column ){
				column.backendSort = !self.isBackendSort;;
			}
			
			self.loadMoreTodos = data.loadMoreTodos;
			self.canLoadMoreTodos = data.canLoadMoreTodos;
			// self.loadPrevTodos = data.loadPrevTodos;
			// self.canLoadPrevTodos = data.canLoadPrevTodos;
			self.maxToToDosLoaded = data.maxToToDosLoaded;
            self.saveOverride = function () {
                // If patient has been removed - clear all associated programs:
                if (self.modalEntity().todo() && self.modalEntity().todo().programIds().length > 0 && !self.modalEntity().todo().patientId()) {                    
					self.modalEntity().todo().programIds.removeAll();                    
                }
                datacontext.saveToDo(self.modalEntity().todo(), 'Update').then(saveCompleted);

                function saveCompleted() {
                    self.modalEntity().todo().isNew(false);
					self.modalEntity().todo().clearDirty();
                    self.modalEntity().todo().entityAspect.acceptChanges();
					self.originalProgramIds.removeAll();
                }
            };          

			/**
			*	close todo modal dialog and cancel any changes if made.
			*			note: rejectChanges issue with complex type array have a known bug that clears todo.programIds.
			*			the workaround uses originalProgramIds observable.			
			*	@method	cancelOverride 
			*/
			self.cancelOverride = function () {				
				if(self.modalEntity().todo().entityAspect.entityState.isAddedModifiedOrDeleted()){
					datacontext.cancelEntityChanges(self.modalEntity().todo());					
					
					/**	
					*	@example datacontext.cancelEntityChanges
					*	note: breeze rejectChanges has a known issue with array of complex type being cleared, regardless if there were actual changes or not.
					*		the array will not get back to its original state.
					*	(https://github.com/Breeze/breeze.js/issues/47)
					*	
					*		//debug:
					*		console.log('cancelOverride after datacontext.cancelEntityChanges: todo.programIds = ' + self.modalEntity().todo().programIds().length + ' self.modalEntity().todo().entityAspect.entityState.name= ' + self.modalEntity().todo().entityAspect.entityState.name);
					*	
					*	for todo.programIds - as a workaround/correction we will use the originalProgramIds to restore the original programIds:
					*
					*		var progIds = self.modalEntity().todo().programIds();
					*		ko.utils.arrayPushAll(progIds, self.originalProgramIds());
					*/	
					
					self.modalEntity().todo().programIds.removeAll();
					var progIds = self.modalEntity().todo().programIds();					
					if(self.originalProgramIds().length > 0){
						ko.utils.arrayPushAll(progIds, self.originalProgramIds());
						self.originalProgramIds.removeAll();	
					}
					//clear the entityAspect.entityState back to Unchanged state - to hide this correction:
					self.modalEntity().todo().entityAspect.setUnchanged();	

					//breeze rejectChanges also misses to get the category /assignedTo properties back to its original state when originally it was cleared:
					//(we could also resolve this using a nullo obj):
					if(!self.modalEntity().todo().categoryId()){
						self.modalEntity().todo().category(null);
					}
					if(!self.modalEntity().todo().assignedToId()){
						self.modalEntity().todo().assignedTo(null);
					}
					self.modalEntity().todo().clearDirty();					
				}	                		
            };
			
    		// A list of columns to display
    		self.columns = ko.computed(function () {
    			var tempcols = [];
    			var thesecols = data.columns();
    			ko.utils.arrayForEach(thesecols, function (col) {
    				var matchingCol = findColumnByName(col);
    				if (matchingCol) {
    					tempcols.push(matchingCol);
    				}
    			});
                // If no columns are found, show a default set of columns
                if (!tempcols) {
                    tempcols = ['status','patient','category','title','duedate'];
                }
    			return tempcols;
    		});
			
			/**
			*	opens a todo details modal view.
			*		note - we record the todo.programIds in originalProgramIds.
			*	@method editToDo 			
			*
			*/
    		self.editToDo = function (todo) {
                // If there is an associated patient object,
                if (todo && todo.patientDetails() && todo.patientId()) {
                    // Go get the program details, if not already loaded
                    if (todo.patient() && todo.patient().programs().length === 0) {
                        datacontext.getEntityList(null, patientProgramEndPoint().EntityType, patientProgramEndPoint().ResourcePath + todo.patientId() + '/Programs', null, null, true);
                    } else {
                        // Go select a patient by their Id.
                        datacontext.getEntityById(null, todo.patientId(), patientEndPoint().EntityType, patientEndPoint().ResourcePath, true);
                        // And get their programs
                        datacontext.getEntityList(null, patientProgramEndPoint().EntityType, patientProgramEndPoint().ResourcePath + todo.patientId() + '/Programs', null, null, true);
                    }
                }
				
				//keep the original program ids
				self.originalProgramIds.removeAll();
				var progIds = self.originalProgramIds();
				ko.utils.arrayPushAll(progIds, todo.programIds());
				var modalSettings = {
					title: 'Edit To Do',
					relatedPatientName: todo.patientName,
					entity: self.modalEntity, 
					templatePath: 'viewmodels/templates/todo.edit', 
					showing:self.modalShowing , 
					saveOverride: self.saveOverride, 
					cancelOverride: self.cancelOverride, 
					deleteOverride: null, 
					classOverride: null
				}
				self.modal = new modelConfig.modal(modalSettings);
				todo.watchDirty();	
                self.modalEntity().todo(todo);
                shell.currentModal(self.modal);
                self.modalShowing(true);
    		}
    		self.computedTodos = ko.computed(function () {
    			ko.utils.arrayForEach(self.todos(), function (td) {
    				if (!td.edit) {
    					td.edit = function () {
    						self.editToDo(td);
    					}
    				}
    			});
                // Filter out deleted todos
                var finalTodos = ko.utils.arrayFilter(self.todos(), function (td) {
                    return !td.deleteFlag();
                });
                //var temparray = self.todos().sort(self.todoSort);
                //return temparray.sort(self.descendingDateSort);
                return finalTodos;
    		});
			self.noDataFound = ko.computed( function(){
				return (!self.todosReloading() && self.computedTodos().length == 0);
			});
    	}

        /**
		*	clearing KO subscriptions/computeds memory 		
		*	@method detached
		*	@example	
		*	for subscriptions: 
		*		1. declare tokens collection:	var subscriptionTokens= [];
		*		2. keep the returned token/s:
		*	
		*			var token = something.subscribe(function (newValue) {...});
		*		
		*		3. dispose tokens:
		*			
		*			ko.utils.arrayForEach(subscriptionTokens, function (token) {
		*				token.dispose();
		*			});
		*	@example
		*	for computeds:
		*	
		*		self.someComputed.dispose();
		*/
        ctor.prototype.detached = function () {
            var self = this;
            // Clear out all of the subscriptions to dispose of this properly: computeds:
            patientEndPoint.dispose();
            patientProgramEndPoint.dispose();
            //self.modalEntity().canSave.dispose(); remarked: causes timing issues
            self.columns.dispose();
            self.computedTodos.dispose();
			self.noDataFound.dispose();	
        };

    	function findColumnByName(name) {
    		var match = ko.utils.arrayFirst(allColumns, function (allcol) {
    			return allcol.name.toLowerCase() === name.toLowerCase();
    		});
    		if (!match) {
    			console.log('bad column name used - ', name);
    		}
    		return match;
    	}

    	function Column(name, displayname, cssclass, sortprop, backendSort) {
    		var self = this;
    		self.name = name;
    		self.displayName = displayname;
            // If the name contains small,
            if (name.substr(name.length - 5, 5) === 'small') {
                self.path = 'views/templates/todo.' + name.substr(0, name.length - 6) + '.html';
            } else {
                self.path = 'views/templates/todo.' + name + '.html';                
            }
    		self.cssClass = cssclass;
            self.sortProperty = sortprop;
			self.backendSort = backendSort;
    	}

    	return ctor;

        function ModalEntity(modalShowing) {
            var self = this;
            self.todo = ko.observable();
            self.canSaveObservable = ko.observable(true);
            self.canSave = ko.computed({
                read: function () {
                    var todook = false;
					var isShowing = modalShowing();
					var todo = self.todo();
					if(!isShowing) return false;
                    if (todo) {                        
                        todook = todo.isValid();
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

	});