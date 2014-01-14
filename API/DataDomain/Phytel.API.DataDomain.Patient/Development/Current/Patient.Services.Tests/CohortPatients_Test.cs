using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Diagnostics;

namespace Phytel.API.DataDomain.Patient.Services.Test
{
    [TestClass]
    public class User_Services_Test
    {
        [TestMethod]
        public void GetCohortPatientsByID()
        {
            string controlValue = "Tony";
            string sampleValue;
            string cohortID = "52781cd8fe7a5925fcee5bf3";
            string contractNumber = "InHealth001";
            string context ="NG";
            IRestClient client = new JsonServiceClient();

            //JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIKey:{0}", "12345"));

            GetCohortPatientsDataResponse response = client.Post<GetCohortPatientsDataResponse>("http://localhost:8888/NG/data/CohortPatients",
                new GetCohortPatientsDataRequest { CohortID = cohortID, ContractNumber = contractNumber, Context = context } as object);

            sampleValue = string.Empty;

            Assert.AreEqual(controlValue, sampleValue);
        }

        [TestMethod]
        public void Get_CohortPatientsList_Test()
        {
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string cohortID = "528aa048d4332317acc50977";

            IRestClient client = new JsonServiceClient();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            GetCohortPatientsDataResponse response =
                client.Get<GetCohortPatientsDataResponse>(string.Format("{0}/{1}/{2}/{3}/CohortPatients/{4}?Skip=0&Take=5000",
                "http://azurephytel.cloudapp.net:59901/CohortPatients", context, version, contractNumber, cohortID));

            //CohortPatientDetailsResponse response =
            //    client.Get<CohortPatientDetailsResponse>(string.Format("{0}/{1}/{2}/{3}/CohortPatients/{4}?Skip=0&Take=1000",
            //    "http://localhost:8888/CohortPatients", context, version, contractNumber, cohortID));

            sw.Stop();
            string elapsed = sw.Elapsed.ToString();
        }


        [TestMethod]
        public void Get_CohortPatientsListWithFilter_Test()
        {
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string cohortID = "528e4feb072ef713fc4f258d";
            string searchFilter = "Jul";

            IRestClient client = new JsonServiceClient();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            GetCohortPatientsDataResponse response =
                client.Get<GetCohortPatientsDataResponse>(string.Format("{0}/{1}/{2}/{3}/CohortPatients/{4}?Skip=0&Take=1000&SearchFilter={5}",
                "http://localhost:8888/CohortPatients", context, version, contractNumber, cohortID, searchFilter));

            sw.Stop();
            string elapsed = sw.Elapsed.ToString();
        }
    }
}
