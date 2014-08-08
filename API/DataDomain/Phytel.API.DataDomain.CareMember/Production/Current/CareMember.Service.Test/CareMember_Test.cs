using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.CareMember.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.CareMember.Services.Test
{
    [TestClass]
    public class User_CareMember_Test
    {
        [TestMethod]
        public void Post_CareMemberByID()
        {
            string url = "http://localhost:8888/Program";
            string ProgramID = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetCareMemberDataResponse response = client.Post<GetCareMemberDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/CareMember/{4}", url, context, version, contractNumber, ProgramID),
                new GetCareMemberDataResponse() as object);

            Assert.AreEqual(string.Empty, string.Empty);
        }



        [TestMethod]
        public void DeleteCareMemberByPatientId_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientId = "5325db70d6a4850adcbba946";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/CareMember";
            IRestClient client = new JsonServiceClient();

            // [Route("/{Context}/{Version}/{ContractNumber}/CareMember/Patient/{PatientId}/Delete", "DELETE")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/CareMember/Patient/{4}/Delete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber,
                                        patientId), userId);
            DeleteCareMemberByPatientIdDataResponse response = client.Delete<DeleteCareMemberByPatientIdDataResponse>(url);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UndoDeleteCareMembers_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/CareMember";
            IRestClient client = new JsonServiceClient();

            //[Route("/{Context}/{Version}/{ContractNumber}/CareMember/UndoDelete", "PUT")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/CareMember/UndoDelete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber), userId);
            List<string> ids = new List<string>();
            ids.Add("53c5a74dd433231880d25ae0");
            ids.Add("53c5a714d6a48506ecd6c13b");
            UndoDeleteCareMembersDataResponse response = client.Put<UndoDeleteCareMembersDataResponse>(url, new UndoDeleteCareMembersDataRequest
            {   
                Ids  = ids,
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
