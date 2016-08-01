using Funq;
using Phytel.API.AppDomain.NG.Allergy;
using Phytel.API.AppDomain.NG.Search;

namespace Phytel.API.AppDomain.NG.Service.Containers
{
    public static class SearchContainer
    {
        public static Container Build(Container container)
        {
            container.RegisterAutoWiredAs<SearchManager, ISearchManager>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<SearchUtil, ISearchUtil>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<SearchEndpointUtil, ISearchEndpointUtil>().ReusedWithin(ReuseScope.Request);
            return container;
        }
    }
}