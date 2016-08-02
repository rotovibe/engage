using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Diagnostics;

namespace Phytel.API.DataDomain.Program.Services.Test
{
    [TestClass]
    public class Contract_Program_Test
    {
        [TestMethod]
        public void Get_AllActivePrograms()
        {
            string url = "http://localhost:8888/Program";
            string ProgramID = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context ="NG";
            double version = 1.0;
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetProgramResponse response = client.Get<GetProgramResponse>(
                string.Format("{0}/{1}/{2}/{3}/Programs/Active", url, context, version, contractNumber, ProgramID));

            //Assert.AreEqual(ProgramID, response.Program.ProgramID);
        }

        [TestMethod]
        public void Put_ContractProgramWithPatient()
        {
            string url = "http://localhost:8888/Program";
            string token = "5367fcf4d6a485044ccaf2f9";
            string patientID = "5325db09d6a4850adcbba812";
            string ContractProgramID = "5330920da38116ac180009d2";
            string userID = "5325c821072ef705080d3488";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;

            Stopwatch st = new Stopwatch();
            st.Start();

            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));
            string urls = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Programs/?ContractProgramId={5}",
                                                    url,
                                                    "NG",
                                                    version,
                                                    contractNumber,
                                                    patientID,
                                                    ContractProgramID), userID);

            PutProgramToPatientResponse dataDomainResponse =
                client.Put<PutProgramToPatientResponse>(urls, new PutProgramToPatientRequest { UserId = userID } as object);

            st.Stop();
            string time = st.ElapsedMilliseconds.ToString();

            Assert.AreEqual(dataDomainResponse.Outcome.Result, 0);
            //Assert.AreEqual(ProgramID, response.Program.ProgramID);
        }

        //[TestMethod]
        //public void Put_ContractProgramWithPatient()
        //{
        //    string url = "http://localhost:8888/Program";
        //    string patientID = "5325db50d6a4850adcbba8e6";
        //    string ContractProgramID = "5330920da38116ac180009d2";
        //    string contractNumber = "InHealth001";
        //    string context = "NG";
        //    double version = 1.0;

        //    Stopwatch st = new Stopwatch();
        //    st.Start();

        //    IRestClient client = new JsonServiceClient();
        //    string urls = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Programs/?ContractProgramId={5}",
        //        url,
        //        context,
        //        version,
        //        contractNumber,
        //        patientID,
        //        ContractProgramID), patientID);

        //    PutProgramToPatientResponse response = client.Put<PutProgramToPatientResponse>(
        //        urls, new PutProgramToPatientRequest() as object);

        //    st.Stop();
        //    string time = st.ElapsedMilliseconds.ToString();

        //    Assert.AreEqual(response.Outcome.Result, 0);
        //    //Assert.AreEqual(ProgramID, response.Program.ProgramID);
        //}

        [TestMethod]
        public void get_ContractProgramWithPatient()
        {
            string url = "http://localhost:8888/Program";
            string patientID = "52e26f3b072ef7191c117b21";
            string ContractProgramID = "52f17c781e60150accb7e9d3";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetContractProgramResponse response = client.Get<GetContractProgramResponse>(
                string.Format("{0}/{1}/{2}/{3}/ContractProgram/{4}/",
                url,
                context,
                version,
                contractNumber,
                ContractProgramID));

            //Assert.AreEqual(ProgramID, response.Program.ProgramID);
        }
    }
}
