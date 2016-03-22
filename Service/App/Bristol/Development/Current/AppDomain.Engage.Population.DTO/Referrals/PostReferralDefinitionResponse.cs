using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace AppDomain.Engage.Population.DTO.Referrals
{
    public class PostReferralDefinitionResponse : IDomainResponse
    {
        public double Version { get; set; }

        public string ReferralId { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public ResponseStatus Status { get; set; }
    }
}