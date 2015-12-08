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
            self.task = ko.unwrap(self.settings.entity);
			self.existingDetailsOpen = ko.observable(false);
			self.toggleOpen = function () {
				self.existingDetailsOpen(!self.existingDetailsOpen());
			};
            self.goalTaskStatuses = datacontext.enums.goalTaskStatuses;
            // Decides whether we can change status or not
            self.isNew = (self.task && self.task.goal()) ? self.task.goal().isNew : function () { return false; };
            self.computedBarriers = ko.computed(self.task.goal().barriers);
            self.canSave = self.settings.canSave ? self.settings.canSave : true;
            self.showing = self.settings.showing ? self.settings.showing : true;
        };

        ctor.prototype.attached = function () {

        };

        return ctor;
    });