using Phytel.API.DataDomain.PatientSystem.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientSystem;

namespace Phytel.API.DataDomain.PatientSystem
{
    public class PatientSystemDataManager : IPatientSystemDataManager
    {
        public IPatientSystemRepositoryFactory Factory { get; set; }

        public GetPatientSystemDataResponse GetPatientSystem(GetPatientSystemDataRequest request)
        {
            GetPatientSystemDataResponse result = new GetPatientSystemDataResponse();

            IPatientSystemRepository repo = Factory.GetRepository(request, RepositoryType.PatientSystem);
                //PatientSystemRepositoryFactory<GetPatientSystemDataResponse>.GetPatientSystemRepository(request.ContractNumber, request.Context, request.UserId);
            
            result.PatientSystem = repo.FindByID(request.PatientSystemID) as PatientSystemData;

            return result;
        }

        public GetAllPatientSystemsDataResponse GetAllPatientSystems(GetAllPatientSystemsDataRequest request)
        {
            GetAllPatientSystemsDataResponse result = new GetAllPatientSystemsDataResponse();

            IPatientSystemRepository repo = Factory.GetRepository(request, RepositoryType.PatientSystem);
            //IPatientSystemRepository<GetAllPatientSystemsDataResponse> repo = PatientSystemRepositoryFactory<GetAllPatientSystemsDataResponse>.GetPatientSystemRepository(request.ContractNumber, request.Context, request.UserId);
            
            return result;
        }

        public PutPatientSystemDataResponse PutPatientSystem(PutPatientSystemDataRequest request)
        {
            //IPatientSystemRepository<PutPatientSystemDataResponse> repo = PatientSystemRepositoryFactory<PutPatientSystemDataResponse>.GetPatientSystemRepository(request.ContractNumber, request.Context, request.UserId);
            IPatientSystemRepository repo = Factory.GetRepository(request, RepositoryType.PatientSystem);

            PutPatientSystemDataResponse result = repo.Insert(request) as PutPatientSystemDataResponse;
            return result;
        }
    }
}   
