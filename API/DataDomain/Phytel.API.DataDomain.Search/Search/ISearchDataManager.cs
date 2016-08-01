using System.Collections.Generic;
using Phytel.API.DataDomain.Search.DTO;

namespace Phytel.API.DataDomain.Search
{
    public interface ISearchDataManager
    {
        List<object> GetTermSearchResults(GetSearchRequest request, SearchEnum type);
        bool InsertMedDocInIndex(PutMedRegistrationRequest request);
        bool InsertAllergyDocInIndex(PutAllergyRegistrationRequest request);
        List<DTO.Search> GetSearchList(GetAllSearchsRequest request);
        bool DeleteMedDocs(List<MedNameSearchDocData> request);
    }
}