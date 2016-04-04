using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.DataDomain.Medication.Test;
using DataDomain.Medication.Repo;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using Phytel.API.DataDomain.Medication.Test;

namespace Phytel.API.DataDomain.Medication.Test
{
    //[TestClass]
    public class Data_PatientMedication_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:8888/Medication";
        IRestClient client = new JsonServiceClient();

   //    [TestMethod]
   //    public void GetPatientAllergies_Test()
   //    {
   //        GetPatientAllergiesDataRequest request = new GetPatientAllergiesDataRequest
   //        {
   //            Context = context,
   //            ContractNumber = contractNumber,
   //            PatientId = "5458fdef84ac050ea472df8e",
   //            UserId = userId,
   //            Version = version
   //        };
   //        //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/{PatientId}", "GET")]
   //        GetPatientAllergiesDataResponse response = client.Post<GetPatientAllergiesDataResponse>(
   //string.Format("{0}/{1}/{2}/{3}/PatientAllergy/{4}", url, context, version, contractNumber, request.PatientId), request);

   //        Assert.IsNotNull(response);
   //    }

        [TestMethod]
        public void SavePatientMedSupp_Test()
        {
            PatientMedSuppData pms = new PatientMedSuppData
            {
                CategoryId  = 1,
                DeleteFlag = false,
                Dosage = "One",
                EndDate = DateTime.UtcNow,
                Form = "tablet",
                FreqHowOftenId = "545be059d43323224896663a",
                FreqQuantity = "Twice",
                FreqWhenId = "545be126d433232248966643",
                Id = "",
                Name = "Exilir",
                //NDCs = ,
                Notes = "This is my note",
                PatientId = "5325d9e9d6a4850adcbba4b1",
                //PharmClasses = ,
                PrescribedBy = "PCP",
                Reason = "This is my reason",
                Route = "Oral",
                //SigCode = ,
                SourceId = "544e9976d433231d9c0330ae",
                //StartDate = ,
                StatusId = 1,
                Strength = "90 mg",
                //SystemName = "Engage",
                TypeId = "545bdfa6d433232248966639",
            };


            PutPatientMedSuppDataRequest request = new PutPatientMedSuppDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                PatientMedSuppData  = pms,
                UserId = userId,
                Version = version
            };

            //[Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/Update", "PUT")]
            PutPatientMedSuppDataResponse response = client.Put<PutPatientMedSuppDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/PatientMedSupp/Update", url, context, version, contractNumber), request);
            Assert.IsNotNull(response);
        }

        //[TestMethod]
        //public void DeletePatientAllergies_Test()
        //{
        //    DeleteAllergiesByPatientIdDataRequest request = new DeleteAllergiesByPatientIdDataRequest
        //    {
        //        Context = context,
        //        ContractNumber = contractNumber,
        //        PatientId = "5458fdef84ac050ea472df8e",
        //        UserId = userId,
        //        Version = version
        //    };

        //    //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/Patient/{PatientId}/Delete", "DELETE")]
        //    DeleteAllergiesByPatientIdDataResponse response = client.Delete<DeleteAllergiesByPatientIdDataResponse>(
        //        string.Format("{0}/{1}/{2}/{3}/PatientAllergy/Patient/{4}/Delete?UserId={5}", url, context, version, contractNumber, request.PatientId, request.UserId));
        //    Assert.IsNotNull(response);
        //}

        //[TestMethod]
        //public void UndoDeletePatientAllergies_Test()
        //{
        //    UndoDeletePatientAllergiesDataRequest request = new UndoDeletePatientAllergiesDataRequest
        //    {
        //        Context = context,
        //        ContractNumber = contractNumber,
        //        Ids = new List<string> { "545920a184ac05124c984711", "5459271684ac05124ce6862a", "5459281584ac05124c362845", "54593ce684ac05124c3628cf" },
        //        UserId = userId,
        //        Version = version
        //    };

        //    //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/UndoDelete", "PUT")]
        //    UndoDeletePatientAllergiesDataResponse response = client.Put<UndoDeletePatientAllergiesDataResponse>(
        //        string.Format("{0}/{1}/{2}/{3}/PatientAllergy/UndoDelete", url, context, version, contractNumber), request);
        //    Assert.IsNotNull(response);
        //}

        [TestMethod]
        public void DeletePatientMedSupp_Test()
        {
            DeletePatientMedSuppDataRequest request = new DeletePatientMedSuppDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                Id = "54ef4bac84ac0711a867ccde",
                UserId = userId,
                Version = version
            };

            //[Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/{Id}", "DELETE")]
            DeletePatientMedSuppDataResponse response = client.Delete<DeletePatientMedSuppDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/PatientMedSupp/{4}?UserId={5}", url, context, version, contractNumber, request.Id, request.UserId));
            Assert.IsNotNull(response);
        }
    }
}
