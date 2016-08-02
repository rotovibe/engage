define(['services/datacontext'],
  function (datacontext) {

    var ctor = function () {
      var self = this;
    };

	var descendingDateSort = function (l, r) { return (l.createdOn() == r.createdOn()) ? (l.createdOn() < r.createdOn() ? 1 : -1) : (l.createdOn() < r.createdOn() ? 1 : -1) };

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
      // A list of the problem allergies for this patient
      self.newPatientAllergies = ko.computed(function () {
        var filteredPatAllergies = [];
        var patientAllergies = self.selectedPatient().allergies();
        // Filter the list only to patientAllergies that are of type 'Problem'
        filteredPatAllergies = ko.utils.arrayFilter(patientAllergies, function (item) {
          // If the item has a type, return if it matches problem, else if there is no type return false
          return !item.deleteFlag() && item.isNew();
        });
        return filteredPatAllergies.sort(descendingDateSort);
      }).extend({ throttle: 50 });
      // TODO: Remove dead code
      // self.showActions = self.settings.hasOwnProperty('showActions') ? self.settings.showActions : self.showing;
      // self.saveFocusProblems = self.settings.saveFocusProblems || function () { return false; };
      // self.cancelFocusProblems = self.settings.cancelFocusProblems || function () { return false; };
    };

    ctor.prototype.attached = function () {

    };

    return ctor;
  });