using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.ProgramDesign.DTO;
using MongoDB.Bson;


namespace Phytel.API.DataDomain.ProgramDesign.Test
{
    [TestClass]
    public class ProgramDesignTest
    {
        string contractNumber = "InHealth001";
        //private ProgramDesignDataManager pm;

        [TestMethod]
        public void GetProgramDesignByID()
        {
            GetProgramDesignRequest request = new GetProgramDesignRequest{ ProgramDesignID = "5"};

            GetProgramDesignResponse response = ProgramDesignDataManager.GetProgramDesignByID(request);

            Assert.IsTrue(response.ProgramDesign.ProgramDesignID == "??");
        }

        [TestMethod]
        public void GetProgramByID()
        {
            GetProgramRequest request = new GetProgramRequest { ProgramID = "537260915a4d13161889a293" };

            GetProgramResponse response = ProgramDesignDataManager.GetProgramByID(request);

            Assert.IsTrue(response.Program.ProgramID == "537260915a4d13161889a293");
        }

        [TestMethod]
        public void GetModuleByID()
        {
            GetModuleRequest request = new GetModuleRequest { ModuleID = "53726dfa5a4d131618d2544c" };

            GetModuleResponse response = ProgramDesignDataManager.GetModuleByID(request);

            Assert.IsTrue(response.Module.Id == "53726dfa5a4d131618d2544c");
        }

        [TestMethod]
        public void GetActionByID()
        {
            GetActionDataRequest request = new GetActionDataRequest { ActionID = "537270205a4d1316184d6ac5" };

            GetActionDataResponse response = ProgramDesignDataManager.GetActionByID(request);

            Assert.IsTrue(response.Action.ID == "537270205a4d1316184d6ac5");
        }

        [TestMethod]
        public void GetStepByID()
        {
            GetStepDataRequest request = new GetStepDataRequest { StepID = "53727a675a4d1316182bf23d" };

            GetStepDataResponse response = ProgramDesignDataManager.GetStepByID(request);

            Assert.IsTrue(response.Step.ID == "53727a675a4d1316182bf23d");
        }

