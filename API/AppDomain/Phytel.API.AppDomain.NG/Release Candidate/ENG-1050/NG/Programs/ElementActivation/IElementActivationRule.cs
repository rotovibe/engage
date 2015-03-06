using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public interface IElementActivationRule
    {
        int ElementType { get;}
        SpawnType Execute(string userId, PlanElementEventArg arg, SpawnElement se, ProgramAttributeData pad);
    }
}