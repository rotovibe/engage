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
            double version = 1.0;
            string token = "5331b06cd6a4850998e38975";
            string patientId = "52f5586e072ef709f84e65fd";
            string typeId = "53067453fe7a591a348e1b66";
            IRestClient client = new JsonServiceClient();

            //JsonServiceClient.HttpWebRequestFilter = x =>
            //                x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "5331b06cd6a4850998e38975"));
            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));

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
            double version = 1.0;
            string token = "5318e69cd6a48503fc5fbbd8";
            string patientId = "52f5586e072ef709f84e65fd";
            IRestClient client = new JsonServiceClient();

            PostUpdateObservationItemsResponse response = client.Post<PostUpdateObservationItemsResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Observation/Update/?Token={3}",
                version,
                contractNumber,
                patientId,
                token), new PostUpdateObservationItemsRequest { PatientObservations = GetObservations() } as object);
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
                EndDate = null,
                Source = "CM"
            });

            pos.Add(new PatientObservation
            {
                Id = "530c26fcfe7a592f64473e37",
                ObservationId = "530c26fcfe7a592f64473e37",
                Name = "Blood Pressure",
                Values = GetBPValues(),
                Units = "mmHg",
                StartDate = System.DateTime.UtcNow,
                EndDate = null,
                Source = "CM"
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
                Id = "531762a1fe7a59042c3c3274",
                Text = "Diastolic blood pressure",
                Value = "99.9"
            });

            ov.Add(new ObservationValue
            {
                Id = "531762a2fe7a59042c3c3278",
                Text = "Systolic blood pressure",
                Value = "999.9"
            });

            return ov;
        }

        private List<ObservationValue> GetValues()
        {
            List<ObservationValue> ov = new List<ObservationValue>();
            ov.Add(new ObservationValue
            {
                Id = "5317629bfe7a59042c3c3267",
                Text = "Body height",
                Value = "99.9"
            });

            return ov;
        }

        [TestMethod]
        public void GetAllowedObservationStates_Test()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string token = "534406e6d6a48508c45b62e0";
            string type = "Lab";
            IRestClient client = new JsonServiceClient();

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));
            //GET	/{Version}/{ContractNumber}/Observation/States/{TypeName}	
            GetAllowedStatesResponse response = client.Get<GetAllowedStatesResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Observation/States/{2}?Context={3}",
                version,
                contractNumber,
                type,
                context));

            Assert.IsNotNull(response.States);
        }

        [TestMethod]
        public void Get_PatientProblemsSummary_Test()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string token = "5346bbb6d6a48504b493083b";
            string patientId = "5325da03d6a4850adcbba4fe";
            string typeId = "53067453fe7a591a348e1b66";
            IRestClient client = new JsonServiceClient();

            //JsonServiceClient.HttpWebRequestFilter = x =>
            //                x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "5331b06cd6a4850998e38975"));
            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));

            GetPatientProblemsResponse response = client.Get<GetPatientProblemsResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Observation/Problems",
                version,
                contractNumber,
                patientId,
                token,
                typeId));
        }

        [TestMethod]
        public void SavePatientProblems_Test()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string patientId = "5325db00d6a4850adcbba802";
            string token = "5346d352d6a48504b4930c16";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));
            PostUpdateObservationItemsResponse response = client.Post<PostUpdateObservationItemsResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Observation/Update",
                version,
                contractNumber,
                patientId), new PostUpdateObservationItemsRequest { PatientObservations = GetProblems() } as object);
        }

        private List<PatientObservation> GetProblems()
        {
            List<PatientObservation> pos = new List<PatientObservation>();
            pos.Add(new PatientObservation
            {
                Id = "5346effcd43323252c52d47b",
                ObservationId = "533ed16ed4332307bc592bb9",
                StartDate = System.DateTime.UtcNow.AddDays(2),
                DisplayId = 2,
                StateId = 4,
                DeleteFlag= false
            });

            pos.Add(new PatientObservation
            {
                Id = "5346ef85d4332324584dd049",
                ObservationId = "533ed16ed4332307bc592bba",
                StartDate = System.DateTime.UtcNow.AddDays(1),
                DisplayId = 0,
                StateId = 3,
                DeleteFlag = true

            });
            return pos;
        }
    }
}
