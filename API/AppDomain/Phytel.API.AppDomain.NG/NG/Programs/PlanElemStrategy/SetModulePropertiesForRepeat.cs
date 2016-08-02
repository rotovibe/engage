using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Specifications;
using System;

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
