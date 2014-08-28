using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public interface IElementActivationRule
    {
        int ElementType { get;}
        string Execute(string uid, PlanElementEventArg arg, SpawnElement se);
    }
}