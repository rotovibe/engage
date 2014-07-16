using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientObservation.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientObservation.Services.Test
{
    [TestClass]
    public class User_PatientObservation_Test
    {
        [TestMethod]
        public void Get_PatientProblems()
        {
            string url = "http://localhost:8888/PatientObservation";
            string patientId = "5325da6fd6a4850adcbba63e";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string userId = "531f2df9072ef727c4d2a3df";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", userId));

            // /{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Observation//
            GetPatientProblemsSummaryResponse response = client.Get<GetPatientProblemsSummaryResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Observation/Problems?UserId={5}", url, context, version, contractNumber, patientId, userId));

            Assert.AreEqual(string.Empty, string.Empty);
        }

        [TestMethod]
        public void Get_PatientObservationByID()
        {
            string url = "http://localhost:8888/PatientObservation";
            string ObservationID = "533ed16ed4332307bc592bb8";
            string patientId = "5325da03d6a4850adcbba4fe";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string userId = "531f2df9072ef727c4d2a3df";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", userId));

            GetPatientObservationResponse response = client.Get<GetPatientObservationResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Observation/{5}?UserId={6}", url, context, version, contractNumber, patientId, ObservationID, userId));

            Assert.AreEqual(string.Empty, string.Empty);
        }

        [TestMethod]
        public void DeletePatientObservationByPatientId_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientId = "5325db70d6a4850adcbba946";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/PatientObservation";
            IRestClient client = new JsonServiceClient();

            // [Route("/{Context}/{Version}/{ContractNumber}/PatientObservation/Patient/{PatientId}/Delete", "DELETE")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientObservation/Patient/{4}/Delete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber,
                                        patientId), userId);
            DeletePatientObservationByPatientIdDataResponse response = client.Delete<DeletePatientObservationByPatientIdDataResponse>(url);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UndoDeletePatientObservations_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/PatientObservation";
            IRestClient client = new JsonServiceClient();

            //[Route("/{Context}/{Version}/{ContractNumber}/PatientObservation/UndoDelete", "PUT")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientObservation/UndoDelete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber), userId);
            List<string> ids = new List<string>();
            ids.Add("53c6a795d6a48506ec49f9a6");
            ids.Add("53c6a7a1d6a48506ec49fa59");
            ids.Add("53c6a7a1d6a48506ec49fa64");
            ids.Add("53c6a7add6a48506ec49fa97");
            ids.Add("53c6a7bad6a48506ec49fb11");
            UndoDeletePatientObservationsDataResponse response = client.Put<UndoDeletePatientObservationsDataResponse>(url, new UndoDeletePatientObservationsDataRequest 
            { 
                Ids = ids,
                Context = context, 
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            });
            Assert.IsNotNull(response);
        }
    }
}
