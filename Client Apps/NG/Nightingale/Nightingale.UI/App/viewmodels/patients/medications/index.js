﻿define(['plugins/router', 'services/navigation', 'config.services', 'services/session', 'services/datacontext', 'viewmodels/patients/index', 'viewmodels/shell/shell', 'models/base', 'viewmodels/patients/data/index'],
  function (router, navigation, servicesConfig, session, datacontext, patientsIndex, shell, modelConfig, dataIndex) {

    var selectedPatient = ko.computed(function () {
      return patientsIndex.selectedPatient();
    });

    var descendingDateSort = function (l, r) { return (l.createdOn() == r.createdOn()) ? (l.createdOn() < r.createdOn() ? 1 : -1) : (l.createdOn() < r.createdOn() ? 1 : -1) };

    var medGroupSort = function (l, r) {
      var o1 = l.medSortDate();
      var o2 = r.medSortDate();

      if (o1 < o2) return 1;
      if (o1 > o2) return -1;
      return 0;
    };
    var alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };

    var initialized = false;

    var medicationModalShowing = ko.observable(false);
    var allergyModalShowing = ko.observable(false);

    var MedicationGroup = function (name, firstMedication) {
        var self = this;
        self.name = ko.observable(name);
        self.medications = ko.observableArray([firstMedication]);
        self.sortedMedications = ko.computed(function () {
            var results = [];
            results = self.medications().sort(medGroupSort);
            return results;
        });
        self.firstMedication = ko.computed(function () {
            return self.sortedMedications()[0];
        });
        self.isActive = ko.observable(false);
        self.isExpanded = ko.observable(false);
    }

    var groupedMedications = ko.computed(function () {
        var medications = selectedPatient().medications();
        var groups = [];
        var result = [];
        ko.utils.arrayForEach(medications, function (medication) {
            var name = medication.name();
            var isActive = medication.statusId() === '1';
            var match = groups.filter(function (group) {
                return group.name() === name;
            })[0];
            if (!match) {
                var newGroup = new MedicationGroup(name, medication);
                newGroup.isActive(isActive);
                groups.push(newGroup);
            } else {
                match.medications.push(medication);
                if (!match.isActive() && isActive) {
                    match.isActive(true);
                }
            }
        });
        return groups;
    });
    var activeMedicationGroups = ko.computed(function () {
      var result = [];
      result = groupedMedications().filter(function (group) {
        return group.isActive();
      });
      return result;
    });
    var inactiveMedicationGroups = ko.computed(function () {
      var result = [];
      result = groupedMedications().filter(function (group) {
        return !group.isActive();
      });
      return result;
    });

    var medicationSaving = ko.computed(datacontext.medicationSaving);
    var selectedSortColumn = ko.observable();
    var activeMedication = ko.observable();
    var activeAllergy = ko.observable();
    var activeMedicationColumns = ko.computed(function () {
      return ['expand','type-small','name-small','strength-small','reason','status'];
    });
    var toggleMedicationSort = function (sender) {
      if (self.selectedSortColumn() && self.selectedSortColumn().indexOf(sender.sortProperty) !== -1) {
        if (self.selectedSortColumn() && self.selectedSortColumn().substr(self.selectedSortColumn().length - 4, 4) === 'desc' ) {
          self.selectedSortColumn(null);
        } else {
          self.selectedSortColumn(sender.sortProperty + ' desc');
        }
      } else {
        self.selectedSortColumn(sender.sortProperty);
      }
    };

    function widget(data, column) {
      var self = this;
      self.name = ko.observable(data.name);
      self.path = ko.observable(data.path);
      self.isOpen = ko.observable(data.open);
      self.column = column;
      self.isFullScreen = ko.observable(false);
      self.filtersOpen = ko.observable(true);
      self.activationData = { widget: self, selectedPatient: selectedPatient, defaultSort: data.defaultSort };
      self.allowAdd = data.allowAdd;
      self.statusIds = data.statusIds;
      self.medicationGroups = data.medicationGroups;
    }

    function column(name, open, widgets) {
      var self = this;
      self.name = ko.observable(name);
      self.isOpen = ko.observable(open).extend({ notify: 'always' });
      self.isOpen.subscribe(function () {
        computedOpenColumn(self);
      });
      self.widgets = ko.observableArray();
      $.each(widgets, function (index, item) {
        self.widgets.push(new widget(item, self))
      });
    }

    var medicationColumnWidgets = [
        {
            name: 'Allergies',
            path: 'viewmodels/patients/widgets/allergies',
            open: false,
            statusIds: [1,2,3,4,5,6,7]
        }, {
            name: 'Active Medications',
            path: 'viewmodels/patients/widgets/medication.groups',
            open: true,
            medicationGroups: activeMedicationGroups,
            allowAdd: true
        }, {
            name: 'Inactive Medications',
            path: 'viewmodels/patients/widgets/medication.groups',
            open: false,
            medicationGroups: inactiveMedicationGroups,
            defaultSort: 'endDate desc, name'
        }
    ];
    var medicationColumn = ko.observable(new column('medications', false, medicationColumnWidgets));
    var detailsColumn = ko.observable(new column('medicationDetails', false, [{ name: 'MedicationDetails', path: 'viewmodels/patients/sections/medication.details', open: true }, { name: 'AllergyDetails', path: 'viewmodels/patients/sections/allergy.details', open: true }]));

    var setActiveMedication = function (medication) {
      var self = this;
      self.activeMedication(medication);
      self.activeAllergy(null);
    };
    var setActiveAllergy = function (allergy) {
      var self = this;
      self.activeAllergy(allergy);
      self.activeMedication(null);
    };
    var openColumn = ko.observable();
    var computedOpenColumn = ko.computed({
      read: function () {
        return openColumn();
      },
      write: function (value) {
        if (!value.isOpen()) {
          if (value === openColumn() && value.name() === 'medicationColumn') {
            openColumn(detailsColumn());
          } else if (value === openColumn()) {
            openColumn(medicationColumn());
          }
        } else {
          openColumn(value);
        }
      }
    });

    var isComposed = ko.observable(true);

    var vm = {
      medicationSaving: medicationSaving,
      selectedSortColumn: selectedSortColumn,
      activeMedicationColumns: activeMedicationColumns,
      toggleMedicationSort: toggleMedicationSort,
      activate: activate,
      isComposed: isComposed,
      activeMedication: activeMedication,
      activeAllergy: activeAllergy,
      setActiveMedication: setActiveMedication,
      setActiveAllergy: setActiveAllergy,
      selectedPatient: selectedPatient,
      computedOpenColumn: computedOpenColumn,
      medicationColumn: medicationColumn,
      detailsColumn: detailsColumn,
      setOpenColumn: setOpenColumn,
      minimizeThisColumn: minimizeThisColumn,
      maximizeThisColumn: maximizeThisColumn,
      editMedication: editMedication,
      editAllergy: editAllergy,
      toggleFullScreen: toggleFullScreen,
      attached: attached,
      title: 'index',
      toggleWidgetOpen: toggleWidgetOpen,
      addMedication: addMedication
    };

    return vm;

    function Group(name) {
      var self = this;
      self.Name = ko.observable(name);
      self.Notes = ko.observableArray();
    }

    function addMedication () {
      dataIndex.addData();
    };

    function activate() {
      var spToken = selectedPatient.subscribe(function (newValue) {
        activeMedication(null);
        activeAllergy(null);
      });
      openColumn(medicationColumn());
      isComposed(false);
    }

    function attached() {
      if (!initialized) {
        initialized = true;
      }
      isComposed(true);
    }

    function detached() {
      isComposed(false);
    }

    function editMedication(medication, msg) {
        var modalEntity = ko.observable(new MedicationModalEntity(medication));
         medication.editMedicationCancelled(false);
        var saveOverride = function () {
            datacontext.saveMedication(modalEntity().medication());
        };
        var cancelOverride = function () {
            var medicationCancel = modalEntity().medication();
             medicationCancel.editMedicationCancelled(true);            
            medicationCancel.entityAspect.rejectChanges();
        };
        msg = msg ? msg : 'Edit Medication';
        var modalSettings = {
            title: msg,
            showSelectedPatientInTitle: true,
            entity: modalEntity,
            templatePath: 'viewmodels/patients/sections/medication.edit',
            showing: medicationModalShowing,
            saveOverride: saveOverride,
            cancelOverride: cancelOverride,
            deleteOverride: null,
            classOverride: 'modal-lg'
        };
        var modal = new modelConfig.modal(modalSettings);
        medicationModalShowing(true);
        shell.currentModal(modal);
    }

    function editAllergy(allergy, msg) {
        var modalEntity = ko.observable(new AllergyModalEntity(allergy));
        var reactionIdsBeforeEdit = allergy.reactionIds().slice(0);
        var saveOverride = function () {
            if (!modalEntity().allergy().isValid()) {               
                var keepModalOpen = true;
                return keepModalOpen;
            }

            datacontext.saveAllergies([modalEntity().allergy()], 'Update').then(saveCompleted);

            function saveCompleted() {
                self.modalEntity().allergy().isNew(false);
                self.modalEntity().allergy().isUserCreated(false);
                self.modalEntity().allergy().entityAspect.acceptChanges();
            }
        };
        var cancelOverride = function () {
            var allergyCancel = modalEntity().allergy(); 
             allergyCancel.recalcReactionString(reactionIdsBeforeEdit);          
            allergyCancel.entityAspect.rejectChanges();
        };
        msg = msg ? msg : 'Edit Allergy';
        var modalSettings = {
            title: msg,
            showSelectedPatientInTitle: true,
            entity: modalEntity,
            templatePath: 'viewmodels/templates/allergy.edit',
            showing: allergyModalShowing,
            saveOverride: saveOverride,
            cancelOverride: cancelOverride,
            deleteOverride: null,
            classOverride: 'modal-lg'
        };
        var modal = new modelConfig.modal(modalSettings);
        allergyModalShowing(true);
        shell.currentModal(modal);
    }

    function MedicationModalEntity(med) {
      var self = this;
      self.medication = ko.observable(med);
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
      self.activationData = { selectedPatient: selectedPatient, medication: self.medication, canSave: self.canSave, showing: allergyModalShowing };
    }

    function AllergyModalEntity(allergy) {
        var self = this;
        self.allergy = ko.observable(allergy);
        self.canSaveObservable = ko.observable(true);
        self.canSave = ko.computed({
            read: function () {
                var allergyok = false;
                if (self.allergy()) {
                    var allergytitle = !!self.allergy().allergyName();
                    var allergystatus = !!self.allergy().status();
                    allergyok = allergytitle && allergystatus;
                }
                return allergyok && self.canSaveObservable();
            },
            write: function (newValue) {
                self.canSaveObservable(newValue);
            }
        });
        // Object containing parameters to pass to the modal
        self.activationData = { allergy: self.allergy, canSave: self.canSave, showing: allergyModalShowing  };
    }

    function setOpenColumn(sender) {
      openColumn(sender);
    }

    function minimizeThisColumn(sender) {
      sender.column.isOpen(false);
    }

    function maximizeThisColumn(sender) {
      sender.column.isOpen(true);
    }

    function toggleFullScreen(sender) {
      sender.isFullScreen(!sender.isFullScreen());
    }

    function refreshTodoView() {
      patientsIndex.getPatientsAllergies();
    }

    function toggleWidgetOpen(sender) {
      var openwidgets = ko.utils.arrayFilter(sender.column.widgets(), function (wid) {
        return wid.isOpen();
      });
      if (openwidgets.length === 1 && openwidgets[0] === sender) {
      } else {
        sender.isOpen(!sender.isOpen());
      }
    }
  });