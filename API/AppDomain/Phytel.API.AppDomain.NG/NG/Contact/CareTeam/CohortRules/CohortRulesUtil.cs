using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.Common.Extensions;
using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.AppDomain.NG
{
    public class CohortRoleUtils: ICohortRuleUtil
    {
        public  bool CheckIfCareTeamHasActiveCorePCM(CareTeam team)
        {
            var hasPCM = false;

            if (team.Members.IsNullOrEmpty())
                return false;

            hasPCM = team.Members.Any(c =>
                    c.StatusId == (int)CareTeamMemberStatus.Active && c.Core == true &&
                    c.RoleId == Constants.PCMRoleId);


            return hasPCM;

        }

        public bool ActiveCorePcmIsUser(CareTeam team, List<string> usersContactIds )
        {
            bool res = false;

            if (team.Members.IsNullOrEmpty() || usersContactIds==null)
                return res;

            var activeCorePcm = team.Members.FirstOrDefault(c =>
                    c.StatusId == (int)CareTeamMemberStatus.Active && c.Core == true &&
                    c.RoleId == Constants.PCMRoleId);

            if (activeCorePcm==null) return res;

            res = usersContactIds.Contains(activeCorePcm.ContactId);
            return res;

        }

        public  Member GetCareTeamActiveCorePCM(CareTeam team)
        {
            Member res = null;

            if (team.Members.IsNullOrEmpty())
                return res;

            res = team.Members.FirstOrDefault(c =>
                    c.StatusId == (int)CareTeamMemberStatus.Active && c.Core == true &&
                    c.RoleId == Constants.PCMRoleId);


            return res;

        }

        public  bool HasMultipleActiveCorePCM(CareTeam careTeam)
        {
            bool res = false;

            if (careTeam == null) return false;
            var activeCorePCMs =
                careTeam.Members.Where(
                    c =>
                        c.RoleId == Constants.PCMRoleId && c.Core == true &&
                        c.StatusId == (int)CareTeamMemberStatus.Active).ToList();
            if (!activeCorePCMs.IsNullOrEmpty() && activeCorePCMs.Count() > 1)
                res = true;

            return res;
        }

        public  bool HasMultipleActiveCorePCP(CareTeam careTeam)
        {
            bool res = false;

            if (careTeam == null) return false;
            var activeCorePCMs =
                careTeam.Members.Where(
                    c =>
                        c.RoleId == Constants.PCPRoleId && c.Core == true &&
                        c.StatusId == (int)CareTeamMemberStatus.Active).ToList();
            if (!activeCorePCMs.IsNullOrEmpty() && activeCorePCMs.Count() > 1)
                res = true;

            return res;
        }
    }
}
