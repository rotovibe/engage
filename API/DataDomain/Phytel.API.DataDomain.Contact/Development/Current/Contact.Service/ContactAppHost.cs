using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Phytel.API.DataDomain.Contact.Service.Containers;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.DataDomain.Contact.Service
{
    public class ContactAppHost : AppHostBase
    {
        public ContactAppHost() : base("Phytel Contact Data Domain Services", typeof(ContactService).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            Plugins.Add(new RequestLogsFeature() { RequiredRoles = new string[] { } });

           
            HttpServiceContainer.Build(container);
        }
    }
}