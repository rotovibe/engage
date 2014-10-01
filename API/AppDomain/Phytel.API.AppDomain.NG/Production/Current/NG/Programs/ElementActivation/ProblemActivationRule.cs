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

        public override object Execute(string userId, PlanElementEventArg arg, SpawnElement pe, ProgramAttributeData pad)
        {
            HandlePatientProblemRegistration(arg, userId, pe);
            return _alertType;
        }

        private void HandlePatientProblemRegistration(PlanElementEventArg e, string userId, SpawnElement rse)
        {
            try
            {
                // check if problem code is already registered for patient
                var ppd = EndpointUtil.GetPatientProblem(rse.ElementId, e, userId);

                var uspc = new UpdateSpawnProblemCode(e, rse, ppd, true) {EndPointUtil = EndpointUtil};
                var updateSpawnProblemCode = new SpawnElementStrategy(uspc);

                var spc = new RegisterSpawnProblemCode(e, rse, ppd) {EndPointUtil = EndpointUtil};
                var registerSpawnProblemCode = new SpawnElementStrategy(spc);

                if (ppd != null)
                {
                    if (ppd.StateId != 2) updateSpawnProblemCode.Evoke();
                }
                else{ registerSpawnProblemCode.Evoke(); }

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
