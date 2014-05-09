using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.PlanCOR
{
    public delegate void ProcessedElementEventHandler(object s, ProcessElementEventArgs e);

    public abstract class PlanProcessor
    {
        public event ProcessedElementEventHandler _processedElementEvent;
        public EventHandler<PlanElementEventArg> PlanElement;
        public PlanProcessor Successor { get; set; }

        public abstract void PlanElementHandler(object sender, PlanElementEventArg e);

        protected void OnProcessIdEvent(object pe)
        {
            if (_processedElementEvent != null)
            {
                _processedElementEvent(this, new ProcessElementEventArgs { PlanElement = pe });
            }
        }

        public PlanProcessor()
        {
            PlanElement += PlanElementHandler;
        }

        public void ProcessWorkflow(IPlanElement planElem, Program program, string userId, string patientId, Actions action, IAppDomainRequest request)
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
    }
}
