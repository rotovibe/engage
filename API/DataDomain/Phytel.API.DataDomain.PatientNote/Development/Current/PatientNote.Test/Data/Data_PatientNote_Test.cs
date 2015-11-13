using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientNote.Test;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientNote.Test
{
    [TestClass]
    public class Data_PatientNote_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "5325da92d6a4850adcbba6aa";
        double version = 1.0;
        string url = "http://localhost:8888/PatientNote";
        IRestClient client = new JsonServiceClient();

       [TestMethod]
       public void GetPatientNotes_Test()
       {
           GetAllPatientNotesDataRequest request = new GetAllPatientNotesDataRequest
           {
               Context = context,
               ContractNumber = contractNumber,
               PatientId = "5325da92d6a4850adcbba6aa",
               UserId = userId,
               Version = version
           };
           //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Notes/{Count}", "GET")]
           GetAllPatientNotesDataResponse response = client.Get<GetAllPatientNotesDataResponse>(
   string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Notes/{5}?UserId={6}", url, context, version, contractNumber, request.PatientId, 10, request.UserId));

           Assert.IsNotNull(response);
       }

       [TestMethod]
       public void UpdatePatientNote_Test()
       {
           List<string> prog = new List<string>();
           prog.Add("558c756184ac0707d02f72c8");
           
           PatientNoteData data = new PatientNoteData {
               Id = "558c757284ac05114837dc38",
               PatientId = "5429d29984ac050c788bd34f",
              Text = "ABCABC",
              ProgramIds = prog,
               TypeId = "54909997d43323251c0a1dfe",
               MethodId = "540f1da7d4332319883f3e8c",
               WhoId = "540f1fc3d4332319883f3e97",
               OutcomeId = "540f1f14d4332319883f3e93",
               SourceId = "540f2091d4332319883f3e9c",
               Duration = 10,
              ValidatedIdentity = false,
              ContactedOn = DateTime.Now.AddDays(4)
           };
           
           UpdatePatientNoteDataRequest request = new UpdatePatientNoteDataRequest
           {
               Context = context,
               ContractNumber = contractNumber,
               UserId = userId,
               Version = version,
               PatientNoteData = data,
               Id  = data.Id,
               PatientId  = data.PatientId
           };

           string requestURL = string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Note/{5}", url, context, version, contractNumber, data.PatientId, data.Id);
           //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Note/{Id}", "PUT")]
           UpdatePatientNoteDataResponse response = client.Put<UpdatePatientNoteDataResponse>(requestURL, request);

           Assert.IsNotNull(response);
       }

       [TestMethod]
       public void InsertPatientNote_Test()
       {
           List<string> prog = new List<string>();
           prog.Add("558c756184ac0707d02f72c8");

           PatientNoteData data = new PatientNoteData
           {
               PatientId = "5429d29984ac050c788bd34f",
               Text = "GGGG",
               ProgramIds = prog,
               TypeId = "54909997d43323251c0a1dfe",
               MethodId = "540f1da7d4332319883f3e8c",
               ValidatedIdentity = false,
               ContactedOn = DateTime.Now.AddDays(4)
           };

           InsertPatientNoteDataRequest request = new InsertPatientNoteDataRequest
           {
               Context = context,
               ContractNumber = contractNumber,
               UserId = userId,
               Version = version,
               PatientNote = data,
               PatientId = "5429d29984ac050c788bd34f",
           };

           string requestURL = string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Note", url, context, version, contractNumber, data.PatientId);
           //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Note", "POST")]
           InsertPatientNoteDataResponse response = client.Post<InsertPatientNoteDataResponse>(requestURL, request);

           Assert.IsNotNull(response);
       }
    }
}
