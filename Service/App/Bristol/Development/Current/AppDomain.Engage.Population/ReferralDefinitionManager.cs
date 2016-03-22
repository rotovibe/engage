using AppDomain.Engage.Population.DataDomainClient;
using AppDomain.Engage.Population.DTO.Context;
using AppDomain.Engage.Population.DTO.Referrals;

namespace AppDomain.Engage.Population
{
    public class ReferralDefinitionManager : IReferralDefinitionManager
    {
        private readonly IServiceContext _context; // this is a general request context. it will have things like contract name, etc.
        public UserContext UserContext { get; set; } // this is a platform specific context. Has user authentication data if it is needed.
        private readonly IPatientDataDomainClient _client;

        public ReferralDefinitionManager(IServiceContext context, IPatientDataDomainClient client)
        {
            _context = context;
            _client = client;
        }

        // Collect the data from the 
        public PostReferralDefinitionResponse PostReferralDefinition(PostReferralDefinitionRequest request)
        {
            ReferralDefinitionData referral = request.ReferralDefinitionData;
            return _client.PostReferralDefinition(referral);
        }

    }
}