        [TestMethod]
        public void PutProgram()
        {
            PutProgramDataRequest request = new PutProgramDataRequest 
            { 
                Name = "programdesign.test", 
                UserId = "531f2df9072ef727c4d2a3df", 
                ContractNumber = contractNumber 
            };

            PutProgramDataResponse response = ProgramDesignDataManager.InsertProgram(request);

            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));

        }

        [TestMethod]
        public void PutModule()
        {
            PutModuleDataRequest request = new PutModuleDataRequest 
            { 
                Name = "programdesign.test", 
                UserId = "531f2df9072ef727c4d2a3df",
                ContractNumber = contractNumber
            };

            PutModuleDataResponse response = ProgramDesignDataManager.InsertModule(request);

            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));

        }

        [TestMethod]
        public void PutAction()
        {
            PutActionDataRequest request = new PutActionDataRequest 
            { 
                Name = "programdesign.test",
                UserId = "531f2df9072ef727c4d2a3df",
                ContractNumber = contractNumber
            };

            PutActionDataResponse response = ProgramDesignDataManager.InsertAction(request);

            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));

        }

        [TestMethod]
        public void PutStep()
        {
            PutStepDataRequest request = new PutStepDataRequest 
            { 
                Title = "programdesign.test",
                UserId = "531f2df9072ef727c4d2a3df",
                ContractNumber = contractNumber
            };

            PutStepDataResponse response = ProgramDesignDataManager.InsertStep(request);

            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));

        }

        [TestMethod]
        public void UpdateProgram()
        {
            PutUpdateProgramDataRequest request = new PutUpdateProgramDataRequest
            {
                Id = "537283e05a4d1318ac23303a",
                Description = "programdesign.test",
                UserId = "531f2df9072ef727c4d2a3df",
                ContractNumber = contractNumber
            };

            PutUpdateProgramDataResponse response = ProgramDesignDataManager.UpdateProgram(request);

            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));

        }

        [TestMethod]
        public void UpdateModule()
        {
            PutUpdateModuleDataRequest request = new PutUpdateModuleDataRequest
            {
                Id = "537283ff5a4d13160c9ca1b3",
                Description = "programdesign.test",
                UserId = "531f2df9072ef727c4d2a3df",
                ContractNumber = contractNumber
            };

            PutUpdateModuleDataResponse response = ProgramDesignDataManager.UpdateModule(request);

            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));

        }

        [TestMethod]
        public void UpdateAction()
        {
            PutUpdateActionDataRequest request = new PutUpdateActionDataRequest
            {
                ActionId = "537284295a4d130550fe252f",
                Description = "programdesign.test",
                UserId = "531f2df9072ef727c4d2a3df",
                ContractNumber = contractNumber
            };

            PutUpdateActionDataResponse response = ProgramDesignDataManager.UpdateAction(request);

            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));

        }

        [TestMethod]
        public void UpdateStep()
        {
            PutUpdateStepDataRequest request = new PutUpdateStepDataRequest
            {
                StepId = "537284a85a4d13157cc97772",
                Text = "programdesign.test",
                UserId = "531f2df9072ef727c4d2a3df",
                ContractNumber = contractNumber
            };

            PutUpdateStepDataResponse response = ProgramDesignDataManager.UpdateStep(request);

            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));

        }

        [TestMethod]
        public void AddModuleToProgram()
        {
            PutModuleInProgramRequest request = new PutModuleInProgramRequest
            {
                ProgramId = "537283e05a4d1318ac23303a",
                ModuleId = "537283ff5a4d13160c9ca1b3",
                UserId = "531f2df9072ef727c4d2a3df",
                ContractNumber = contractNumber
            };

            PutModuleInProgramResponse response = ProgramDesignDataManager.AddModuleToProgram(request);

            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void AddActionToModule()
        {
            PutActionInModuleRequest request = new PutActionInModuleRequest
            {
                ModuleId = "537283ff5a4d13160c9ca1b3",
                ActionId = "537284295a4d130550fe252f",
                UserId = "531f2df9072ef727c4d2a3df",
                ContractNumber = contractNumber
            };

            PutActionInModuleResponse response = ProgramDesignDataManager.AddActionToModule(request);

            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void AddStepToAction()
        {
            PutStepInActionRequest request = new PutStepInActionRequest
            {
                ActionId = "537284295a4d130550fe252f",
                StepId = "537284a85a4d13157cc97772",
                UserId = "531f2df9072ef727c4d2a3df",
                ContractNumber = contractNumber
            };

            PutStepInActionResponse response = ProgramDesignDataManager.AddStepToAction(request);

            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void DeleteProgram()
        {
            DeleteProgramDataRequest request = new DeleteProgramDataRequest
            {
                ProgramId = "537283e05a4d1318ac23303a",
                UserId = "531f2df9072ef727c4d2a3df",
                ContractNumber = contractNumber
            };

            DeleteProgramDataResponse response = ProgramDesignDataManager.DeleteProgram(request);

            Assert.IsTrue(response.Deleted);
        }

        [TestMethod]
        public void DeleteModule()
        {
            DeleteModuleDataRequest request = new DeleteModuleDataRequest
            {
                ModuleId = "537283ff5a4d13160c9ca1b3",
                UserId = "531f2df9072ef727c4d2a3df",
                ContractNumber = contractNumber
            };

            DeleteModuleDataResponse response = ProgramDesignDataManager.DeleteModule(request);

            Assert.IsTrue(response.Deleted);
        }

        [TestMethod]
        public void DeleteAction()
        {
            DeleteActionDataRequest request = new DeleteActionDataRequest
            {
                ActionId = "537284295a4d130550fe252f",
                UserId = "531f2df9072ef727c4d2a3df",
                ContractNumber = contractNumber
            };

            DeleteActionDataResponse response = ProgramDesignDataManager.DeleteAction(request);

            Assert.IsTrue(response.Deleted);
        }

        [TestMethod]
        public void DeleteStep()
        {
            DeleteStepDataRequest request = new DeleteStepDataRequest
            {
                StepId = "537284a85a4d13157cc97772",
                UserId = "531f2df9072ef727c4d2a3df",
                ContractNumber = contractNumber
            };

            DeleteStepDataResponse response = ProgramDesignDataManager.DeleteStep(request);

            Assert.IsTrue(response.Deleted);
        }
    }
}
