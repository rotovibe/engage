/**
*	Individual Status section in individual details 
*	@module status
*/
define(['models/base', 'services/datacontext', 'viewmodels/shell/shell'],
    function (modelConfig, datacontext, shell) {

        var ctor = function () {

        };
		
        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.selectedPatient = self.settings.selectedPatient;            												
            self.statusModalShowing = ko.observable(false);
            self.saveStatus = function () {
                datacontext.saveIndividual(self.selectedPatient);
            }
            self.cancelStatus = function () {
                self.selectedPatient.entityAspect.rejectChanges();
            }
            self.modal = new modelConfig.modal('Edit Status', self.selectedPatient, 'templates/patient.status.html', self.statusModalShowing, self.saveStatus, self.cancelStatus);
            self.isOpen = ko.observable(true);
            self.isEditing = ko.observable(false);
            self.isExpanded = ko.observable(false);
            self.toggleEditing = function () {
                if (self.isEditing()) {
                    self.statusModalShowing(false);
                }
                else {
                    shell.currentModal(self.modal);
                    self.statusModalShowing(true);
                    var editingToken = self.statusModalShowing.subscribe(function () {
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