using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientGoal.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientGoal.Services.Test
{
    [TestClass]
    public class Patient_Task_Put_Test
    {
        [TestMethod]
        public void Put_Patient_Task_Insert()
        {
            string url = "http://localhost:8888/PatientGoal";
            string patientId = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            IRestClient client = new JsonServiceClient();

            PutNewPatientTaskResponse response = client.Put<PutNewPatientTaskResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PatientTask/Insert",
                url,
                context,
                version,
                contractNumber,
                patientId),
                new PutNewPatientTaskRequest() as object);
        }
    }
}
