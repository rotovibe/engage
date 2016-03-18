using AppDomain.Engage.Population.DTO.Referrals;

namespace AppDomain.Engage.Population
{
    public interface IReferralDefinitionManager
    {
        PostReferralDefinitionResponse PostReferralDefinition(PostReferralDefinitionRequest request);
    }
}
