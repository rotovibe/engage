using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Observations_Tests
    {
        [TestMethod]
        public void Get_PatientObservation_Standard_Test()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string token = "53079028d2d8e91748f416cc";
            string patientId = "52e26f0b072ef7191c111c4d";
            string typeId = "53067453fe7a591a348e1b66";
            IRestClient client = new JsonServiceClient();

            GetStandardObservationItemsResponse response = client.Get<GetStandardObservationItemsResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Observation/?Token={3}&TypeId={4}",
                version,
                contractNumber,
                patientId,
                token,
                typeId));

            //GetStandardObservationItemsResponse response = client.Get<GetStandardObservationItemsResponse>(
            //    string.Format(@"http://azurephyteldev.cloudapp.net:59900/Nightingale/{0}/{1}/Patient/{2}/Observation/?Token={3}&TypeId={4}",
            //    version,
            //    contractNumber,
            //    patientId,
            //    token,
            //    typeId));
        }
    }
}
