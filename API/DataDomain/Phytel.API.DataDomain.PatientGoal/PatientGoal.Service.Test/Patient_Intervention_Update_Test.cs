using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientGoal.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientGoal.Services.Test
{
    [TestClass]
    public class Patient_Intervention_Update_Test
    {
        [TestMethod]
        public void Delete_Intervention_Test()
        {
            string url = "http://localhost:8888/PatientGoal";
            string patientId = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string id = "52fd3fcefe7a5912b0149acd";
            string patientGoaldId = "52fd2d6cd433231c845e7d25";
            IRestClient client = new JsonServiceClient();

            DeleteTaskDataResponse response = client.Delete<DeleteTaskDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Intervention/{6}/Delete/?UserId={7}",
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
        public void Update_Patient_Intervention()
        {
            string url = "http://localhost:8888/PatientGoal";
            string patientId = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string patientGoalId = "12341cf8fe7a592d046c548f";
            string id = "52fd3fcefe7a5912b0149acd";
            IRestClient client = new JsonServiceClient();

            PutUpdateInterventionResponse response = client.Put<PutUpdateInterventionResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Intervention/{6}/Update",
                url,
                context,
                version,
                contractNumber,
                patientId,
                patientGoalId,
                id),
                new PutUpdateInterventionRequest
                {
                    Intervention = new PatientInterventionData
                    {
                        Id = "52fd3fcefe7a5912b0149acd",
                        PatientGoalId = "12341cf8fe7a592d046c548f",
                        Description = "This is an example update",
                        StartDate = System.DateTime.UtcNow,
                        StatusId = 1,
                        StatusDate = System.DateTime.UtcNow,
                        BarrierIds = GetBarriers(),
                        AssignedToId = "Test",
                        CategoryId = "12341cf8fe7a592d046c548f"
                    }
                } as object);
        }

        private List<string> GetInterventions()
        {
            List<string> ints = new List<string>();
            ints.Add("52fba33cd6a4850aa450d8f1");
            return ints;
        }

        private List<string> GetBarriers()
        {
            List<string> ints = new List<string>();
            ints.Add("5200033cd6a4850aa450d8f1");
            return ints;
        }

        private List<CustomAttributeData> GetAttributes()
        {
            List<CustomAttributeData> tas = new List<CustomAttributeData>();
            tas.Add(new CustomAttributeData
            {
                ControlType = 2,
                Id = "5200033cd6a4850aa450d8f1",
                Values = new List<string> { "This is the value" }
            });
            return tas;
        }
    }
}
