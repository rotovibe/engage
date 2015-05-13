define(['config.models', 'config.services', 'services/datacontext', 'services/session'],
    function (models, servicesConfig, datacontext, session) {

        var ctor = function () {
            var self = this;
        };
        
        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.program = ko.observable(self.settings.program);
            self.activeAction = self.settings.activeAction;
            self.setActiveAction = function (sender) {
                // Only set the active action if it is enabled
                if (sender.enabled() === true) {
                    self.activeAction(sender);
                }
            };
            // Should the program be 'open' in the list?
            self.isOpen = ko.observable(false);
            // If the modules have already been loaded,
            if (self.program().modules().length !== 0) {
                // Open the program
                self.isOpen(true);
            }
            self.isOpen.subscribe(function () {
                if (self.isOpen() && self.program().modules().length === 0) {
                    //Go get the program details
                    datacontext.getMelsEntityById(self.program, null, self.contractProgramsEndPoint().EntityType, self.contractProgramsEndPoint().ResourcePath, true, null).then(function () { patientsListFlyoutOpen(false); });
                }
            });
            self.isFullScreen = ko.observable(false);
            self.contractProgramsEndPoint = ko.computed(function () {
                var currentUser = session.currentUser();
                var thisProgram = self.program();
                if (!currentUser || !thisProgram) {
                    return '';
                }
                return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + thisProgram.patientId() + '/Program/' + thisProgram.id(), 'Program');
            });
            self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
        };

        ctor.prototype.attached = function () {
        };

        return ctor;
    });