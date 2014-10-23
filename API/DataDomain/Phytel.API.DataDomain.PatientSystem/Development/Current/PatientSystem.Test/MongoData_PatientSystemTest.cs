using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.DataDomain.PatientSystem.Test
{
    public class MongoData_PatientSystemTest
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        
        public void GetPatientSystemByID()
        {
            GetPatientSystemDataRequest request = new GetPatientSystemDataRequest { PatientSystemID = "52f55899072ef709f84e763b" };
            IPatientSystemDataManager psM = new PatientSystemDataManager { Factory = new PatientSystemRepositoryFactory() };
            GetPatientSystemDataResponse response = psM.GetPatientSystem(request);

            Assert.IsTrue(response.PatientSystem.SystemId == "7890");
        }

        public void GetPatientSystemsByPatientId_Test()
        {
            GetPatientSystemsDataRequest request = new GetPatientSystemsDataRequest { PatientId = "5446d99f84ac051254108d69", Context = context , ContractNumber = contractNumber, UserId = userId, Version = version};
            IPatientSystemDataManager psM = new PatientSystemDataManager { Factory = new PatientSystemRepositoryFactory() };
            GetPatientSystemsDataResponse response = psM.GetPatientSystems(request);

            Assert.IsTrue(response.PatientSystems.Count > 0);
        }

        public void InsertPatientSystem_Test()
        {
            PutPatientSystemDataRequest request = new PutPatientSystemDataRequest {
                Context = context,
                ContractNumber = contractNumber,
                DisplayLabel = "ID",
                PatientID = "5446d99f84ac051254108d69",
                SystemID = "122345",
                SystemName = "Knoxville",
                UserId = userId,
                Version = version
                };
            IPatientSystemDataManager psM = new PatientSystemDataManager { Factory = new PatientSystemRepositoryFactory() };
            PutPatientSystemDataResponse response = psM.InsertPatientSystem(request);

            Assert.IsNotNull(response.PatientSystemId);
        }

        public void UpdatePatientSystem_Test()
        {
            PutUpdatePatientSystemDataRequest request = new PutUpdatePatientSystemDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                Id = "5446da60d433230a7c4b41ee",
               DisplayLabel = "ID",
               PatientID = "5446d99f84ac051254108d69",
               SystemID = "222",
               SystemName = "Lamar",
                UserId = userId,
                Version = version,
                DeleteFlag = false
            };
            IPatientSystemDataManager psM = new PatientSystemDataManager { Factory = new PatientSystemRepositoryFactory() };
            PutUpdatePatientSystemDataResponse response = psM.UpdatePatientSystem(request);

            Assert.IsNotNull(response);
        }

   }
}
