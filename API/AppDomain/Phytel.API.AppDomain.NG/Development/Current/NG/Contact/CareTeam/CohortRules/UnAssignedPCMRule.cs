using System;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using ServiceStack.Text;

namespace Phytel.API.AppDomain.NG
{

    public class UnAssignedPCMRule : ICareMemberCohortRule, ICohortCommand
    {
        public void Run(CareTeam careTeam)
        {
            if(careTeam == null)
                throw new ArgumentNullException("careTeam");

            if (CheckIfCareTeamHasActiveCorePCM(careTeam))
            {
                //Remove from UnAssigned PCM
            }
            else
            {
               //Add to UnAssigned PCM.
            }

        }

        public void Add()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }

        private bool CheckIfCareTeamHasActiveCorePCM(CareTeam team)
        {
            var hasPCM = false;

            if (team.Members.IsNullOrEmpty())
                return false;

            hasPCM = team.Members.Any(c =>
                    c.StatusId == (int) CareTeamMemberStatus.Active && c.Core == true &&
                    c.RoleId == Constants.PCMRoleId);
            

            return hasPCM;

        }

    }
}
