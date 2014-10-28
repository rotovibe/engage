using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Allergy;
using Phytel.API.AppDomain.NG.DTO.Search;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class Data_Search_Test
    {
        string contractNumber = "InHealth001";
        double version = 1.0;
        string userId = "5325c822072ef705080d3492";
        SearchManager manager = new SearchManager();

        [TestMethod]
        public void GetSearchResult_Test()
        {
            GetSearchResultsRequest request = new GetSearchResultsRequest();
            request.ContractNumber = contractNumber;
            request.UserId = userId;
            request.Version = 1;
            request.SearchDomain = "Allergy";
            request.SearchTerm = "Ammonium";
            //List<DTO.Search.SearchedItem> response = manager.GetSearchDomainResults(request);
            //Assert.IsTrue(response.Count > 0);
        }
    }
}