/**
*
* 	@module careMembers.panel
*/

define(['viewmodels/patients/team/index'],
	function(teamIndex){
		var ctor = function(){
			var self = this;
		};
		
		ctor.prototype.activate = function (data) {
			var self = this;
			self.careMembers = data.careMembers;
			self.selectedCareMember = teamIndex.selectedCareMember;
		}
	}
);