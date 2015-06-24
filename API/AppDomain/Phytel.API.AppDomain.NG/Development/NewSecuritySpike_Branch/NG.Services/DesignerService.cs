using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Platform.Session.DTO;
using Phytel.Services.API.Platform.Filter;
using Phytel.Services.API.Platform.Filter.Attributes;
using Phytel.Services.API.Provider;

namespace Phytel.API.AppDomain.NG.Service
{
    [IsAuthenticatedFilter(PhytelPlatformApplicationContextNames.LoweredEngageContextKey)]
    public partial class DesingerService : ServiceStack.ServiceInterface.Service
    {
        protected internal IHostContextProxy _hostContextProxy;
        public DesingerService(IHostContextProxy hostContextProxy)
        {
            _hostContextProxy = hostContextProxy;
        }

        public GetTestResponse Get(GetTestRequest request)
        {
            var tokenInfo = _hostContextProxy.GetItem(FilterAttributesConstants.UserSessionName) as SessionInfo;
            return new GetTestResponse();
        }
    }
}