using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.DataDomain.PatientObservation.DTO;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.AppDomain.NG.ElementActivation
{
    public class UpdateSpawnProblemCode : ISpawn
    {
        public IEndpointUtils EndPointUtil { get; set; }
        private PlanElementEventArg _e;
        private SpawnElement _se;
        private PatientObservation _pod;
        private bool _active;

        public UpdateSpawnProblemCode(PlanElementEventArg e, SpawnElement rse, PatientObservation ppd, bool active)
        {
            _e = e;
            _se = rse;
            _pod = ppd;
            _active = active;

            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public void Execute()
        {
            // if updating patient problem
            PutUpdateObservationDataResponse response = EndPointUtil.UpdatePatientProblem(_e.PatientId, _e.UserId, _se.ElementId, _pod, _active, _e.DomainRequest);
        }
    }
}
