﻿using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.Template;
using Phytel.API.DataDomain.Template.DTO;

namespace DataDomain.Template.Repo.Containers
{
    public static class TemplateContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {
            container = TemplateRepositoryContainer.Configure(container);

            container.Register<ITemplateDataManager>(c =>
                new TemplateDataManager(c.ResolveNamed<IMongoTemplateRepository>(Constants.Domain))
                ).ReusedWithin(Funq.ReuseScope.Default);

            container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<Helpers, IHelpers>().ReusedWithin(Funq.ReuseScope.Request);
            return container;
        }
    }
}
