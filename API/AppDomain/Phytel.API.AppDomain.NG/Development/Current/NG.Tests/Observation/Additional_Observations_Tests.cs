using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Observation;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Additional_Observations_Tests
    {
        [TestMethod]
        public void Get_Additional_PatientObservation_Test()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string token = "53079028d2d8e91748f416cc";
            string patientId = "52f558a3072ef709f84e79dc";
            string typeId = "53067453fe7a591a348e1b66";
            string observationId = "530c275bfe7a592f64473e3e";
            IRestClient client = new JsonServiceClient();

            GetAdditionalObservationItemResponse response = client.Get<GetAdditionalObservationItemResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Observation/{3}?Token={4}",
                version,
                contractNumber,
                patientId,
                observationId,
                token));
        }

        [TestMethod]
        public void Get_Additional_PatientObservation_Diastolic_BP_Test()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string token = "53079028d2d8e91748f416cc";
            string patientId = "52f558a3072ef709f84e79dc";
            string typeId = "53067453fe7a591a348e1b66";
            string observationId = "530c26fcfe7a592f64473e37"; // diastolic BP
            IRestClient client = new JsonServiceClient();

            GetAdditionalObservationItemResponse response = client.Get<GetAdditionalObservationItemResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Observation/{3}?Token={4}",
                version,
                contractNumber,
                patientId,
                observationId,
                token));
        }

        [TestMethod]
        public void Get_Additional_PatientObservation_Systolic_BP_Test()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string token = "53079028d2d8e91748f416cc";
            string patientId = "52f558a3072ef709f84e79dc";
            string typeId = "53067453fe7a591a348e1b66";
            string observationId = "530c270afe7a592f64473e38"; // diastolic BP
            IRestClient client = new JsonServiceClient();

            GetAdditionalObservationItemResponse response = client.Get<GetAdditionalObservationItemResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Observation/{3}?Token={4}",
                version,
                contractNumber,
                patientId,
                observationId,
                token));
        }
    }
}
