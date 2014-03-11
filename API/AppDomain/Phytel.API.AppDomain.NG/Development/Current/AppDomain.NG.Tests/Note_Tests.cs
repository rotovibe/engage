using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class Note_Tests
    {
        [TestMethod]
        public void InsertPatientNote_Test()
        {
            List<string> pgIds = new List<string>();
            pgIds.Add("52f927abd6a4850e5c0f038b");
            PatientNote note = new PatientNote { 
                Id = "-2",
                Text = "AD Patrick test",
                CreatedById = "bb241c64a0ff4e01ba5f4246ef50780e",
                PatientId = "52f5589b072ef709f84e7706",
                ProgramIds = pgIds
            };
            
            PostPatientNoteRequest request = new PostPatientNoteRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "AD_TestHarness"; 
            request.Version = 1;
            request.PatientId = "52f5589b072ef709f84e7706";
            request.Token = "5307bcf5d6a4850cd4abe0dd";
            request.Note = note;

            NotesManager nManager = new NotesManager();
            PostPatientNoteResponse response = nManager.InsertPatientNote(request, note.CreatedById);

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
            GetAllPatientNotesRequest request = new GetAllPatientNotesRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "AD_TestHarness";
            request.Version = 1;
            request.PatientId = "52f55877072ef709f84e69b0";
            request.UserId = "Snehal";
            request.Count = 10;

            NotesManager gManager = new NotesManager();
            List<PatientNote> response = gManager.GetAllPatientNotes(request);

            Assert.IsNotNull(response);
        }

        //[TestMethod]
        //public void SavePatientGoal_Test()
        //{
        //    PostPatientGoalRequest request = new PostPatientGoalRequest();
        //    request.ContractNumber = "InHealth001";
        //    request.UserId = "AD_TestHarness";
        //    request.Version = 1;
        //    request.PatientId = "52f55874072ef709f84e68c5";
        //    request.UserId = "Snehal";
        //    request.Goal = new PatientGoal { Name =  "my name", SourceId = "my source"};

        //    GoalsManager gManager = new GoalsManager();
        //    PostPatientGoalResponse response = gManager.SavePatientGoal(request);

        //    Assert.IsNotNull(response);
        //}


        
        //[TestMethod]
        //public void InitializePatientGoal_Test()
        //{
        //    GetInitializeGoalRequest request = new GetInitializeGoalRequest();
        //    request.ContractNumber = "InHealth001";
        //    request.UserId = "AD_TestHarness";
        //    request.Version = 1;
        //    request.PatientId = "52f55877072ef709f84e69b0";
        //    request.UserId = "Snehal";

        //    GoalsManager gManager = new GoalsManager();
        //    GetInitializeGoalResponse response = gManager.GetInitialGoalRequest(request);

        //    Assert.IsNotNull(response);
        //}
    }
}
