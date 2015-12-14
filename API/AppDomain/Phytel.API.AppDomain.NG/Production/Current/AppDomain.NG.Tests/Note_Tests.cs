using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Context;
using Phytel.API.AppDomain.NG.DTO.Note.Context;
using Phytel.API.AppDomain.NG.Notes;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class Note_Tests
    {
        [TestMethod]
        public void InsertPatientNote_Test()
        {
            PatientNote note = new PatientNote {
                ContactedOn = DateTime.UtcNow,
                CreatedById = "5325c821072ef705080d3488",
                CreatedOn = DateTime.UtcNow,
                Duration = 15,
                MethodId = "540f1da7d4332319883f3e8c",
                OutcomeId = "540f1f10d4332319883f3e92",
                PatientId = "5325db71d6a4850adcbba94a",
                SourceId = "540f208ed4332319883f3e9b",
                Text = "HEllo Hello",
                TypeId = "54909992d43323251c0a1dfd",
                ValidatedIdentity  = true,
                WhoId = "540f1fc7d4332319883f3e99"
            };
            
            PostPatientNoteRequest request = new PostPatientNoteRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "5325c821072ef705080d3488"; 
            request.Version = 1;
            request.PatientId = "5325db71d6a4850adcbba94a";
            request.Token = "5307bcf5d6a4850cd4abe0dd";
            request.Note = note;

            NotesManager nManager = new NotesManager();
            PostPatientNoteResponse response = nManager.InsertPatientNote(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetPaitentNote_Test()
        {
            GetPatientNoteRequest request = new GetPatientNoteRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "AD_TestHarness";
            request.Version = 1;
            request.Id = "5307b27fd433232ed88e5020";
            request.PatientId = "52f55877072ef709f84e69b0";
            request.UserId = "Snehal";

            NotesManager gManager = new NotesManager();
            PatientNote response = gManager.GetPatientNote(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetAllPatientNotes_Test()
        {
            var context = new ServiceContext
            {
                Contract = "InHealth001",
                UserId = "AD_TestHarness",
                Version = 1,
                Tag = new PatientNoteContext
                {
                    PatientId = "52f55877072ef709f84e69b0",
                    UserId = "Snehal",
                    Count = 10
                }
            };

            NotesManager gManager = new NotesManager();
            List<PatientNote> response = gManager.GetAllPatientNotes(context);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UpdatePatientNote_Test()
        {
            List<string> prog = new List<string>();
            prog.Add("558c756184ac0707d02f72c8");

            PatientNote data = new PatientNote
            {
                Id = "558c757284ac05114837dc38",
                PatientId = "5429d29984ac050c788bd34f",
                Text = "JJJJ",
                ProgramIds = prog,
                TypeId = "54909997d43323251c0a1dfe",
                MethodId = "540f1da7d4332319883f3e8c",
                WhoId = "540f1fc3d4332319883f3e97",
                OutcomeId = "540f1f14d4332319883f3e93",
                SourceId = "540f2091d4332319883f3e9c",
                Duration = 123,
                ValidatedIdentity = true,
                ContactedOn = DateTime.Now.AddDays(4)
            };

            UpdatePatientNoteRequest request = new UpdatePatientNoteRequest
            {
                ContractNumber = "InHealth001",
                UserId = "54909997d43323251c0a1dfe",
                Version = 1.0,
                PatientNote = data,
                Id = data.Id,
                PatientId = data.PatientId
            };

            string requestURL = string.Format("{0}/{1}/{2}/Patient/{3}/Note/{4}?UserId={5}", "http://localhost:888/Nightingale", request.Version, request.ContractNumber, data.PatientId, data.Id, request.UserId);
            //[Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Note/{Id}", "PUT")]
            IRestClient client = new JsonServiceClient();
            UpdatePatientNoteResponse response = client.Put<UpdatePatientNoteResponse>(requestURL, request);

            Assert.IsNotNull(response);
        }
    }
}
