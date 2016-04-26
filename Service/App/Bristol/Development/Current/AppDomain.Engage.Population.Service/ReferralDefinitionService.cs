using System;
using System.Web;
using AppDomain.Engage.Population.DTO.Context;
using AppDomain.Engage.Population.DTO.Referrals;
using AppDomain.Engage.Population.DTO.Demographics;
using Phytel.API.AppDomain.Platform.Security.DTO;
using Phytel.API.ASE.Client.Interface;
using Phytel.API.Common.Audit;
using Phytel.Services.API.Platform.Filter;
using Phytel.Services.API.Platform.Filter.Attributes;
using Phytel.Services.API.Provider;



namespace AppDomain.Engage.Population.Service
{
    [IsAuthenticatedFilter("br")]
    public class ReferralDefinitionService : ServiceBase
    {
        private readonly IHostContextProxy _hostContextProxy;
        private readonly IAuditLogger _auditLogger;
        private readonly IASEClient _client;
        private UserContext _userContext;
        // used to retrieve the referral/registry/cohort definition.
        public ICohortManager CohortManager { get; set; }
        public IDemographicsManager DemographicsManager { get; set; }

        public IAuditHelpers AuditHelpers { get; set; }

        public ReferralDefinitionService(IHostContextProxy hostContextProxy, IAuditLogger auditLogger, IASEClient client)
        {
            _hostContextProxy = hostContextProxy;
            _userContext = BuildUserContext(_hostContextProxy.GetItem(FilterAttributesConstants.UserSessionName) as AppDomainSessionInfo);
            _client = client;
            _auditLogger = auditLogger;
        }

        public PostReferralDefinitionResponse Post(PostReferralDefinitionRequest request)
        {
            CohortManager.UserContext = _userContext; // initialize this for every service handler.
            var response = new PostReferralDefinitionResponse();
            try
            {
                response = CohortManager.PostReferralDefinition(request.ReferralDefinitionData);

                // FYI : use this to log any patients that you might see.
                //_auditLogger.Patients = new List<string> { "patient1", "patient2", "patient3" };
            }
            catch (Exception ex)
            {
                FormatException(request, new PostReferralDefinitionResponse(), _client, ex);
            }
            return response;
        }

        public PostReferralWithPatientsListResponse Post(PostReferralWithPatientsListRequest request)
        {
            
            CohortManager.UserContext = _userContext;
            DemographicsManager.UserContext = _userContext;
            CohortManager.UserContext = _userContext;
            var patientsResponse = new PostReferralWithPatientsListResponse();
               
                try
                {
                    //create request and make call to referraldefinitionmanager.Postreferraldefinition
                    var responsereferraldefinition = new PostReferralDefinitionResponse();
                    responsereferraldefinition = CohortManager.PostReferralDefinition(request.ReferralDefinitionData);

                    //get the referral id back from response
                    string referralid = responsereferraldefinition.ReferralId;

                    if (!String.IsNullOrEmpty(referralid))
                    {
                        //create request and make call to patientdatamanager.bulkinsertpatients with request.patientslist and referralid
                        patientsResponse = DemographicsManager.InsertBulkPatients(request.PatientsData);

                        //get the patiendids that are inserted from response of previous call and insert into patientreferral
                        if (patientsResponse.ProcessedPatients.InsertedPatients != null && patientsResponse.ProcessedPatients.InsertedPatients.Count > 0)
                            foreach (ProcessedData pd in patientsResponse.ProcessedPatients.InsertedPatients)
                                CohortManager.CreatePatientReferral(pd.Id, referralid);

                        
                        patientsResponse.ReferralId = referralid;
                    }

                }

                catch (Exception ex)
                {
                CommonFormatter.FormatExceptionResponse(patientsResponse, base.Response, ex);
                }
                return patientsResponse;
            
        }
    }
}