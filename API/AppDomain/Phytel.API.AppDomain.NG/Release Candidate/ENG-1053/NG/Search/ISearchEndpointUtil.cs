using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO.Search;
using MedicationMap = Phytel.API.AppDomain.NG.DTO.MedicationMap;

namespace Phytel.API.AppDomain.NG.Search
{
    public interface ISearchEndpointUtil
    {
        List<MedicationMap> GetMedicationMapsByName(GetMedFieldsRequest e, string userId);
        List<TextValuePair> GetTermSearchResults(string term);
    }
}