using Phytel.API.DataDomain.Program;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.DataDomain.Program.Service
{
    public class ProgramService : ServiceStack.ServiceInterface.Service
    {
        public ProgramResponse Post(ProgramRequest request)
        {
            ProgramResponse response = ProgramDataManager.GetProgramByID(request);
            response.Version = request.Version;
            return response;
        }

        public ProgramResponse Get(ProgramRequest request)
        {
            ProgramResponse response = ProgramDataManager.GetProgramByID(request);
            response.Version = request.Version;
            return response;
        }

        public ProgramListResponse Post(ProgramListRequest request)
        {
            ProgramListResponse response = ProgramDataManager.GetProgramList(request);
            response.Version = request.Version;
            return response;
        }
    }
}