define(['viewmodels/shell/shell', 'viewmodels/patients/data/index', 'viewmodels/patients/careplan/index', 'models/base', 'services/datacontext'], function (shell, dataIndex, carePlanIndex, modelConfig, datacontext) {
    
    var ctor = function () {
        var self = this;
    };

    ctor.prototype.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };

    function ModalEntity() {
        var self = this;
        self.activeDataType = ko.observable();
        self.selectedPatient = dataIndex.selectedPatient;
        self.showDropdown = true;
        self.showActions = false;
        // Object containing parameters to pass to the modal
        self.activationData = { selectedPatient: self.selectedPatient, activeDataType: self.activeDataType, showDropdown: self.showDropdown, showActions: self.showActions };
        self.canSave = ko.observable(true);
    }

    ctor.prototype.activate = function (settings) {
        var self = this;
        self.settings = settings;        
        self.tab = self.settings.tab;
        self.activeAction = self.settings.activeAction;        
        self.availableHistoricalActions = settings.availableHistoricalActions;
        self.selectedHistoricalAction = settings.selectedHistoricalAction;
        self.modalShowing = ko.observable(false);
        self.programsSaving = ko.computed(datacontext.programsSaving);
        self.modalEntity = ko.observable(new ModalEntity());
        self.saveOverride = function () {
            self.modalEntity().activeDataType(null);
            dataIndex.saveAllData();
        };
        self.cancelOverride = function () {
            self.modalEntity().activeDataType(null);
            dataIndex.cancelDataEntry();
        };
		var modalSettings = {
			title: 'Data Entry',
			showSelectedPatientInTitle: true,
			entity: self.modalEntity, 
			templatePath: 'viewmodels/templates/clinical.dataentry', 
			showing: self.modalShowing, 
			saveOverride: self.saveOverride, 
			cancelOverride: self.cancelOverride, 
			deleteOverride: null, 
			classOverride: 'modal-lg'
		}
        self.modal = new modelConfig.modal(modalSettings);
        self.toggleModalShowing = function () {
			shell.currentModal(self.modal);
            self.modalShowing(!self.modalShowing());
        }
    };
    
    return ctor;
});