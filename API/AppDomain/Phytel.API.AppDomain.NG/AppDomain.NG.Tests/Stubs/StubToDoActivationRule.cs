using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.Programs.ElementActivation;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubToDoActivationRule : ElementActivationRule, IElementActivationRule
    {
        private const int _elementType = 111;
        private const string _alertType = "ToDo";
        public int ElementType { get { return _elementType; } }

        public override SpawnType Execute(string userId, PlanElementEventArg arg, SpawnElement pe, ProgramAttributeData pad)
        {
            var spawnType = new SpawnType {Type = _alertType.ToString()};
            return spawnType;
        }
    }
}
