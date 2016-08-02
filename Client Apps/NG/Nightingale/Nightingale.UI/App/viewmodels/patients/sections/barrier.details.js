define(['models/base', 'config.services', 'services/datacontext', 'services/session', 'viewmodels/patients/goals/index', 'viewmodels/shell/shell'],
    function (modelConfig, servicesConfig, datacontext, session, goalsIndex, shell) {

        var ctor = function () {
            var self = this;
        };

        var modalShowing = ko.observable(true);

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };
            self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
            self.settings = settings;
            self.barrier = self.settings.barrier;
            self.computedBarrier = ko.computed(function () { return self.barrier; }).extend({ throttle: 75 });
            self.isExpanded = ko.observable(false);
            self.isDetailsExpanded = ko.observable(false);
            self.hasDetails = ko.computed( function(){
                var details = self.barrier.details();
                return (details != null && details.length > 0);
            });
            self.toggleIsExpanded = function () {
                self.isExpanded( !self.isExpanded() );
            };
            self.toggleDetailsExpanded = function(){
                var isOpen = self.isDetailsExpanded();
                var details = self.barrier.details();
                if( !details && !isOpen ){
                    return;
                }
                self.isDetailsExpanded( !self.isDetailsExpanded() );
            }

            self.editBarrier = function (barrier) {
                var modalEntity = ko.observable(new ModalEntity(barrier, 'name'));
                var saveOverride = function () {
                    saveBarrier(barrier);
                };
                var cancelOverride = function () {
                    cancel(barrier);
                    getGoalDetails(barrier.goal());
                };
                editEntity('Edit Barrier', modalEntity, 'viewmodels/templates/barrier.edit', saveOverride, cancelOverride);
            };

            self.deleteBarrier = function (barrier) {
                var result = confirm('You are about to delete a barrier.  Press OK to continue, or cancel to return without deleting.');
                if (result === true) {
                    var thisGoal = barrier.goal();
                    ko.utils.arrayForEach(barrier.goal().tasks(), function (task) {
                        var thisTaskBarrierId = ko.utils.arrayFirst(task.barrierIds(), function (barId) {
                            return barId.id() === barrier.id();
                        });
                        if (thisTaskBarrierId) {
                            task.barrierIds.remove(thisTaskBarrierId);

                            datacontext.saveTask(task);
                        }
                    });
                    ko.utils.arrayForEach(barrier.goal().interventions(), function (intervention) {
                        var thisTaskBarrierId = ko.utils.arrayFirst(intervention.barrierIds(), function (barId) {
                            return barId.id() === barrier.id();
                        });
                        if (thisTaskBarrierId) {
                            intervention.barrierIds.remove(thisTaskBarrierId);
                            datacontext.saveIntervention(intervention);
                        }
                    });
                    barrier.entityAspect.rejectChanges();
                    barrier.deleteFlag(true);
                    datacontext.saveBarrier(barrier).then(saveCompleted);

                    function saveCompleted() {
                        if (barrier && barrier.goal()) {
                            barrier.goal().barriers.remove(barrier);
                        }
                        if (barrier && barrier.patientGoalId) {
                            barrier.patientGoalId(null);
                        }
                    }
                }
                else {
                    return false;
                }
            };
        };

        function editEntity (msg, entity, path, saveoverride, canceloverride) {
            var modalSettings = {
                title: msg,
                showSelectedPatientInTitle: true,
                entity: entity,
                templatePath: path,
                showing: modalShowing,
                saveOverride: saveoverride,
                cancelOverride: canceloverride,
                deleteOverride: null,
                classOverride: null
            }
            var modal = new modelConfig.modal(modalSettings);
            modalShowing(true);
            shell.currentModal(modal);
        }

        function saveBarrier (barrier) {
            datacontext.saveBarrier(barrier);
        }

        function cancel (item) {
            item.entityAspect.rejectChanges();
        }

        return ctor;

        function getGoalDetails(goal) {
            goalsIndex.getGoalDetails(goal, true);
        }

        function ModalEntity(entity, reqpropname) {
            var self = this;
            self.entity = entity;
            self.activationData = { entity: self.entity };
            self.canSave = ko.computed(function () {
                var result = self.entity[reqpropname]();
                return result;
            });
        }

    });