define([], function () {

    var ctor = function () {
        var self = this;
    };

    ko.extenders.isShowing = function (target, value) {
        target().showing = ko.observable(value);
        target.subscribe(function (newValue) {
            var isshowing = newValue.length > 0;
            target().showing(isshowing);
        });
    };

    var objectiveSort = function (l, r) { return (l.objective().name() == r.objective().name()) ? (l.objective().name() > r.objective().name() ? 1 : -1) : (l.objective().name() > r.objective().name() ? 1 : -1) };

    ctor.prototype.activate = function (settings) {
        var self = this;
        self.settings = settings;
        self.tab = self.settings.tab;
        self.activeProgram = self.settings.activeProgram;
        // A quick variable to decide on instantiation whether it should be showing or not
        self.shouldBeShowing = self.activeProgram.objectives() ? self.activeProgram.objectives().length > 0 : false;
        self.computedObjectives = ko.computed(function () {
            var theseObjectives = [];
            if (self.settings.activeProgram && self.settings.activeProgram.objectives()) {
                var theseObjectives = self.settings.activeProgram.objectives().sort(objectiveSort);   
            }
            return theseObjectives;
        }).extend({ isShowing: self.shouldBeShowing });
    };

    return ctor;
});