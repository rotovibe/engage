using System.Collections.Generic;
using Phytel.API.DataDomain.Search.DTO;

namespace Phytel.API.DataDomain.Search
{
    public interface ISearchDataManager
    {
        List<TextValuePair> GetTermSearchResults(GetSearchRequest request);
        List<DTO.Search> GetSearchList(GetAllSearchsRequest request);
    }
}