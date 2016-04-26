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

        public UnAssignedPCMRule(IContactEndpointUtil contactEndpointUtil, ILogger logger)
        {
            if (contactEndpointUtil == null)
                throw new ArgumentNullException("contactEndpointUtil");

            if (logger == null)
                throw new ArgumentNullException("logger");

            _contactEndpointUtil = contactEndpointUtil;
            _logger = logger;

        }

        #region ICareMemberCohortRule Members

        public CohortRuleResponse Run(CareTeam careTeam)
        {
            var response = new CohortRuleResponse();
            try
            {
                if (careTeam == null)
                    throw new ArgumentNullException("careTeam");

                //TODO : Remove the NGUtils dependency and Inject for mocking.
                if (NGUtils.CheckIfCareTeamHasActiveCorePCM(careTeam))
                {

                }
                else
                {
                    //Add to UnAssigned PCM.
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
