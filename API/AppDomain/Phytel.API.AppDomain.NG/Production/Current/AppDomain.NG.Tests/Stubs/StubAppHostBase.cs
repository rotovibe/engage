using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.AppDomain.NG.Service;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubAppHostBase : AppHostBase
    {
        public StubAppHostBase() : base("SimpleRestTestBase", typeof (NGService).Assembly)
        {
            Instance = null;
            Init();
        }

        public override void Configure(Funq.Container container)
        {
            //register any dependencies your services use, e.g:
            container.RegisterAutoWiredAs<StubSecurityManager, ISecurityManager>();
            container.RegisterAutoWiredAs<StubCommonFormatterUtil, ICommonFormatterUtil>();
            //container.RegisterAutoWiredAs<EndPointUtils, IEndpointUtils>();
            container.RegisterAutoWiredAs<StubPlanElementUtils, IPlanElementUtils>();
            container.RegisterAutoWiredAs<StubNGManager, INGManager>();
            container.RegisterAutoWiredAs<StubAuditUtil, IAuditUtil>();
        }
    }
}

