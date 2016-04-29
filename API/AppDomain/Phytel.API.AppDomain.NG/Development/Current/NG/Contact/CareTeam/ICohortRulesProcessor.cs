using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG
{
    public interface ICohortRulesProcessor
    {
        void EnqueueCohorRuleCheck(CohortRuleCheckData cohortRuleCheckData);
        int GetQueueCount();
        void Start();
        void Stop();        
        string GetCareTeamActiveCorePCMId(CohortRuleCheckData cohortRuleCheckData);      
       
    }
}
