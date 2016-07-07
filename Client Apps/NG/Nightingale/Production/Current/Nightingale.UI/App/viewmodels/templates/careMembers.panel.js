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
				//team validation single active core pcp/pcm 
				var error = null;
				if( member.statusId() == activeCareMemberStatus ){
					error = validateMemberRole( member );
				}								
				if( error ){
					alert( error );
				}
				else{
					member.core(true);
					saveMember( member );
				}
			}
			self.clearCore = function( member ){
				member.core( false );
				saveMember( member );
			}
			self.activate = function( member ){
				//team validation single active core pcp/pcm
				var error = null;
				if( member.core() ){
					error = validateMemberRole( member );
				}
				if( error ){
					alert( error );
				}
				else{
					member.statusId( activeCareMemberStatus );
					saveMember( member );
				}
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

		function validateMemberRole( member ){
			//assuming the member is active statusId and core, 
			//check if there is already pcm / pcp assigned in the members team:
			var error = null;
			var toDupName = '';
			if( member.roleId() == teamIndex.pcmContactSubType().id() ){
				var pcManagers = member.careTeam().primaryCareManagers();
				var pcm = ko.utils.arrayFirst( pcManagers, function(p){
					return p.id() != member.id();
				});
				if( pcm ){
					error = teamIndex.pcmContactSubType().role();
					if( pcm.contact() && pcm.contact().fullName() ){
						toDupName = ' as: ' + pcm.contact().fullName();
					}
				}
			}
			else if( member.roleId() == teamIndex.pcpContactSubType().id() ){
				var pcPysicians = member.careTeam().primaryCarePhysicians();
				var pcp = ko.utils.arrayFirst( pcPysicians, function(p){
					return p.id() != member.id(); 
				});
				if( pcp ){
					error = teamIndex.pcpContactSubType().role();
					if( pcp.contact() && pcp.contact().fullName() ){
						toDupName = ' as: ' + pcp.contact().fullName();
					}
				}
			}
			if( error ){
				error += ' is already assigned' + toDupName;
			}			
			return error;
		}
		
		function saveMember( member ){
			return datacontext.saveCareTeamMember( member );
		}		
		
		return ctor;
	}
);