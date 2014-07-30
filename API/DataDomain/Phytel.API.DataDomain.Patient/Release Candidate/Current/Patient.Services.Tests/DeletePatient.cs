using System;
using System.Collections.Generic;
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
        string patientId = "5325db68d6a4850adcbba92e";
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


        [TestMethod]
        public void UndoDeletePatient_Test()
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/Patient/UndoDelete", "PUT")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/UndoDelete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber), userId);
            UndoDeletePatientDataResponse response = client.Put<UndoDeletePatientDataResponse>(url, new UndoDeletePatientDataRequest 
            { 
                Id = "5325db68d6a4850adcbba92e", Context = context, ContractNumber = contractNumber, UserId = userId, Version = version 
            }
            as object);
            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void UndoDeletePatientUser_Test()
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/PatientUser/UndoDelete", "PUT")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientUser/UndoDelete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber), userId);
            List<String> ids = new List<string>();
            ids.Add("53c453bdd6a48506ec180428");
            ids.Add("53c450bdd6a48506ec18039b");
            UndoDeletePatientUsersDataResponse response = client.Put<UndoDeletePatientUsersDataResponse>(url, new UndoDeletePatientUsersDataRequest
            {
                PatientUserId = null,
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            } as object);
            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void UndoDeleteCohortPatientView_Test()
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/CohortPatientView/UndoDelete", "PUT")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/CohortPatientView/UndoDelete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber), userId);
            UndoDeleteCohortPatientViewDataResponse response = client.Put<UndoDeleteCohortPatientViewDataResponse>(url, new UndoDeleteCohortPatientViewDataRequest
            {
                Id = "5325db68d6a4850adcbba92f",
                 Context = context,
                  ContractNumber = contractNumber,
                   UserId = userId,
                    Version = version
            }
            as object);
            Assert.IsNotNull(response);
        }
    }
}
