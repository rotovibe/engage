define(['config.models', 'config.services', 'services/datacontext', 'services/session'],
    function (models, servicesConfig, datacontext, session) {

        var ctor = function () {
            var self = this;
        };
        
        ctor.prototype.activate = function (settings) {
            var self = this;
            self.alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };
            self.alphabeticalDescriptionSort = function (l, r) { return (l.description() == r.description()) ? (l.description() > r.description() ? 1 : -1) : (l.description() > r.description() ? 1 : -1) };
            self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
            self.settings = settings;
            self.activeGoal = self.settings.activeGoal;
            //self.contractProgramsEndPoint = ko.computed(function () {
            //    var currentUser = session.currentUser();
            //    var thisProgram = self.program();
            //    if (!currentUser || !thisProgram) {
            //        return '';
            //    }
            //    return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + thisProgram.patientId() + '/Program/' + thisProgram.id(), 'Program');
            //});
        };

        ctor.prototype.attached = function () {
        };

        return ctor;
    });