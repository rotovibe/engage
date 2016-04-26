using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG
{
    public interface ICareMemberCohortRule
    {
        CohortRuleResponse Run(CareTeam careTeam, CohortRuleCheckData data);
    }
}
