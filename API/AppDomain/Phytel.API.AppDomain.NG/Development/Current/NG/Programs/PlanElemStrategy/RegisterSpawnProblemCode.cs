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
    public class RegisterSpawnProblemCode : ISpawn
    {
        private PlanElementEventArg _e;
        private SpawnElement _se;
        protected static readonly string DDPatientProblemServiceUrl = ConfigurationManager.AppSettings["DDPatientProblemServiceUrl"];

        public RegisterSpawnProblemCode(PlanElementEventArg e, SpawnElement rse, PatientObservation ppd)
        {
            _e = e;
            _se = rse;
        }

        public void Execute()
        {
            // if patient problem registration is new
            PutRegisterPatientObservationResponse response = PlanElementEndpointUtil.PutNewPatientProblem(_e.PatientId,
                _e.UserId, _se.ElementId, _e.DomainRequest);
        }
    }
}
