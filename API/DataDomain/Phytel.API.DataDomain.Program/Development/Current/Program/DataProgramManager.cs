using Phytel.API.DataDomain.Program.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Program;

namespace Phytel.API.DataDomain.Program
{
    public static class ProgramDataManager
    {
        public static ProgramResponse GetProgramByID(ProgramRequest request)
        {
            ProgramResponse result = new ProgramResponse();

            IProgramRepository<ProgramResponse> repo = ProgramRepositoryFactory<ProgramResponse>.GetProgramRepository(request.ContractNumber, request.Context);
            result = repo.FindByID(request.ProgramID) as ProgramResponse;
            
            return (result != null ? result : new ProgramResponse());
        }

        public static ProgramListResponse GetProgramList(ProgramListRequest request)
        {
            ProgramListResponse result = new ProgramListResponse();

            IProgramRepository<ProgramListResponse> repo = ProgramRepositoryFactory<ProgramListResponse>.GetProgramRepository(request.ContractNumber, request.Context);

            return result;
        }
    }
}   
