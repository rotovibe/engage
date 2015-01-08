using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.AppDomain.NG
{
    public interface ISearchManager
    {
        List<IdNamePair> GetSearchDomainResults(GetSearchResultsRequest request);
        List<TextValuePair> GetSearchMedNameResults(GetMedNamesRequest request);
        MedFieldsLists GetSearchMedFieldsResults(GetMedFieldsRequest request);
        void LogException(Exception ex);
        void RegisterDocumentInSearchIndex(DTO.Allergy allergy, string contractNumber);
        void RegisterMedDocumentInSearchIndex(DTO.Medication med, string contractNumber);
    }
}