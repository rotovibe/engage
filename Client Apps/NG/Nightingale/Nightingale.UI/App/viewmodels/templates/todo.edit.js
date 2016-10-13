/**
*	todo details edit / add view
*	@module todo.edit 
*	@class todo.edit
*/
define(['services/datacontext', 'services/local.collections'],
    function (datacontext, localCollections) {
		
		var subscriptionTokens= [];
        var alphabeticalSort = function (l, r) { return (l.preferredName() == r.preferredName()) ? (l.preferredName() > r.preferredName() ? 1 : -1) : (l.preferredName() > r.preferredName() ? 1 : -1) };

        var ctor = function () {
            var self = this;
        };

        function ErrorMsg(msg) {
            var self = this;
            self.Message = msg;
        }

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.todo = self.settings.todo;		
            self.canSave = self.settings.canSave;
            self.showing = self.settings.showing;
            self.careManagers = datacontext.enums.careManagers;
            self.toDoCategories = datacontext.enums.toDoCategories;
            self.statuses = datacontext.enums.goalTaskStatuses;
            self.priorities = datacontext.enums.priorities;
            self.errors = ko.computed(function () {
                var errorlist = [];
                // Make sure it has a title
                var thistitle = self.todo().title();
                if (!thistitle) {
                    errorlist.push(new ErrorMsg("'Title' is required"));
                }
                return errorlist;
            });
            self.hasErrors = ko.computed(function () {
                return self.errors().length > 0;
            });
            // Function to delete the todo
            self.deleteToDo = function () {
                // Prompt the user to confirm deletion
                var result = confirm('Are you sure you want to delete the to-do?');
                // If they press OK,
                if (result === true) {
                    // Set the delete flag to true
                    self.todo().deleteFlag(true);
                    // Save it
                    datacontext.saveToDo(self.todo(), 'Update').then(saveCompleted);
                }
                else {
                    // Do nothing
                    return false;
                }

                // Once the save completes
                function saveCompleted() {
                    // Accept the changes to make sure there are no conflicts
                    self.todo().entityAspect.acceptChanges();
                    // Stop showing the modal
                    self.showing(false);
                    // Remove it from the todos list
                    localCollections.todos.remove(self.todo());
                    self.todo().patientId(null);
                    // Give all views a chance to clear the todo before detaching it
                    setTimeout(function () {
                        // Detach the entity from the manager
                        datacontext.detachEntity(self.todo());
                    }, 25);                    
                }
            };
            // Track whether there is a patient or not
            self.noPatient = ko.computed(function () {
                // Return true or false only, not the object
                return !!self.todo().patientId();
            });
            self.availablePrograms = ko.computed(function () {
                var computedPrograms = [];
                if (self.todo().patientId() && self.todo().patient()) {
                    var thesePrograms = self.todo().patient().programs.slice(0).sort(self.alphabeticalNameSort);
                    ko.utils.arrayForEach(thesePrograms, function (program) {
                        if (program.elementState() !== 1) {
                            if (program.elementState() != 5 && program.elementState() != 6) {
                                computedPrograms.push(program);
                            }
                                //5 or 6 within last 30 days
                            else {
                                var today = moment(new Date());
                                var stateUpdatedDate = moment(program.stateUpdatedOn());
                                if (today.diff(stateUpdatedDate, 'days') <= 30) {
                                    computedPrograms.push(program);
                                }
                            }
                        }
                    });
                }
                return computedPrograms;
            });
            // Set the displayed value to the current assign to
            self.assignedToDisplay = ko.observable((self.todo() && self.todo().assignedTo()) ? self.todo().assignedTo().preferredName() : '');
            var token = self.assignedToDisplay.subscribe(function (newValue) {
                // else, Find the care manager that matches
                var matchedCareManager = ko.utils.arrayFirst(self.careManagers(), function (cm) {
                    return cm.preferredName() === newValue;
                });
                // If a match is found,
                if (matchedCareManager) {
                    self.todo().assignedTo(matchedCareManager);
                } else {
                    console.log('No match found');
                }
            });
			subscriptionTokens.push(token);
            self.checkForMatch = function () {
                return self.assignedToDisplay() === (self.todo().assignedTo() ? self.todo().assignedTo().preferredName() : '');                
            };
            self.removeAssignment = function () {
                self.todo().assignedTo(null);
				self.todo().assignedToId(null);
                self.assignedToDisplay('');
            };
            self.removePatientAssociation = function () {                				
				self.todo().patientId(null);	
				//clear all associated programs:
                if (self.todo() && self.todo().programIds().length > 0 && !self.todo().patientId()) {                    
					self.todo().programIds.removeAll();                    
                }
            };
			self.removeUserAssociation = function () {                
				self.removeAssignment();									
            };
			
            self.validMatch = ko.computed({
				read: function () {
					var result = false;
					if (self.todo()) {
						// Check if it matches a valid value
						result = self.checkForMatch();
						// If there is an invalid value,
						if (!result) {
							// if the assign to id has been changed,
							if (self.todo() && self.todo().assignedToId()) {
								// Reset the value
								self.assignedToDisplay((self.todo() && self.todo().assignedTo()) ? self.todo().assignedTo().preferredName() : '');
								self.assignedToDisplay.valueHasMutated();
								// And check again
								result = self.checkForMatch();
							} else {
								// Else clear the value
								self.removeAssignment();
							}
						}
					}
					// Enable or disable the can save state
					self.canSave(result);
					return result;
				},
				deferEvaluation: true
			}).extend({ throttle: 25 });
            self.careManagersBloodhound = new Bloodhound({
                datumTokenizer: function (d) {
                    return Bloodhound.tokenizers.whitespace(d.name());
                },
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                remote: {
                    url: '%QUERY',
                    transport: function (url, options, onSuccess, onError) {
                        var theseCareManagers = self.careManagers().sort(alphabeticalSort);
                        var deferred = $.Deferred();
                        deferred.done(function () { onSuccess(this); });

                        var filterVal = url.toLowerCase();
                        var result = theseCareManagers.filter(function (item) {
                            if (item && item.firstLastOrPreferredName) {
                                return !!~item.firstLastOrPreferredName().toLowerCase().indexOf(filterVal);
                            }
                            return false;
                        });
                        deferred.resolveWith(result);
                        return deferred.promise();
                    }
                },
                limit: 25
            });
            self.careManagersBloodhound.initialize();
        };

        ctor.prototype.attached = function () {

        };
		
		/**
		*	clearing KO subscriptions/computeds memory
		*	@method detached		
		*	@example	 	
		*	for subscriptions: 
		*
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
            // dispose computeds:
            self.errors.dispose();
            self.hasErrors.dispose();
			self.noPatient.dispose();
			self.availablePrograms.dispose();
			self.validMatch.dispose();
			//self.canSave.dispose(); remarked: causes timing issues.
			//dispose subscriptions:
			ko.utils.arrayForEach(subscriptionTokens, function (token) {
				token.dispose();
			});			
        };
        return ctor;
    });