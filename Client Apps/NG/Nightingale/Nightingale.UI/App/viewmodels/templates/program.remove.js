define(['services/datacontext'],
    function (datacontext) {

        var alphabeticalSort = function (l, r) { return (l.preferredName() == r.preferredName()) ? (l.preferredName() > r.preferredName() ? 1 : -1) : (l.preferredName() > r.preferredName() ? 1 : -1) };

        var ctor = function () {
            var self = this;
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.program = self.settings.program;
            self.showing = self.settings.showing;
            self.reason = self.settings.reason;
        };

        ctor.prototype.attached = function () {

        };

        return ctor;
    });