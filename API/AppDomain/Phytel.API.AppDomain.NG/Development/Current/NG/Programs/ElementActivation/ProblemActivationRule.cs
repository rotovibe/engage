using System;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.ElementActivation;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.PlanElementStrategy;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public class ProblemActivationRule : ElementActivationRule, IElementActivationRule
    {
        private const int _elementType = 101;
        private const string AlertType = "Problems";
        public int ElementType{ get { return _elementType; } }
        public IEndpointUtils EndpointUtil { get; set; }
        public IPlanElementUtils PlanUtils { get; set; }

        public ProblemActivationRule()
        {
            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public override string Execute(string UserId, PlanElementEventArg arg, SpawnElement pe)
        {
            HandlePatientProblemRegistration(arg, UserId, pe);
            return AlertType;
        }

        private void HandlePatientProblemRegistration(PlanElementEventArg e, string userId, SpawnElement rse)
        {
            try
            {
                // check if problem code is already registered for patient
                PatientObservation ppd = EndpointUtil.GetPatientProblem(rse.ElementId, e, userId);

                var uspc = new UpdateSpawnProblemCode(e, rse, ppd, true) {EndPointUtil = EndpointUtil};
                IPlanElementStrategy updateSpawnProblemCode = new SpawnElementStrategy(uspc);

                var spc = new RegisterSpawnProblemCode(e, rse, ppd) {EndPointUtil = EndpointUtil};
                IPlanElementStrategy registerSpawnProblemCode = new SpawnElementStrategy(spc);

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
