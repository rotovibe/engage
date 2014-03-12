using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientObservation.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientObservation.Services.Test
{
    [TestClass]
    public class Observation_Test
    {
        [TestMethod]
        public void Get_ObservationByTypeID()
        {
            string url = "http://localhost:8888/PatientObservation";
            //string url = "http://azurePhytel.cloudapp.net:59901/PatientObservation";
            string TypeID = "53067453fe7a591a348e1b66";
            string patientId = "531f2dcc072ef727c4d29e1a";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetStandardObservationsResponse response = client.Get<GetStandardObservationsResponse>(
                string.Format("{0}/{1}/{2}/{3}/Observation/?TypeId={4}&PatientId={5}", 
                url, 
                context, 
                version, 
                contractNumber, 
                TypeID,
                patientId));
        }
    }
}
