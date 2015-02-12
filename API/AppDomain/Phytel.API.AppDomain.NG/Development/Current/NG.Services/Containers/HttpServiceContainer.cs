using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Phytel.API.AppDomain.NG.Allergy;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.Medication;
using Phytel.API.AppDomain.NG.Observation;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.AppDomain.NG.Programs.ElementActivation;
using Phytel.API.AppDomain.NG.Search;
using Phytel.API.AppDomain.NG.Search.LuceneStrategy;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.Common;

namespace Phytel.API.AppDomain.NG.Service.Containers
{
    public static class HttpServiceContainer
    {
        public static Funq.Container Build(Funq.Container container)
        {
            //register any dependencies your services use, e.g:
            container.RegisterAutoWiredAs<SecurityManager, ISecurityManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<EndpointUtils, IEndpointUtils>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<AllergyEndpointUtil, IAllergyEndpointUtil>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<PlanElementUtils, IPlanElementUtils>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<NGManager, INGManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<AuditUtil, IAuditUtil>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<ObservationEndpointUtil, IObservationEndpointUtil>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<ObservationsManager, IObservationsManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<AllergyManager, IAllergyManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<MedicationManager, IMedicationManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<MedicationEndpointUtil, IMedicationEndpointUtil>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<ElementActivationStrategy, IElementActivationStrategy>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<StepPlanProcessor, IStepPlanProcessor>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<GoalsEndpointUtils, IGoalsEndpointUtils>().ReusedWithin(Funq.ReuseScope.Request);
            return container;
        }
    }
}