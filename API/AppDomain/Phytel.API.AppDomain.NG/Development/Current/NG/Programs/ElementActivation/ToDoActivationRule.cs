using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public class ToDoActivationRule : ElementActivationRule, IElementActivationRule
    {
        private const int _elementType = 111;
        private const string AlertType = "ToDo";
        public int ElementType{ get { return _elementType; } }
        public IEndpointUtils EndpointUtil { get; set; }
        public IPlanElementUtils PlanUtils { get; set; }

        public ToDoActivationRule()
        {
            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public override string Execute(string UserId, PlanElementEventArg arg, SpawnElement pe)
        {
            return AlertType;
        }
    }
}
