using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.Common;
using Phytel.API.DataDomain.Patient.DTO;

namespace Phytel.API.DataDomain.Patient.Test
{
    [TestClass]
    public class PatientTest
    {
        static string userId = "5325c821072ef705080d3488";
        static string contractNumber = "InHealth001";
        static string context = "NG";
        static int version = 1;
        
        [TestMethod]
        public void GetPatientByID()
        {
            GetPatientDataRequest request = new GetPatientDataRequest { PatientID = "531f2dcc072ef727c4d29e1a" };

            IPatientDataManager pm = new PatientDataManager();
            GetPatientDataResponse response = pm.GetPatientByID(request);

            Assert.IsTrue(response.Patient.FirstName == "Phyliss");
        }

        [TestMethod]
        public void GetPatientSSN()
        {
            GetPatientSSNDataRequest request = new GetPatientSSNDataRequest { PatientId = "531f2dce072ef727c4d2a065", UserId = "531f2df6072ef727c4d2a3c0" };

            IPatientDataManager pm = new PatientDataManager();
            GetPatientSSNDataResponse response = pm.GetPatientSSN(request);

            Assert.IsNotNull(response.SSN);
        }

        [TestMethod]
        public void GetPatients()
        {
            List<string> patientIds = new List<string>();
            patientIds.Add("5325da2dd6a4850adcbba576");
            patientIds.Add("5325da31d6a4850adcbba582");
            patientIds.Add("5325da3ad6a4850adcbba59a");
            GetPatientsDataRequest request = new GetPatientsDataRequest { PatientIds = patientIds };

            IPatientDataManager pm = new PatientDataManager { Factory = new PatientRepositoryFactory() };
            GetPatientsDataResponse response = pm.GetPatients(request);

            Assert.IsNotNull(response.Patients);
        }

        [TestMethod]
        public void InitializePatient_Test()
        {
            PutInitializePatientDataRequest request = new PutInitializePatientDataRequest {  Context = context, ContractNumber = contractNumber, UserId = userId, Version = version};
            IPatientDataManager pm = new PatientDataManager { Factory = new PatientRepositoryFactory() };
            PutInitializePatientDataResponse response = pm.InitializePatient(request);

            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void SavePatient_Test()
        {
            PatientData data = new PatientData 
            {
                Id = "53f3c367d6a48508586bbace",
                FirstName = "Ellaha",
                LastName = "Bullock"
            };
            PutUpdatePatientDataRequest request = new PutUpdatePatientDataRequest {Insert = true, Context = context, ContractNumber = contractNumber, UserId = userId, Version = version, PatientData = data };
            IPatientDataManager pm = new PatientDataManager { Factory = new PatientRepositoryFactory() };
            PutUpdatePatientDataResponse response = pm.UpdatePatient(request);

            Assert.IsNotNull(response);
        }

        //[TestMethod]
        //public void InsertBatchPatients_Test()
        //{
        //    List<PatientData> lPsd = (List<PatientData>)Helpers.DeserializeObject<List<PatientData>>("PatientsExample.txt");

        //    InsertBatchPatientsDataRequest request = new InsertBatchPatientsDataRequest
        //    {
        //        Context = context,
        //        ContractNumber = contractNumber,
        //        UserId = userId,
        //        Version = version,
        //        PatientsData = lPsd
        //    };
        //    IPatientDataManager pm = new PatientDataManager { Factory = new PatientRepositoryFactory() };
        //    InsertBatchPatientsDataResponse response = pm.InsertBatchPatients(request);
            
        //    Assert.IsNotNull(response);
        //}
    }
}
