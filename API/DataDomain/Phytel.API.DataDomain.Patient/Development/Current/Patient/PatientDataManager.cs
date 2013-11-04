using Phytel.API.DataDomain.Patient.DTO;
using System.Data.SqlClient;

namespace Phytel.API.DataDomain.Patient
{
    public static class PatientDataManager
    {
        public static PatientDataResponse LoginUser(PatientDataRequest request)
        {
            PatientDataResponse result = null;

            IPatientRepository<PatientDataResponse> repo = Phytel.API.DataDomain.Patient.PatientRepositoryFactory<PatientDataResponse>.GetPatientRepository(string.Empty);
            result = repo.ProcessUserToken(request.UserToken);
            if (result == null)
                result = new PatientDataResponse();

            return result;
        }
    }
}   
