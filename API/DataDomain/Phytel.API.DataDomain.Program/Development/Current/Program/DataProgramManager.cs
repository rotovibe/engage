using Phytel.API.DataDomain.Program.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Program;

namespace Phytel.API.DataDomain.Program
{
    public static class ProgramDataManager
    {
        public static GetProgramResponse GetProgramByID(GetProgramRequest request)
        {
            GetProgramResponse result = new GetProgramResponse();

            IProgramRepository<GetProgramResponse> repo = ProgramRepositoryFactory<GetProgramResponse>.GetProgramRepository(request.ContractNumber, request.Context);
            result = repo.FindByID(request.ProgramID) as GetProgramResponse;
            
            return (result != null ? result : new GetProgramResponse());
        }

        public static GetAllProgramsResponse GetProgramList(GetAllProgramsRequest request)
        {
            GetAllProgramsResponse result = new GetAllProgramsResponse();

            IProgramRepository<GetAllProgramsResponse> repo = ProgramRepositoryFactory<GetAllProgramsResponse>.GetProgramRepository(request.ContractNumber, request.Context);

            return result;
        }
    }
}   
