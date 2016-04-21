using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG
{
    public interface ICareMemberCohortRuleFactory
    {
        List<ICareMemberCohortRule> GenerateEngageCareMemberCohortRules();
    }
}
