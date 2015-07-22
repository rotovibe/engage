/**
*	manages the display of patient system ids on the left side bar section - IDs.
*	and manages launch of the edit individual IDs screen.
*	@module system.ids
*/
define(['services/datacontext', 'services/session', 'models/base', 'viewmodels/shell/shell'],
    function (datacontext, session, modelConfig, shell) {

        var ctor = function () {
			var self = this;			
        };

        ctor.prototype.alphabeticalSort = function (l, r) { return (l.name().toLowerCase() == r.name().toLowerCase()) ? (l.name().toLowerCase() > r.name().toLowerCase() ? 1 : -1) : (l.name().toLowerCase() > r.name().toLowerCase() ? 1 : -1) };
        
        function ModalEntity( modalShowing, selectedPatient ) {
            var self = this;
            self.patient = ko.observable(selectedPatient);
			self.canSave = ko.observable();
			self.activationData = { selectedPatient: self.patient, showing: modalShowing, canSave: self.canSave  };
		}   
		
		function systemIdsSortFunc(a,b){
			if( a.system().displayLabel().toLowerCase() < b.system().displayLabel().toLowerCase() ) return -1;
			if( a.system().displayLabel().toLowerCase() > b.system().displayLabel().toLowerCase() ) return 1;
			return 0;
		};				
		
        ctor.prototype.activate = function (settings) {
            var self = this;  
			self.settings = settings;	
            self.selectedPatient = self.settings.selectedPatient;
            self.patientSystems = self.selectedPatient.patientSystems;			
			self.isExpanded = ko.observable(false);
            
            self.computedPatientSystemsDisplay = ko.computed(function () {
				var systemIds = self.selectedPatient.patientSystems();
				var theseIds = [];
				var limitToFive = (!self.isExpanded());				
				ko.utils.arrayForEach( systemIds, function(record){					
					if( Number(record.statusId()) === 1 && !record.isDeleted() ){	//only active ids
						if (theseIds.length < 5 || !limitToFive) {
							theseIds.push( record );
						}
					}						
				});
				theseIds.sort( systemIdsSortFunc );
                return theseIds;
            }).extend({ throttle: 15 });		
			
            self.isOpen = ko.observable(false);
            self.patientSystemsModalShowing = ko.observable(false);
            self.savePatientSystems = function () {
				var changes = self.processChanges();
				if( changes ){	
					// if any deletions, wait till its deleted and only after that move on to updates / inserts
					//	reason: a primary could be deleted, in order to nominate a new primary (insert or update)
					// also - after deletions are done (if any) - if any updates - do them before inserts, 
					//	and wait untill the updates completed, before calling insert.
					
					if( changes.deleted.length > 0 ){
						//deletes
						console.log('deleting patient systems - start');
						return datacontext.deletePatientSystems( changes.deleted ).then(afterDelete);
					}
					else{
						return afterDelete();
					}
					function afterDelete(){
						console.log('afterDelete');
						if( changes.updated.length > 0 ){
							//updates
							console.log('updating patient systems - start');
							return datacontext.savePatientSystems( changes.updated ).then( afterUpdate );
						}
						else{
							return afterUpdate();
						}
					}
					function afterUpdate(){
						console.log('afterUpdate');
						if( changes.created.length > 0 ){
							//inserts					
							console.log('insert patient systems - start');
							return datacontext.savePatientSystems( changes.created );					
						}	
						else{
							return Q();
						}
					}					
				}				
            };
			
            self.cancelPatientSystems = function () {				
				var changes = self.processChanges();
				if( changes ){
					var result = confirm('Your changes will be discarded.  Press OK to continue, or cancel to return.');
					if( result === true ){	
						//user approved the cancel action
						//discard new ones
						ko.utils.arrayForEach( changes.created, function( patSys ){
							patSys.entityAspect.setDeleted();
						});
						//undelete deleted
						ko.utils.arrayForEach( changes.deleted, function( patSys ){
							patSys.isDeleted(false);
							patSys.entityAspect.rejectChanges();	//revert possible changes done before it was deleted
						});
						//revert any changes
						ko.utils.arrayForEach( changes.updated, function( patSys ){
							patSys.entityAspect.rejectChanges();
						});								
						return true;
					}
					else{
						return false;	//user cancelled the cancel action. 
					}					
				}										
            };	
			
			self.processChanges = function(){
				var updated = [];
				var created = [];
				var deleted = [];
				ko.utils.arrayForEach( self.patientSystems(), function( patSys ){
					if( patSys.isNew() && !patSys.isDeleted() ){									
						created.push( patSys );
					} else if( patSys.isDeleted() ){
						if( patSys.isNew() ){
							//added, then deleted: never saved:
							patSys.entityAspect.setDeleted();	
						}
						else{
							deleted.push( patSys );								
						}						
					} else if( patSys.entityAspect.entityState.name === 'Modified' ){
						updated.push( patSys );
					}											
				});					
				if( created.length || deleted.length || updated.length ){
					return {created: created, deleted: deleted, updated: updated}
				}
				else{
					return null;
				}
			}

            self.editSystemIds = function () {
				var modalEntity = ko.observable( new ModalEntity( self.patientSystemsModalShowing, self.selectedPatient ) );
				var modal = new modelConfig.modal('Individual ID', modalEntity, 'viewmodels/templates/patient.systems', self.patientSystemsModalShowing, self.savePatientSystems, self.cancelPatientSystems);
                shell.currentModal(modal);
                self.patientSystemsModalShowing(true);
                self.isOpen(true);
            };
        };

        ctor.prototype.attached = function () {
        };

        return ctor;
    });
