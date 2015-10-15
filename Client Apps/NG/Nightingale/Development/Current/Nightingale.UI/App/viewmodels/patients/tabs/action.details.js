define(['services/datacontext', 'viewmodels/shell/shell', 'models/base', 'services/dataservices/programsservice'], function (datacontext, shell, modelConfig, programsService) {

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
        self.activeAction = self.settings.activeAction;
        self.modalEntity().action(self.activeAction);
        self.saveOverride = function () {
            // If the action already has changes,
            var programId = self.activeAction.module().programId();
            var patientId = self.activeAction.module().program().patientId();
            // Grab the element state so we can reassign it after it is saved
            var elementStateId = self.activeAction.elementState();
            self.activeAction.entityAspect.acceptChanges();
            datacontext.savePlanElemAttr(self.activeAction, programId, patientId).then(saveCompleted);

            function saveCompleted() {
                // Reassign the element state
                self.activeAction.elementState(elementStateId);
            }
        };
        self.cancelOverride = function () {
            // Grab the element state so we can reassign it after it is cancelled
            var elementStateId = self.activeAction.elementState();            
            datacontext.cancelEntityChanges(self.modalEntity().action());
            self.activeAction.elementState(elementStateId);            
        };
        self.availableHistoricalActions = settings.availableHistoricalActions;
        self.selectedHistoricalAction = settings.selectedHistoricalAction;
		var modalSettings = {
			title: 'Individual Attributes',
			showSelectedPatientInTitle: true,
			entity: self.modalEntity, 
			templatePath: 'viewmodels/templates/action.edit', 
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
        self.shouldBeShowing = (self.activeAction && self.activeAction.objectives()) ? self.activeAction.objectives().length > 0 : false;
        self.computedObjectives = ko.computed(function () {
            var theseObjectives = [];
            if (self.settings.activeAction && self.settings.activeAction.objectives()) {
                var theseObjectives = self.settings.activeAction.objectives().sort(objectiveSort);
            }
            return theseObjectives;
        }).extend({ isShowing: self.shouldBeShowing });
        self.historicalComputedObjectives = ko.computed(function () {
            var theseObjectives = [];
            if (self.settings.activeAction && self.settings.activeAction.objectives()) {
                var theseObjectives = self.settings.activeAction.objectives().sort(objectiveSort);
            }
            return theseObjectives;
        }).extend({ isShowing: self.shouldBeShowing });
        self.toggleModalShowing = function ()  {
            shell.currentModal(self.modal);
            self.modalShowing(!self.modalShowing());
        };
        self.canEdit = ko.computed(function () {
            var result = self.activeAction && self.activeAction.elementStateModel().name() !== 'Completed';
            return result;
        });
    };
    
    return ctor;

    function ModalEntity(modalShowing) {
        var self = this;
        self.action = ko.observable();
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
        self.activationData = { action: self.action, canSave: self.canSave, showing: modalShowing  };
    }

    function checkForActionChanges(action) {
        ko.utils.arrayForEach(action.steps.peek(), function (step) {
            if (step.entityAspect.entityState.isAddedModifiedOrDeleted()) {
                return true;
            }
            ko.utils.arrayForEach(step.responses.peek(), function (response) {
                if (response.entityAspect.entityState.isAddedModifiedOrDeleted()) {
                    return true;
                }
            });
        });
        return false;
    }

});