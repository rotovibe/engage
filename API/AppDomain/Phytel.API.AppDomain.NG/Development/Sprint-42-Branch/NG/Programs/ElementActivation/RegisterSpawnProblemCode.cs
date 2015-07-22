using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.DataDomain.PatientObservation.DTO;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.AppDomain.NG.ElementActivation
{
    public class RegisterSpawnProblemCode : ISpawn
    {
        public IEndpointUtils EndPointUtil { get; set; }
        private PlanElementEventArg _e;
        private SpawnElement _se;

        public RegisterSpawnProblemCode(PlanElementEventArg e, SpawnElement rse)
        {
            _e = e;
            _se = rse;

            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public void Execute()
        {
            // if patient problem registration is new
            PutRegisterPatientObservationResponse response = EndPointUtil.PutNewPatientProblem(_e.PatientId,
                _e.UserId, _se.ElementId, _e.DomainRequest);
        }
    }
}
