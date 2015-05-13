define(['config.models', 'config.services', 'services/datacontext', 'services/session', 'viewmodels/patients/goals/index'],
    function (models, servicesConfig, datacontext, session, goalsIndex) {

        var ctor = function () {
            var self = this;
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };
            self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
            self.settings = settings;
            self.activeGoal = self.settings.activeGoal;
            self.isEditing = goalsIndex.isEditing;
            self.focusAreas = datacontext.enums.focusAreas;
            self.sources = datacontext.enums.sources;
            self.goalTypes = datacontext.enums.goalTypes;
            self.goalTaskStatuses = datacontext.enums.goalTaskStatuses;
            self.barrierStatuses = datacontext.enums.barrierStatuses;
            self.computedBarriers = ko.computed(self.activeGoal.barriers);
            self.interventionStatuses = datacontext.enums.interventionStatuses;
            self.goalTypes = datacontext.enums.goalTypes;
            self.barrierCategories = datacontext.enums.barrierCategories;
            self.interventionCategories = datacontext.enums.interventionCategories;
            self.careManagers = datacontext.enums.careManagers;
            self.availablePrograms = ko.computed(function () {
                var computedPrograms = [];
                if (self.activeGoal.patient()) {
                    var thesePrograms = self.activeGoal.patient().programs.slice(0).sort(self.alphabeticalNameSort);
                    ko.utils.arrayForEach(thesePrograms, function (program) {
                        if (program.elementState() !== 6 && program.elementState() !== 1 && program.elementState() !== 5) {
                            computedPrograms.push(program);
                        }
                    });
                }
                return computedPrograms;
            });
            self.attributesSectionOpen = ko.observable(true);
            self.barriersSectionOpen = ko.observable(true);
            self.tasksSectionOpen = ko.observable(true);
            self.interventionsSectionOpen = ko.observable(true);
            self.selectedFocusAreas = ko.observableArray();
            self.addBarrier = function (goal) {
                self.barriersSectionOpen(true);
                self.addEntity('Barrier', goal);
            };
            self.addTask = function (goal) {
                var startDate = new Date();
                self.tasksSectionOpen(true);
                self.addEntity('Task', goal, startDate);
            };
            self.addIntervention = function (goal) {
                self.interventionsSectionOpen(true);
                var startDate = new Date();
                // var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
                //     return caremanager.id() === session.currentUser().userId();
                // });
                self.addEntity('Intervention', goal, startDate, session.currentUser().userId());
            };
            self.removeBarrier = function (barrier) {
                ko.utils.arrayForEach(barrier.goal().tasks(), function (task) {
                    var thisTaskBarrierId = ko.utils.arrayFirst(task.barrierIds(), function (barId) {
                        // If the barrier id is equal to this barrier id,
                        return barId.id() === barrier.id();
                    });
                    // If a barrier id is returned,
                    if (thisTaskBarrierId) {
                        // Remove it from the list
                        task.barrierIds.remove(thisTaskBarrierId);
                    }
                });
                ko.utils.arrayForEach(barrier.goal().interventions(), function (intervention) {
                    var thisTaskBarrierId = ko.utils.arrayFirst(intervention.barrierIds(), function (barId) {
                        // If the barrier id is equal to this barrier id,
                        return barId.id() === barrier.id();
                    });
                    // If a barrier id is returned,
                    if (thisTaskBarrierId) {
                        // Remove it from the list
                        intervention.barrierIds.remove(thisTaskBarrierId);
                    }
                });
                // Trigger the goal to refresh
                barrier.entityAspect.rejectChanges();
                if (barrier && barrier.goal()) {
                    barrier.goal().barriers.remove(barrier);
                }
                if (barrier && barrier.patientGoalId) {
                    barrier.patientGoalId(null);
                }
            }
            self.removeTask = function (task) {
                // Trigger the goal to refresh
                task.entityAspect.rejectChanges();
                if (task && task.goal()) {
                    task.goal().tasks.remove(task);
                }
                if (task && task.patientGoalId) {
                    task.patientGoalId(null);
                }
            }
            self.removeIntervention = function (intervention) {
                // Trigger the goal to refresh
                intervention.entityAspect.rejectChanges();
                if (intervention && intervention.goal()) {
                    intervention.goal().interventions.remove(intervention);
                }
                if (intervention && intervention.patientGoalId) {
                    intervention.patientGoalId(null);
                }
            }
            self.cancelAdd = function (entity) {
                // Trigger the goal to refresh
                entity.entityAspect.rejectChanges();
                entity.goal(null);
                // Remove the association
                entity.patientGoalId(null);
            }
            self.canSave = ko.computed(function () {
                // The active goal needs a name and a status to save
                return (self.activeGoal.name() && self.activeGoal.sourceId());
            });
            self.save = function (goal) {
                // TODO : Call the save goal method
                datacontext.saveGoal(goal);
                goalsIndex.activeGoal(null);
                goalsIndex.isEditing(false);
            };
            self.cancel = function (goal) {
                goal.entityAspect.rejectChanges();
                ko.utils.arrayForEach(goal.tasks(), function (task) {
                    task.entityAspect.rejectChanges();
                });
                ko.utils.arrayForEach(goal.barriers(), function (barrier) {
                    barrier.entityAspect.rejectChanges();
                });
                ko.utils.arrayForEach(goal.interventions(), function (intervention) {
                    intervention.entityAspect.rejectChanges();
                });
                goalsIndex.activeGoal(null);
                goalsIndex.isEditing(false);
            };
        };

        ctor.prototype.addEntity = function (type, goal, startDate, assignedToId) {
            var self = this;
            var thisPatientId = self.activeGoal.patientId();
            var thisGoalId = goal.id();
            datacontext.initializeEntity(null, type, thisPatientId, thisGoalId).then(entityReturned);

            function entityReturned(data) {
                var thisId = data.httpResponse.data.Id;
                if (thisId) {
                    var params = {};
                    params.id = thisId;
                    params.patientGoalId = thisGoalId;
                    params.statusId = 1;
                    if (startDate) {
                        params.startDate = startDate;
                    }
                    if (assignedToId) {
                        params.assignedToId = assignedToId;
                    }
                    var thisEntity = datacontext.createEntity(type, params);
                }
                else {
                    var thisTask = data.results[0];
                    thisTask.startDate(new Date());
                    thisTask.statusId(1);
                    thisTask.patientGoalId(thisGoalId);
                }
            }
        }

        ctor.prototype.attached = function () {
        };

        return ctor;
    });