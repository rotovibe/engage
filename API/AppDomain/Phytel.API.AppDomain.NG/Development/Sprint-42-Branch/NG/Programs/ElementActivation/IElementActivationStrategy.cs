using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public interface IElementActivationStrategy
    {
        SpawnType Run(PlanElementEventArg e, SpawnElement rse, string userId, ProgramAttributeData pad);
    }
}