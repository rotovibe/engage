using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nightingale.Site
{
    public class UrlHelper
    {
        private static string ResolveEnvironment()
        {
            string host = HttpContext.Current.Request.Url.Host;
            var env = "";
            if (host.Contains("login.phytel"))
            {
                env = "PROD";
            }
            if (host.Contains("ngmodel.phytel"))
            {
                env = "MODEL";
            }
            if (host.Contains("localhost") || host.Contains("azurephyteldev"))
            {
                env = "DEV";
            }
            if (host.Contains("azurephytel.cloudapp.net"))
            {
                env = "QA";
            }
            return env;
        }
        public static string GetGoogleAnalyticsId()
        {
            string analyticsId = "";
            var env = ResolveEnvironment();
            switch (env)
            {
                case "DEV":
                    {
                        analyticsId = "UA-44886803-3";
                    }
                    break;
                case "MODEL":
                    {
                        analyticsId = "UA-44886803-6";
                    }
                    break;
                case "PROD":
                    {
                        analyticsId = "UA-44886803-4";
                    }
                    break;
                case "QA":
                    {
                        analyticsId = "UA-44886803-5";
                    }
                    break;
                default:
                    break;
            }
            return analyticsId;
        }
    }
}