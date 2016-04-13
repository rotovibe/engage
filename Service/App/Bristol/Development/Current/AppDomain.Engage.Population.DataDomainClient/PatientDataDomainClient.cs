using System;
using AppDomain.Engage.Population.DTO.Context;
using AppDomain.Engage.Population.DTO.Referrals;
using AutoMapper;
using Phytel.API.Common;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using ServiceStack.Service;
using PostReferralDefinitionRequest = Phytel.API.DataDomain.Cohort.DTO.Referrals.PostReferralDefinitionRequest;

namespace AppDomain.Engage.Population.DataDomainClient
{
    public class PatientDataDomainClient : IPatientDataDomainClient
    {
        private readonly string _domainUrl;
        private readonly IHelpers _urlHelper;
        private readonly IRestClient _client;
        private readonly IServiceContext _context;
        protected readonly IMappingEngine _mappingEngine;
        public UserContext UserContext { get; set; }

        public PatientDataDomainClient(IMappingEngine mappingEngine, string domainUrl, IHelpers urlHelper, IRestClient client, IServiceContext context)
        {
            _mappingEngine = mappingEngine;
            _domainUrl = domainUrl;
            _urlHelper = urlHelper;
            _client = client;
            _context = context;
        }

        // example implementation
        public string PostPatientDetails(PatientData patients)
        {
            var patientId = string.Empty;
            try
            {
                if (patients == null) return patientId;

                var url = _urlHelper.BuildURL(string.Format("{0}/{1}/{2}/{3}/{4}/Patient", 
                    _domainUrl,
                    "api",
                    "NG",
                    _context.Version,
                    _context.Contract), _context.UserId);

                PutPatientDataResponse response = _client.Put<PutPatientDataResponse>(url, new PutPatientDataRequest
                {
                    Context = "NG",
                    ContractNumber = _context.Contract,
                    Patient = patients,
                    UserId = _context.UserId,
                    Version = _context.Version
                } as object);

                if (response != null)
                {
                    patientId = response.Id;
                }
                return patientId;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("AD:PostPatientDetails()::" + ex.Message, ex.InnerException);
            }
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
    }
}