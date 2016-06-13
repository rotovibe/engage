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
            self.intervention = self.settings.activeIntervention;
            self.isFullScreen = ko.observable(false);
            self.isDetailsExpanded = ko.observable(false);
            self.hasDetails = ko.computed( function(){
                var details = self.intervention() ? self.intervention().details() : [];
                return (details != null && details.length > 0);
            });
            self.toggleFullScreen = function () {
                self.isFullScreen(!self.isFullScreen());
            };
            self.toggleDetailsExpanded = function(){
                var isOpen = self.isDetailsExpanded();
                var details = self.intervention().details();
                if( !details && !isOpen ){
                    return;
                }
                self.isDetailsExpanded( !self.isDetailsExpanded() );
            };

            self.addBarrier = function (intervention) {
                goalsIndex.addEntity('Barrier', intervention.goal()).then(doSomething);

                function doSomething(barrier) {
                    var newBarrierId = datacontext.createComplexType('Identifier', { id: barrier.id() });
                    self.intervention().barrierIds().push(newBarrierId);
                    self.editBarrier(barrier, 'Add Barrier');
                }
            };

            self.editBarrier = function (barrier, msg) {
                var thisGoal = barrier.goal();
                var modalEntity = ko.observable(new ModalEntity(barrier, 'name'));
                var saveOverride = function () {
                    saveBarrier(barrier)
                    saveIntervention(self.intervention());
                };
                var cancelOverride = function () {
                    cancel(barrier);
                    cancel(self.intervention());
                    getGoalDetails(thisGoal);
                };
                msg = msg ? msg : 'Edit Barrier';
                editEntity(msg, modalEntity, 'viewmodels/templates/barrier.edit', saveOverride, cancelOverride);
            };


            self.editIntervention = function (intervention) {
                getGoalDetails(intervention.goal());
                var modalEntity = ko.observable(new ModalEntity(intervention, 'description'));
                var saveOverride = function () {
                    saveIntervention(intervention);
                };
                var cancelOverride = function () {
                    intervention.clearDirty();
                    cancel(intervention);
                    getGoalDetails(intervention.goal());
                };
                editEntity('Edit Intervention', modalEntity, 'viewmodels/templates/intervention.edit', saveOverride, cancelOverride);
            };
            self.deleteIntervention = function (intervention) {
                var result = confirm('You are about to delete an intervention.  Press OK to continue, or cancel to return without deleting.');
                if (result === true) {
                    var thisGoal = intervention.goal();
                    intervention.entityAspect.rejectChanges();
                    intervention.deleteFlag(true);
                    datacontext.saveIntervention(intervention).then(saveCompleted);
                    self.settings.activeIntervention(null);

                    function saveCompleted () {
                        if (intervention && intervention.goal()) {
                            intervention.goal().interventions.remove(intervention);
                        }
                        if (intervention && intervention.patientGoalId) {
                            intervention.patientGoalId(null);
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
            barrier.checkAppend();
            datacontext.saveBarrier(barrier);
        }

        function saveIntervention(intervention) {
            function saved(){
                intervention.clearDirty();
            }
            intervention.checkAppend();
            datacontext.saveIntervention(intervention).then(saved);
        }

        function cancel (item) {
            item.entityAspect.rejectChanges();
        }

        return ctor;

        function getGoalDetails(goal) {
            goalsIndex.getGoalDetails(goal, true);
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

    });