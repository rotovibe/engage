define(['services/session', 'services/datacontext', 'config.services', 'viewmodels/shell/shell', 'models/base', 'plugins/router', 'services/navigation'],
    function (session, datacontext, servicesConfig, shell, modelConfig, router, navigation) {

        var goalEndPoint = ko.computed(function () {
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Goal');
        });

        var ctor = function () {
    		var self = this;
    		self.descendingDateSort = function (l, r) { return (l.dueDate() == r.dueDate()) ? (l.dueDate() < r.dueDate() ? 1 : -1) : (l.dueDate() < r.dueDate() ? 1 : -1) };
	        self.interventionSort = function (l, r) {
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
    		new Column('category', 'Category','span2', 'category.name'),
            new Column('category-small', 'Category','span1', 'category.name'),
    		new Column('description', 'Title','span3', 'description'),
            new Column('description-small', 'Title','span2', 'description'),
            new Column('goal', 'Goal','span3', 'goalName'),
    		new Column('duedate', 'Due Date','span2', 'dueDate'),			
    		new Column('assignedto', 'Assigned To','span2', 'assignedTo.preferredName'),
            new Column('closeddate', 'Closed Date','span2', 'closedDate'),
            new Column('closeddate-small', 'Closed Date','span1', 'closedDate'),
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
    		self.interventions = data.interventions;
            self.selectedSortColumn = data.selectedSortColumn;
            self.toggleSort = data.toggleSort;
            self.canSort = data.canSort ? data.canSort : false;
            self.saveOverride = function () {
                // If patient has been removed and program ids are still there
                if (self.modalEntity().intervention() && self.modalEntity().intervention().barrierIds().length > 0 && !self.modalEntity().intervention().computedPatient().id()) {
                    // Go through each one,
                    ko.utils.arrayForEach(self.modalEntity().intervention().barrierIds(), function (progId) {
                        // And remove it
                        self.modalEntity().intervention().barrierIds.remove(progId);
                    });
                }
                datacontext.saveIntervention(self.modalEntity().intervention(), 'Update');
                //.then(saveCompleted);

                function saveCompleted() {
                    self.modalEntity().intervention().isNew(false);
                    self.modalEntity().intervention().entityAspect.acceptChanges();
                }
            };
            self.cancelOverride = function () {
				var intervention = self.modalEntity().intervention();
                datacontext.cancelEntityChanges(intervention);
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
    		self.editIntervention = function (intervention) {
                // If there is an associated patient object,
                if (intervention && intervention.patientId()) {
                    // Go get the program details, if not already loaded
                    // if (intervention.goal() && intervention.goal().patient() && intervention.goal().patient().programs().length === 0) {
                    //     datacontext.getEntityList(null, patientProgramEndPoint().EntityType, patientProgramEndPoint().ResourcePath + intervention.patientId() + '/Programs', null, null, true).then(loadModalCompleted);
                    // } else {
                        // Go select a patient by their Id.
                        datacontext.getEntityById(null, intervention.patientId(), patientEndPoint().EntityType, patientEndPoint().ResourcePath, true).then(function () {
                            // Go grab the goal details
                            getGoalDetails(intervention.patientGoalId(), intervention.computedPatient().id()).then(loadModalCompleted);
                            // And get their programs
                            datacontext.getEntityList(null, patientProgramEndPoint().EntityType, patientProgramEndPoint().ResourcePath + intervention.patientId() + '/Programs', null, null, true);
                        });
                    //}
                // } else {
                //     // Already loaded everything, load it up
                //     loadModalCompleted();
                }

                function loadModalCompleted () {
					var modalSettings = {
						title: 'Edit Intervention',
						showSelectedPatientInTitle: true,
						relatedPatientName: intervention.computedPatient().fullName,
						entity:self.modalEntity , 
						templatePath: 'viewmodels/templates/intervention.edit', 
						showing: self.modalShowing, 
						saveOverride: self.saveOverride, 
						cancelOverride: self.cancelOverride, 
						deleteOverride: null, 
						classOverride: null
					}
					self.modal = new modelConfig.modal(modalSettings);
                    self.modalEntity().intervention(intervention);
                    shell.currentModal(self.modal);
                    self.modalShowing(true);
                }
    		};
    		self.computedInterventions = ko.computed(function () {
    			ko.utils.arrayForEach(self.interventions(), function (td) {
                    // Make sure all interventions have an edit function
    				if (!td.edit) {
    					td.edit = function () {
    						self.editIntervention(td);
    					}
    				}
                    if (!td.gotoGoal) {
                        td.gotoGoal = function () {
                            // If we are already in the context of the patient
                            if (navigation.currentRoute() && navigation.currentRoute().config.title === 'Individual') {
                                // Get the goals subroute
                                var thisSubRoute = ko.utils.arrayFirst(navigation.currentRoute().config.settings.pages, function (page) {
                                    return page.title === 'goals';
                                });
                                // And set the new sub route
                                navigation.setSubRoute(thisSubRoute);
                            } else {
                                // Else navigate to the patient
                                router.navigate('#patients/' + td.computedPatient().id());  
                                navigation.indexOverride(1);                                
                            }
                        };
                    }
    			});
                return self.interventions();
    		});
    	}

        // The view has detached so remove all subscriptions
        ctor.prototype.detached = function () {
            var self = this;
            // Clear out all of the subscriptions to dispose of this properly
            goalEndPoint.dispose();
            patientEndPoint.dispose();
            patientProgramEndPoint.dispose();
            //self.modalEntity().canSave.dispose();
            self.columns.dispose();
            self.computedInterventions.dispose();
        };

        function getGoalDetails (goalId, patientId) {
            return datacontext.getEntityById(null, goalId, goalEndPoint().EntityType, goalEndPoint().ResourcePath + patientId + '/Goal/', true);
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
                self.path = 'views/templates/intervention.' + name.substr(0, name.length - 6) + '.html';
            } else {
                self.path = 'views/templates/intervention.' + name + '.html';                
            }
    		self.cssClass = cssclass;
            self.sortProperty = sortprop;
    	}

    	return ctor;

        function ModalEntity(modalShowing) {
            var self = this;
            self.intervention = ko.observable();
            self.canSaveObservable = ko.observable(true);
            self.canSave = ko.computed({
                read: function () {
                    var isValid = false;
                    if (self.intervention()) {
						isValid = self.intervention().isValid();                        
                    }
                    return isValid && self.canSaveObservable();
                },
                write: function (newValue) {
                    self.canSaveObservable(newValue);
                }
            });
            // Object containing parameters to pass to the modal
            self.activationData = { entity: self.intervention, canSave: self.canSave, showing: modalShowing  };
        }

	});