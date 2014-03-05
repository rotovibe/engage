using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Observation;
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
            string token = "5317440bd6a4850c20c998a2";
            string patientId = "52f5586e072ef709f84e65fd";
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

        [TestMethod]
        public void Save_Patient_Observations_Test()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string token = "5317440bd6a4850c20c998a2";
            string patientId = "52f5586e072ef709f84e65fd";
            IRestClient client = new JsonServiceClient();

            PostUpdateObservationItemsResponse response = client.Post<PostUpdateObservationItemsResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Observation/Update/?Token={3}",
                version,
                contractNumber,
                patientId,
                token), new PostUpdateObservationItemsRequest { Observations = GetObservations() } as object);
        }

        private List<PatientObservation> GetObservations()
        {
            List<PatientObservation> pos = new List<PatientObservation>();
            pos.Add(new PatientObservation
            {
                Id = "530c2718fe7a592f64473e39",
                ObservationId = "530c2718fe7a592f64473e39",
                Name = "Body height",
                Values = GetValues(),
                Units = "inches",
                StartDate = System.DateTime.UtcNow,
                EndDate = null
            });

            pos.Add(new PatientObservation
            {
                Id = "530c26fcfe7a592f64473e37",
                ObservationId = "530c26fcfe7a592f64473e37",
                Name = "Blood Pressure",
                Values = GetBPValues(),
                Units = "mmHg",
                StartDate = System.DateTime.UtcNow,
                EndDate = null
            });

            //pos.Add(new PatientObservation
            //{
            //    Id = "530c26fcfe7a592f64473e37",
            //    Name = "Test Observation",
            //    Values = GetTestValues(),
            //    Units = "mmHg",
            //    StartDate = System.DateTime.UtcNow,
            //    EndDate = null
            //});

            return pos;
        }

        //private List<ObservationValue> GetTestValues()
        //{
        //    List<ObservationValue> ov = new List<ObservationValue>();
        //    ov.Add(new ObservationValue
        //    {
        //        Id = "530e5393fe7a5908dc1e3621",
        //        Text = "Test value",
        //        Value = "testing values!"
        //    });

        //    return ov;
        //}

        private List<ObservationValue> GetBPValues()
        {
            List<ObservationValue> ov = new List<ObservationValue>();
            ov.Add(new ObservationValue
            {
                Id = "531746b8fe7a591ef85c3786",
                Text = "Diastolic blood pressure",
                Value = "60.4"
            });

            ov.Add(new ObservationValue
            {
                Id = "531746b8fe7a591ef85c3789",
                Text = "Systolic blood pressure",
                Value = "117.1"
            });

            return ov;
        }

        private List<ObservationValue> GetValues()
        {
            List<ObservationValue> ov = new List<ObservationValue>();
            ov.Add(new ObservationValue
            {
                Id = "531746b7fe7a591ef85c377a",
                Text = "Body height",
                Value = "71.1"
            });

            return ov;
        }
    }
}
