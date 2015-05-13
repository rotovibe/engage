define(['services/datacontext'],
    function (datacontext) {

        var ctor = function () {
            var self = this;
            self.alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };
            self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.barrier = self.settings.entity;
            // Decides whether we can change status or not
            self.isNew = self.barrier.goal().isNew;            
            self.barrierStatuses = datacontext.enums.barrierStatuses;
            self.barrierCategories = datacontext.enums.barrierCategories;
            self.canSave = self.settings.canSave ? self.settings.canSave : true;
            self.showing = self.settings.showing ? self.settings.showing : true;
        };

        ctor.prototype.attached = function () {

        };

        return ctor;
    });