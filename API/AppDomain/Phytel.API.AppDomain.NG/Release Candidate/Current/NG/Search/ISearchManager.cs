using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.Common.CustomObject;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.NG
{
    public interface ISearchManager
    {
        List<IdNamePair> GetSearchAllergyResults(GetSearchResultsRequest request);
        List<TextValuePair> GetSearchMedNameResults(GetMedNamesRequest request);
        MedFieldsLists GetSearchMedFieldsResults(GetMedFieldsRequest request);
        void LogException(Exception ex);
        void RegisterAllergyDocumentInSearchIndex(DTO.Allergy allergy, string contractNumber, IAppDomainRequest request);
        void RegisterMedDocumentInSearchIndex(DTO.Medication med, IAppDomainRequest request);
        void DeleteMedDocuments(DeleteMedicationMapsRequest request);
    }
}