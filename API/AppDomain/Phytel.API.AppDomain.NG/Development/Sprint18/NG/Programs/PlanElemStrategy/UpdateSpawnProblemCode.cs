using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.DataDomain.PatientProblem.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.PlanElementStrategy
{
    public class UpdateSpawnProblemCode : ISpawn
    {
        private PlanElementEventArg _e;
        private SpawnElement _se;
        private PatientObservation _pod;
        private bool _active;

        protected static readonly string DDPatientProblemServiceUrl = ConfigurationManager.AppSettings["DDPatientProblemServiceUrl"];

        public UpdateSpawnProblemCode(PlanElementEventArg e, SpawnElement rse, PatientObservation ppd, bool active)
        {
            _e = e;
            _se = rse;
            _pod = ppd;
            _active = active;
        }

        public void Execute()
        {
            // if patient problem registration is new
            PutUpdateObservationDataResponse response = PlanElementEndpointUtil.UpdatePatientProblem(_e.PatientId, _e.UserId, _se.ElementId, _pod, _active, _e.DomainRequest);
        }
    }
}
