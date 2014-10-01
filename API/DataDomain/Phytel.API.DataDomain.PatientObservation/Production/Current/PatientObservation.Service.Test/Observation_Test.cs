using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientObservation.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using Phytel.API.Common;
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
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "5331b06cd6a4850998e38975"));

            GetStandardObservationsResponse response = client.Get<GetStandardObservationsResponse>(
                string.Format("{0}/{1}/{2}/{3}/Observation/?TypeId={4}&PatientId={5}", 
                url, 
                context, 
                version, 
                contractNumber, 
                TypeID,
                patientId));
        }

        [TestMethod]
        public void GetAllowedObservationStates_CorrectType_Test()
        {
            string url = "http://localhost:8888/PatientObservation";
           //string url = "http://azurePhytel.cloudapp.net:59901/PatientObservation";
            string type = "Lab";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string userId = "000000000000000000";
            IRestClient client = new JsonServiceClient();

            GetAllowedStatesDataResponse response = client.Get<GetAllowedStatesDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Observation/States/{4}?UserId={5}",
                url,
                context,
                version,
                contractNumber,
                type,
                userId));

            Assert.IsNotNull(response.StatesData);
        }

        [TestMethod]
        public void GetAllowedObservationStates_IncorrectType_Test()
        {
            string url = "http://localhost:8888/PatientObservation";
            //string url = "http://azurePhytel.cloudapp.net:59901/PatientObservation";
            string type = "Labs";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string userId = "000000000000000000";
            IRestClient client = new JsonServiceClient();

            GetAllowedStatesDataResponse response = client.Get<GetAllowedStatesDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Observation/States/{4}?UserId={5}",
                url,
                context,
                version,
                contractNumber,
                type,
                userId));

            Assert.IsNull(response.StatesData);
        }

        [TestMethod]
        public void GetObservations_Test()
        {
            string url = "http://localhost:8888/PatientObservation";
            //string url = "http://azurePhytel.cloudapp.net:59901/PatientObservation";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string userId = "000000000000000000";
            IRestClient client = new JsonServiceClient();

            //[Route("/{Context}/{Version}/{ContractNumber}/Observation/Type/{TypeId}/MatchLibrary/{Standard}", "GET")]

            GetObservationsDataResponse response = client.Get<GetObservationsDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Observations?UserId={4}",
                url,
                context,
                version,
                contractNumber,
                userId));

            Assert.IsNotNull(response.ObservationsData);
        }
    }
}
