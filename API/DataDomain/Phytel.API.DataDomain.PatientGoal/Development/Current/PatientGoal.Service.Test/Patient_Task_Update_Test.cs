using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientGoal.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientGoal.Services.Test
{
    [TestClass]
    public class Patient_Task_Update_Test
    {
        [TestMethod]
        public void Update_Patient_Task()
        {
            string url = "http://localhost:8888/PatientGoal";
            string patientId = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string id = "52fd4832fe7a5912b0050354";
            string patientGoaldId = "52fd1aa2fe7a592d04b9d567";
            IRestClient client = new JsonServiceClient();

            PutUpdateTaskResponse response = client.Put<PutUpdateTaskResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Task/{6}/Update",
                url,
                context,
                version,
                contractNumber,
                patientId,
                patientGoaldId,
                id),
                new PutUpdateTaskRequest
                {
                    Task = new PatientTaskData
                    {
                        Id = id,
                        PatientGoalId = patientGoaldId,
                        Description = "Roumel Testing This!!",
                        StartDate = System.DateTime.UtcNow,
                        Status = 2,
                        StatusDate = System.DateTime.UtcNow,
                        TargetDate = System.DateTime.UtcNow.AddDays(7),
                        TargetValue = "!!This is a task update test",
                        Attributes = GetAttributes(),
                        Barriers = GetBarriers()
                    }
                } as object);
        }

        private List<string> GetBarriers()
        {
            List<string> ints = new List<string>();
            ints.Add("5200033cd6a4850aa450d8f1");
            return ints;
        }

        private List<AttributeData> GetAttributes()
        {
            List<AttributeData> tas = new List<AttributeData>();
            tas.Add(new AttributeData
            {
                ControlType = "2",
                Name = "Attribute name",
                Value = "This is the value"
            });

            tas.Add(new AttributeData
            {
                ControlType = "1",
                Name = "Think language",
                Value = "Value of think."
            });
            return tas;
        }
    }
}
