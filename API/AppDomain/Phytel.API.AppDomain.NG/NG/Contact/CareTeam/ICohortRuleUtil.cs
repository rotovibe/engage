using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG
{

    public interface ICohortRuleUtil
    {
        bool CheckIfCareTeamHasActiveCorePCM(CareTeam team);
        Member GetCareTeamActiveCorePCM(CareTeam team);
        bool HasMultipleActiveCorePCM(CareTeam careTeam);
        bool HasMultipleActiveCorePCP(CareTeam careTeam);
        bool ActiveCorePcmIsUser(CareTeam team, List<string> usersContactIds);
    }
}
