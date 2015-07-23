/**
*	modal dialog internal module, works from parent module: system.ids.
*	this module manages the logics and internals of individual (patient) ids.
*	@module patient.systems
*/
define(['services/datacontext'],
  function (datacontext) {

    var ctor = function () {
		var self = this;
		self.newSystemId = ko.observable();
		self.systems = datacontext.enums.systems;
		self.patientSystemStatuses = datacontext.enums.patientSystemStatus;        
		
		self.primarySystem = ko.utils.arrayFirst( self.systems(), function( system ){
			return (system.primary() === true);
		});
    };

    function newThenAlphabeticalSort(a, b) {				
		
        // Primary sort property
        var p1 = a.isNew() ? 0 : 1;
        var p2 = b.isNew() ? 0 : 1;
		if (p1 != p2) {
            if (p1 < p2) return -1;
            if (p1 > p2) return 1;			              
        }
		if (p1 === 0 && p2 === 0){
			// 2'nd sort prop for new entries - show new ones at the top as they are added:
			if( a.id() < b.id() ) return 1; //as negative the id is - its a fresher one
			if( a.id() > b.id() ) return -1;
			return 0;
		}
        // 3'rd sort property
        var o1 = a.system().displayLabel().toLowerCase();
        var o2 = b.system().displayLabel().toLowerCase();                    
        if (o1 < o2) return -1;
        if (o1 > o2) return 1;
        return 0;
    };

	
    ctor.prototype.activate = function (settings) {
      var self = this;
      self.settings = settings;
      self.showing = ko.computed(function () { return true; });
      self.selectedPatient = self.settings.selectedPatient;
	  self.canSave = self.settings.canSave;
	  
	  self.patientSystems = ko.computed( function(){
		  var result = [];
		  var patient = self.selectedPatient;
		  var systemIds = patient().patientSystems();		  
		  result = patient().patientSystems();		  
		  return result;
	  });
	  
	  self.newPatientSystemIdValue = ko.observable();
	  self.newSelectedSystem = ko.observable();	  
	  self.duplicate = ko.observable();
	  
	  /**
		*	validation errors for the add new id area only 
		*	@method newIdValidationErrors
		*/		
	  self.newIdValidationErrors = ko.computed( function(){
			var value = self.newPatientSystemIdValue();
			var system = self.newSelectedSystem();
			var systemIds = self.patientSystems();
			var errors = [];
			if( !value && !system ){
				//dont show any errors if the user didnt fill in a value or selected a system  
				return [];
			}
			if( !value ){
				errors.push({PropName: 'newValue', Message: 'a new Value is required'});
			} 
			else if( !value.trim().length ){
				errors.push({PropName: 'newValue', Message: 'a new Value cannot be blank'});
			}
			if( !system ){
				errors.push({PropName: 'newSystem', Message: 'a new System is required'});
			}
			self.duplicate(null);
			if( value && value.trim().length && system ){
				//verify no dups			
				var dup = ko.utils.arrayFirst( systemIds, function(patSys){
					return (patSys.isDeleted() === false && patSys.value().trim() === value.trim() && patSys.systemId() === system.id());
				});
				if( dup ){
					errors.push({PropName: 'newValue', Message: 'this individual id combination of System and Value already exist'});
					self.duplicate(dup);
				}
			}
			return errors;
	  });
	  	  
	  self.canAddNewPatientSystem = ko.computed( function(){
			var value = self.newPatientSystemIdValue();
			var system = self.newSelectedSystem();
			var errors = self.newIdValidationErrors();
			return (value && value.trim() && system && errors.length === 0);
	  });
	  
	  self.validationErrors = ko.computed( function(){		  
			var patient = self.selectedPatient;			
			var systemIds = self.patientSystems();
			var errors = [];
			var newIdErrors = self.newIdValidationErrors();
			ko.utils.arrayForEach( newIdErrors, function( err ){
				errors.push( err );
			});
			ko.utils.arrayForEach( systemIds, function( patSys ){				
				if( !patSys.isValid() ){
					isValid = false;
					ko.utils.arrayForEach( patSys.validationErrors(), function( err ){
						errors.push( err );	
					});					
				}
			});
			if( self.canAddNewPatientSystem() ){
				//a new patient system id entered but not added:
				errors.push({PropName:'', Message:'a new individual id entered but not added. (click add or x to clear)'});
			}
			return errors;
	  });
	  
	  self.validationErrorsArray = ko.computed(function () {
			var thisArray = [];
			var validationErrors = self.validationErrors();
			ko.utils.arrayForEach( validationErrors, function (error) {
				thisArray.push(error.PropName);
				self.canSave(false);				
			});
			if( thisArray.length === 0 ){ 
				self.canSave(true);
			}	
			return thisArray;
   	  });
				
	  self.setPrimary = function( patientSystem ){
		  //clear all other 
		  ko.utils.arrayForEach( self.selectedPatient().patientSystems(), function( systemIdRecord ){
			  systemIdRecord.primary(false);
		  });
		  patientSystem.primary(true);
	  }
	  
	  self.hasPrimarySelected = function(){		  
			//look for non deleted primary item:
			var primary = ko.utils.arrayFirst( self.patientSystems(), function( systemIdRecord ){
				return ( systemIdRecord.primary() && ( systemIdRecord.isDeleted() === false ));
			});
			var message = primary ? (primary.system().displayLabel()  +' : ' + primary.value()) : 'no primary';
			return primary;
	  }	
	  
      self.canAddId = ko.computed(function () {
        return true;
      });
	  
      self.computedEditSystemIds = ko.computed(function () {
        var patientSystems = self.selectedPatient().patientSystems();
		var theseIds = ko.utils.arrayFilter( patientSystems, function( patSys ){
			return !patSys.isDeleted();				
		});		
        return theseIds.sort( newThenAlphabeticalSort );
      }).extend({ throttle: 50 });//, notify: 'always'
	  		  
	  self.newIdValidationErrorsArray = ko.computed( function(){
			var thisArray = [];
			ko.utils.arrayForEach( self.newIdValidationErrors(), function(error){
				thisArray.push( error.PropName );
			});
			return thisArray;
	  });
	  
	  self.isInvalidProp = function( patientSystemId, propName ){
		  //propName: 'value' / 'system'
		  var patientSystemPropertyHasError = ko.utils.arrayFirst( self.validationErrors(), function(error){
			  return ( error.Id === patientSystemId && error.PropName === propName );
		  });
		  return patientSystemPropertyHasError || (self.duplicate() && self.duplicate().id() === patientSystemId);
	  }
	  
	  self.clearNewId = function(){
		  self.newSelectedSystem(null);
		  self.newPatientSystemIdValue(null);		  
	  }
	  
	  self.showCancelNewId = ko.computed(function(){
			return (self.newSelectedSystem() || self.newPatientSystemIdValue());
	  });
	  // self.newPatientSystem = ko.observable();
      self.createNewId = function () {
        var newId = (self.selectedPatient().patientSystems().length + 1)*-1;
		console.log('createNewId - newId = ' + newId);
		var primary = false;
		if( self.primarySystem && self.primarySystem.id() === self.newSelectedSystem().id() && !self.hasPrimarySelected() ){
			//user selected a primary system in the added item, and there is no primary item in the list:
			primary = true;			
		}		
        var entity = datacontext.createEntity('PatientSystem', 
						{ 
							id: newId, 
							patientId: self.selectedPatient().id(), 
							system: self.newSelectedSystem(), 
							value: self.newPatientSystemIdValue(), 
							systemSource: 'Engage', 
							statusId: '1', 
							primary: primary
						});
		if( entity ){
			entity.isNew(true);			
			//self.newPatientSystem(entity);
			self.newSelectedSystem(null);
			self.newPatientSystemIdValue(null);
		} else{ 
			console.log('createNewId - datacontext.createEntity did not return an entity'); 
		}		
      }

	  self.deletePatientSystemId = function( patientSystem ){
		  patientSystem.isDeleted(true);
		  patientSystem.primary(false);
		  if( !isNaN(patientSystem.id()) && patientSystem.id() < 0 ){
			  //fresh newly added now deleted before it was saved: completely remove the entity:			  
			  datacontext.detachEntity(patientSystem);			  
		  }		  	  		  
	  }
    };

    ctor.prototype.attached = function () {

    };

    return ctor;
  });
