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
            string patientId = "";//"5325db20d6a4850adcbba84e";
            List<int> statusIds = new System.Collections.Generic.List<int> { 1 };

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));
            //[Route("/{Version}/{ContractNumber}/ToDo/AssignedTo/{AssignedToId}/Patient/{PatientId}/Status/{StatusIds}", "GET")]
            IRestClient client = new JsonServiceClient();
            //[Route("/{Version}/{ContractNumber}/Contact/{ContactId}/RecentPatients", "GET")]
            string url = string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Scheduling/ToDos",
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

        [TestMethod]
        public void InsertToDos_Test()
        {
            string contractNumber = "InHealth001";
            double version = 1.0;
            string token = "53fb6f20d6a4851b0ce14611";

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));
            IRestClient client = new JsonServiceClient();
            string url = string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Scheduling/ToDo/Insert",
                version,
                contractNumber);

            ToDo data = new ToDo {
                AssignedToId = "5325c821072ef705080d3488",
                CategoryId = "53fb585dd4332324207eddc6",
                Description = "fourth todo insert desc",
                DueDate = DateTime.UtcNow,
                PatientId = "53ed25aed433232e6c26182c",
                PriorityId = 2,
                Title = "fourth todo insert title"
            };
            PostInsertToDoResponse response = client.Post<PostInsertToDoResponse>(
                url, new PostInsertToDoRequest
                {
                    ContractNumber = contractNumber,
                    Token = token,
                    UserId = "5325c821072ef705080d3488",
                    Version = version,
                    ToDo = data
                } as object);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UpdateToDos_Test()
        {
            string contractNumber = "InHealth001";
            double version = 1.0;
            string token = "53fb6f20d6a4851b0ce14611";

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));
            IRestClient client = new JsonServiceClient();
            string url = string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Scheduling/ToDo/Update",
                version,
                contractNumber);

            ToDo data = new ToDo
            {
                Id = "53fba66ad433230f9813504e",
                AssignedToId = "5325c821072ef705080d3488",
                CategoryId = "53fb585dd4332324207eddc6",
                Description = "Edited fourth todo insert desc",
                DueDate = DateTime.UtcNow,
                PatientId = "",
                PriorityId = 3,
                Title = "Edited fourth todo insert title",
                StatusId = 2
            };
            PostInsertToDoResponse response = client.Post<PostInsertToDoResponse>(
                url, new PostInsertToDoRequest
                {
                    ContractNumber = contractNumber,
                    Token = token,
                    UserId = "5325c821072ef705080d3488",
                    Version = version,
                    ToDo = data
                } as object);

            Assert.IsNotNull(response);
        }
    }
}
