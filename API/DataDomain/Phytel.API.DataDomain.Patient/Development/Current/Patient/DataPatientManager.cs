using Phytel.API.DataDomain.Patient.DTO;
using System.Data.SqlClient;

namespace Phytel.API.DataDomain.Patient
{
    public static class PatientDataManager
    {
        public static PatientResponse GetPatientByID(PatientRequest request)
        {
            PatientResponse result = new PatientResponse();

            IPatientRepository<PatientResponse> repo = Phytel.API.DataDomain.Patient.PatientRepositoryFactory<PatientResponse>.GetPatientRepository(request.ContractNumber, request.Context);
            result = repo.FindByID(request.PatientID) as PatientResponse;
            
            return (result != null ? result : new PatientResponse());
        }

        public static PatientListResponse GetPatientList(PatientListRequest request)
        {
            PatientListResponse result = new PatientListResponse();

            IPatientRepository<PatientListResponse> repo = Phytel.API.DataDomain.Patient.PatientRepositoryFactory<PatientListResponse>.GetPatientRepository(request.ContractNumber, request.Context);

            return result;
        }
    }
}   
