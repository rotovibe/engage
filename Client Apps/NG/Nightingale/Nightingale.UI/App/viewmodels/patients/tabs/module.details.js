define(['services/datacontext', 'viewmodels/shell/shell', 'models/base'], function (datacontext, shell, modelConfig) {

    ko.extenders.isShowing = function (target, value) {
        target().showing = ko.observable(value);
        target.subscribe(function (newValue) {
            var isshowing = newValue.length > 0;
            target().showing(isshowing);
        });
    };

    var objectiveSort = function (l, r) { return (l.objective().name() == r.objective().name()) ? (l.objective().name() > r.objective().name() ? 1 : -1) : (l.objective().name() > r.objective().name() ? 1 : -1) };

    var ctor = function () {
        var self = this;
        self.modalShowing = ko.observable(false);
        self.modalEntity = ko.observable(new ModalEntity(self.modalShowing));
    };

    ctor.prototype.activate = function (settings) {
        var self = this;
        self.settings = settings;
        self.tab = self.settings.tab;
        self.activeModule = self.settings.activeModule;
        self.modalEntity().module(self.activeModule);
        self.saveOverride = function () {
            var inProgActs = getInProgressActions(self.activeModule);
            // Accept the modules changes
            self.activeModule.entityAspect.acceptChanges();
            // Go through and accept changes for each action
            ko.utils.arrayForEach(self.activeModule.actions(), function (act) {
                act.entityAspect.acceptChanges();
            });
            var programId = self.activeModule.programId();
            var patientId = self.activeModule.program().patientId();
            datacontext.savePlanElemAttr(self.activeModule, programId, patientId).then(saveCompleted);

            function saveCompleted() {
                setInProgressActions(inProgActs);
            }
        };
        self.cancelOverride = function () {
            datacontext.cancelEntityChanges(self.modalEntity().module());
        };
		var modalSettings = {
			title:'Individual Attributes' ,
			showSelectedPatientInTitle: true,
			entity: self.modalEntity, 
			templatePath: 'viewmodels/templates/module.edit', 
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
        self.objectivesSectionOpen = ko.observable(false);        
        self.shouldBeShowing = (self.activeModule && self.activeModule.objectives()) ? self.activeModule.objectives().length > 0 : false;
        self.computedObjectives = ko.computed(function () {
            var theseObjectives = [];
            if (self.settings.activeModule && self.settings.activeModule.objectives()) {
                var theseObjectives = self.settings.activeModule.objectives().sort(objectiveSort);   
            }
            return theseObjectives;
        }).extend({ isShowing: self.shouldBeShowing });
        self.toggleModalShowing = function ()  {
            shell.currentModal(self.modal);
            self.modalShowing(!self.modalShowing());
        };        
        self.canEdit = ko.computed(function () {
            var result = self.activeModule && self.activeModule.elementStateModel().name() !== 'Completed';
            return result;
        });
    };
    
    return ctor;

    function ModalEntity(modalShowing) {
        var self = this;
        self.module = ko.observable();
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
        self.activationData = { module: self.module, canSave: self.canSave, showing: modalShowing };
    }

    function getInProgressActions(module) {
        var theseActions = ko.utils.arrayFilter(module.actions(), function (act) {
            // Add actions with element state of 4
            return act.elementState() === 4;
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