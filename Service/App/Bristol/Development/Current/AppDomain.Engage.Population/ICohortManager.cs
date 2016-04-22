using AppDomain.Engage.Population.DTO.Context;
using AppDomain.Engage.Population.DTO.Referrals;
namespace AppDomain.Engage.Population
{
    public interface ICohortManager
    {
        UserContext UserContext { get; set; }
        string CreatePatientReferral(string patientId, string referralId);
        PostReferralDefinitionResponse PostReferralDefinition(ReferralDefinitionData referral);
    }
}