using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Phytel.API.AppDomain.NG.Allergy;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.Search;
using Phytel.API.AppDomain.NG.Search.LuceneStrategy;
using ServiceStack.Common;

namespace Phytel.API.AppDomain.NG.Service.Containers
{
    public static class SearchContainer
    {
        public static Funq.Container Build(Funq.Container container)
        {
            container.RegisterAutoWiredAs<SearchManager, ISearchManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<SearchUtil, ISearchUtil>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<SearchEndpointUtil, ISearchEndpointUtil>().ReusedWithin(Funq.ReuseScope.Request);
            
            // search index initialization
            container.RegisterAutoWiredAs<MedNameLuceneStrategy<MedNameSearchDoc, TextValuePair>, IMedNameLuceneStrategy<MedNameSearchDoc, TextValuePair>>().ReusedWithin(Funq.ReuseScope.Container);

            return container;
        }
    }
}