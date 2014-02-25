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
            string patientId = "52f55853072ef709f84e5bf0";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            IRestClient client = new JsonServiceClient();

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
