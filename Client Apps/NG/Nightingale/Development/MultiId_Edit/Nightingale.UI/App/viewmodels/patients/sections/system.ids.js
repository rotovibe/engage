define(['services/datacontext', 'services/session', 'models/base', 'viewmodels/shell/shell'],
    function (datacontext, session, modelConfig, shell) {

        var ctor = function () {
        };

        ctor.prototype.alphabeticalSort = function (l, r) { return (l.name().toLowerCase() == r.name().toLowerCase()) ? (l.name().toLowerCase() > r.name().toLowerCase() ? 1 : -1) : (l.name().toLowerCase() > r.name().toLowerCase() ? 1 : -1) };
        
        function ModalEntity(patient) {
            var self = this;
            self.selectedPatient = ko.observable(patient);
            // Object containing parameters to pass to the modal
            self.activationData = { selectedPatient: self.selectedPatient, activeDataType: self.activeDataType, showDropdown: self.showDropdown, showActions: self.showActions };
            // Create a computed property to subscribe to all of
            // the patients' observations and make sure they are
            // valid
            self.canSave = ko.computed(function () {
                var changedid = ko.utils.arrayFirst(self.selectedPatient().patientSystems(), function (patSys) {
                    return patSys.systemId() && (patSys.entityAspect.entityState.name === 'Modified' || patSys.entityAspect.entityState.name === 'Added');
                });
                return !!changedid;
            }).extend({throttle:1});
            self.isOpen = ko.observable(false);
        }

        ctor.prototype.activate = function (settings) {
            var self = this;            
            self.selectedPatient = settings.selectedPatient;
            self.patientSystems = self.selectedPatient.patientSystems;
			self.isExpanded = ko.observable(false);
            // Compute a list of the patient ids to display
            self.computedPatientSystems = ko.computed(function () {
				var systemIds = self.patientSystems();
				var theseIds = [];
				var limitToFive = (!self.isExpanded());				
				ko.utils.arrayForEach( systemIds, function(record){					
					if( Number(record.statusId()) === 1 ){	//only active ids
						if (theseIds.length < 5 || !limitToFive) {
							theseIds.push( record );
						}
					}						
				});
				theseIds.sort( function(a,b){
					if( a.system().displayLabel() < b.system().displayLabel() ) return -1;
					if( a.system().displayLabel() > b.system().displayLabel() ) return 1;
					return 0;
				});				
                return theseIds;
            }).extend({ throttle: 15 });
            self.isOpen = ko.observable(false);
			
            self.patientSystemsModalShowing = ko.observable(false);
            self.savePatientSystems = function () {
				//TODO: multi ID's
	alert( 'not implemented.');
    //            datacontext.savePatientSystems(self.computedPatientSystems());
            };
            self.cancelPatientSystems = function () {
                ko.utils.arrayForEach(self.computedPatientSystems(), function (patSys) {
                    patSys.entityAspect.rejectChanges();
                });
            };
            self.modalEntity = ko.observable(new ModalEntity(self.selectedPatient));
            self.modal = new modelConfig.modal('Individual ID', self.modalEntity, 'viewmodels/templates/patient.systems', self.patientSystemsModalShowing, self.savePatientSystems, self.cancelPatientSystems);
            // self.isOpen = ko.observable(true);
            // self.isFullScreen = ko.observable(false);
            self.toggleEditing = function () {
                shell.currentModal(self.modal);
                self.patientSystemsModalShowing(true);
                self.isOpen(true);
            };
        };

        ctor.prototype.attached = function () {
        };

        return ctor;
    });