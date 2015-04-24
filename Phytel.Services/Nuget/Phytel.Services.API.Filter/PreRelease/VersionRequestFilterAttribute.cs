using Phytel.Services.API.Provider;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;

namespace Phytel.Services.API.Filter
{
    public class VersionRequestFilterAttribute : RequestFilterAttribute
    {
        public IVersionProvider VersionProvider { get; set; }
        public IHostContextProxy HostContextProxy { get; set; }

        public override void Execute(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            if (HostContextProxy == null)
            {
                throw new Exception("IHostContextProxy was not initialized. Make sure IHostContextProxy has been registered with the IoC container.");
            }
            if (VersionProvider == null)
            {
                throw new Exception("IVersionProvider was not initialized. Make sure IVersionProvider has been registered with the IoC container.");
            }

            double version = VersionProvider.Get(requestDto);

            HostContextProxy.SetItem(Provider.Constants.HostContextKeyVersion, version);
        }
    }
}