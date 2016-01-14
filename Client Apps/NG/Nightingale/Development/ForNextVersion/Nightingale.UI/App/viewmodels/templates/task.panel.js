define(['services/session', 'services/datacontext', 'config.services', 'viewmodels/shell/shell', 'models/base', 'plugins/router', 'services/navigation'],
    function (session, datacontext, servicesConfig, shell, modelConfig, router, navigation) {

        var goalEndPoint = ko.computed(function () {
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Goal');
        });

        var ctor = function () {
    		var self = this;
    		self.descendingDateSort = function (l, r) { return (l.dueDate() == r.dueDate()) ? (l.dueDate() < r.dueDate() ? 1 : -1) : (l.dueDate() < r.dueDate() ? 1 : -1) };
	        self.taskSort = function (l, r) {
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
    		new Column('status', 'Status', 'span2', 'status.id'),
            new Column('status-small', 'Status', 'span1', 'status.id'),
    		new Column('patient', 'Individual','span2', 'patientDetails.lastName'),
            new Column('targetvalue', 'Target Value', 'span2', 'targetValue'),
            new Column('targetvalue-small', 'Target Value', 'span1', 'targetValue'),
    		new Column('description', 'Title','span4', 'description'),
            new Column('description-small', 'Title','span3', 'description'),
            new Column('goal', 'Goal','span3', 'goalName'),
    		new Column('startdate', 'Start Date','span2', 'startDate'),
            new Column('closeddate', 'Closed Date','span2', 'closedDate'),
            new Column('targetdate', 'Target Date','span2', 'targetDate'),
            new Column('statusdate', 'Status Date','span2', 'statusDate'),
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
    		self.tasks = data.tasks;
            self.selectedSortColumn = data.selectedSortColumn;
            self.toggleSort = data.toggleSort;
            self.canSort = data.canSort ? data.canSort : false;
            self.saveOverride = function () {
				self.modalEntity().task().checkAppend();
                datacontext.saveTask(self.modalEntity().task(), 'Update');

                function saveCompleted() {
                    //self.modalEntity().task().isNew(false);
                    self.modalEntity().task().entityAspect.acceptChanges();
                }
            };
            self.cancelOverride = function () {
				var task = self.modalEntity().task();
                datacontext.cancelEntityChanges(task);
            };
			var modalSettings = {
				title:'Edit Task' ,
				showSelectedPatientInTitle: true,
				entity: self.modalEntity, 
				templatePath: 'viewmodels/templates/task.edit', 
				showing: self.modalShowing, 
				saveOverride: self.saveOverride, 
				cancelOverride: self.cancelOverride, 
				deleteOverride: null, 
				classOverride: null
			}
            self.modal = new modelConfig.modal(modalSettings);
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
    		self.editTask = function (task) {
                // If there is an associated patient object,
                if (task && task.goal() && task.goal().patientId()) {
                    // Go get the program details, if not already loaded
                    if (task.goal() && task.goal().patient() && task.goal().patient().programs().length === 0) {
                        // Always get goal details
                        getGoalDetails(task.patientGoalId(), task.patientId()).then(loadModalCompleted);
                        // datacontext.getEntityList(null, patientProgramEndPoint().EntityType, patientProgramEndPoint().ResourcePath + task.patientId() + '/Programs', null, null, true).then(loadModalCompleted);
                    } else {
                        // No matter what!
                        getGoalDetails(task.patientGoalId(), task.patientId()).then(loadModalCompleted);
                    }
                } else {
                    // Already loaded everything, load it up
                    loadModalCompleted();
                }

                function loadModalCompleted () {
                    self.modalEntity().task(task);
                    shell.currentModal(self.modal);
                    self.modalShowing(true);
                }
    		}
    		self.computedTasks = ko.computed(function () {
    			ko.utils.arrayForEach(self.tasks(), function (td) {
                    // Make sure all Tasks have an edit function
    				if (!td.edit) {
    					td.edit = function () {
    						self.editTask(td);
    					}
                    if (!td.gotoGoal) {
                        td.gotoGoal = function () {
                            // Get the goals subroute
                            var thisSubRoute = ko.utils.arrayFirst(navigation.currentRoute().config.settings.pages, function (page) {
                                return page.title === 'goals';
                            });
                            navigation.setSubRoute(thisSubRoute);
                        };
                    }
    				}
    			});
                return self.tasks();
    		});
    	};

        // The view has detached so remove all subscriptions
        ctor.prototype.detached = function () {
            var self = this;
            // Clear out all of the subscriptions to dispose of this properly
            goalEndPoint.dispose();
            patientEndPoint.dispose();
            patientProgramEndPoint.dispose();
            //self.modalEntity().canSave.dispose();
            self.columns.dispose();
            self.computedTasks.dispose();
        };

        function getGoalDetails (goalId, patientId) {
            return datacontext.getEntityById(null, goalId, goalEndPoint().EntityType, goalEndPoint().ResourcePath + patientId + '/Goal/', true);
        }

    	function findColumnByName(name) {
    		var match = ko.utils.arrayFirst(allColumns, function (allcol) {
    			return allcol.name.toLowerCase() === name.toLowerCase();
    		});
    		if (!match) {
    		}
    		return match;
    	}

    	function Column(name, displayname, cssclass, sortprop) {
    		var self = this;
    		self.name = name;
    		self.displayName = displayname;
            // If the name contains small,
            if (name.substr(name.length - 5, 5) === 'small') {
                self.path = 'views/templates/task.' + name.substr(0, name.length - 6) + '.html';
            } else {
                self.path = 'views/templates/task.' + name + '.html';                
            }
    		self.cssClass = cssclass;
            self.sortProperty = sortprop;
    	}

    	return ctor;

        function ModalEntity(modalShowing) {
            var self = this;
            self.task = ko.observable();
            self.canSaveObservable = ko.observable(true);
            self.canSave = ko.computed({
                read: function () {
                    var taskok = false;
                    if (self.task()) {
                        var taskdesc = !!self.task().description();
                        taskok = taskdesc;
                    }
                    return taskok && self.canSaveObservable();
                },
                write: function (newValue) {
                    self.canSaveObservable(newValue);
                }
            });
            // Object containing parameters to pass to the modal
            self.activationData = { entity: self.task, canSave: self.canSave, showing: modalShowing  };
        }

	});