using Atmosphere;
using C3.Business;
using C3.Data;
using C3.Web.Helpers;
using Phytel.Framework.SQL.Cache;
using Phytel.Framework.SQL.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web;

namespace C3.Web
{
    public class Global : System.Web.HttpApplication //, IContainerAccessor
    {
        public Global()
        {
            System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new AssemblyResourceProvider());
        }

        #region Private Variables
        private SimpleCache cache = new SimpleCache();
        private List<ControlPermission> _cachedControlPermissions;
        private List<Control> _cachedControls;
        #endregion

        protected void Application_Start(object sender, EventArgs e)
        {
            //How to access the cache
            try
            {
                new CacheAccessor("C3Cache", "ClientsConfigurations");
                cache.Put("ClientsConfigurations", null);

                _cachedControlPermissions = ControlService.Instance.GetAllControlsPermissions();
                cache.Put("ControlPermissions", _cachedControlPermissions);

                _cachedControls = ControlService.Instance.GetAllControls();
                cache.Put("Controls", _cachedControls);

                cache.Put("ApplicationMessages", ApplicationMessageService.Instance.GetAll());
                cache.Put("ApplicationSettings", ApplicationSettingService.Instance.GetAll());

            }
            catch { }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["UserPageInfo"] = new PageInfo();
            Session["AppointmentListDate"] = DateTime.Now.ToLongDateString();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string uri = HttpContext.Current.Request.Url.ToString();
            var provider = System.Web.Hosting.HostingEnvironment.VirtualPathProvider;
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            //// Handle HTTP errors
            if (exc.GetType() == typeof(HttpException))
            {
            //    // The Complete Error Handling Example generates
            //    // some errors using URLs with "NoCatch" in them;
            //    // ignore these here to simulate what would happen
            //    // if a global.asax handler were not implemented.
            //    if (exc.Message.Contains("NoCatch"))
            //        return;

            //    //Redirect HTTP errors to HttpError page
            //    //Server.Transfer("HttpErrorPage.aspx");
            }
            
            //Session["Error"] += exc.Message + ",";
            //// Clear the error from the server,I think your  Session variables doesn't work duo to missing this
            //Server.ClearError();
            //Server.Transfer(HttpContext.Current.Request.Url.PathAndQuery.ToString());

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}