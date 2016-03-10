using Phytel.API.DataDomain.Cohort.DTO.Model;

namespace Phytel.API.DataDomain.Cohort
{
    public interface IDataPatientReferralManager
    {
        void SavePatientReferralData(PatientReferralData patientReferral);
    }
}