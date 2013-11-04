using Phytel.API.DataDomain.Patient.DTO;
using System.Data.SqlClient;

namespace Phytel.API.DataDomain.Patient
{
    public static class PatientDataManager
    {
        public static PatientResponse GetPatientByID(PatientRequest request)
        {
            PatientResponse result = null;

            IPatientRepository<PatientResponse> repo = Phytel.API.DataDomain.Patient.PatientRepositoryFactory<PatientResponse>.GetPatientRepository("INHEALTH001", string.Empty);
            result = (PatientResponse)repo.FindByID(request.PatientID);
            if (result == null)
                result = new PatientResponse();

            return result;
        }
    }
}   
