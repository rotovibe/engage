using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public interface IElementActivationStrategy
    {
        string Run(PlanElementEventArg e, SpawnElement rse, string userId);
    }
}