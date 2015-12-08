define(['services/datacontext', 'services/navigation', 'plugins/router'],
    function (datacontext, navigation, router) {

        var ctor = function () {
            var self = this;
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.event = self.settings.event;
            self.canSave = self.settings.canSave;
            self.showing = self.settings.showing;
            self.btnMsg = ko.computed(function () {
                var thisValue = 'Go to Individual';
                // // If it is an intervention,
                // if (self.event && self.event() && self.event().type().name() === 'Intervention') {
                    // thisValue = '';
                // } else {
                    // thisValue = 'Go to Individual';
                // } 
                return thisValue;
            });
            self.gotoSource = function () {
                // If it is an intervention,
                if (self.event && self.event() && self.event().type().name() === 'Intervention') {
                    self.showing(false);
                    router.navigate('#patients/' + self.event().patientId());
                    // // Make sure we go to the patient's overview page
                    navigation.indexOverride(0);
                } else {
                    self.showing(false);
                    // Else it is a todo so navigate to the patient anyway
                    router.navigate('#patients/' + self.event().patientId());
                    // // Make sure we go to the patient's overview page
                    navigation.indexOverride(0);
                }
            };			
			self.hasTimes = ko.computed( function(){
				return self.event && self.event() && self.event().hasTimes(); 
			});
            self.hasPatient = ko.computed(function () {
                return !!self.event().patientId();
            });
        };
		
			
        ctor.prototype.attached = function () {

        };

        return ctor;
    });