using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG
{
    /// <summary>
    /// This rules checks a care team to ensure that if a member is an Active Core PCM then
    /// a corresponding "Assigned to me PCM"  cohort exists for the referenced individual.
    /// </summary>
    public class AssignedToMePCMRule : ICareMemberCohortRule
    {
        private readonly ICohortRuleUtil _cohortRuleUtil;

        public AssignedToMePCMRule(IContactEndpointUtil contactEndpointUtil, ILogger logger, ICohortRuleUtil cohortRuleUtil)
        {
            _cohortRuleUtil = cohortRuleUtil;
        }

        public CohortRuleResponse Run(CareTeam careTeam, CohortRuleCheckData data)
        {
            if (careTeam == null)
                throw new ArgumentNullException("careTeam");

            var activeCorePCM = _cohortRuleUtil.GetCareTeamActiveCorePCM(careTeam);

            if (activeCorePCM != null)
            {
                Add();
            }
            else
            {
                Remove();
            }

            return new CohortRuleResponse();

        }

        public void Add()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
