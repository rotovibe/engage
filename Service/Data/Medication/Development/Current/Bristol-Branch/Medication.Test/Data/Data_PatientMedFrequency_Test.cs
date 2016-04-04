using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.DataDomain.Medication.Test;
using DataDomain.Medication.Repo;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Allergy.Test
{
    //[TestClass]
    public class Data_PatientMedFrequency_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:8888/Medication";
        IRestClient client = new JsonServiceClient();

        [TestMethod]
        public void GetPatientMedFrequency_Test()
        {
            GetPatientMedFrequenciesDataRequest request = new GetPatientMedFrequenciesDataRequest
            {
                PatientId = "5325daefd6a4850adcbba7ca",
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            };
            //[Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/Frequency/{PatientId}", "GET")]
            GetPatientMedFrequenciesDataResponse response = client.Get<GetPatientMedFrequenciesDataResponse>(string.Format("{0}/{1}/{2}/{3}/PatientMedSupp/Frequency/{4}?UserId={5}", url, context, version, contractNumber, request.PatientId, request.UserId));
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void InsertPatientMedFrequency_Test()
        {
            PatientMedFrequencyData data = new PatientMedFrequencyData
            {
                Name = "abc",
                PatientId = "5325da88d6a4850adcbba68a", 
            };

            PostPatientMedFrequencyDataRequest request = new PostPatientMedFrequencyDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                PatientMedFrequencyData = data,
                UserId = userId,
                Version = version
            };

            //[Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/Frequency/Insert", "POST")]
            PostPatientMedFrequencyDataResponse response = client.Post<PostPatientMedFrequencyDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/PatientMedSupp/Frequency/Insert", url, context, version, contractNumber), request);
            Assert.IsNotNull(response);
        }
    }
}
