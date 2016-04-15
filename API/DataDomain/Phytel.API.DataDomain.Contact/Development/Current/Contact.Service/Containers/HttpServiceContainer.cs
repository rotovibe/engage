using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Phytel.API.Common;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.Contact.CareTeam;
using Phytel.API.DataDomain.Contact.ContactTypeLookUp;
using ServiceStack.Common;

namespace Phytel.API.DataDomain.Contact.Service.Containers
{
    public class HttpServiceContainer
    {
        private static string Proxy = "ContactProxy";

        public static Funq.Container Build(Funq.Container container)
        {
            container.Register<HostContext>(c => HostContext.Instance).ReusedWithin(Funq.ReuseScope.Request);

            container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>();
            container.RegisterAutoWiredAs<ContactRepositoryFactory, IContactRepositoryFactory>();
            container.RegisterAutoWiredAs<Helpers, IHelpers>();
            container.RegisterAutoWiredAs<ContactDataManager, IContactDataManager>();
            container.RegisterAutoWiredAs<ContactTypeLookUpRepositoryFactory, IContactTypeLookUpRepositoryFactory>();
            container.RegisterAutoWiredAs<ContactTypeLookUpManager, IContactTypeLookUpManager>();

            //Register CareTeam 
            container.RegisterAutoWiredAs<MongoCareTeamRepository, ICareTeamRepository>();
            container.RegisterAutoWiredAs<CareTeamRepositoryFactory, ICareTeamRepositoryFactory>();
            container.RegisterAutoWiredAs<CareTeamDataManager, ICareTeamManager>();


            container.RegisterAutoWiredAs<AuditHelpers, IAuditHelpers>();

            container = ContactContainer.Configure(container);

            return container;
        }
    }
}