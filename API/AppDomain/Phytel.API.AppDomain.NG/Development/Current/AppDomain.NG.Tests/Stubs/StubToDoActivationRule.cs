using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.Programs.PlanElemStrategy;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubToDoActivationRule : PlanElementActivationRule, IPlanElementActivationRule
    {
        private const int _elementType = 111;
        private const string AlertType = "ToDo";
        public int ElementType { get { return _elementType; } }

        public override string Execute(string userId, PlanElementEventArg arg, SpawnElement pe)
        {
            return AlertType;
        }
    }
}
