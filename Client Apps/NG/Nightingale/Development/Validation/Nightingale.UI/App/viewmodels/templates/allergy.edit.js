define(['services/datacontext'],
  function (datacontext) {

    var ctor = function () {
      var self = this;
    };

    ctor.prototype.allergyTypes = ko.computed(datacontext.enums.allergyTypes);
    ctor.prototype.severities = ko.computed(datacontext.enums.severities);
    ctor.prototype.reactions = ko.computed(datacontext.enums.reactions);
    ctor.prototype.allergySources = ko.computed(datacontext.enums.allergySources);
    ctor.prototype.allergyStatuses = ko.computed(datacontext.enums.allergyStatuses);

    ctor.prototype.removeAllergy = function (sender) {
      sender.entityAspect.rejectChanges();
      sender.deleteFlag(true);
    };

    ctor.prototype.activate = function (settings) {
      var self = this;
      self.settings = settings;
      self.showing = ko.computed(function () { return true; });
      self.selectedPatient = self.settings.selectedPatient;
      self.allergy = self.settings.allergy;
    };

    ctor.prototype.attached = function () {

    };

    return ctor;
  });