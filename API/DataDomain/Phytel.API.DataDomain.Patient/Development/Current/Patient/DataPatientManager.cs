using Phytel.API.DataDomain.Patient.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Patient;

namespace Phytel.API.DataDomain.Patient
{
    public static class PatientDataManager
    {
        public static PatientResponse GetPatientByID(PatientRequest request)
        {
            PatientResponse result = new PatientResponse();

            IPatientRepository<PatientResponse> repo = PatientRepositoryFactory<PatientResponse>.GetPatientRepository(request.ContractNumber, request.Context);
            result = repo.FindByID(request.PatientID) as PatientResponse;
            
            return (result != null ? result : new PatientResponse());
        }

        public static PatientListResponse GetPatientList(PatientListRequest request)
        {
            PatientListResponse result = new PatientListResponse();

            IPatientRepository<PatientListResponse> repo = PatientRepositoryFactory<PatientListResponse>.GetPatientRepository(request.ContractNumber, request.Context);
            
            return result;
        }

        public static PatientDetailsResponse GetPatientDetailsList(PatientDetailsRequest request)
        {
            IPatientRepository<PatientDetailsResponse> repo = PatientRepositoryFactory<PatientDetailsResponse>.GetPatientRepository(request.ContractNumber, request.Context);
            PatientDetailsResponse result = repo.Select(request.PatientIds);

            return result;
        }
    }
}   
