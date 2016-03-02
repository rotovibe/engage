using System;
using System.Configuration;
using AppDomain.Engage.Population.DataDomainClient;
using AppDomain.Engage.Population.DTO.Context;
using Funq;
using Phytel.API.Common;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.Common;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace AppDomain.Engage.Population.Service.Container
{
    public class HttpServiceContainer
    {
        public static Funq.Container Build(Funq.Container container)
        {
            container.Register<IServiceContext>(c =>
                new ServiceContext
                {
                    Contract = HostContext.Instance.Items["Contract"].ToString(),
                    Version = Convert.ToDouble(HostContext.Instance.Items["Version"].ToString())
                }).ReusedWithin(ReuseScope.Request);

            container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<AuditHelpers, IAuditHelpers>().ReusedWithin(ReuseScope.Request);
            container.Register<IHelpers>(c => new Helpers()).ReusedWithin(ReuseScope.Request);
            container.Register<IRestClient>(c => new JsonServiceClient()).ReusedWithin(ReuseScope.Request);
            container.Register<IPatientDataDomainClient>(c => new PatientDataDomainClient(ConfigurationManager.AppSettings["DDPatientServiceUrl"], 
                c.Resolve<IHelpers>(), 
                c.Resolve<IRestClient>(), 
                c.Resolve<IServiceContext>()))
                .ReusedWithin(ReuseScope.Request);

            container.Register<IDemographicsManager>(c => new DemographicsManager(c.Resolve<IServiceContext>(), c.Resolve<IPatientDataDomainClient>())).ReusedWithin(ReuseScope.Request);

            return container;
        }
    }
}