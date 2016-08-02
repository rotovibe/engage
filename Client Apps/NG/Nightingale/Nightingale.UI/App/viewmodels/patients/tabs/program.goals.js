define([], function () {

    var ctor = function () {
        var self = this;
    };

    ctor.prototype.activate = function (settings) {
        var self = this;
        self.settings = settings;
        self.tab = self.settings.tab;
        self.activeProgram = self.settings.activeProgram;
    };

    return ctor;
});