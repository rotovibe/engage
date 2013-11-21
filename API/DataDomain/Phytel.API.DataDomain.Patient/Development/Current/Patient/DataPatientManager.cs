using Phytel.API.DataDomain.Patient.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Patient;

namespace Phytel.API.DataDomain.Patient
{
    public static class PatientDataManager
    {
        public static GetPatientDataResponse GetPatientByID(GetPatientDataRequest request)
        {
            GetPatientDataResponse result = new GetPatientDataResponse();

            IPatientRepository<GetPatientDataResponse> repo = PatientRepositoryFactory<GetPatientDataResponse>.GetPatientRepository(request.ContractNumber, request.Context);
            result.Patient = repo.FindByID(request.PatientID) as DTO.PatientData;
            
            return (result != null ? result : new GetPatientDataResponse());
        }

        public static GetPatientsDataResponse GetPatients(GetPatientsDataRequest request)
        {
            IPatientRepository<GetPatientsDataResponse> repo = PatientRepositoryFactory<GetPatientsDataResponse>.GetPatientRepository(request.ContractNumber, request.Context);
            GetPatientsDataResponse result = repo.Select(request.PatientIDs);

            return result;
        }
    }
}   
