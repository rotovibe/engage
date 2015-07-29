using ServiceStack.WebHost.Endpoints;

namespace Phytel.Services.API.Provider
{
    public class ServiceStackServiceConfigProxy : IServiceConfigProxy
    {
        public string GetServiceName()
        {
            return AppHostBase.Instance.Config.ServiceName;
        }
    }
}