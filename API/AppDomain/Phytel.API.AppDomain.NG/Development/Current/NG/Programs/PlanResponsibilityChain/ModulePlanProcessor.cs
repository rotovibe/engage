﻿using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.PlanCOR
{
    public class ModulePlanProcessor : PlanProcessor
    {
        private ProgramAttributeData _programAttributes;
        public IPlanElementUtils PEUtils { get; set; }

        public ModulePlanProcessor()
        {
            _programAttributes = new ProgramAttributeData();
            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public override void PlanElementHandler(object sender, PlanElementEventArg e)
        {
            try
            {
                if (e.PlanElement.GetType().Equals(typeof(Module)))
                {
                    Module module = e.PlanElement as Module;
                    //PlanElementUtil.SetProgramInformation(_programAttributes, e.Program);
                    _programAttributes.PlanElementId = e.Program.Id;

                    if (module.Actions != null)
                    {
                        module.Completed = PEUtils.SetCompletionStatus(module.Actions);
                        if (module.Completed)
                        {
                            module.CompletedBy = e.UserId;
                            module.ElementState = 5;
                            module.DateCompleted = System.DateTime.UtcNow;
                            // look at spawnelement and trigger enabled state.
                            if (module.SpawnElement != null)
                            {
                                PEUtils.SpawnElementsInList(module.SpawnElement, e.Program, e.UserId, _programAttributes);
                            }
                            // save any program attribute changes
                            PEUtils.SaveReportingAttributes(_programAttributes, e.DomainRequest);
                            OnProcessIdEvent(module);
                        }
                    }
                }
                else if (Successor != null)
                {
                    Successor.PlanElementHandler(this, e);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:ModulePlanProcessor:PlanElementHandler()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
