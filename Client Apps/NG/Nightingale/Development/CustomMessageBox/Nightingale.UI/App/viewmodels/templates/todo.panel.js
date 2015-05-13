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
    	}

        // All the available columns
    	var allColumns = [
            new Column('priority', 'Priority', 'span2', 'priority.id'),
    		new Column('status', 'Status', 'span2', 'status.id'),
            new Column('priority-small', 'Priority', 'span1', 'priority.id'),
            new Column('status-small', 'Status', 'span1', 'status.id'),
    		new Column('patient', 'Individual','span2', 'patientDetails.lastName'),
    		new Column('category', 'Category','span2', 'category.name'),
            new Column('category-small', 'Category','span1', 'category.name'),
    		new Column('title', 'Title','span4', 'title'),
            new Column('title-small', 'Title','span3', 'title'),
    		new Column('duedate', 'Due Date','span2', 'dueDate'),
    		new Column('assignedto', 'Assigned To','span2', 'assignedTo.preferredName'),
            new Column('closedon', 'Date','span2', 'closedDate'),
            new Column('closedon-small', 'Date','span1', 'closedDate'),
            new Column('updatedon', 'Date','span2', 'updatedOn'),
            new Column('updatedon-small', 'Date', 'span1', 'updatedOn')
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
            self.saveOverride = function () {
                // If patient has been removed and program ids are still there
                if (self.modalEntity().todo() && self.modalEntity().todo().programIds().length > 0 && !self.modalEntity().todo().patientId()) {                    
                    // Go through each one,
                    ko.utils.arrayForEach(self.modalEntity().todo().programIds(), function (progId) {
                        // And remove it
                        self.modalEntity().todo().programIds.remove(progId);
                    });
                }
                datacontext.saveToDo(self.modalEntity().todo(), 'Update').then(saveCompleted);

                function saveCompleted() {
                    self.modalEntity().todo().isNew(false);
                    self.modalEntity().todo().entityAspect.acceptChanges();
                }
            };
            self.cancelOverride = function () {
                datacontext.cancelEntityChanges(self.modalEntity().todo());
            };
            self.modal = new modelConfig.modal('Edit To Do', self.modalEntity, 'viewmodels/templates/todo.edit', self.modalShowing, self.saveOverride, self.cancelOverride);
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
    	}

        // The view has detached so remove all subscriptions
        ctor.prototype.detached = function () {
            var self = this;
            // Clear out all of the subscriptions to dispose of this properly
            patientEndPoint.dispose();
            patientProgramEndPoint.dispose();
            //self.modalEntity().canSave.dispose();
            self.columns.dispose();
            self.computedTodos.dispose();
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

    	function Column(name, displayname, cssclass, sortprop) {
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
    	}

    	return ctor;

        function ModalEntity(modalShowing) {
            var self = this;
            self.todo = ko.observable();
            self.canSaveObservable = ko.observable(true);
            self.canSave = ko.computed({
                read: function () {
                    var todook = false;
                    if (self.todo()) {
                        var todotitle = !!self.todo().title();
                        var todostatus = !!self.todo().status();
                        todook = todotitle && todostatus;
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