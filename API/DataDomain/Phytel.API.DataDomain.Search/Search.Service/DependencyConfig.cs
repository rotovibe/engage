using Phytel.API.DataDomain.Search.MongoDB.Repository;
using Phytel.API.DataDomain.Search.Service.ContextProxy;

namespace Phytel.API.DataDomain.Search.Service
{
    public static class DependencyConfig
    {
        public static void Config(Funq.Container container)
        {
            container.RegisterAutoWiredAs<SearchDataManager, ISearchDataManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.Register<IHostContextProxy>(new HostContextProxy());

            container.Register<string>(RepositoryContainer.NamedUnitOfWorkSearch, c =>
            {
                IHostContextProxy hostContextProxy = c.Resolve<IHostContextProxy>();
                return hostContextProxy.ContractNumber;
            }).ReusedWithin(Funq.ReuseScope.Request);

            RepositoryContainer.Configure(container);
        }
    }
}