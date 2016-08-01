using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientGoal.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientGoal.Services.Test
{
    [TestClass]
    public class Patient_Barrier_Put_Test
    {
        [TestMethod]
        public void Put_Patient_Barrier_Insert()
        {
            string url = "http://localhost:8888/PatientGoal";
            string patientId = "52a0da34fe7a5915485bdfd6";
            string patientgoalid = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            IRestClient client = new JsonServiceClient();

            PutInitializeBarrierDataResponse response = client.Put<PutInitializeBarrierDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Barrier/Initialize",
                url,
                context,
                version,
                contractNumber,
                patientId,
                patientgoalid),
                new PutInitializeBarrierDataRequest() as object);
        }
    }
}
