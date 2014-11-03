using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO.Search;

namespace Phytel.API.AppDomain.NG.Search
{
    public interface ISearchUtil
    {
        List<MedFieldsSearchDoc> FilterFieldResultsByParams(GetMedFieldsRequest request, List<MedFieldsSearchDoc> matches);
    }
}