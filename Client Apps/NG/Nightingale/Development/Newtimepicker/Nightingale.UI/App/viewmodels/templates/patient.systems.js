define(['services/datacontext'],
  function (datacontext) {

    var ctor = function () {
      var self = this;
      self.newSystemId = ko.observable();
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

    ctor.prototype.activate = function (settings) {
      var self = this;
      self.settings = settings;
      self.showing = ko.computed(function () { return true; });
      self.selectedPatient = ko.computed(self.settings.selectedPatient);
      self.canAddId = ko.computed(function () {
        return self.selectedPatient().patientSystems().length === 0;
      });
      // A list of the problem observations for this patient
      self.computedSystemIds = ko.computed(function () {
        var patientsystems = self.selectedPatient().patientSystems();
        return patientsystems;
      }).extend({ throttle: 50 });
      self.createNewId = function () {
        var newId = (self.selectedPatient().patientSystems() + 1)*-1;
        datacontext.createEntity('PatientSystem', { id: newId, patient: self.selectedPatient(), systemName: 'Lawson', displayLabel: 'ID', systemId: self.newSystemId() });
      }
    };

    ctor.prototype.attached = function () {

    };

    return ctor;
  });