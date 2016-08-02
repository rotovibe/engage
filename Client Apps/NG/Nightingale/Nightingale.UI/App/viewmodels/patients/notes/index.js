/**
*
*	@module notes.index
*/
define(['services/datacontext'],
	function (datacontext) {

		var ctor = function () {
				var self = this;
		};

		ctor.prototype.activate = function (settings) {
			var self = this;
			self.settings = settings;
			self.note = self.settings.note;
			self.selectedTemplate = ko.observable();
			self.showing = ko.computed(function () { return !!self.note.type() });
			self.noteTypes = datacontext.enums.noteTypes;
			var type = self.note.type()? 	self.note.type().name().toLowerCase() : null;
			self.templatePath = ko.computed( function(){
				var type = self.note.type();
				var typeName = type ? type.name().toLowerCase() : null;
				switch( typeName ){
					case 'touchpoint':
					{
						return 'viewmodels/templates/note.touchpoint.edit';
						break;
					}
					case 'utilization':
					{
						return 'viewmodels/templates/note.utilization.edit';
						break;
					}
					case null:
					case undefined:
					{
						return null;
						break;
					}
					default:
					{
						return 'viewmodels/templates/note.general.edit';
						break;
					}
				}
				return null;
			});
			self.saveNote = self.settings.saveNote || function () { return false; };
			self.cancelNote = self.settings.cancelNote || function () { return false; };
			self.canSave = self.settings.canSave ? self.settings.canSave : true;
			self.showing = self.settings.showing ? self.settings.showing : true;
			self.validationErrors = ko.computed( function(){
				var currentNote = self.note;
				var isValid = currentNote.isValid();
				return currentNote.validationErrors();
			});
		};

	ctor.prototype.attached = function () {

	};

	return ctor;
});