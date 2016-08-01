using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Allergy;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Test
{
    //[TestClass]
    public class Data_Allergy_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:888/Nightingale";
        IRestClient client = new JsonServiceClient();
        string token = "5453c51984ac0510a8437c85";

        [TestMethod]
        public void InitializeAllergy_Test()
        {
           PostInitializeAllergyRequest request = new PostInitializeAllergyRequest {
                Context = context,
                ContractNumber = contractNumber,
                AllergyName = "New Allergy 2",
                UserId = userId,
                Version = version
            };

           JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("{0}: {1}", "Token", token));
           //[Route("/{Version}/{ContractNumber}/Allergy/Initialize", "POST")]
            PostInitializeAllergyResponse response = client.Post<PostInitializeAllergyResponse>(
    string.Format("{0}/{1}/{2}/Allergy/Initialize", url, version, contractNumber), request);

            Assert.IsNotNull(response);
        }

    }
}
