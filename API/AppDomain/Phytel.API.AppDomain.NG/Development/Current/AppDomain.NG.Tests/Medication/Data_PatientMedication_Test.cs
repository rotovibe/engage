using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Medication;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class Data_PatientMedication_Test
    {
        string context = "NG";
        string contractNumber = "Demo001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:888/Nightingale";
        IRestClient client = new JsonServiceClient();
        string token = "546bac6f60e4b90c7839b9eb";

        [TestMethod]
        public void GetPatientMedSupps_Test()
        {
            GetPatientMedSuppsRequest request = new GetPatientMedSuppsRequest
            {
                ContractNumber = contractNumber,
                PatientId = "534685c160e4b90f8c8966a8",
                StatusIds = new List<int>{1},
                CategoryIds = new List<int> { 1 },
                UserId = userId,
                Version = version
            };

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "Token", token));

            //[Route("/{Version}/{ContractNumber}/PatientMedSupp/{PatientId}", "POST")]
            GetPatientMedSuppsResponse response = client.Post<GetPatientMedSuppsResponse>( string.Format("{0}/{1}/{2}/PatientMedSupp/{3}", url, version, contractNumber, request.PatientId), request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void SavePatientMedSupp_Test()
        {
            PatientMedSupp pms = new PatientMedSupp
            {
                CategoryId  = 1,
                DeleteFlag = false,
               // Dosage = "Two",
                //EndDate = DateTime.UtcNow,
                //FreqHowOftenId = "545be059d43323224896663a",
                //FreqQuantity = "4",
             //   FreqWhenId = "545be126d433232248966643",
                // Id = "5462991ed4332323a01ce994",
                Name = "KINERASE SPF 30 BROAD SPECTRUM UVA/UVB",
             //   Form = "INJECTION, SOLUTION",
               // Route = "INTRADERMAL",
              //  Strength = ".0021 g/mL",
                //NDCs = ,
               // Notes = "note for Acetomophine 2",
                PatientId = "5325db30d6a4850adcbba882",
                //PharmClasses = ,
            //  PrescribedBy = "Dr Basu",
               // Reason = "Reason for Acetomophine 2",
                //SigCode = ,
                SourceId = "544e9976d433231d9c0330ae",
                StartDate = DateTime.UtcNow,
                StatusId = 2,
                SystemName = "Engage",
                TypeId = "545bdfa1d433232248966638"
            };

            PostPatientMedSuppRequest request = new PostPatientMedSuppRequest
            {
                ContractNumber = contractNumber,
                PatientMedSupp = pms,
                RecalculateNDC = true,
                UserId = userId,
                Version = version
            };

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("{0}: {1}", "Token", token));
            //[Route("/{Version}/{ContractNumber}/PatientMedSupp/Save", "POST")]
            PostPatientMedSuppResponse response = client.Post<PostPatientMedSuppResponse>(
                string.Format("{0}/{1}/{2}/PatientMedSupp/Save", url, version, contractNumber), request);
            Assert.IsNotNull(response);
        }
    }
}
