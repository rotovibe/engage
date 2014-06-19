using Phytel.API.DataDomain.PatientSystem.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientSystem;

namespace Phytel.API.DataDomain.PatientSystem
{
    public static class PatientSystemDataManager
    {
        public static GetPatientSystemDataResponse GetPatientSystem(GetPatientSystemDataRequest request)
        {
            GetPatientSystemDataResponse result = new GetPatientSystemDataResponse();

            IPatientSystemRepository<GetPatientSystemDataResponse> repo = PatientSystemRepositoryFactory<GetPatientSystemDataResponse>.GetPatientSystemRepository(request.ContractNumber, request.Context, request.UserId);
            
            result.PatientSystem = repo.FindByID(request.PatientSystemID) as PatientSystemData;

            return result;
        }

        public static GetAllPatientSystemsDataResponse GetAllPatientSystems(GetAllPatientSystemsDataRequest request)
        {
            GetAllPatientSystemsDataResponse result = new GetAllPatientSystemsDataResponse();

            IPatientSystemRepository<GetAllPatientSystemsDataResponse> repo = PatientSystemRepositoryFactory<GetAllPatientSystemsDataResponse>.GetPatientSystemRepository(request.ContractNumber, request.Context, request.UserId);
            
            return result;
        }

        public static PutPatientSystemDataResponse PutPatientSystem(PutPatientSystemDataRequest request)
        {
            IPatientSystemRepository<PutPatientSystemDataResponse> repo = PatientSystemRepositoryFactory<PutPatientSystemDataResponse>.GetPatientSystemRepository(request.ContractNumber, request.Context, request.UserId);

            PutPatientSystemDataResponse result = repo.Insert(request) as PutPatientSystemDataResponse;
            return result;
        }
    }
}   
