/**
*
*	@module	note.touchpoint.edit
*/
define(['services/datacontext'],
	function (datacontext) {

		var ctor = function () {
			var self = this;
			self.alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };
			self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
			// Available properties of a note
			self.methods = datacontext.enums.noteMethods;
			self.whos = datacontext.enums.noteWhos;
			self.sources = datacontext.enums.noteSources;
			self.outcomes = datacontext.enums.noteOutcomes;
			self.existingContentOpen = ko.observable(false);
			self.toggleOpen = function () {
				self.existingContentOpen(!self.existingContentOpen());
			};
		};

		ctor.prototype.activate = function (settings) {
			var self = this;
			self.settings = settings;
			self.note = self.settings.note;
			self.focusAreas = datacontext.enums.focusAreas;

			self.availablePrograms = ko.computed(function () {
				var computedPrograms = [];
				if (self.note.patient()) {
					var thesePrograms = self.note.patient().programs.slice(0).sort(self.alphabeticalNameSort);
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