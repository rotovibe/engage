using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.Search;
using Phytel.API.DataDomain.Search.DTO;

namespace DataDomain.Search.Repo.Containers
{
    public static class SearchContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {
            container = SearchRepositoryContainer.Configure(container);

            //container.Register<ISearchDataManager>(c =>
            //    new SearchDataManager(c.ResolveNamed<IMongoSearchRepository>(Constants.Domain))
            //    ).ReusedWithin(Funq.ReuseScope.Default);

            container.RegisterAutoWiredAs<SearchDataManager, ISearchDataManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<Helpers, IHelpers>().ReusedWithin(Funq.ReuseScope.Request);
            return container;
        }
    }
}
