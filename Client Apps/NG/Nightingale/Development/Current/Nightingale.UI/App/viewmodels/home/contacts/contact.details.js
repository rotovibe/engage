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