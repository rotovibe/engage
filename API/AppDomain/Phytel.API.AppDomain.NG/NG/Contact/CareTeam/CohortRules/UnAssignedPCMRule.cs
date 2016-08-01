using System;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG
{
    /// <summary>
    /// Check if a care member has Active,Core PCM and if not remove the user from PCM.
    /// </summary>
    public class UnAssignedPCMRule : ICareMemberCohortRule
    {
        private readonly IContactEndpointUtil _contactEndpointUtil;
        private readonly ILogger _logger;
        private readonly ICohortRuleUtil _cohortRuleUtil;

        public UnAssignedPCMRule(IContactEndpointUtil contactEndpointUtil, ILogger logger, ICohortRuleUtil cohortRuleUtil)
        {
            if (contactEndpointUtil == null)
                throw new ArgumentNullException("contactEndpointUtil");

            if (logger == null)
                throw new ArgumentNullException("logger");

            if (cohortRuleUtil == null)
                throw new ArgumentNullException("cohortRuleUtil");

            _contactEndpointUtil = contactEndpointUtil;
            _logger = logger;
            _cohortRuleUtil = cohortRuleUtil;
        }

        #region ICareMemberCohortRule Members

        public CohortRuleResponse Run(CareTeam careTeam, CohortRuleCheckData data)
        {
            var response = new CohortRuleResponse();
            try
            {
                if (careTeam == null)
                    throw new ArgumentNullException("careTeam");

                //Check if any of the members have active core PCM role.
                if (!_cohortRuleUtil.CheckIfCareTeamHasActiveCorePCM(careTeam))
                {
                    //Add to UnAssigned PCM.
                   var isSuccessful = _contactEndpointUtil.RemovePCMCohortPatientView(data.PatientId,data.Version, data.ContractNumber, data.UserId);

                    response.IsSuccessful = isSuccessful;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorCode = "UnAssignedPCMRule.Cohort.Error";
                response.Message = ex.Message;

                _logger.Log(ex);
            }

            return response;
        }

        #endregion
    }
}
