define(['services/session', 'services/datacontext', 'config.services', 'viewmodels/shell/shell', 'models/base', 'viewmodels/patients/medications/index', 'durandal/app'],
  function (session, datacontext, servicesConfig, shell, modelConfig, medicationsIndex, app) {

    var nameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };

    var ctor = function () {
      var self = this;
      self.modalShowing = ko.observable(false);
      self.modalEntity = ko.observable(new ModalEntity(self.modalShowing));
    }

    ctor.prototype.activate = function (data) {
      var self = this;
      self.medicationGroups = data.medicationGroups;
      self.selectedSortColumn = data.selectedSortColumn;
      self.toggleSort = data.toggleSort;
      self.canSort = data.canSort ? data.canSort : false;
      self.activeMedication = medicationsIndex.activeMedication;

      self.saveOverride = function () {
        var medication = self.modalEntity().medication();
        if( medication.needToSave() ){
          if( medication.isValid() ){
            datacontext.saveMedication(self.modalEntity().medication(), 'Update').then(saveCompleted);
          } else{
            medication.entityAspect.rejectChanges();
          }
        }

        function saveCompleted() {
          self.modalEntity().medication().isEditing(false);
          self.modalEntity().medication().isNew(false);
          self.modalEntity().medication().isUserCreated(false);
          self.modalEntity().medication().entityAspect.acceptChanges();
        }
      };

      self.cancelOverride = function () {
        self.modalEntity().medication().isEditing(false);
        datacontext.cancelEntityChanges(self.modalEntity().medication());
      };

      var modalSettings = {
        title: 'Edit Medication',
        showSelectedPatientInTitle: true,
        entity: self.modalEntity,
        templatePath: 'viewmodels/patients/sections/medication.edit',
        showing: self.modalShowing,
        saveOverride: self.saveOverride,
        cancelOverride: self.cancelOverride,
        deleteOverride: null,
        classOverride: 'modal-lg'
      };
      self.modal = new modelConfig.modal(modalSettings);
      self.setActiveMedication = function (medication) {
        medicationsIndex.setActiveMedication(medication);
      };
      self.editMedication = function (medication) {
        medication.isEditing(true);
        self.modalEntity().medication(medication);
        shell.currentModal(self.modal);
        self.modalShowing(true);
      }
      self.computedMedicationGroups = ko.computed(function () {
        ko.utils.arrayForEach(self.medicationGroups(), function (group) {
          ko.utils.arrayForEach(group.medications(), function (med) {
            if (!med.edit) {
              med.edit = function () {
                self.editMedication(med);
              }
            }
            if (!med.setActiveMedication) {
              med.setActiveMedication = function () {
                self.setActiveMedication(med);
              }
            }
          });
          if (!group.isExpanded) {
            group.isExpanded = ko.observable(false);
          }
        });
        return self.medicationGroups().sort(nameSort);
      });
    }

    return ctor;

    function ModalEntity(modalShowing) {
      var self = this;
      self.medication = ko.observable();
      self.canSaveObservable = ko.observable(true);
      self.canSave = ko.computed({
        read: function () {
          var medicationok = false;
          if (self.medication()) {
            var medicationtitle = !!self.medication().name();
            var medicationstatus = !!self.medication().status();
            medicationok = medicationtitle && medicationstatus;
          }
          return medicationok && self.canSaveObservable();
        },
        write: function (newValue) {
          self.canSaveObservable(newValue);
        }
      });
      self.activationData = { selectedPatient: medicationsIndex.selectedPatient, medication: self.medication, canSave: self.canSave, showing: modalShowing  };
    }
  });
