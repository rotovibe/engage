using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Program.DTO;
using Program = Phytel.API.AppDomain.NG.DTO.Program;

namespace Phytel.API.AppDomain.NG.Programs.ProgramAttributes
{
    public interface IProgramAttributeRule
    {
        int ProgramAttributeType { get; }
        void Execute(SpawnElement r, Program program, string userId, ProgramAttributeData progAttr);
    }
}