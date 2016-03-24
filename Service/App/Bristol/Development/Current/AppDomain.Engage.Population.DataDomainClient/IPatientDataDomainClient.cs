using AppDomain.Engage.Population.DTO.Context;
using AppDomain.Engage.Population.DTO.Referrals;
using Phytel.API.DataDomain.Patient.DTO;

namespace AppDomain.Engage.Population.DataDomainClient
{
    public interface IPatientDataDomainClient
    {
        string PostPatientDetails(PatientData patients);

        PostReferralDefinitionResponse PostReferralDefinition(ReferralDefinitionData referral, UserContext userContext);
    }
}