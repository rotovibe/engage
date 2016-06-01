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
        var isEditable = self.medication().dataSource() === 'Engage';
        if (isEditable) {
            medicationsIndex.editMedication(self.medication(), 'Edit Medication');
        }
      }
      self.isNotesExpanded = ko.observable(false);
      self.delete = function () {
        var isEditable = self.medication().dataSource() === 'Engage';
        if (isEditable) {
            var result = confirm('You are about to delete a medication.  Press OK to continue, or cancel to return without deleting.');
            if (result === true) {
              datacontext.deletePatientMedication(self.medication());
              self.settings.activeMedication(null);
            }
            else {
              return false;
            }
        }
      };
      self.createdString = ko.computed(function () {
        var result = '';
        var medication = self.medication();
        if (!medication) {
            return '';
        }
        // var createdBy = medication.createdBy();
        var createdOn = medication.createdOn();
        // result += createdBy ? createdBy : '';
        result += createdOn ? createdOn + ' on ' : '';
        return result;
      });
      self.updatedString = ko.computed(function () {
        var result = '';
        if (!medication) {
            return '';
        }
        var medication = self.medication();
        // var updatedBy = medication.updatedBy();
        var updatedOn = medication.updatedOn();
        // result += updatedBy ? updatedBy : '';
        result += updatedOn ? updatedOn + ' on ' : '';
        return result;
      });
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
