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
            PatientNote note = new PatientNote {
                ContactedOn = DateTime.UtcNow,
                CreatedById = "5325c821072ef705080d3488",
                CreatedOn = DateTime.UtcNow,
                DurationId = "540f2181d4332319883f3ea5",
                MethodId = "540f1da7d4332319883f3e8c",
                OutcomeId = "540f1f10d4332319883f3e92",
                PatientId = "5325db71d6a4850adcbba94a",
                SourceId = "540f208ed4332319883f3e9b",
                 Text = "HEllo Hello",
                 TypeId = 2,
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
