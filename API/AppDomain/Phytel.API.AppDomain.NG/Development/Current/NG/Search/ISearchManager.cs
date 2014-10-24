using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Search;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public interface ISearchManager
    {
        List<DTO.Search.SearchedItem> GetSearchDomainResults(GetSearchResultsRequest request);
        void LogException(Exception ex);
    }
}