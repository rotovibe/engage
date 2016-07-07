using System;
using Funq;
using Phytel.API.AppDomain.NG.Allergy;
using Phytel.API.AppDomain.NG.DTO.Context;
using Phytel.API.AppDomain.NG.Medication;
using Phytel.API.AppDomain.NG.Notes;
using Phytel.API.AppDomain.NG.Observation;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.AppDomain.NG.Programs.ElementActivation;
using Phytel.API.AppDomain.NG.Programs.ProgramAttributes;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.Common;

namespace Phytel.API.AppDomain.NG.Service.Containers
{
    public static class HttpServiceContainer
    {
        public static Container Build(Container container)
        {
            //register any dependencies your services use, e.g:
            container.Register<IServiceContext>(c =>
                new ServiceContext
                {
                    Contract = HostContext.Instance.Items["Contract"].ToString(),
                    Version = Convert.ToDouble(HostContext.Instance.Items["Version"].ToString())
                }).ReusedWithin(ReuseScope.Request);

            container.RegisterAutoWiredAs<SecurityManager, ISecurityManager>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<EndpointUtils, IEndpointUtils>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<AllergyEndpointUtil, IAllergyEndpointUtil>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<ProgramAttributeStrategy, IProgramAttributeStrategy>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<PlanElementUtils, IPlanElementUtils>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<UtilizationManager, IUtilizationManager>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<NotesManager, INotesManager>().ReusedWithin(ReuseScope.Request);
            
            container.RegisterAutoWiredAs<AuditUtil, IAuditUtil>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<ObservationEndpointUtil, IObservationEndpointUtil>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<ObservationsManager, IObservationsManager>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<AllergyManager, IAllergyManager>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<MedicationManager, IMedicationManager>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<MedicationEndpointUtil, IMedicationEndpointUtil>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<ElementActivationStrategy, IElementActivationStrategy>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<StepPlanProcessor, IStepPlanProcessor>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<GoalsEndpointUtils, IGoalsEndpointUtils>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<PatientSystemEndpointUtil, IPatientSystemEndpointUtil>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<PatientSystemManager, IPatientSystemManager>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<ContactEndpointUtil, IContactEndpointUtil>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<EngageCareMemberCohortRuleFactory, ICareMemberCohortRuleFactory>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<CohortRoleUtils, ICohortRuleUtil>().ReusedWithin(ReuseScope.Request);
            

            RegisterInstances(container);

            return container;
        }

        private static void RegisterInstances(Funq.Container container)
        {
            container.RegisterAutoWiredAs<ASEExceptionLogger, ILogger>();
            container.RegisterAutoWiredAs<ContactManager, IContactManager>();
            container.RegisterAutoWiredAs<CohortRulesProcessor, ICohortRulesProcessor>();
            container.RegisterAutoWiredAs<NGManager, INGManager>();
        }
    }
}