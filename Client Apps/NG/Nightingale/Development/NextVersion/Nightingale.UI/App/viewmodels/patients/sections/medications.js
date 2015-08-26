define(['models/base', 'config.services', 'services/datacontext', 'services/session', 'viewmodels/patients/careplan/index'],
    function (models, servicesConfig, datacontext, session, carePlanIndex) {

        var ctor = function () {
            var self = this;
        };

        ctor.prototype.contractProgramsEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser || !carePlanIndex.selectedPatient()) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + carePlanIndex.selectedPatient().id(), 'Program');
        });
        
        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.selectedPatient = self.settings.selectedPatient;
            self.programs = self.selectedPatient.programs;
            self.passedInSort = self.settings.sortOverride;
            self.elementStateIds = self.settings.elementStateIds;
            self.selectedSort = self.passedInSort ? self.passedInSort : self.alphabeticalOrderSort;
            self.computedPrograms = ko.computed(function () {
                var thesePrograms = [];
                if (carePlanIndex.selectedPatient()) {
                    thesePrograms = ko.utils.arrayFilter(carePlanIndex.selectedPatient().programs(), function (thisProgram) {
                        return self.elementStateIds.indexOf(thisProgram.elementState()) !== -1;
                    });
                }
                return thesePrograms;
            });
            self.activeAction = self.settings.activeAction;
            self.activeProgram = self.settings.activeProgram;
            self.activeModule = self.settings.activeModule;
            self.setActiveAction = function (sender) {
                // Only set the active action if it is enabled
                if (sender.enabled() === true) {
                    self.activeProgram(null);
                    self.activeModule(null);
                    self.activeAction(sender);
                    if (sender.historicalAction && sender.historicalAction()) {
                        carePlanIndex.getStepsForAction(sender.historicalAction());
                    }
                }
            };
            self.setActiveProgram = function (sender) {
                // If the active program is not already the sent program,
                if (self.activeProgram() !== sender) {
                    // Set the active action to null so it hides,
                    self.activeAction(null);
                    self.activeModule(null);
                    // Set the active program to the sent program,
                    self.activeProgram(sender);
                    // And if we don't have the program details yet,
                    if (self.activeProgram().modules().length === 0) {
                        //Go get the program details
                        datacontext.getMelsEntityById(null, null, self.contractProgramsEndPoint().EntityType, self.contractProgramsEndPoint().ResourcePath + '/Program/' + self.activeProgram().id(), true, null).then(function () { patientsListFlyoutOpen(false); });
                    }                    
                }
            };
            self.setActiveModule = function (sender) {
                // If the active program is not already the sent program,
                if (self.activeModule() !== sender) {
                    // Set the active action to null so it hides,
                    self.activeAction(null);
                    self.activeProgram(null);
                    // Set the active program to the sent program,
                    self.activeModule(sender);    
                }
            };
            self.toggleOpen = function (sender) {
                var thisProgram = sender;
                if (!thisProgram.isOpen()) {
                    thisProgram.isOpen(true);
                    if (thisProgram.isOpen() && thisProgram.modules().length === 0) {
                        //Go get the program details
                        datacontext.getMelsEntityById(null, null, self.contractProgramsEndPoint().EntityType, self.contractProgramsEndPoint().ResourcePath + '/Program/' + thisProgram.id(), true, null);
                    }
                } else {
                    thisProgram.isOpen(false);
                }
            };
            self.isFullScreen = ko.observable(false);
            self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
            self.toggleAllExpanded = function (sender) {
                // If any modules are closed,
                if (!sender.allModulesOpen()) {
                    // Open them all
                    ko.utils.arrayForEach(sender.modules(), function (module) {
                        module.isOpen(true);
                    });
                } else {
                    // Close them all
                    ko.utils.arrayForEach(sender.modules(), function (module) {
                        module.isOpen(false);
                    });
                }
            };
        };

        ctor.prototype.attached = function () {
        };

        return ctor;
    });