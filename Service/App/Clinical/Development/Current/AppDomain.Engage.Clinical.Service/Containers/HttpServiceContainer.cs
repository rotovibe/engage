using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using AppDomain.Engage.Clinical.DataDomainClient;
using AppDomain.Engage.Clinical.DTO.Context;
using AutoMapper;
using Funq;
using Phytel.API.Common;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.Common;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace AppDomain.Engage.Clinical.Service.Containers
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
            container.Register<IMedicationDataDomainClient>(c => new MedicationDataDomainClient(Mapper.Engine, ConfigurationManager.AppSettings["DDCohortServiceUrl"],
                c.Resolve<IHelpers>(),
                c.Resolve<IRestClient>(),
                c.Resolve<IServiceContext>()))
                .ReusedWithin(ReuseScope.Request);

            container.Register<IClinicalManager>(c => new ClinicalManager(c.Resolve<IServiceContext>(), c.Resolve<IMedicationDataDomainClient>())).ReusedWithin(ReuseScope.Request);

            return container;
        }
    }
}