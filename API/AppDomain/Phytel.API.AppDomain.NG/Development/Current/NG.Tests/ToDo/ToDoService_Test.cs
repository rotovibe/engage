using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Services.Test.Contact
{
    [TestClass]
    public class ToDoService_Test
    {
        [TestMethod]
        public void GetToDos_Test()
        {
            string contractNumber = "InHealth001";
            double version = 1.0;
            string token = "53fb6f20d6a4851b0ce14611";
            string assignedToId = "5325c821072ef705080d3488";
            string patientId = "5325db20d6a4850adcbba84e";
            List<int> statusIds = new System.Collections.Generic.List<int> { 1 };

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));
            //[Route("/{Version}/{ContractNumber}/ToDo/AssignedTo/{AssignedToId}/Patient/{PatientId}/Status/{StatusIds}", "GET")]
            IRestClient client = new JsonServiceClient();
            //[Route("/{Version}/{ContractNumber}/Contact/{ContactId}/RecentPatients", "GET")]
            string url = string.Format(@"http://localhost:888/Nightingale/{0}/{1}/ToDos",
                version,
                contractNumber,
                assignedToId,
                patientId,
                statusIds);
            GetToDosResponse response = client.Post<GetToDosResponse>(
                url, new GetToDosRequest
                {
                    AssignedToId = assignedToId,
                    ContractNumber = contractNumber,
                    PatientId = patientId,
                    StatusIds = statusIds,
                    Token = token,
                    UserId = "5325c821072ef705080d3488",
                    Version = version
                } as object);

            Assert.IsNotNull(response);
        }
    }
}
