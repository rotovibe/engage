define(['models/base', 'config.services', 'services/datacontext', 'services/session'],
    function (modelConfig, servicesConfig, datacontext, session) {

        var ctor = function () {
            var self = this;
            // Categories to show in the category dropdown
            self.categories = ko.observableArray();
            // Category that is selected in the category dropdown
            self.selectedCategory = ko.observable();
            // Contract Programs to show in the program dropdown
            self.contractPrograms = ko.observableArray();
            // Program that is selected in the program dropdown
            self.selectedProgram = ko.observable();
            self.isSaving = ko.observable();
        };

        function Category(id, name) {
            var self = this;
            self.Id = ko.observable(id);
            self.Name = ko.observable(name);
        }

        ctor.prototype.contractProgramsEndPoint = ko.computed(function () {
            //var self = this;
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'programs/active', 'ContractProgram');
        });

        ctor.prototype.createCategories = function () {
            var self = this;
            self.categories.push(new Category(1, 'Programs'));
            self.selectedCategory(self.categories()[0]);
        };

        ctor.prototype.getContractProgramsByCategory = function () {
            var self = this;
            if (self.contractProgramsEndPoint()) {
                datacontext.getEntityList(self.contractPrograms, self.contractProgramsEndPoint().EntityType, self.contractProgramsEndPoint().ResourcePath, null, null, true, { ContractNumber: session.currentUser().contracts()[0].number() }, 'name');
            }
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.selectedPatient = settings.data.selectedPatient;
            self.isShowing = self.settings.data.isShowing;
            self.cancelAssign = function () {
                self.selectedProgram(null);
                self.isShowing(false);
            };
            self.startDate = ko.observable(new moment());
            self.saveAssign = function () {
                if (self.selectedProgram()) {
                    self.isSaving(true);
					function programAssigned() {
                        self.isShowing(false);
                        self.isSaving(false);
                        self.selectedProgram(null);
                    }
                    datacontext.saveChangesToPatientProperty(self.selectedPatient, 'Programs', null, [new modelConfig.Parameter('ContractProgramId', self.selectedProgram().id())])
								.then(programAssigned);                    
                }
            };
            self.canSave = ko.computed(function () {
                return self.selectedProgram() && !self.isSaving();
            });
            self.selectedCategory.subscribe(function () {
                self.getContractProgramsByCategory();
            });
            self.createCategories();
        };

        ctor.prototype.attached = function () {
        };

        return ctor;
    });