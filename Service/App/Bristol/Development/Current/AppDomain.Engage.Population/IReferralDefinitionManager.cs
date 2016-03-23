using AppDomain.Engage.Population.DTO.Context;
using AppDomain.Engage.Population.DTO.Referrals;

namespace AppDomain.Engage.Population
{
    public interface IReferralDefinitionManager
    {
        UserContext UserContext { get; set; }
        PostReferralDefinitionResponse PostReferralDefinition(ReferralDefinitionData referral);
    }
}
