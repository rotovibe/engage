using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public class StepActivationRule : ElementActivationRule, IElementActivationRule
    {
        private const int _elementType = 4;
        public int ElementType{ get { return _elementType; } }
        public IPlanElementUtils PlanUtils { get; set; }

        public StepActivationRule()
        {
            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public override SpawnType Execute(string userId, PlanElementEventArg arg, SpawnElement se, ProgramAttributeData pad)
        {
            PlanElement value = HandlePlanElementActivation(PlanUtils, arg, se);
            var spawnType = new SpawnType { Type = _elementType.ToString()};
            return spawnType;
        }
    }
}
