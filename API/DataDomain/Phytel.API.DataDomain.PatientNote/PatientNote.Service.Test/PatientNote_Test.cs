using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientNote;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientNote.Service;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientNote.Services.Test
{
    [TestClass]
    public class PatientNote_Test
    {
        [TestMethod]
        public void InsertPatientNote_Test()
        {
            string url = "http://localhost:8888/PatientNote";
            PatientNoteData note = new PatientNoteData { Text = "DD_Service test note 2", CreatedById = "53043e53d433231f48de8a7a", PatientId = "52f55877072ef709f84e69b0" };
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Note/Insert", "PUT")]
            PutPatientNoteDataResponse response = client.Put<PutPatientNoteDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Note/Insert", url, context, version, contractNumber, note.PatientId),
                new PutPatientNoteDataRequest { Context = context, ContractNumber = contractNumber, PatientId = "52f55877072ef709f84e69b0", PatientNote = note, UserId = "53043e53d433231f48de8a7a", Version = version } as object);

            Assert.IsNotNull(response.Id);
        }

        [TestMethod]
        public void DeletePatientNoteByPatientId_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientId = "5325db70d6a4850adcbba946";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/PatientNote";
            IRestClient client = new JsonServiceClient();

            // [Route("/{Context}/{Version}/{ContractNumber}/PatientNote/Patient/{PatientId}/Delete", "DELETE")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientNote/Patient/{4}/Delete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber,
                                        patientId), userId);
            DeleteNoteByPatientIdDataResponse response = client.Delete<DeleteNoteByPatientIdDataResponse>(url);
            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void UndoDeletePatientNotes_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/PatientNote";
            IRestClient client = new JsonServiceClient();

            //[Route("/{Context}/{Version}/{ContractNumber}/PatientNote/UndoDelete", "PUT")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientNote/UndoDelete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber), userId);
            List<string> ids = new List<string>();
            ids.Add("53c5ab5cd6a48506ec660ad4");
            ids.Add("53c5ab59d6a48506ec660ad0");
            UndoDeletePatientNotesDataResponse response = client.Put<UndoDeletePatientNotesDataResponse>(url, new UndoDeletePatientNotesDataRequest 
            { 
                Ids = ids,
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            });
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void RemoveProgramInPatientNotes_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/PatientNote";
            string programId = "53cedd21d6a4850d58889ee7";
            IRestClient client = new JsonServiceClient();

            //[Route("/{Context}/{Version}/{ContractNumber}/Note/RemoveProgram/{ProgramId}/Update", "PUT")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Note/RemoveProgram/{4}/Update",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber, programId), userId);
            RemoveProgramInPatientNotesDataResponse response = client.Put<RemoveProgramInPatientNotesDataResponse>(url, new RemoveProgramInPatientNotesDataRequest
            {
                ProgramId = programId,
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            });
            Assert.IsNotNull(response);
        }
    }
}
