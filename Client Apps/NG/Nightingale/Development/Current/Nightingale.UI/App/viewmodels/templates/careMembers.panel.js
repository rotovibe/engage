/**
*
* 	@module careMembers.panel
*/

define(['viewmodels/patients/team/index', 'services/datacontext'],
	function(teamIndex, datacontext){
		var ctor = function(){
			var self = this;
		};
		
		var activeCareMemberStatus = 1;
		var inActiveCarememberStatus = 2;
		
		ctor.prototype.activate = function (data) {
			var self = this;
			self.careMembers = data.careMembers;
			self.selectedCareMember = teamIndex.selectedCareMember;
			self.selectCareMember = function( member ){
				self.selectedCareMember( member );
			}
			self.setCore = function( member ){
				member.core(true);				
				saveMember( member );
			}
			self.clearCore = function( member ){
				member.core( false );
				saveMember( member );
			}
			self.activate = function( member ){
				member.statusId( activeCareMemberStatus );
				saveMember( member );
			}
			self.deactivate = function( member ){
				member.statusId( inActiveCarememberStatus );
				saveMember( member );
			}
			self.editCareMember = function( member ){
				teamIndex.editCareMember( member );
			}
			self.deleteCareMember = function( member ){
				teamIndex.deleteCareMember( member );
			}
			
		}

		function saveMember( member ){
			return datacontext.saveCareTeamMember( member );
		}		
		
		return ctor;
	}
);