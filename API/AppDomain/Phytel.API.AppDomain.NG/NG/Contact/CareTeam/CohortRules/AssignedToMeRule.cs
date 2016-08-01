using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using ServiceStack.Common.Extensions;
using ServiceStack.Text;

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
                var contactIdsToAdd = new List<string>();
                foreach (var member in careTeam.Members)
                {
                    if (member.StatusId == (int) CareTeamMemberStatus.Active)
                    {
                        if (data.UsersContactIds.Contains(member.ContactId))
                        {
                            contactIdsToAdd.Add(member.ContactId);
                        }
                    }
                    
                }

                _contactEndpointUtil.AssignContactsToCohortPatientView(data.PatientId, contactIdsToAdd.Distinct().ToList(), data.Version, data.ContractNumber, data.UserId);
                response.IsSuccessful = true; 

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
