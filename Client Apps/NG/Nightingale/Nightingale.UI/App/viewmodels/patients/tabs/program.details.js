define(['services/datacontext', 'viewmodels/shell/shell', 'models/base'], function (datacontext, shell, modelConfig) {

    var ctor = function () {
        var self = this;
        self.modalShowing = ko.observable(false);
        self.modalEntity = ko.observable(new ModalEntity(self.modalShowing));
    };

    ctor.prototype.activate = function (settings) {
        var self = this;
        self.settings = settings;
        self.tab = self.settings.tab;
        self.activeProgram = self.settings.activeProgram;
        self.modalEntity().program(self.activeProgram);
        self.saveOverride = function () {
            var inProgActs = getInProgressActions(self.activeProgram);
            // Accept the modules changes
            self.activeProgram.entityAspect.acceptChanges();
            // Go through and accept changes for each action
            ko.utils.arrayForEach(self.activeProgram.modules(), function (mod) {
                ko.utils.arrayForEach(mod.actions(), function (act) {
                    act.entityAspect.acceptChanges();
                });
            });
            var programId = self.activeProgram.id();
            var patientId = self.activeProgram.patientId();
            datacontext.savePlanElemAttr(self.activeProgram, programId, patientId).then(saveCompleted);

            function saveCompleted() {
                setInProgressActions(inProgActs);
            }
        };
        self.cancelOverride = function () {
            datacontext.cancelEntityChanges(self.modalEntity().program());
        };
		var modalSettings = {
			title: 'Individual Attributes',
			showSelectedPatientInTitle: true,
			entity: self.modalEntity, 
			templatePath: 'viewmodels/templates/program.edit', 
			showing: self.modalShowing, 
			saveOverride: self.saveOverride, 
			cancelOverride: self.cancelOverride, 
			deleteOverride: null, 
			classOverride: null
		}
        self.modal = new modelConfig.modal(modalSettings);
        self.descriptionSectionOpen = ko.observable(false);
        self.individualAttributesSectionOpen = ko.observable(true);
        self.attributesSectionOpen = ko.observable(false);
        self.toggleModalShowing = function ()  {
            shell.currentModal(self.modal);
            self.modalShowing(!self.modalShowing());
        };
        self.canEdit = ko.computed(function () {
            var result = self.activeProgram && self.activeProgram.elementStateModel().name() !== 'Completed' && self.activeProgram.elementStateModel().name() !== 'Closed';
            return result;
        });
    };
    
    return ctor;

    function ModalEntity(modalShowing) {
        var self = this;
        self.program = ko.observable();
        self.canSaveObservable = ko.observable(!datacontext.programsSaving);
        self.canSave = ko.computed({
            read: function () {
                return !datacontext.programsSaving() && self.canSaveObservable();
            },
            write: function (newValue) {
                self.canSaveObservable(newValue);
            }
        });
        // Object containing parameters to pass to the modal
        self.activationData = { program: self.program, canSave: self.canSave, showing: modalShowing };
    }

    function getInProgressActions(program) {
        var theseActions = [];
        ko.utils.arrayForEach(program.modules(), function (mod) {
            ko.utils.arrayForEach(mod.actions(), function (act) {
                // Add actions with element state of 4
                if (act.elementState() === 4) {
                    theseActions.push(act);
                }
            });
        });
        return theseActions;
    }

    function setInProgressActions(actions) {
        ko.utils.arrayForEach(actions, function (act) {
            if (act.elementState() !== 4) {
                act.stateUpdatedOn(new moment().toISOString());
                act.elementState(4);
            }
        });
    }
});