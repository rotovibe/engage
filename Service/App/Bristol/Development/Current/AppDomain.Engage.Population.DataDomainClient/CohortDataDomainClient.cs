using System;
using System.Collections.Generic;
using AutoMapper;
using ServiceStack.Service;
using Phytel.API.Common;
using AppDomain.Engage.Population.DTO.Context;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;
using AppDomain.Engage.Population.DTO.Referrals;
using PostReferralDefinitionRequest = Phytel.API.DataDomain.Cohort.DTO.Referrals.PostReferralDefinitionRequest;
using PostReferralDefinitionResponse = AppDomain.Engage.Population.DTO.Referrals.PostReferralDefinitionResponse;

namespace AppDomain.Engage.Population.DataDomainClient
{
    public class CohortDataDomainClient : ICohortDataDomainClient
    {
        private readonly string _domainUrl;
        private readonly IHelpers _urlHelper;
        private readonly IRestClient _client;
        private readonly IServiceContext _context;
        protected readonly IMappingEngine _mappingEngine;
        public UserContext UserContext { get; set; }

        public CohortDataDomainClient(IMappingEngine mappingEngine, string domainUrl, IHelpers urlHelper, IRestClient client, IServiceContext context)
        {
            _mappingEngine = mappingEngine;
            _domainUrl = domainUrl;
            _urlHelper = urlHelper;
            _client = client;
            _context = context;
        }

        public PostReferralDefinitionResponse PostReferralDefinition(ReferralDefinitionData referralDefinitionData, UserContext userContext)
        {
            try
            {

                ReferralData referralData = _mappingEngine.Map<ReferralData>(referralDefinitionData);
                var url = _urlHelper.BuildURL(string.Format("{0}/{1}/{2}/{3}/{4}/Referrals",
                    _domainUrl,
                    "api",
                    "NG",
                    _context.Version,
                    _context.Contract), userContext.UserId);

                var response = _client.Post<PostReferralDefinitionResponse>(url, new PostReferralDefinitionRequest
                {
                    Referral = referralData,
                    Context = "NG",
                    ContractNumber = _context.Contract,
                    UserId = userContext.UserId,
                    Version = _context.Version
                });

                return response;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("AD:PostReferralDefinition()::" + ex.Message, ex.InnerException);
            }
        }

        public string PostPatientReferralDefinition(string patientId, string referralId,UserContext userContext)
        {
            PatientReferralData patientReferral = new PatientReferralData();
            patientReferral.PatientId = patientId;
            patientReferral.ReferralId = referralId;
            patientReferral.ReferralDate = DateTime.Now;
            patientReferral.CreatedBy = _context.UserId;
            // check whether hardcoding can be removed by checking request object
            var url = _urlHelper.BuildURL(string.Format("{0}/{1}/{2}/{3}/{4}/PatientReferrals",
                   _domainUrl,
                   "api",
                   "NG",
                   _context.Version,
                   _context.Contract), userContext.UserId);
            var response = _client.Post<PostPatientReferralDefinitionResponse>(url, new PostPatientReferralDefinitionRequest
            {
                PatientReferral = patientReferral,
                Context = "NG",
                ContractNumber = _context.Contract,
                Version = _context.Version,
                UserId = userContext.UserId
            });

            return response.PatientReferralId;

        }



    }
}
