using System;
using AppDomain.Engage.Population.DTO.Context;
using Funq;
using ServiceStack.Common;

namespace AppDomain.Engage.Population.Service.Container
{
    public class HttpServiceContainer
    {
        public static Funq.Container Build(Funq.Container container)
        {
            container.Register<IServiceContext>(c =>
                new ServiceContext{
                    Contract = HostContext.Instance.Items["Contract"].ToString(),
                    Version = Convert.ToDouble(HostContext.Instance.Items["Version"].ToString())
                }).ReusedWithin(ReuseScope.Request);

            container.Register<IIntegrationManager>(c => new IntegrationManager(c.Resolve<IServiceContext>())).ReusedWithin(ReuseScope.Request);

            return container;
        }
    }
}