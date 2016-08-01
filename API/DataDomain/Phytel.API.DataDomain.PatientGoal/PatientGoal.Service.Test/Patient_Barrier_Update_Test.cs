using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientGoal.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientGoal.Services.Test
{
    [TestClass]
    public class Patient_Barrier_Update_Test
    {
        [TestMethod]
        public void Delete_Barrier_Test()
        {
            string url = "http://localhost:8888/PatientGoal";
            string patientId = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string id = "52fe51a1d6a4850944a9d19d";
            string patientGoaldId = "52fd2d6cd433231c845e7d25";
            IRestClient client = new JsonServiceClient();

            DeleteBarrierDataResponse response = client.Delete<DeleteBarrierDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Barrier/{6}/Delete/?UserId={7}",
                url,
                context,
                version,
                contractNumber,
                patientId,
                patientGoaldId,
                id,
                patientId));
        }
        [TestMethod]
        public void Update_Patient_Barrier()
        {
            string url = "http://localhost:8888/PatientGoal";
            string patientId = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string id = "52fe4f5ffe7a591e9c60a7c5";
            string patientGoaldId = "52fd1aa2fe7a592d04b9d567";
            IRestClient client = new JsonServiceClient();

            PutUpdateBarrierResponse response = client.Put<PutUpdateBarrierResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Barrier/{6}/Update",
                url,
                context,
                version,
                contractNumber,
                patientId,
                patientGoaldId,
                id),
                new PutUpdateBarrierRequest
                {
                    Barrier = new PatientBarrierData
                   {
                       Id = id,
                       CategoryId = "52a0da34fe7a5915485bdfd6",
                       Name = "this is a test name " + new Random().Next(1, 100),
                       PatientGoalId = patientGoaldId,
                       StatusId = 2,
                       StatusDate = System.DateTime.UtcNow,
                   },
                    UserId = "52a0da34fe7a5915485bdfd6"
                } as object);
        }
    }
}
