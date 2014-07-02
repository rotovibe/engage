using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class DeletePatient
    {
        double version = 1.0;
        string contractNumber = "InHealth001";
        string context = "NG";
        string patientId = "5325db70d6a4850adcbba946";
        string userId = "000000000000000000000000";
        string ddUrl = "http://localhost:8888/Patient";
        IRestClient client = new JsonServiceClient();

        [TestMethod]
        public void DeletePatient_Test()
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{Id}/Delete", "DELETE")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Delete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber,
                                        patientId), userId);
            DeletePatientDataResponse response = client.Delete<DeletePatientDataResponse>(url);
            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void DeletePatientUser_Test()
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/PatientUser/Patient/{PatientId}/Delete", "DELETE")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientUser/Patient/{4}/Delete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber,
                                        patientId), userId);
            DeletePatientUserByPatientIdDataResponse response = client.Delete<DeletePatientUserByPatientIdDataResponse>(url);
            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void DeleteCohortPatientView_Test()
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/PatientUser/Patient/{PatientId}/Delete", "DELETE")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/CohortPatientView/Patient/{4}/Delete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber,
                                        patientId), userId);
            DeleteCohortPatientViewDataResponse response = client.Delete<DeleteCohortPatientViewDataResponse>(url);
            Assert.IsNotNull(response);
        }
    }
}
