using AppDomain.Engage.Population.DTO.Referrals;

namespace AppDomain.Engage.Population
{
    public interface IReferralDefinitionManager
    {
        string PostReferralDefinition(PostReferralDefinitionRequest request);
    }
}
