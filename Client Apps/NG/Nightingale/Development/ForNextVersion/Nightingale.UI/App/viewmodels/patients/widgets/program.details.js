define(['services/datacontext', 'models/base', 'viewmodels/shell/shell'],
    function (datacontext, modelConfig, shell) {

        function tab (name, path, activeTab) {
            var self = this;
            self.name = ko.observable(name);
            self.path = ko.observable(path);
            self.isActive = ko.computed(function () {
                var thisActiveTab = activeTab();
                return (thisActiveTab && thisActiveTab.name() === self.name());
            });
        };

        var ctor = function () {
            var self = this;
            self.activeTab = ko.observable();
            self.tabs = ko.observableArray([
                new tab('Details', 'viewmodels/patients/tabs/program.details', self.activeTab),
                new tab('Objectives', 'viewmodels/patients/tabs/program.objectives', self.activeTab),
            ]);
            self.programActions = ko.observableArray([{ text: ko.observable('Remove') }]);
            self.selectedAction = ko.observable();
            self.reason = ko.observable();
            self.modalShowing = ko.observable(false);
            self.modalEntity = ko.observable(new ModalEntity(self.modalShowing, self.reason));
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.activeProgram = self.settings.activeProgram;
            self.isFullScreen = ko.observable(false);
            self.toggleFullScreen = function () {
                self.isFullScreen(!self.isFullScreen());
            };
            self.setActiveTab = function (sender) {
                self.activeTab(sender);
            };
            self.activeTab(self.tabs()[0]);
            self.modalEntity().program(self.activeProgram());
            self.cancelOverride = function () {
                self.reason(null);
            };
            self.deleteOverride = function () {
                datacontext.removeProgram(self.activeProgram(), self.reason()).then(deleteFinished).fail(deleteFailed);

                function deleteFinished() {
                    setTimeout(function () { location.reload(); }, 2000);
                }

                function deleteFailed(error) {
                    console.log(error);
                }
            };
			var modalSettings = {
				title: 'Remove '+ self.activeProgram().name() ,
				showSelectedPatientInTitle: true,
				entity: self.modalEntity, 
				templatePath: 'viewmodels/templates/program.remove', 
				showing: self.modalShowing, 
				saveOverride: null, 
				cancelOverride: self.cancelOverride, 
				deleteOverride: self.deleteOverride, 
				classOverride: null
			}
            self.modal = new modelConfig.modal(modalSettings);
            self.modal.canDelete(true);
            self.modal.deleteText('Remove');
            self.selectedAction.subscribe(function (newValue) {
                if (newValue && newValue.text() === 'Remove') {
                    shell.currentModal(self.modal);
                    self.modalShowing(true);
                    self.selectedAction(null);
                }
            });
            self.activeProgram.subscribe(function () {
                self.activeTab(self.tabs()[0]);
                if (self.activeProgram()) {
                    self.modalEntity().program(self.activeProgram());
					var modalSettings = {
						title: 'Remove '+ self.activeProgram().name() ,
						entity: self.modalEntity, 
						templatePath: 'viewmodels/templates/program.remove', 
						showing: self.modalShowing, 
						saveOverride: null, 
						cancelOverride: self.cancelOverride, 
						deleteOverride: self.deleteOverride, 
						classOverride: null
					}
                    self.modal = new modelConfig.modal(modalSettings);
                    self.modal.canDelete(true);
                    self.modal.deleteText('Remove');
                }
            });
        };

            ctor.prototype.attached = function () {

            };

            function ModalEntity(modalShowing, reason) {
                var self = this;
                self.program = ko.observable();
                self.canSave = ko.computed(function () {
                    return !datacontext.programsSaving();
                });
            // Object containing parameters to pass to the modal
            self.activationData = { program: self.program, showing: modalShowing, reason: reason };
        }

        return ctor;
    });