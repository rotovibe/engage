using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.UI;

namespace Atmosphere
{
    public static class UrlHelper
    {
        public static string ResolveUrl(string path)
        {
            string assetRoot = ConfigurationManager.AppSettings["Asset_Root"];
            path = path.Replace("{asset_root}", assetRoot);

            UriBuilder builder = new UriBuilder();
            builder.Port = HttpContext.Current.Request.Url.Port;
            builder.Scheme = HttpContext.Current.Request.Url.Scheme;
            builder.Host = HttpContext.Current.Request.Url.Host;
            builder.Path = builder.Path + path;

            return builder.ToString();
        }
        private static string ResolveEnvironment()
        {
            string host = HttpContext.Current.Request.Url.Host;
            var env = "";
            if (host.Contains("login.phytel"))
            {
                env = "PROD";
            }
            if (host.Contains("localhost") || host.Contains("azurephyteldev"))
            {
                env = "DEV";
            }
            if (host.Contains("azurephytel.cloudapp.net"))
            {
                env = "QA";
            }
            if (host.Contains("ngmodel.phytel"))
            {
                env = "MODEL";
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
                case "MODEL":
                    {
                        analyticsId = "UA-44886803-6";
                    }
                    break;
                default:
                    break;
            }
            return analyticsId;
        }
    }
}
