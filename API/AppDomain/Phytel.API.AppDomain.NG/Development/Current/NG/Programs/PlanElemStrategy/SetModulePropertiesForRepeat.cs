using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.Specifications;
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
    public class SetModulePropertiesForRepeat : IPlanElementAction
    {
        private Module _module;
        ISpecification<Module> isModuleAlreadyCompleted = new IsModuleCompletedSpecification<Module>();

        public SetModulePropertiesForRepeat(Module module)
        {
            _module = module;
        }

        public void Execute()
        {
            // modify module if it is completed
            if (isModuleAlreadyCompleted.IsSatisfiedBy(_module))
            {
                // change state to 4 from 5
                _module.ElementState = 4;
                // stateupdated date to current date
                _module.StateUpdatedOn = DateTime.UtcNow;
                // completed flag should be set to false
                _module.Completed = false;
                // datecomplated blanked
                _module.DateCompleted = null;
                // completedby to null
                _module.CompletedBy = string.Empty;
            }
        }
    }
}
