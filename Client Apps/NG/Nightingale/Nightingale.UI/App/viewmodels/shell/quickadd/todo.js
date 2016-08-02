define(['models/base', 'config.services', 'services/datacontext', 'services/session', 'services/navigation', 'services/local.collections', 'viewmodels/patients/index'],
    function (modelConfig, servicesConfig, datacontext, session, navigation, localCollections, patientIndex) {

        var alphabeticalSort = function (l, r) { return (l.preferredName() == r.preferredName()) ? (l.preferredName() > r.preferredName() ? 1 : -1) : (l.preferredName() > r.preferredName() ? 1 : -1) };

        var alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };

        var ctor = function () {
            var self = this;
            // Note that is used when creating a new note
            self.newTodo = ko.observable();
            self.isSaving = ko.observable();
            self.categories = ko.computed(datacontext.enums.toDoCategories);
            self.statuses = ko.computed(datacontext.enums.goalTaskStatuses);
            self.priorities = ko.computed(datacontext.enums.priorities);
            self.careManagers = datacontext.enums.careManagers;
            self.savable = ko.observable(true);
        };

        function ErrorMsg(msg) {
            var self = this;
            self.Message = msg;
        }

        ctor.prototype.createNewTodo = function () {
            var self = this;
            var patientId;
            // The following shouldn't be needed
            if (self.selectedPatient) {
                patientId = self.selectedPatient().id();
            } else {
                patientId = self.selectedPatient().id();
            }
            if (patientId) {
                // Create a local version of this patient
                var thisPatient = self.selectedPatient();
                // Create a patient to use
                var patientDetailsEntity = ko.observable();
                datacontext.checkForEntityLocally(patientDetailsEntity, thisPatient.id(), 'ToDoPatient');
                // If a local patient details object wasn't found,
                if (!patientDetailsEntity()) {
                    patientDetailsEntity(datacontext.createEntity('ToDoPatient', { id: thisPatient.id(), firstName: thisPatient.firstName(), lastName: thisPatient.lastName(), middleName: thisPatient.middleName(), suffix: thisPatient.suffix(), preferredName: thisPatient.preferredName() }));
                }
                // Create a new todo
                self.newTodo(datacontext.createEntity('ToDo', { id: -2, statusId: 1, priorityId: 0, createdById: session.currentUser().userId(), assignedToId: session.currentUser().userId(), patientId: patientId, patientDetails: patientDetailsEntity() }));
            } else {
                // Create a new todo
                self.newTodo(datacontext.createEntity('ToDo', { id: -2, statusId: 1, priorityId: 0, createdById: session.currentUser().userId(), assignedToId: session.currentUser().userId(), patientId: patientId }));                
            }
            self.newTodo().isNew(true);
			self.newTodo().watchDirty();
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.selectedPatient = settings.data.selectedPatient;
            // Track whether there is a patient or not
            self.noPatient = ko.computed(function () {
                if (self.newTodo()) {
                    // Return true or false only, not the object
                    return !!self.newTodo().patientId();   
                } else {
                    return false;
                }
            });
            self.thisTodoId = ko.computed(function () {
                return self.selectedPatient().todos().length * -1;
            });
            self.isShowing = self.settings.data.isShowing;
            self.cancel = function () {
                self.newTodo().entityAspect.rejectChanges();
				self.newTodo().clearDirty();
                self.createNewTodo();
                self.isShowing(false);
            };
            self.availablePrograms = ko.computed(function () {
                var computedPrograms = [];
                if (self.selectedPatient() && self.newTodo() && self.newTodo().patient()) {
                    var thesePrograms = self.selectedPatient().programs.slice(0).sort(self.alphabeticalNameSort);
                    ko.utils.arrayForEach(thesePrograms, function (program) {
                        if (program.elementState() !== 6 && program.elementState() !== 1 && program.elementState() !== 5) {
                            computedPrograms.push(program);
                        }
                    });
                }
                return computedPrograms;
            });			
            
            self.startDate = ko.observable(new moment());
            self.save = function () {
                datacontext.saveToDo(self.newTodo(), 'Insert').then(saveCompleted);

                function saveCompleted(todo) {
					todo.clearDirty();
                    todo.isNew(false);
                    self.isShowing(false);
                    todo.entityAspect.acceptChanges();
                    localCollections.todos.push(self.todo);
                    self.createNewTodo();
                }
            };
            self.selectedPatient.subscribe(function () {
                self.cancel();
            });
            self.canSave = ko.computed({
                read: function () {
                    return self.newTodo() && !self.isSaving() && self.newTodo().isValid() && self.savable() && !datacontext.todosSaving();
                },
                write: function (newValue) {
                    self.savable(newValue);
                }
            });
            self.removeAssignment = function () {
                self.newTodo().assignedTo(null);
                self.assignedToDisplay('');
            };
            self.removeAssociation = function () {
                // Remove the association to the patient
                self.newTodo().patientId(null);
                // If patient has been removed: clear all associated programs (if any):
                if (self.newTodo() && self.newTodo().programIds().length > 0 && !self.newTodo().patientId()) {                    
                    self.newTodo().programIds.removeAll();					
                }
            };
			self.removeUserAssociation = function () {                
				self.removeAssignment();									
            };
            self.createNewTodo();
            // Set the displayed value to the current assign to
            self.assignedToDisplay = ko.observable((self.newTodo() && self.newTodo().assignedTo()) ? self.newTodo().assignedTo().preferredName() : '');
            self.assignedToDisplay.subscribe(function (newValue) {
                // else, Find the care manager that matches
                var matchedCareManager = ko.utils.arrayFirst(self.careManagers(), function (cm) {
                    return cm.preferredName() === newValue;
                });
                // If a match is found,
                if (matchedCareManager) {
                    self.newTodo().assignedTo(matchedCareManager);
                } else {
                    console.log('No match found');
                }
            });
            self.checkForMatch = function () {
                return self.assignedToDisplay() === (self.newTodo().assignedTo() ? self.newTodo().assignedTo().preferredName() : '');                
            };
            self.validMatch = ko.computed(function () {
                var result = false;
                if (self.newTodo()) {
                    // Check if it matches a valid value
                    result = self.checkForMatch();
                    // If there is an invalid value,
                    if (!result) {
                        // if the assign to id has been changed,
                        if (self.newTodo() && self.newTodo().assignedToId()) {
                            // Reset the value
                            self.assignedToDisplay((self.newTodo() && self.newTodo().assignedTo()) ? self.newTodo().assignedTo().preferredName() : '');
                            self.assignedToDisplay.valueHasMutated();
                            // And check again
                            result = self.checkForMatch();
                        } else {
                        }
                    }
                }
                // Enable or disable the can save state
                self.canSave(result);
                return result;
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
            patientIndex.leaving.subscribe(function (newValue) {
                if (newValue) {
                    self.cancel();
                } else {
                }
            });
        };

        ctor.prototype.attached = function () {
        };

        return ctor;
    });