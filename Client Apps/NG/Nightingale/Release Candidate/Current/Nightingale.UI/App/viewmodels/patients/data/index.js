/**
 * data.index module manages/ hosts the data dialog
 *
 * 	@module data.index
 *	@class data.index
 */
 

define(['plugins/router', 'services/navigation', 'config.services', 'services/session', 'services/datacontext', 'viewmodels/patients/index', 'viewmodels/shell/shell', 'models/base'],
    function (router, navigation, servicesConfig, session, datacontext, patientsIndex, shell, modelConfig) {

        var selectedPatient = ko.computed(function () {
            return patientsIndex.selectedPatient();
        });

        var modalShowing = ko.observable(false);
        var modalEntity = ko.observable(new ModalEntity());

        // Add an additional data type of allergies
        var allergiesType = { id: ko.observable(-1), name: ko.observable('Allergies') };

        // Add an additional data type of allergies
        var medicationsType = { id: ko.observable(-2), name: ko.observable('Medications') };

        function saveAllData () {
            modalEntity().activeDataType(null);
            saveAllergies();
            saveNewMedication();
            saveDataEntry();
        };
        function cancelOverride () {
            modalEntity().activeDataType(null);
            cancelDataEntry();
            patientsIndex.getPatientsAllergies();
        };

        function toggleModalShowing () {
			var modalSettings = {
				title: 'Data Entry',
				showSelectedPatientInTitle: true,
				entity: modalEntity, 
				templatePath: 'viewmodels/templates/clinical.dataentry', 
				showing: modalShowing, 
				saveOverride: saveAllData, 
				cancelOverride: cancelOverride, 
				deleteOverride: null, 
				classOverride: 'modal-lg'
			}
            var modal = new modelConfig.modal(modalSettings);
            shell.currentModal(modal);
            modalShowing(!modalShowing());
        }

        var dataObservations = ko.observableArray();

        var activeDataType = ko.observable();
        var dataTypes = ko.computed(function () {
            var theseObservationTypes = datacontext.enums.observationTypes();
            var thisArray = [];
            ko.utils.arrayFilter(theseObservationTypes, function (type) {
                if (type.name().toLowerCase() !== 'problems') {
                    thisArray.push(type);
                }
            });
            return thisArray;
        }).extend({ throttle: 1 });
        
        var alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };

        var openColumn = ko.observable();
        var widgets = ko.observableArray();

        var initialized = false;

        function widget(data, column) {
            var self = this;
            self.name = ko.observable(data.name);
            self.path = ko.observable(data.path);
            self.isOpen = ko.observable(data.open);
            self.column = column;
            self.isFullScreen = ko.observable(false);
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

        var columns = ko.observableArray([
            new column('dataType', false, [{ name: 'Data Summary', path: 'patients/widgets/data.summary.html', open: true }])
        ]);

        var computedOpenColumn = ko.computed({
            read: function () {
                return openColumn();
            },
            write: function (value) {
                // If this column is being set to closed
                if (!value.isOpen()) {
                    // Check if this is the open column and it's also the first column
                    if (value === openColumn() && value === columns()[0]) {
                        // Set the open column to be the second column
                        openColumn(columns()[1]);
                    }
                        // Or else check if this is the open column
                    else if (value === openColumn()) {
                        // and Set the open column to be the first column
                        openColumn(columns()[0]);
                    }
                }
                    // If it's being set to open, just set this column to be the open column
                else {
                    openColumn(value);
                }
            }
        });

        // Haaaaack alert.  Can't figure out how else to trigger observations in data.list.js to refresh yet
        var needToRefreshObservations = ko.observable(false);

        var vm = {
            activate: activate,
            attached: attached,
            openColumn: openColumn,
            columns: columns,
            widgets: widgets,
            computedOpenColumn: computedOpenColumn,
            selectedPatient: selectedPatient,
            alphabeticalOrderSort: alphabeticalOrderSort,
            minimizeThisColumn: minimizeThisColumn,
            maximizeThisColumn: maximizeThisColumn,
            toggleFullScreen: toggleFullScreen,
            activeDataType: activeDataType,
            setActiveDataType: setActiveDataType,
            dataTypes: dataTypes,
            saveAllergies: saveAllergies,
            saveDataEntry: saveDataEntry,
            saveAllData: saveAllData,
            cancelDataEntry: cancelDataEntry,
            title: 'index',
            addData: addData,
            dataObservations: dataObservations,
            needToRefreshObservations: needToRefreshObservations,
            modalEntity: modalEntity,
            allergiesType: allergiesType,
            medicationsType: medicationsType
        };

        return vm;

        function activate() {
            // Set a local instance of selectedPatient equal to the injected patient
            selectedPatient.subscribe(function (newValue) {
                datacontext.checkIfAllObservationsAreLoadedYet(selectedPatient().id());
                activeDataType(null);
            });
            openColumn(columns()[0]);
        }

        function attached() {
            if (!initialized) {
                if (selectedPatient()) {
                    datacontext.checkIfAllObservationsAreLoadedYet(selectedPatient().id());
                }
                initialized = true;
            }
        }

        function addData () {
            toggleModalShowing();
        }

        function saveAllergies() {
            if (selectedPatient().allergies().length === 0) {
                return false;
            }
            // If there are any allergies that need saving
            var needsSaving = ko.utils.arrayFilter(selectedPatient().allergies(), function (allergy) {
                return allergy.needToSave() && allergy.isValid();
            });
			
            if (needsSaving.length > 0) {
                datacontext.saveAllergies(needsSaving).then(saveAllergiesCompleted);
				
				var saveAllergiesCompleted = function(data) {
                    // For each saved allergy
                    ko.utils.arrayForEach(data, function (allg) {
                        // Make sure it is set to not be new anymore
                        allg.isNew(false);
                        allg.isUserCreated(false);
                    });
                }
            }
			var destroyThese = ko.utils.arrayFilter( selectedPatient().allergies(), function (allergy) {
				return allergy.needToSave() && !allergy.isValid();
			});
			removeDataEntries( destroyThese );
        }

        /**
        *   saveNewMedication saves a new patient medication (insert only)
        *   using child module: medication.edit
        *
        * 	@method saveNewMedication
        */
        function saveNewMedication() {

            if (selectedPatient().medications().length === 0) {
                return false;
            }
            // If there are any medications that need saving
            var needsSaving = ko.utils.arrayFirst(selectedPatient().medications(), function (medication) {
                return medication.needToSave() && medication.isValid();
            });
            if (needsSaving) {
				datacontext.saveMedication(needsSaving).then(saveMedicationCompleted);									
				
                var saveMedicationCompleted = function(data) {
                }
            }                            
            else{ 
                cleanInvalidMedications();
            }             
        }
        
        function cleanInvalidMedications() {
            var thisMedsArray = selectedPatient().medications().slice(0);
            // Go through each med,
            ko.utils.arrayForEach(thisMedsArray, function (med) {                
                if( med.isNew() && !med.isValid() ) {
                    // Delete it from the manager
                    med.entityAspect.rejectChanges();
                }
            });
        }

		/**
		*	saves all observations in data entry modal.
		*	@method saveDataEntry
		*/
        function saveDataEntry() {
            if (selectedPatient().observations().length === 0 ) {
                return false;
            }
            // Go save the current patients' observations
            datacontext.saveObservations(selectedPatient().id()).then(saveCompleted);

            function saveCompleted() {
                // Clear out the standard observations
                var thisArray = selectedPatient().observations().slice(0);
                var deleteThese = [];
                // Go through each of the observations
                ko.utils.arrayForEach(thisArray, function (obs) {
                    // If it's not a problem,
                    if (obs.deleteFlag()) {
                        deleteThese.push(obs);
                    } else if (obs.type().name().toLowerCase() !== 'problems') {
                        // If it is new,
                        if (obs.isNew()) {                        
                            // Delete all new ones
                            deleteThese.push(obs);
                        } else if (obs.deleteFlag()) {
                            // Flagged for deletion
                            deleteThese.push(obs);
                        }
                    } else if (obs.entityAspect.entityState.isModified() || obs.isNew()) {
                        // If it is a problem that has valid changes
						if( obs.isValid() ){
							obs.entityAspect.acceptChanges();
						} else {
							obs.entityAspect.rejectChanges();
							deleteThese.push(obs);
						}
						obs.isNew(false);						
                        if (obs.deleteFlag()) {
                            deleteThese .push(obs);
                        }
                    } else if (obs.deleteFlag()) {
                        deleteThese.push(obs);
                    }
                });
                var refreshDataPage = (deleteThese.length > 0);
                // Blow the entity out of the cache
                while (deleteThese.length > 0) {
                    var observation = deleteThese[0];
                    observation.entityAspect.setDeleted();
                    observation.entityAspect.acceptChanges();
                    deleteThese.splice(0, 1);
                };
                // Haaaaaaaack
                //     Only doing this because the artifacts keep persisting in data.list
                if (refreshDataPage) {
                    needToRefreshObservations(true);
                }
                selectedPatient().observations.valueHasMutated();
                // HACK : Trigger notify subscribers to go get more
                activeDataType.valueHasMutated();
            }
        }

        function cancelDataEntry() {
            var thisObsArray = selectedPatient().observations().slice(0);
            var thisAllgArray = selectedPatient().allergies().slice(0);
            var thisMedsArray = selectedPatient().medications().slice(0);
            var destroyThese = [];
            //ko.utils.arrayForEach(thisArray, function (observation) {
            //    // HACK : 
            //    // Set the date to clear to allow the UI to clear the value 
            //    // in the date picker control
            //    observation.startDate('clear');
            //});
            // Go through each observation,
            ko.utils.arrayForEach(thisObsArray, function (observation) {
                // If the observation is new,
                if (observation.isNew()) {
                    // Delete it from the manager
                    destroyThese.push(observation);
                }
				else{
					observation.entityAspect.rejectChanges();
				}
            });
            // Go through each alleg,
            ko.utils.arrayForEach(thisAllgArray, function (alleg) {
                // If the alleg is new,
                if (alleg.isNew()) {
                    // Delete it from the manager
                    destroyThese.push(alleg);
                }
            });
            // Go through each med,
            ko.utils.arrayForEach(thisMedsArray, function (med) {
                // If the med is new,
                if (med.isNew()) {
                    // Delete it from the manager
                    med.entityAspect.rejectChanges();
                }
            });
			removeDataEntries( destroyThese );
            
            // Force the observations lists' to recalculate
            var thisActiveDataType = activeDataType();
            activeDataType(null);
            activeDataType(thisActiveDataType);
        }
		
		function removeDataEntries( destroyThese ){
			while (destroyThese.length > 0) {
                var entity = destroyThese[0];
                entity.entityAspect.setDeleted();
                entity.entityAspect.acceptChanges();
                destroyThese.splice(0, 1);
            };
		}
        function setActiveDataType(sender) {
            activeDataType(sender);
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

        function ModalEntity() {
            var self = this;
            self.activeDataType = ko.observable();
            self.selectedPatient = selectedPatient;
            self.showDropdown = true;
            self.showActions = false;
            // Object containing parameters to pass to the modal
            self.activationData = { selectedPatient: self.selectedPatient, activeDataType: self.activeDataType, showDropdown: self.showDropdown, showActions: self.showActions };
            self.canSave = ko.observable(true);
        }
    });