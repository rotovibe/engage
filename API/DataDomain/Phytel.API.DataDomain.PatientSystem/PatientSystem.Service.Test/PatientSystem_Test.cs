using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientSystem.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientSystem.Services.Test
{
    [TestClass]
    public class User_Services_Test
    {
        [TestMethod]
        public void GetPatientSystems()
        {
            string controlValue = "Tony";
            string sampleValue;
            string patientID = "52781cd8fe7a5925fcee5bf3";
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetAllPatientSystemsDataResponse response = client.Post<GetAllPatientSystemsDataResponse>("http://localhost:8888/NG/data/PatientSystem",
                new GetAllPatientSystemsDataRequest { PatientID = patientID, ContractNumber = contractNumber, Context = context, Version = 1 } as object);

            sampleValue = string.Empty;

            Assert.AreEqual(controlValue, sampleValue);
        }

        [TestMethod]
        public void GetPatientSystemByID()
        {
            string controlValue = "Tony";
            string sampleValue;
            string patientSystemID  = "52781cd8fe7a5925fcee5bf3";
            string contractNumber = "InHealth001";
            string context ="NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetPatientSystemDataResponse response = client.Post<GetPatientSystemDataResponse>("http://localhost:8888/NG/data/PatientSystem",
                new GetPatientSystemDataRequest { PatientSystemID = patientSystemID, ContractNumber = contractNumber, Context = context, Version = 1 } as object);

            sampleValue = string.Empty;

            Assert.AreEqual(controlValue, sampleValue);
        }

        [TestMethod]
        public void DeletePatientSystemByPatientId_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientId = "5325db70d6a4850adcbba946";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/PatientSystem";
            IRestClient client = new JsonServiceClient();

            // [Route("/{Context}/{Version}/{ContractNumber}/PatientSystem/Patient/{PatientId}/Delete", "DELETE")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientSystem/Patient/{4}/Delete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber,
                                        patientId), userId);
            DeletePatientSystemByPatientIdDataResponse response = client.Delete<DeletePatientSystemByPatientIdDataResponse>(url);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UndoDeletePatientSystems_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/PatientSystem";
            IRestClient client = new JsonServiceClient();

            //[Route("/{Context}/{Version}/{ContractNumber}/PatientSystem/UndoDelete", "Put")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientSystem/UndoDelete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber), userId);
            List<string> ids = new List<string>();
            ids.Add("53c5af9ad433231880d25ae2");
            ids.Add("53c5af8cd433231880d25ae1");
            UndoDeletePatientSystemsDataResponse response = client.Put<UndoDeletePatientSystemsDataResponse>(url, new UndoDeletePatientSystemsDataRequest
            {
                Ids = ids,
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            }
            );
            Assert.IsNotNull(response);
        }
    }
}
