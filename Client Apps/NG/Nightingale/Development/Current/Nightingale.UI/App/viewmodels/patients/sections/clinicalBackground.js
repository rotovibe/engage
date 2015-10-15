define(['models/base', 'services/datacontext', 'viewmodels/shell/shell'],
    function (modelConfig, datacontext, shell) {

        var ctor = function () {

        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.selectedPatient = self.settings.selectedPatient;
            self.clinicalBackground = self.selectedPatient.clinicalBackground;
            self.backgroundModalShowing = ko.observable(false);
            self.saveBackground = function () {
                datacontext.saveIndividual(self.selectedPatient);
            }
            self.cancelBackground = function () {
                self.selectedPatient.entityAspect.rejectChanges();
            }
			var modalSettings = {
				title: 'Edit Clinical Background',
				showSelectedPatientInTitle: true,
				entity: self.selectedPatient, 
				templatePath: 'templates/clinicalBackground.html', 
				showing: self.backgroundModalShowing, 
				saveOverride: self.saveBackground, 
				cancelOverride: self.cancelBackground, 
				deleteOverride: null, 
				classOverride: null
			}
            self.modal = new modelConfig.modal(modalSettings);
            self.isOpen = ko.observable(false);
            self.isEditing = ko.observable(false);
            self.isExpanded = ko.observable(false);
            self.toggleEditing = function () {
                if (self.isEditing()) {
                    self.backgroundModalShowing(false);
                }
                else {
                    shell.currentModal(self.modal);
                    self.backgroundModalShowing(true);
                    var editingToken = self.backgroundModalShowing.subscribe(function () {
                        self.isEditing(false);
                        editingToken.dispose();
                    });
                }
                self.isEditing(!self.isEditing());
                self.isOpen(true);
            };
        };

        ctor.prototype.attached = function () {
        };

        return ctor;
    });