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
        public void Update_Patient_Intervention()
        {
            string url = "http://localhost:8888/PatientGoal";
            string patientId = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string id = "52fd1cf8fe7a592d046c548f";
            IRestClient client = new JsonServiceClient();

            PutUpdateInterventionResponse response = client.Put<PutUpdateInterventionResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Intervention/{5}/Update",
                url,
                context,
                version,
                contractNumber,
                patientId,
                id),
                new PutUpdateInterventionRequest
                {
                    Task = new PatientIntervention
                    {
                        Id = "52fd1cf8fe7a592d046c548f",
                        Description = "This is an example update",
                        StartDate = System.DateTime.UtcNow,
                        Status = 1,
                        StatusDate = System.DateTime.UtcNow,
                        Attributes = GetAttributes(),
                        Barriers = GetBarriers(),
                        AssignedTo = "Test",
                        Category = 1
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

        private List<InterventionAttribute> GetAttributes()
        {
            List<InterventionAttribute> tas = new List<InterventionAttribute>();
            tas.Add(new InterventionAttribute
            {
                ControlType = "2",
                Name = "Attribute name",
                Value = "This is the value"
            });
            return tas;
        }
    }
}
