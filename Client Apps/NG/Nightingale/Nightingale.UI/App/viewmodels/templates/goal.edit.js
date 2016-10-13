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
            self.goal = self.settings.goal;
			self.existingDetailsOpen = ko.observable(false);
			self.toggleOpen = function () {
				self.existingDetailsOpen(!self.existingDetailsOpen());
			};
            self.focusAreas = datacontext.enums.focusAreas;
            self.sources = datacontext.enums.sources;
            self.goalTaskStatuses = datacontext.enums.goalTaskStatuses;            
            self.goalTypes = datacontext.enums.goalTypes;
            self.availablePrograms = ko.computed(function () {
                var computedPrograms = [];
                if (self.goal.patient()) {
                    var thesePrograms = self.goal.patient().programs.slice(0).sort(self.alphabeticalNameSort);
                    ko.utils.arrayForEach(thesePrograms, function (program) {
                        if (program.elementState() !== 1) {
                            if (program.elementState() != 5 && program.elementState() != 6) {
                                computedPrograms.push(program);
                            }
                                //5 or 6 within last 30 days
                            else {
                                var today = moment(new Date());
                                var stateUpdatedDate = moment(program.stateUpdatedOn());
                                if (today.diff(stateUpdatedDate, 'days') <= 30) {
                                    computedPrograms.push(program);
                                }
                            }
                        }
                    });
                }
                return computedPrograms;
            });
            self.canSave = self.settings.canSave ? self.settings.canSave : true;
            self.showing = self.settings.showing ? self.settings.showing : true;
        };

        ctor.prototype.attached = function () {

        };

        return ctor;
    });