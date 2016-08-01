using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public class ProgramAttributeActivationRule : ElementActivationRule, IElementActivationRule
    {
        private const int _elementType = 10;
        public int ElementType{ get { return _elementType; } }
        public IPlanElementUtils PlanUtils { get; set; }

        public ProgramAttributeActivationRule()
        {
            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public override SpawnType Execute(string userId, PlanElementEventArg arg, SpawnElement se, ProgramAttributeData pad)
        {
            PlanUtils.SetProgramAttributes(se, arg.Program, arg.UserId, pad);
            var spawnType = new SpawnType { Type = "KickAss" };
            return spawnType;
        }
    }
}
