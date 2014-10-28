using System;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.Search.LuceneStrategy;
using Phytel.API.Common.CustomObject;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public class SearchManager : ManagerBase, ISearchManager
    {
        public void RegisterDocumentInSearchIndex(DTO.Allergy allergy)
        {
            try
            {
                var np =  AutoMapper.Mapper.Map<IdNamePair>(allergy);
                new AllergyLuceneStrategy<IdNamePair>().AddUpdateLuceneIndex(np);
            }
            catch (Exception ex)
            {
                throw new WebServiceException("AD:RegisterDocumentInSearchIndex()::" + ex.Message, ex.InnerException);
            }
        }

        public List<IdNamePair> GetSearchDomainResults(GetSearchResultsRequest request)
        {
            try
            {
                // create a switch to determine which lucene strat to use
                var result = new AllergyLuceneStrategy<IdNamePair>().SearchComplex(request.SearchTerm, "Name");
                //var result = new AllergyLuceneStrategy<IdNamePair>().Search(request.SearchTerm);
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetSearchDomainResults()::" + ex.Message, ex.InnerException);
            }
        }


    }
}
