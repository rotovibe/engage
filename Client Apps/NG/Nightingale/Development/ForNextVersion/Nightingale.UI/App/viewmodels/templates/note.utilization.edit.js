/**
*	edit utilization dialog screen
*	@module	note.utilization.edit
*/
define(['services/datacontext'],
    function (datacontext) {

        var ctor = function () {
          var self = this;
          self.alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };
          self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
					// Available properties of a note
					self.visitTypes = datacontext.enums.visitTypes;
					self.utilizationSources = datacontext.enums.utilizationSources;
					self.dispositions = datacontext.enums.dispositions;
					self.utilizationLocations = datacontext.enums.utilizationLocations;
					self.existingContentOpen = ko.observable(false);
					self.toggleOpen = function () {
						self.existingContentOpen(!self.existingContentOpen());
					};
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.note = self.settings.note;

            self.availablePrograms = ko.computed(function () {
                var computedPrograms = [];
                if (self.note.patient()) {
                    var thesePrograms = self.note.patient().programs.slice(0).sort(self.alphabeticalNameSort);
                    ko.utils.arrayForEach(thesePrograms, function (program) {
                        if (program.elementState() !== 6 && program.elementState() !== 1 && program.elementState() !== 5) {
                            computedPrograms.push(program);
                        }
                    });
                }
                return computedPrograms;
            });
            self.canSave = false; //this is temporary. TODO:  self.settings.canSave ? self.settings.canSave : true;
            self.showing = self.settings.showing ? self.settings.showing : true;
        };

        ctor.prototype.attached = function () {

        };

        return ctor;
    });