using Phytel.API.AppDomain.NG.Test.Stubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.Service.Tests.Factories
{
    public static class SecurityManagerFactory
    {
        private static string mode;

        public static ISecurityManager Get()
        {
            switch (mode)
            {
                case "true":
                    return new StubSecurityManager();
                default:
                    return new SecurityManager();
            }
        }

        static SecurityManagerFactory()
        {
            mode = ConfigurationManager.AppSettings["UnitTestMode"];
        }
    }
}
