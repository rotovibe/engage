using Phytel.API.AppDomain.NG.Test.Stubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.Service.Tests.Factories
{
    public static class NGManagerFactory
    {
        private static string mode;

        public static INGManager Get()
        {
            switch (mode)
            {
                case "true":
                    return new StubNGManager();
                default:
                    return new NGManager();
            }
        }

        static NGManagerFactory()
        {
            mode = ConfigurationManager.AppSettings["UnitTestMode"];
        }
    }
}
