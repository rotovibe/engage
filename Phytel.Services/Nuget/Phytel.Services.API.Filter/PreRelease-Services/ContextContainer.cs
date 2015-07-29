using Phytel.Services.API.Provider;
using Phytel.Services.IOC;
using ServiceStack.Common;

namespace Phytel.Services.API.Filter
{
    public class ContextContainer : ContainerDecorator
    {
        public ContextContainer(ContainerBuilder containerBuilder)
            : base(containerBuilder)
        {
        }

        protected override void OnBuild(Funq.Container container)
        {
            container.Register<HostContext>(c => HostContext.Instance).ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<HostContextProxy, IHostContextProxy>().ReusedWithin(Funq.ReuseScope.Request);

            container.Register<string>(Constants.NamedStringContext, c =>
            {
                IHostContextProxy hostContextProxy = c.Resolve<IHostContextProxy>();
                return hostContextProxy.GetItem<string>(API.Provider.Constants.HostContextKeyContext);
            }).ReusedWithin(Funq.ReuseScope.Request);
        }
    }
}