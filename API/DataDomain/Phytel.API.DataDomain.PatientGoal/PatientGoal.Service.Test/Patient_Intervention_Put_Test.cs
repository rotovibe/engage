using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientGoal.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientGoal.Services.Test
{
    [TestClass]
    public class Patient_Intervention_Put_Test
    {
        [TestMethod]
        public void Put_Patient_Intervention_Insert()
        {
            string url = "http://localhost:8888/PatientGoal";
            string patientId = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string patientGoalId = "52a0da34fe7a5915485bd111";
            IRestClient client = new JsonServiceClient();

            PutInitializeInterventionResponse response = client.Put<PutInitializeInterventionResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Intervention/Initialize",
                url,
                context,
                version,
                contractNumber,
                patientId,
                patientGoalId),
                new PutInitializeTaskRequest() as object);
        }
    }
}
