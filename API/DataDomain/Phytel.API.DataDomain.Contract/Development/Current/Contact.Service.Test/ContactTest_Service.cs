using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Contract.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Contract.Services.Test
{
    [TestClass]
    public class ContractTest_Service
    {
        [TestMethod]
        public void Get_Contract_By_Id_Response_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string ContractId = "5325c821072ef705080d3488";

            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", ContractId));

            GetContractByContractIdDataResponse response =
                client.Get<GetContractByContractIdDataResponse>(string.Format("{0}/{1}/{2}/{3}/Contract/{4}/?UserId={5}",
                "http://localhost:8888/Contract",
                context,
                version,
                contractNumber,
                ContractId,
                ContractId));
        }

        [TestMethod]
        public void DeleteContractByPatientId_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientId = "5325da9ed6a4850adcbba6ce";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/Contract";
            IRestClient client = new JsonServiceClient();

            // [Route("/{Context}/{Version}/{ContractNumber}/Contract/Patient/{PatientId}/Delete", "DELETE")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contract/Patient/{4}/Delete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber,
                                        patientId), userId);
            DeleteContractByPatientIdDataResponse response = client.Delete<DeleteContractByPatientIdDataResponse>(url);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UndoDeleteContractByPatientId_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/Contract";
            IRestClient client = new JsonServiceClient();

            //[Route("/{Context}/{Version}/{ContractNumber}/Contract/UndoDelete", "PUT")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contract/UndoDelete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber), userId);
            List<ContractWithUpdatedRecentList> list = new List<ContractWithUpdatedRecentList>();
            list.Add(new ContractWithUpdatedRecentList { ContractId  = "5325c81f072ef705080d347e", PatientIndex = 1 } );
            list.Add(new ContractWithUpdatedRecentList { ContractId  = "5325c821072ef705080d3488", PatientIndex = 3 } );
            UndoDeleteContractDataResponse response = client.Put<UndoDeleteContractDataResponse>(url, new UndoDeleteContractDataRequest { 
                Id = "5325da9fd6a4850adc046d1a",
                ContractWithUpdatedRecentLists = null, 
                Context = context,
                ContractNumber = contractNumber,
                Version  = version,
                UserId = userId
            });
            Assert.IsNotNull(response);
        }
    }
}
