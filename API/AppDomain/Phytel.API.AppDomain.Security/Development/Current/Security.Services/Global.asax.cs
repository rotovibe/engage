﻿using ServiceStack.WebHost.Endpoints;
using System;

namespace Phytel.API.AppDomain.Security.Service
{
    public class Global : System.Web.HttpApplication
    {
        public class SecurityAppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public SecurityAppHost() : base("Phytel Security Services", typeof(SecurityService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                //container.Register<ICacheClient>(new MemoryCacheClient());
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            new SecurityAppHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}