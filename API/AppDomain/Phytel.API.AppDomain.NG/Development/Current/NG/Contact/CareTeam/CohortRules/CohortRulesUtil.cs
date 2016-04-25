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
    public static partial class NGUtils
    {
        public static bool CheckIfCareTeamHasActiveCorePCM(CareTeam team)
        {
            var hasPCM = false;

            if (team.Members.IsNullOrEmpty())
                return false;

            hasPCM = team.Members.Any(c =>
                    c.StatusId == (int)CareTeamMemberStatus.Active && c.Core == true &&
                    c.RoleId == Constants.PCMRoleId);


            return hasPCM;

        }

        public static Member GetCareTeamActiveCorePCM(CareTeam team)
        {
            Member res = null;

            if (team.Members.IsNullOrEmpty())
                return res;

            res = team.Members.FirstOrDefault(c =>
                    c.StatusId == (int)CareTeamMemberStatus.Active && c.Core == true &&
                    c.RoleId == Constants.PCMRoleId);


            return res;

        }

        public static bool HasMultipleActiveCorePCM(CareTeam careTeam)
        {
            bool res = false;

            if (careTeam == null) return false;
            var activeCorePCMs =
                careTeam.Members.Select(
                    c =>
                        c.RoleId == Constants.PCMRoleId && c.Core == true &&
                        c.StatusId == (int)CareTeamMemberStatus.Active);
            if (!activeCorePCMs.IsNullOrEmpty() && activeCorePCMs.Count() > 1)
                res = true; ;

            return res;
        }
    }
}
