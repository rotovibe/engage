using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;

namespace Phytel.API.AppDomain.NG.Programs.PlanElemStrategy
{
    public interface IPlanElementActivationStrategy
    {
        string Run(PlanElementEventArg e, SpawnElement rse, string userId);
    }
}