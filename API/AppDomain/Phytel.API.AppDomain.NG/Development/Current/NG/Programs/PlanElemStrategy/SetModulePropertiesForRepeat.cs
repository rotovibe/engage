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
    public class SetModulePropertiesForRepeat : IElementAction
    {
        private Module _module;
        ISpecification<Module> isModuleAlreadyCompleted = new IsModuleCompletedSpecification<Module>();

        public SetModulePropertiesForRepeat(Module module)
        {
            _module = module;
        }

        public void Execute()
        {
            if (isModuleAlreadyCompleted.IsSatisfiedBy(_module))
            {
                _module.ElementState = 4;
                _module.StateUpdatedOn = DateTime.UtcNow;
                _module.Completed = false;
                _module.DateCompleted = null;
                _module.CompletedBy = null;
            }
        }
    }
}
