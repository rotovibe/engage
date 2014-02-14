using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientGoal.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientGoal.Services.Test
{
    [TestClass]
    public class Patient_Goal_Update_Test
    {
        [TestMethod]
        public void Update_Goal_Task()
        {
            string url = "http://localhost:8888/PatientGoal";
            string patientId = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string id = "52fc609fd43323258c5c8c71";
            string patientGoaldId = "52fc609fd43323258c5c8c71";
            IRestClient client = new JsonServiceClient();

            PutPatientGoalDataResponse response = client.Put<PutPatientGoalDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Update",
                url,
                context,
                version,
                contractNumber,
                patientId,
                patientGoaldId),
                new PutPatientGoalDataRequest
                {
                     GoalData = new PatientGoalData
                    {
                        Id = id,
                         EndDate = System.DateTime.UtcNow.AddDays(10),
                        FocusAreaIds = new List<string> { "52fd4832fe7a5912b0050311" },
                         Name = "test name",
                          PatientId = patientId,
                           Programs = new List<string>{ "52fd4832fe7a591234050354"},
                            Source = "source data",
                        Type = "52fd54321e7a5912b0050354",
                        StartDate = System.DateTime.UtcNow,
                        StatusId = 2,
                        TargetDate = System.DateTime.UtcNow.AddDays(7),
                        TargetValue = "!!This is a task update test",
                        Attributes = GetAttributes(),
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
