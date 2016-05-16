define(['models/base', 'config.services', 'services/datacontext', 'services/session', 'viewmodels/patients/medications/index', 'viewmodels/shell/shell'],
  function (modelConfig, servicesConfig, datacontext, session, medicationsIndex, shell) {

    var modalShowing = ko.observable(true);

    var ctor = function () {
      var self = this;
    };

    ctor.prototype.activate = function (settings) {
      var self = this;
      self.alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };
      self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
      self.settings = settings;
      self.medication = self.settings.activeMedication;
      self.selectedPatient = medicationsIndex.selectedPatient;
      self.isFullScreen = ko.observable(false);
      self.toggleFullScreen = function () {
        self.isFullScreen(!self.isFullScreen());
      };
      self.edit = function () {
        medicationsIndex.editMedication(self.medication(), 'Edit Medication');
      }
      self.delete = function () {
        var result = confirm('You are about to delete a medication.  Press OK to continue, or cancel to return without deleting.');
        if (result === true) {
          datacontext.deleteMedication(self.medication());
          self.settings.activeMedication(null);
        }
        else {
          return false;
        }
      };
    };

    function save (medication) {
      datacontext.saveMedication(medication);
    }

    function cancel(item) {
      item.entityAspect.rejectChanges();
    }

    function getMedicationDetails (medication) {
      medicationsIndex.getMedicationDetails(medication, true);
    };

    function editEntity (msg, entity, path, saveoverride, canceloverride) {
      var modalSettings = {
        title: msg,
        showSelectedPatientInTitle: true,
        entity: entity,
        templatePath: path,
        showing: modalShowing,
        saveOverride: saveoverride,
        cancelOverride: canceloverride
      }
      var modal = new modelConfig.modal(modalSettings);
      modalShowing(true);
      shell.currentModal(modal);
    }

    function ModalEntity(entity) {
      var self = this;
      self.entity = entity;
      self.activationData = { entity: self.entity };
      self.canSave = ko.computed(function () {
        var result = self.entity.isValid();
        return result;
      });
    }

    ctor.prototype.detached = function() {
      var self = this;
    }
    return ctor;
  });
