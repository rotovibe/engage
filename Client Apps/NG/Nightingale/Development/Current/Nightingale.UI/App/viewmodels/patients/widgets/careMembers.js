/**
*	careMembers widget
*	@module careMembers
*/

define(['viewmodels/patients/team/index'],
	function( teamIndex ){
		var ctor = function () {
            var self = this;
        };
		
		ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
			self.widget = self.settings.widget;
			self.statusIds = self.widget.statusIds;
			self.careMembers = self.settings.careMembers;
			self.myCareMembers = ko.computed(function () {
				var members = [];
				members = ko.utils.arrayFilter(self.careMembers, function (member) {
					return ( !member.isNew() && self.statusIds.indexOf( member.statusId ) !== -1 );
                });
				
				return members;
			});
			
			self.addCareMember = teamIndex.addCareMember;
			self.selectedPatient = teamIndex.selectedPatient;
			self.isOpen = self.widget.isOpen;
			self.isFullScreen = self.widget.isFullScreen;
			self.toggleFullScreen = function (sender) {
                self.isFullScreen(!self.isFullScreen());
            }
			self.toggleWidgetOpen = function (sender) {
                // Find how many widgets are open
                var openwidgets = ko.utils.arrayFilter(self.widget.column.widgets(), function (wid) {
                    return wid.isOpen();
                });
                // If the widget is the only open widget
                if (openwidgets.length === 1 && openwidgets[0] === self.widget) {
                    // Do nothing
                } else {
                    sender.isOpen(!sender.isOpen());
                }
            };
			self.toggleOpenColumn = teamIndex.toggleOpenColumn;
			self.leftColumnOpen = teamIndex.leftColumnOpen;	
				//self.leftColumnOpen();
                //self.widget.column.isOpen(!self.widget.column.isOpen());
            
			
		};
		
		ctor.prototype.detached = function(){
            var self = this;
            // dispose computeds:
            self.myCareMembers.dispose();
        }
        return ctor;
	}
);