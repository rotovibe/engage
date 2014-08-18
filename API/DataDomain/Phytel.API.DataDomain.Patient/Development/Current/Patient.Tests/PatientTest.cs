using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void UpdatePatientBackground_Test()
        {
            PutPatientBackgroundDataRequest request = new PutPatientBackgroundDataRequest {  PatientId = "52f55899072ef709f84e7637", UserId = "bb241c64-a0ff-4e01-ba5f-4246ef50780e" };

            IPatientDataManager pm = new PatientDataManager();
            PutPatientBackgroundDataResponse response = pm.UpdatePatientBackground(request);

            Assert.IsTrue(response.Success);
        }


        [TestMethod]
        public void GetPatients()
        {
            string[] patientIds = new string[] { "5325da2dd6a4850adcbba576", "5325da31d6a4850adcbba582", "5325da3ad6a4850adcbba59a" };
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
                Id = "53f274e8d6a4851444cf7a9e",
                FirstName = "eric",
                LastName = "miles",
                DOB = "01/01/1942"
            };
            PutUpdatePatientDataRequest request = new PutUpdatePatientDataRequest {Insert = true, InsertDuplicate = true,  Context = context, ContractNumber = contractNumber, UserId = userId, Version = version, PatientData = data };
            IPatientDataManager pm = new PatientDataManager { Factory = new PatientRepositoryFactory() };
            PutUpdatePatientDataResponse response = pm.UpdatePatient(request);

            Assert.IsNotNull(response);
        }
    }
}
