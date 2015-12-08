define(['models/base', 'viewmodels/patients/index'],
    function (modelConfig, patientsIndex) {

        var ctor = function () {
            var self = this;
            self.selectedPatient = ko.computed(patientsIndex.selectedPatient);
        };

        ctor.prototype.createQuickAddSections = function () {
            var self = this;
            self.quickAddSections.push(new modelConfig.quickAddSection('notes', 'Notes', 'icon-notes', 'viewmodels/shell/quickadd/notes'));
            self.quickAddSections.push(new modelConfig.quickAddSection('todo', 'To Do', 'icon-notes', 'viewmodels/shell/quickadd/todo'));
            self.quickAddSections.push(new modelConfig.quickAddSection('assign', 'Assign', 'icon-assign-patient', 'viewmodels/shell/quickadd/assign'));
            self.activeQuickAddTab(self.quickAddSections()[0]);
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.isShowing = ko.observable(false);
            // Sections to show in the quick add popover
            self.quickAddSections = ko.observableArray();
            self.activeQuickAddTab = ko.observable();
            self.createQuickAddSections();
            self.setActiveTab = function (tab) {
                self.activeQuickAddTab(tab);
            };
            self.patientIsSelected = ko.computed(function () {
                if (self.selectedPatient()) {
                    return true;
                }
                return false;
            });
        };
        
        ctor.prototype.attached = function () {
            var self = this;
        };

        return ctor;
    });