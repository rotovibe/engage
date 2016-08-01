using ServiceStack.Common;

namespace Phytel.API.DataDomain.PatientSystem.Service
{
    public static class HttpServiceContainer
    {
        public static Funq.Container Build(Funq.Container container)
        {
            container.Register<HostContext>(c => HostContext.Instance).ReusedWithin(Funq.ReuseScope.Request);
            container = PatientSystemContainer.Configure(container);
            return container;
        }
    }
}