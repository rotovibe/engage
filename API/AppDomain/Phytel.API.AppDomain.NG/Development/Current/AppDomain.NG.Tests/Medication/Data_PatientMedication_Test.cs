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
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:888/Nightingale";
        IRestClient client = new JsonServiceClient();
        string token = "545d28ee84ac0515d493b3c4";

        [TestMethod]
        public void GetPatientMedSupps_Test()
        {
            GetPatientMedSuppsRequest request = new GetPatientMedSuppsRequest
            {
                ContractNumber = contractNumber,
                PatientId = "5325d9e9d6a4850adcbba4b1",
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
              //  Form = "Liquid",
                //FreqHowOftenId = "545be059d43323224896663a",
                //FreqQuantity = "4",
             //   FreqWhenId = "545be126d433232248966643",
                Id = "545d3b09d433230c244eafff",
                Name = "Acetomophine",
                //NDCs = ,
                //Notes = "note for Acetomophine 2",
                PatientId = "5325d9e9d6a4850adcbba4b1",
                //PharmClasses = ,
                //PrescribedBy = "Dr Basu",
                //Reason = "Reason for Acetomophine 2",
                //Route = "Oral",
                //SigCode = ,
                SourceId = "544e9976d433231d9c0330ae",
                StartDate = DateTime.UtcNow,
                StatusId = 2,
               // Strength = "150 ml",
                SystemName = "Engage",
                TypeId = "545bdfa1d433232248966638"
            };

            PostPatientMedSuppRequest request = new PostPatientMedSuppRequest
            {
                ContractNumber = contractNumber,
                PatientMedSupp = pms,
                Insert = false,
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
