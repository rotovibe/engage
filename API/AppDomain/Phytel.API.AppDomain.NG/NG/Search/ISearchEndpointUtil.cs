using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.Search.DTO;
using Phytel.API.Interface;
using MedicationMap = Phytel.API.AppDomain.NG.DTO.MedicationMap;

namespace Phytel.API.AppDomain.NG.Search
{
    public interface ISearchEndpointUtil
    {
        List<MedicationMap> GetMedicationMapsByName(GetMedFieldsRequest e, string userId);
        List<object> GetTermSearchResults(IAppDomainRequest e, SearchEnum type, string term);
        void RegisterMedDocument(IAppDomainRequest request, MedNameSearchDoc md);
        void RegisterAllergyDocument(IAppDomainRequest request, IdNamePair ad);
        bool DeleteMedDocuments(DeleteMedicationMapsRequest request);
    }
}