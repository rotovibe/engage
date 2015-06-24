using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Search;

namespace Phytel.API.AppDomain.NG.Search
{
    public interface ISearchUtil
    {
        List<MedicationMap> FilterFieldResultsByParams(GetMedFieldsRequest request, List<MedicationMap> matches);
    }
}