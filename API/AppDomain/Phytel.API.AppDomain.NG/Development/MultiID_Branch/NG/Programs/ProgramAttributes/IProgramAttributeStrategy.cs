using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Program.DTO;
using Program = Phytel.API.AppDomain.NG.DTO.Program;

namespace Phytel.API.AppDomain.NG.Programs.ProgramAttributes
{
    public interface IProgramAttributeStrategy
    {
        void Run(SpawnElement r, Program program, string userId, ProgramAttributeData progAttr);
    }
}