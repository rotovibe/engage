﻿using ServiceStack;
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
                SetConfig(new HostConfig { AllowJsonpRequests = true });

                //Permit modern browsers to allow sending of any REST HTTP Method
                SetConfig(new HostConfig
                {
                    GlobalResponseHeaders = { { "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" },
                                                { "Access-Control-Allow-Headers", "Content-Type" },
                                                },
                });
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