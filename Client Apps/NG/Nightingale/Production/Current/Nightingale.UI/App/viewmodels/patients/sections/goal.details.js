define(['models/base', 'config.services', 'services/datacontext', 'services/session', 'viewmodels/patients/goals/index', 'viewmodels/shell/shell'],
    function (modelConfig, servicesConfig, datacontext, session, goalsIndex, shell) {

        var modalShowing = ko.observable(true);

        var ctor = function () {
            var self = this;
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };
            self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
            self.settings = settings;
            self.goal = self.settings.activeGoal;
            self.selectedPatient = goalsIndex.selectedPatient;
            self.isFullScreen = ko.observable(false);
            self.computedGoal = ko.computed(function () { return self.goal; }).extend({ throttle: 60 });
            self.isGoalDetailsExpanded = ko.observable(false);
            self.hasDetails = ko.computed( function(){
                var details = self.goal() ? self.goal().details() : [];
                return (details != null && details.length > 0);
            });
            self.toggleFullScreen = function () {
                self.isFullScreen(!self.isFullScreen());
            };
            self.toggleGoalDetailsExpanded = function(){
                var isOpen = self.isGoalDetailsExpanded();
                var details = self.goal().details();
                if( !details && !isOpen ){
                    return;
                }
                self.isGoalDetailsExpanded( !self.isGoalDetailsExpanded() );
            }
            self.edit = function () {
                goalsIndex.editGoal(self.goal(), 'Edit Goal');
            }
            self.delete = function () {
                var result = confirm('You are about to delete a goal.  Press OK to continue, or cancel to return without deleting.');
                if (result === true) {
                    datacontext.deleteGoal(self.goal());
                    self.settings.activeGoal(null);
                }
                else {
                    return false;
                }
            };
            self.addBarrier = function (goal) {
                goalsIndex.addEntity('Barrier', goal).then(doSomething);

                function doSomething(barriers) {
                    self.editBarrier(barriers, 'Add Barrier');
                }
            };
            self.editBarrier = function (barrier, msg) {
                var thisGoal = barrier.goal();
                var modalEntity = ko.observable(new ModalEntity(barrier, 'name'));
                var saveOverride = function () {
                    saveBarrier(barrier);
                };
                var cancelOverride = function () {
                    cancel(barrier);
                    getGoalDetails(thisGoal);
                };
                msg = msg ? msg : 'Edit Barrier';
                editEntity(msg, modalEntity, 'viewmodels/templates/barrier.edit', saveOverride, cancelOverride);
            };
        };

        function save (goal) {
            datacontext.saveGoal(goal);
        }

        function saveBarrier (barrier) {
            barrier.checkAppend();
            datacontext.saveBarrier(barrier);
        }

        function cancel(item) {
            item.entityAspect.rejectChanges();
        }

        function getGoalDetails (goal) {
            goalsIndex.getGoalDetails(goal, true);
        };

        function editEntity (msg, entity, path, saveoverride, canceloverride) {
            var modalSettings = {
                title: msg,
                showSelectedPatientInTitle: true,
                entity: entity,
                templatePath: path,
                showing: modalShowing,
                saveOverride: saveoverride,
                cancelOverride: canceloverride
            }
            var modal = new modelConfig.modal(modalSettings);
            modalShowing(true);
            shell.currentModal(modal);
        }

        function ModalEntity(entity) {
            var self = this;
            self.entity = entity;
            self.activationData = { entity: self.entity };
            self.canSave = ko.computed(function () {
                var result = self.entity.isValid();
                return result;
            });
        }

        ctor.prototype.detached = function() {
            var self = this;
            self.hasDetails.dispose();
            self.computedGoal.dispose();
        }
        return ctor;
    });