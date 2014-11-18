using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Allergy.DTO;
using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Allergy.Test
{
    [TestClass]
    public class StubPatientAllergy_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        IPatientAllergyDataManager cm = new StubPatientAllergyDataManager();

       [TestMethod]
       public void GetPatientAllergies_Test()
       {
           GetPatientAllergiesDataRequest request = new GetPatientAllergiesDataRequest
           {
               Context = context,
               ContractNumber = contractNumber,
               PatientId = "5458fdef84ac050ea472df8e",
               UserId = userId,
               Version = version
           };
           List<PatientAllergyData> data = cm.GetPatientAllergies(request);

           Assert.IsNotNull(data.Count ==  2);
       }

        [TestMethod]
        public void InitializePatientAllergy_Test()
        {
            PutInitializePatientAllergyDataRequest request = new PutInitializePatientAllergyDataRequest {
                AllergyId = "54489a3dfe7a59146485bafe",
                Context = context,
                ContractNumber = contractNumber,
                PatientId = "54087f43d6a48509407d69cb",
                SystemName = "Engage",
                UserId = userId,
                Version = version
            };

            PatientAllergyData data = cm.InitializePatientAllergy(request);
            Assert.IsTrue(data.AllergyId == request.AllergyId);
        }

        [TestMethod]
        public void UpdatePatientAllergies_Test()
        {

            List<PatientAllergyData> data = new System.Collections.Generic.List<PatientAllergyData>();
            
            PatientAllergyData p1 = new PatientAllergyData
            {
                AllergyId = "54489a72fe7a59146485bce5",
                EndDate = DateTime.UtcNow,
                Id = "5452567ed433231b9c516d8e",
                Notes = "first note for patient allergy",
                PatientId = "54087f43d6a48509407d69cb",
                ReactionIds = new List<string> { "54494b5ad433232a446f7323", "54494b5dd433232a446f7324", "54494b60d433232a446f7325" },
                SeverityId = "54494a96d433232a446f7313",
                SourceId = "544e9976d433231d9c0330ae",
                StartDate = DateTime.UtcNow,
                StatusId = 1,
                SystemName = "Engage1",
                UpdatedOn = DateTime.UtcNow
            };
            data.Add(p1);
            PutPatientAllergiesDataRequest request = new PutPatientAllergiesDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                PatientAllergiesData = data,
                UserId = userId,
                Version = version
            };
            List<PatientAllergyData> response = cm.UpdatePatientAllergies(request);
            Assert.IsTrue(request.PatientAllergiesData.Count == response.Count);
        }

        [TestMethod]
        public void DeletePatientAllergies_Test()
        {
            DeleteAllergiesByPatientIdDataRequest request = new DeleteAllergiesByPatientIdDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                PatientId = "5458fdef84ac050ea472df8e",
                UserId = userId,
                Version = version
            };

            DeleteAllergiesByPatientIdDataResponse response = cm.DeletePatientAllergies(request);
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public void UndoDeletePatientAllergies_Test()
        {
            UndoDeletePatientAllergiesDataRequest request = new UndoDeletePatientAllergiesDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                Ids = new List<string> { "545920a184ac05124c984711", "5459271684ac05124ce6862a", "5459281584ac05124c362845", "54593ce684ac05124c3628cf" },
                UserId = userId,
                Version = version
            };

             UndoDeletePatientAllergiesDataResponse response = cm.UndoDeletePatientAllergies(request);
             Assert.IsTrue(response.Success);
        }
    }
}
