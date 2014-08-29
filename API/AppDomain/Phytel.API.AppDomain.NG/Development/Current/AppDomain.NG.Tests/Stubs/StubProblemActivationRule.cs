using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.Programs.ElementActivation;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubProblemActivationRule : ElementActivationRule, IElementActivationRule
    {
        private const int _elementType = 101;
        private const string _alertType = "Problems";
        public int ElementType { get { return _elementType; } }

        public override object Execute(string userId, PlanElementEventArg arg, SpawnElement pe, ProgramAttributeData pad)
        {
            return _alertType;
        }
    }
}
