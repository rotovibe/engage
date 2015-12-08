using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.Common;
using Phytel.API.DataDomain.PatientSystem.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientSystem.Test
{
    [TestClass]
    public class PatientSystem_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:8888/PatientSystem";
        IRestClient client = new JsonServiceClient();

        [TestMethod]
        public void GetPatientSystems_Test()
        {
            GetPatientSystemsDataRequest request = new GetPatientSystemsDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                PatientId = "546d0d0684ac0508e43299d2"
                
            };
            //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystems", "GET")]

            GetSystemsDataResponse response = client.Get<GetSystemsDataResponse>(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PatientSystems?UserId={5}", url, context, version, contractNumber, request.PatientId, request.UserId));
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void InsertPatientSystem_Test()
        {
            InsertPatientSystemDataRequest request = new InsertPatientSystemDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                PatientSystemsData = new PatientSystemData { PatientId = "55a57abd84ac072348fb7664" },
                PatientId = "55a57abd84ac072348fb7664",
                IsEngageSystem = true
            };

            //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystem", "POST")]         
            InsertPatientSystemsDataResponse response = client.Post<InsertPatientSystemsDataResponse>(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PatientSystem", url, context, version, contractNumber, request.PatientId), request);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void InsertPatientSystems_Test()
        {
            List<PatientSystemData> list = new List<PatientSystemData>();
            list.Add(new PatientSystemData { PatientId = "546d0d0684ac0508e43299d2", Value = " 12345 ", StatusId = 1, Primary = false, SystemId = "559e8c70d4332320bc076f4e" });
            list.Add(new PatientSystemData { PatientId = "546d0d0684ac0508e43299d2", Value = " ABCFG ", StatusId = 1, Primary = true, SystemId = "559e8c70d4332320bc076f4f" });
            
            InsertPatientSystemsDataRequest request = new InsertPatientSystemsDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                PatientSystemsData = list,
                PatientId = "546d0d0684ac0508e43299d2"
            };

            //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystems", "POST")]            
            InsertPatientSystemsDataResponse response = client.Post<InsertPatientSystemsDataResponse>(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PatientSystems", url, context, version, contractNumber, request.PatientId), request);
            Assert.IsNotNull(response);
        }
        [TestMethod]
        public void UpdatePatientSystems_Test()
        {
            List<PatientSystemData> list = new List<PatientSystemData>();
            list.Add(new PatientSystemData { Id = "55a014bbd4332720a4bf5093", PatientId = "546d0d0684ac0508e43299d2", Value = " 12345UP ", StatusId = 2, Primary = false, SystemId = "559e8c70d4332320bc076f4d" });
            list.Add(new PatientSystemData { Id = "55a014bcd4332720a4bf509a", PatientId = "546d0d0684ac0508e43299d2", Value = " ABCFGUP ", StatusId = 2, Primary = false, SystemId = "559e8c70d4332320bc076f4d" });

            UpdatePatientSystemsDataRequest request = new UpdatePatientSystemsDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                PatientSystemsData = list,
                PatientId = "546d0d0684ac0508e43299d2"
            };

            //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystems", "PUT")]        
            UpdatePatientSystemsDataResponse response = client.Put<UpdatePatientSystemsDataResponse>(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PatientSystems", url, context, version, contractNumber, request.PatientId), request);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void DeletePatientSystems_Test()
        {
            List<string> list = new List<string>();
            list.Add("55a01d11d43323224085d1f1");
            list.Add("55a01ef4d43323224085d1f2");

            DeletePatientSystemsDataRequest request = new DeletePatientSystemsDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                PatientId = "546d0d0684ac0508e43299d2",
                Ids = "55a01d11d43323224085d1f1,55a01ef4d43323224085d1f2"
            };

            //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystems/{Ids}", "DELETE")]
            DeletePatientSystemsDataResponse response = client.Delete<DeletePatientSystemsDataResponse>(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PatientSystems/{5}?UserId={6}", url, context, version, contractNumber, request.PatientId, request.Ids,request.UserId));
            Assert.IsNotNull(response);
        }

        //[TestMethod]
        //public void UpsertPatientSystems_Test()
        //{
        //  //  List<PatientSystemData> lPsd = (List<PatientSystemData>)Helpers.DeserializeObject<List<PatientSystemData>>("PatientsSystemExample.txt");

        //    UpsertBatchPatientSystemsDataRequest request = new UpsertBatchPatientSystemsDataRequest
        //    {
        //        Context = context,
        //        ContractNumber = contractNumber,
        //        UserId = userId,
        //        Version = version,
        //        PatientSystemsData = lPsd
        //    };

        //    //[Route("/{Context}/{Version}/{ContractNumber}/Batch/PatientSystems", "POST")]
        //    UpsertBatchPatientSystemsDataResponse response = client.Post<UpsertBatchPatientSystemsDataResponse>(string.Format("{0}/{1}/{2}/{3}/Batch/PatientSystems", url, context, version, contractNumber), request);
        //    Assert.IsNotNull(response);
        //}

   }
}
