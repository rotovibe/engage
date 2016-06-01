/**
*
*	@module contact.details
*/
define([], 
	function(){
				
		
		// var contact = ko.observable();
		
		var activeDetailsTab = ko.observable('Profile');
		
		function setActiveDetailsTab( name ){
			activeDetailsTab(name);	
		}
			
		function activate( settings ){
			var self = this;
			self.contact = settings.contact;
			self.isContactSelected = ko.computed( function(){
				return (self.contact && self.contact()) ? true : false;
			});
			
			self.primaryCommunications = ko.computed(function () {
                // Get the primary communication types and return them
                var communications = [];
                var contactcard = self.contact();
                if (contactcard) {
                    var prefPhone = ko.utils.arrayFirst(contactcard.phones(), function (phone) {
                        return phone.phonePreferred();
                    });
                    if (prefPhone) {
                        prefPhone.template = 'templates/phone.html';
                        communications.push(prefPhone);
                    }
                    var prefText = ko.utils.arrayFirst(contactcard.phones(), function (phone) {
                        return phone.textPreferred();
                    });
                    if (prefText && prefText !== prefPhone) {
                        prefText.template = 'templates/phone.html';
                        communications.push(prefText);
                    }
                    var prefEmail = ko.utils.arrayFirst(contactcard.emails(), function (email) {
                        return email.preferred();
                    });
                    if (prefEmail) {
                        prefEmail.template = 'templates/email.html';
                        communications.push(prefEmail);
                    }
                    var prefAddress = ko.utils.arrayFirst(contactcard.addresses(), function (address) {
                        return address.preferred();
                    });
                    if (prefAddress) {
                        prefAddress.template = 'templates/address.html';
                        communications.push(prefAddress);
                    }
                }
                // Return the list of preferred communications
                return communications;
            }).extend({ throttle: 25 });
			
			return true;
		}
		
		var detailsTabs = ko.observableArray([
				new Tab('Profile', null, '/NightingaleUI/Content/images/patient_neutral_small.png', 'Phone blue small'),
				new Tab('Professional', null, '/NightingaleUI/Content/images/settings_blue.png', 'Phone blue small'),
				new Tab('Comm', 'icon-phone blue', null)				
		]);
		
		function Tab(name, cssClass, imgSource, imgAlt){
			var self = this;
			self.name = name;
			self.cssClass = cssClass;
			self.imgSource = imgSource;
			self.imgAlt = imgAlt;
			self.isShowing = true;
			//self.hasErrors = ko.observable(false);
		};
		
		var detailsTabIndex = {
			profile: 0,
			professional: 1,
			comm: 2
		}
		
		//ctor.prototype.activeDetailsTab = ko.observable();		
		//return ctor;
		var vm = {			
			activate: activate,
			detailsTabs: detailsTabs,
			setActiveDetailsTab: setActiveDetailsTab,
			activeDetailsTab: activeDetailsTab
			//contact: contact
		};
		return vm;
});