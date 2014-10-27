using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Allergy;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class Data_Allergy_Test
    {
        string contractNumber = "InHealth001";
        double version = 1.0;
        string userId = "5325c822072ef705080d3492";
        AllergyManager manager = new AllergyManager{ EndpointUtil = new AllergyEndpointUtil()};

        [TestMethod]
        public void GetAllergy_Test()
        {
            GetAllergiesRequest request = new GetAllergiesRequest();
            request.ContractNumber = contractNumber;
            request.UserId = userId;
            request.Version = 1;
            List<DTO.Allergy> response = manager.GetAllergies(request);
            Assert.IsTrue(response.Count > 0);
        }
    }
}
