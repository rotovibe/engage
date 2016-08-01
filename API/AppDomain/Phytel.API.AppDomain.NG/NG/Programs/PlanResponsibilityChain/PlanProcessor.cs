using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanElementStrategy;
using Phytel.API.AppDomain.NG.Programs.ElementActivation;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using System;
using ServiceStack.WebHost.Endpoints;
using Program = Phytel.API.AppDomain.NG.DTO.Program;

namespace Phytel.API.AppDomain.NG.PlanCOR
{
    public delegate void ProcessedElementEventHandler(object s, ProcessElementEventArgs e);

    public abstract class PlanProcessor
    {
        public static SpawnElementStrategy SpawnStrategy { get; set; }
        public event SpawnEventHandler SpawnEvent;
        protected ProgramAttributeData ProgramAttributes;
        public IPlanElementUtils PEUtils { get; set; }
        public event ProcessedElementEventHandler ProcessedElementEvent;
        public EventHandler<PlanElementEventArg> PlanElement;
        public PlanProcessor Successor { get; set; }
        public IElementActivationStrategy ElementActivationStrategy { get; set; }

        public abstract void PlanElementHandler(object sender, PlanElementEventArg e);

        protected void OnSpawnElementEvent(SpawnType type)
        {
            if (SpawnEvent != null)
            {
                SpawnEvent(this, new SpawnEventArgs {Name = type.Type, Tags = new List<object>{type.Tag}});
            }
        }

        protected void OnProcessIdEvent(object pe)
        {
            if (ProcessedElementEvent != null)
            {
                ProcessedElementEvent(this, new ProcessElementEventArgs {PlanElement = pe});
            }
        }

        public PlanProcessor()
        {
            PlanElement += PlanElementHandler;
            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public void ProcessWorkflow(IPlanElement planElem, Program program, string userId, string patientId,
            Actions action, IAppDomainRequest request)
        {
            try
            {
                OnPlanElement(new PlanElementEventArg
                {
                    PlanElement = planElem,
                    Program = program,
                    UserId = userId,
                    PatientId = patientId,
                    Action = action,
                    DomainRequest = request
                });
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanProcessor:ProcessWorkflow()::" + ex.Message, ex.InnerException);
            }
        }

        public virtual void OnPlanElement(PlanElementEventArg e)
        {
            try
            {
                if (PlanElement != null)
                {
                    PlanElement(this, e);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanProcessor:OnPlanElement()::" + ex.Message, ex.InnerException);
            }
        }

        public void HandlePlanElementActivation(PlanElementEventArg e, SpawnElement rse)
        {
            try
            {
                PlanElement pe = PEUtils.ActivatePlanElement(rse.ElementId, e.Program);
                if (pe != null)
                    OnProcessIdEvent(pe);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:StepPlanProcessor:HandlePlanElementActivation()::" + ex.Message,
                    ex.InnerException);
            }
        }

        public void HandlePlanElementActions(PlanElementEventArg e, string userId, SpawnElement rse)
        {
            // handles the response spawnelements
            if (rse.ElementType < 10)
            {
                HandlePlanElementActivation(e, rse);
            }
            else if (rse.ElementType > 100)
            {
                //HandlePatientProblemRegistration(e, userId, rse);
                var type = ElementActivationStrategy.Run(e, rse, userId, ProgramAttributes);

                if (!string.IsNullOrEmpty(type.ToString()))
                    OnSpawnElementEvent(type);
            }
            else
            {
                PEUtils.SetProgramAttributes(rse, e.Program, e.UserId, ProgramAttributes);
            }
        }
    }
}
