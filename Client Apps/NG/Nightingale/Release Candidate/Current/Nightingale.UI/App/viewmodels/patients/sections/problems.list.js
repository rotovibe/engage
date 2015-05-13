define(['services/datacontext'],
  function (datacontext) {

    var ctor = function () {
      var self = this;
    };

    ctor.prototype.newThenAlphabeticalSort = function (l, r) {

        // Primary sort property
        var p1 = l.isNew() ? 0 : 1;
        var p2 = r.isNew() ? 0 : 1;

        // Secondary sort property
        var o1 = l.name().toLowerCase();
        var o2 = r.name().toLowerCase();
            
        if (p1 != p2) {
            if (p1 < p2) return -1;
            if (p1 > p2) return 1;
            return 0;
        }
        if (o1 < o2) return -1;
        if (o1 > o2) return 1;
        return 0;
    };

    ctor.prototype.observationDisplays = ko.computed(datacontext.enums.observationDisplays);
    ctor.prototype.deleteFocusProblem = function (sender) {
      sender.entityAspect.rejectChanges();
      sender.deleteFlag(true);
    };
    ctor.prototype.observationStates = ko.computed(function () {
      // Find problem observation type from list
      var problemType = ko.utils.arrayFirst(datacontext.enums.observationTypes(), function (type) {
        return type.name() === 'Problems';
      });
      // Filter states down to only those who have allowed type id that contains the problem type id
      var theseStates = ko.utils.arrayFilter(datacontext.enums.observationStates(), function (state) {
        var truthy = false;
        ko.utils.arrayForEach(state.allowedTypeIds(), function (typeId) {
          // If the type is found to match,
          if (typeId.id() === problemType.id()) {
            // Return this state into the list
            truthy = true;
          }
        });
        return truthy;
      });
      return theseStates;
    });

    ctor.prototype.activate = function (settings) {
      var self = this;
      self.settings = settings;
      self.showing = ko.computed(function () { return true; });
      self.selectedPatient = self.settings.selectedPatient;
      // A list of the problem observations for this patient
      self.computedProblemObservations = ko.computed(function () {
        var filteredObservations = [];
        var patientsObservations = self.selectedPatient().observations();
        // Filter the list only to observations that are of type 'Problem'
        filteredObservations = ko.utils.arrayFilter(patientsObservations, function (item) {
          // If the item has a type, return if it matches problem, else if there is no type return false
          return item.type() ? (item.type().name() === 'Problems' && !item.deleteFlag() && (item.entityAspect.entityState.isModified() || item.state().name().toLowerCase() === 'active')) : false;
        }).sort(self.newThenAlphabeticalSort);
        return filteredObservations;
      }).extend({ throttle: 50 });
      // self.showActions = self.settings.hasOwnProperty('showActions') ? self.settings.showActions : self.showing;
      // self.saveFocusProblems = self.settings.saveFocusProblems || function () { return false; };
      // self.cancelFocusProblems = self.settings.cancelFocusProblems || function () { return false; };
    };

    ctor.prototype.attached = function () {

    };

    return ctor;
  });