using AppDomain.Engage.Population.DataDomainClient;
using AppDomain.Engage.Population.DTO.Context;
using AppDomain.Engage.Population.DTO.Referrals;

namespace AppDomain.Engage.Population
{
    public class ReferralDefinitionManager : IReferralDefinitionManager
    {
        private readonly IServiceContext _context;
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