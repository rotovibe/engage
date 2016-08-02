/**
*	patient status dialog module
*	@module patient.status
*/
define(['services/datacontext'],
  function (datacontext) {

    var ctor = function () {
      var self = this;
    };
	
	ctor.prototype.patientStatuses = ko.computed(datacontext.enums.patientStatuses);
	ctor.prototype.patientStatusReasons = ko.computed(datacontext.enums.patientStatusReasons);
	
    ctor.prototype.activate = function (settings) {
      var self = this;
      self.settings = settings;
      self.showing = ko.computed(function () { return true; });
      self.selectedPatient = ko.computed(self.settings.selectedPatient);
    };

    ctor.prototype.attached = function () {

    };

	ctor.prototype.detached = function() { 
		var self = this;
		ko.utils.arrayForEach(subscriptionTokens, function (token) {
			token.dispose();
		});
		//computed cleanup:
		self.showing.dispose();	
		self.selectedPatient.dispose();
	}
	
    return ctor;
  });