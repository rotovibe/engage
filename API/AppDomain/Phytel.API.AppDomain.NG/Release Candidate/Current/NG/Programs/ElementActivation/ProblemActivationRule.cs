using System;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.ElementActivation;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.PlanElementStrategy;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public class ProblemActivationRule : ElementActivationRule, IElementActivationRule
    {
        private const int _elementType = 101;
        private const int _alertType = 100;
        public int ElementType{ get { return _elementType; } }
        public IEndpointUtils EndpointUtil { get; set; }
        public IPlanElementUtils PlanUtils { get; set; }

        public ProblemActivationRule()
        {
            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public override SpawnType Execute(string userId, PlanElementEventArg arg, SpawnElement pe, ProgramAttributeData pad)
        {
            HandlePatientProblemRegistration(arg, userId, pe);
            var spawnType = new SpawnType {Type = _alertType.ToString()};
            return spawnType;
        }

        public void HandlePatientProblemRegistration(PlanElementEventArg e, string userId, SpawnElement rse)
        {
            try
            {
                // check if problem code is already registered for patient
                var ppd = EndpointUtil.GetPatientProblem(rse.ElementId, e, userId);

                if (ppd != null)
                {
                    var uspc = new UpdateSpawnProblemCode(e, rse, ppd, true) {EndPointUtil = EndpointUtil};
                    var updateSpawnProblemCode = new SpawnElementStrategy(uspc);
                    if (ppd.StateId != 2) updateSpawnProblemCode.Evoke();
                }
                else
                {
                    var spc = new RegisterSpawnProblemCode(e, rse) { EndPointUtil = EndpointUtil };
                    var registerSpawnProblemCode = new SpawnElementStrategy(spc);
                    registerSpawnProblemCode.Evoke();
                }

                // register new problem code with cohortpatientview
                PlanUtils.RegisterCohortPatientViewProblemToPatient(rse.ElementId, e.PatientId,
                    e.DomainRequest);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:StepPlanProcessor:HandlePatientProblemRegistration()::" + ex.Message,
                    ex.InnerException);
            }
        }
    }
}
