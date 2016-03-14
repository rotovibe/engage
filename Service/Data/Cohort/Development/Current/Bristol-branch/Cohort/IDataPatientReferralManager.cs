using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;

namespace Phytel.API.DataDomain.Cohort
{
    public interface IDataPatientReferralManager
    {
        PostPatientReferralDefinitionResponse InsertPatientReferral(PostPatientReferralDefinitionRequest patientReferral);
    }
}