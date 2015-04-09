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
    //[TestClass]
    public class Data_PatientMedication_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:888/Nightingale";
        IRestClient client = new JsonServiceClient();
        string token = "54b81d0b84ac050580839a18";

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
                //FamilyId = "54dd28e784ac0714f0cef2cb",
                CategoryId  = 1,
                DeleteFlag = false,
               // Dosage = "Two",
                //EndDate = DateTime.UtcNow,
                //FreqHowOftenId = "545be059d43323224896663a",
                //FreqQuantity = "4",
             //   FreqWhenId = "545be126d433232248966643",
                Id = "54dd2d63d43328283844e107",
                Name = "ARMES",//"NICOTINE",
                Form = "TABLET",
                Route = "ORAL",
                Strength = "56 l",
                //NDCs = ,
               // Notes = "note for Acetomophine 2",
                PatientId = "54dd289384ac0511987c998e",
                //PharmClasses = ,
            //  PrescribedBy = "Dr Basu",
               // Reason = "Reason for Acetomophine 2",
                //SigCode = ,
                SourceId = "544e9976d433231d9c0330ae",
                StartDate = DateTime.UtcNow,
                StatusId = 1,
              //  SystemName = "Engage",
                TypeId = "545bdfa1d433232248966638"
            };

            PostPatientMedSuppRequest request = new PostPatientMedSuppRequest
            {
                ContractNumber = contractNumber,
                PatientMedSupp = pms,
                RecalculateNDC = false,
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

        [TestMethod]
        public void DeletePatientMedSupp_Test()
        {
            
            DeletePatientAllergyRequest request = new DeletePatientAllergyRequest
            {
                ContractNumber = contractNumber,
                Id = "54ef51a984ac0711a867cd43",
                PatientId = "5325db24d6a4850adcbba85a",
                UserId = userId,
                Version = version
            };

            //[Route("/{Version}/{ContractNumber}/Patient/{PatientId}/PatientMedSupp/{Id}", "DELETE")]
            DeletePatientMedSuppResponse response = client.Delete<DeletePatientMedSuppResponse>(
                string.Format("{0}/{1}/{2}/Patient/{3}/PatientMedSupp/{4}?UserId={5}", url, version, contractNumber, request.PatientId, request.Id, request.UserId));
            Assert.IsNotNull(response);
        }
    }
}
