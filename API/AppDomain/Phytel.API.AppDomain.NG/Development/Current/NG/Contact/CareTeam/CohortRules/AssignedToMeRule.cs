using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG
{
    public class AssignedToMeRule : ICareMemberCohortRule
    {
        private readonly IContactEndpointUtil _contactEndpointUtil;
        private readonly ILogger _logger;
        private readonly ICohortRuleUtil _cohortRuleUtil;

        public AssignedToMeRule(IContactEndpointUtil contactEndpointUtil, ILogger logger, ICohortRuleUtil cohortRuleUtil)
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

                //For each member in the careteam that is a user, add an ATO cohort for the referenced individual
                foreach (var member in careTeam.Members)
                {
                    if (data.UsersContactIds.Contains(member.ContactId))
                    {
                        
                    }
                }
                
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorCode = "AssignedToMeRule.Cohort.Error";
                response.Message = ex.Message;

                _logger.Log(ex);
            }

            return response;
        }
        #endregion
    }
}
