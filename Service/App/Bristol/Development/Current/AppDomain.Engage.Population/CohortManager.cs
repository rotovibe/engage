using AppDomain.Engage.Population.DataDomainClient;
using AppDomain.Engage.Population.DTO.Context;
using AppDomain.Engage.Population.DTO.Referrals;
using System;

namespace AppDomain.Engage.Population
{
    public class CohortManager:ICohortManager
    {
        private readonly IServiceContext _context; // this is a general request context. it will have things like contract name, etc.
        public UserContext UserContext { get; set; } // this is a platform specific context. Has user authentication data if it is needed.
        private readonly ICohortDataDomainClient _client;

        public CohortManager(IServiceContext context, ICohortDataDomainClient client)
        {
            _context = context;
            _client = client;
        }
        public PostReferralDefinitionResponse PostReferralDefinition(ReferralDefinitionData referral)
        {
            return _client.PostReferralDefinition(referral, UserContext);
        }

        public string CreatePatientReferral(string patientId, string referralId)
        {
            if (patientId == null)
                throw new ArgumentNullException("Request parameter patientId value cannot be NULL/EMPTY");
            if (referralId == null)
                throw new ArgumentNullException("Request parameter referralId value cannot be NULL/EMPTY");
            return _client.PostPatientReferralDefinition(patientId, referralId, UserContext);
        }
    }
}
