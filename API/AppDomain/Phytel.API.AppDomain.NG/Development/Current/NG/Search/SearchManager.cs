using System.IO;
using System.Linq;
using AutoMapper;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.Search;
using Phytel.API.DataDomain.Allergy.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using Phytel.API.Common;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public class SearchManager : ManagerBase, ISearchManager
    {
        public List<DTO.Search.SearchedItem> GetSearchDomainResults(GetSearchResultsRequest request)
        {
            try
            {
                var result = LuceneManager.SearchComplex(request.SearchTerm);
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetSearchDomainResults()::" + ex.Message, ex.InnerException);
            }
        }


    }
}
