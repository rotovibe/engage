using AppDomain.Engage.Population.DTO.Referrals;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;

namespace AppDomain.Engage.Population.DataDomainClient
{
    public interface IPatientDataDomainClient
    {
        string PostPatientDetails(PatientData patients);

        string PostReferralDefinition(ReferralDefinitionData referral);
    }
}