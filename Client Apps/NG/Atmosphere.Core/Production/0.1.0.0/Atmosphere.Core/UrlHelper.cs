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
    }
}
