/**
*	manages the display of patient system ids on the left side bar section - IDs.
*	and manages launch of the edit individual IDs screen.
*	@module system.ids
*/
define(['services/datacontext', 'services/session', 'models/base', 'viewmodels/shell/shell'],
    function (datacontext, session, modelConfig, shell) {

		var ctor = function () {
		};

		ctor.prototype.alphabeticalSort = function (l, r) { return (l.name().toLowerCase() == r.name().toLowerCase()) ? (l.name().toLowerCase() > r.name().toLowerCase() ? 1 : -1) : (l.name().toLowerCase() > r.name().toLowerCase() ? 1 : -1) };

		function ModalEntity( modalShowing, selectedPatient ) {
			var self = this;
			self.patient = ko.observable(selectedPatient);
				self.canSave = ko.observable();
				self.activationData = { selectedPatient: self.patient, showing: modalShowing, canSave: self.canSave };
		}

		function systemIdsSortFunc(a,b){
			if (a.system().displayLabel() && b.system().displayLabel()) {
				if( a.system().displayLabel() && a.system().displayLabel().toLowerCase() < b.system().displayLabel().toLowerCase() ) return -1;
				if( b.system().displayLabel() && a.system().displayLabel().toLowerCase() > b.system().displayLabel().toLowerCase() ) return 1;
			}
			return 0;
		};

		ctor.prototype.activate = function (settings) {
			var self = this;
			self.settings = settings;
			self.selectedPatient = self.settings.selectedPatient;
			self.patientSystems = self.selectedPatient.patientSystems;
			self.isExpanded = ko.observable(false);
			self.showEllipsis = ko.observable(false);
			self.computedPatientSystemsDisplay = ko.computed(function () {
				var systemIds = self.selectedPatient.patientSystems();
				var theseIds = [];
				var limitToFive = (!self.isExpanded());
				ko.utils.arrayForEach( systemIds, function(record){
					if( Number(record.statusId()) === 1 && !record.isDeleted() ){	//only active ids
						if (theseIds.length < 5 || !limitToFive) {
							theseIds.push( record );
							self.showEllipsis(false);
						}
						else{
							self.showEllipsis(true);
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
						return datacontext.deletePatientSystems( changes.deleted ).then(afterDelete);
					}
					else{
						return afterDelete();
					}
				}
				function afterDelete(){
					if( changes.updated.length > 0 ){
						//updates
						return datacontext.savePatientSystems( changes.updated ).then( afterUpdate );
					}
					else{
						return afterUpdate();
					}
				}
				function afterUpdate(){
					if( changes.created.length > 0 ){
						//inserts
						return datacontext.savePatientSystems( changes.created );
					}
					else{
						return Q();
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
							datacontext.detachEntity(patSys);
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
						self.patientSystems.valueHasMutated();
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
					if( patSys.isNew() ){
						if( !patSys.isDeleted() ){
							created.push( patSys );
						}
						else{
							console.log('processChanges - new and deleted should not be here.');
						}
					}
					else if( patSys.isDeleted() ){
						deleted.push( patSys );
					}
					else if( patSys.entityAspect.entityState.name === 'Modified' ){
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
			self.modalEntityObservable = ko.observable();
			self.editSystemIds = function () {
				self.modalEntityObservable( new ModalEntity( self.patientSystemsModalShowing, self.selectedPatient ) );
				var modalSettings = {
					title: 'Individual IDs',
					showSelectedPatientInTitle: true,
					entity: self.modalEntityObservable, 
					templatePath: 'viewmodels/templates/patient.systems', 
					showing: self.patientSystemsModalShowing, 
					saveOverride: self.savePatientSystems, 
					cancelOverride: self.cancelPatientSystems, 
					deleteOverride: null, 
					classOverride: null
				}
				var modal = new modelConfig.modal(modalSettings);
				shell.currentModal(modal);
				self.patientSystemsModalShowing(true);
				self.isOpen(true);
			};
    	};

		return ctor;
    });
