using System;
using System.Collections.Generic;
using MongoDB.Bson.IO;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Text;

namespace Phytel.API.AppDomain.NG
{
    /// <summary>
    /// This rules checks a care team to ensure that if a member is an Active Core PCM then
    /// a corresponding "Assigned to me PCM"  cohort exists for the referenced individual with the contact Id of the member.
    /// </summary>
    public class AssignedToMePCMRule : ICareMemberCohortRule
    {
        private readonly IContactEndpointUtil _contactEndpointUtil;
        private readonly ICohortRuleUtil _cohortRuleUtil;
        private readonly ILogger _logger;
        public AssignedToMePCMRule(IContactEndpointUtil contactEndpointUtil, ILogger logger, ICohortRuleUtil cohortRuleUtil)
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

        public CohortRuleResponse Run(CareTeam careTeam, CohortRuleCheckData data)
        {
            var response = new CohortRuleResponse();
            if (careTeam == null)
                throw new ArgumentNullException("careTeam");

            try
            {
                var activeCorePCM = _cohortRuleUtil.GetCareTeamActiveCorePCM(careTeam);

                if (activeCorePCM != null)
                {
                    //We need to add Active Core PCM from the CohortPatientView for the referenced individual
                    if (!data.UsersContactIds.IsNullOrEmpty())
                    {
                        _contactEndpointUtil.AddPCMToCohortPatientView(data.PatientId, activeCorePCM.ContactId,data.Version, data.ContractNumber,data.UserId, data.UsersContactIds.Contains(activeCorePCM.ContactId));
                    }
                }

                response.IsSuccessful = true;
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
