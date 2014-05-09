using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanElementStrategy;
using Phytel.API.DataDomain.PatientProblem.DTO;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.PlanCOR
{
    public delegate void SpawnEventHandler(object s, SpawnEventArgs e);

    public class StepPlanProcessor : PlanProcessor
    {
        public event SpawnEventHandler _spawnEvent;
        public static SpawnElementStrategy SpawnStrategy { get; set; }
        private ProgramAttribute _programAttributes;

        public StepPlanProcessor()
        {
            _programAttributes = new ProgramAttribute();
        }

        protected void OnSpawnElementEvent(string type)
        {
            if (_spawnEvent != null)
            {
                _spawnEvent(this, new SpawnEventArgs() { Name = type });
            }
        }

        public override void PlanElementHandler(object sender, PlanElementEventArg e)
        {
            try
            {
                if (e.PlanElement.GetType().Equals(typeof(Step)))
                {
                    // set program information
                    //PlanElementUtil.SetProgramInformation(_programAttributes, e.Program);
                    _programAttributes.PlanElementId = e.Program.Id;
                    Step s = (Step)e.PlanElement;

                    if (e.Action.Completed)
                    {
                        if (s.Completed)
                        {
                            // see if responses have spawn elements
                            if (s.Responses != null)
                                SetCompletedStepResponses(e, s);

                            if (s.SpawnElement != null)
                                PlanElementUtil.SpawnElementsInList(s.SpawnElement, e.Program, e.UserId, _programAttributes);
                        }
                        else
                        {
                            if (s.Responses != null)
                                s.Responses.ForEach(r => { r.Delete = true; });
                        }
                        // save program properties
                        PlanElementUtil.SaveReportingAttributes(_programAttributes, e.DomainRequest);
                        // raise process event to register the id
                        OnProcessIdEvent(e.Program);
                    }
                }
                else if (Successor != null)
                {
                    Successor.PlanElementHandler(this, e);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:StepPlanProcessor:PlanElementHandler()::" + ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Handles the response initialization to deletion of false and evokes any spawnelements.
        /// </summary>
        /// <param name="e">PlanElementEventArg</param>
        /// <param name="s">Step</param>
        private void SetCompletedStepResponses(PlanElementEventArg e, Step s)
        {
            try
            {
                s.Responses.ForEach(r =>
                {
                    r.Delete = false;
                    if (s.SelectedResponseId.Equals(r.Id))
                    {
                        HandleResponseSpawnElements(r, e, e.UserId);
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception("AD:StepPlanProcessor:StepCompletedStepResponse()::" + ex.Message, ex.InnerException);
            }
        }

        private void HandleResponseSpawnElements(Response r, PlanElementEventArg e, string userId)
        {
            try
            {
                if (r.SpawnElement != null)
                {
                    r.SpawnElement.ForEach(rse =>
                    {
                        // handles the response spawnelements
                        if (rse.ElementType.Equals(101))
                        {
                            HandlePatientProblemRegistration(e, userId, rse);
                            OnSpawnElementEvent("Problems");
                        }
                        else
                        {
                            PlanElementUtil.SetProgramAttributes(rse, e.Program, e.UserId, _programAttributes);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:StepPlanProcessor:HandleResponseSpawnElements()::" + ex.Message, ex.InnerException);
            }
        }

        private static void HandlePatientProblemRegistration(PlanElementEventArg e, string userId, SpawnElement rse)
        {
            // check if problem code is already registered for patient
            PatientProblemData ppd = PlanElementEndpointUtil.GetPatientProblem(rse.ElementId, e, userId);
            if (ppd != null)
            {
                if (!ppd.Active)
                {
                    new SpawnElementStrategy(new UpdateSpawnProblemCode(e, rse, ppd, true)).Evoke();
                }
            }
            else
            {
                new SpawnElementStrategy(new RegisterSpawnProblemCode(e, rse, ppd)).Evoke();
            }

            // register new problem code with cohortpatientview
            PlanElementUtil.RegisterCohortPatientViewProblemToPatient(rse.ElementId, e.PatientId, e.DomainRequest);
        }
    }
}
