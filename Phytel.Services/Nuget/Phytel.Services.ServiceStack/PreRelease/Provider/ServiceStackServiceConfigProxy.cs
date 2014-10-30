using ServiceStack.WebHost.Endpoints;

namespace Phytel.Services.ServiceStack.Provider
{
    public class ServiceStackServiceConfigProxy : IServiceConfigProxy
    {
        public string GetServiceName()
        {
            return AppHostBase.Instance.Config.ServiceName;
        }
    }
}