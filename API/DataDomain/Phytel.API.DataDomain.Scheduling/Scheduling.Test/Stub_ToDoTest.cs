using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Scheduling.DTO;
using Phytel.API.DataDomain.Scheduling.Test.Stubs;
using Phytel.API.DataDomain.ToDo.Test.Stubs;

namespace Phytel.API.DataDomain.Scheduling.Test
{
    [TestClass]
    public class Stub_ToDoTest
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;


        [TestMethod]
        public void GetToDos_Test()
        {
            GetToDosDataRequest request = new GetToDosDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                StatusIds = new List<int> { 1, 3},
                CreatedById = userId
            };

            ISchedulingDataManager cm = new StubToDoDataManager { Factory = new StubSchedulingRepositoryFactory() };
            GetToDosDataResponse response = cm.GetToDos(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void InsertToDo_Test()
        {

            ToDoData data = new ToDoData
            {
                AssignedToId = "5325c822072ef705080d3491",
                CategoryId = "53f51afed4332316fcdbeba3",
                Description = "to do description 3.", 
                DueDate = System.DateTime.UtcNow,
                StartTime = System.DateTime.UtcNow,
                Duration = 20,
                Title = "to do title 3.",
                PatientId = "5325da01d6a4850adcbba4fa",
                PriorityId = 0,
                ProgramIds = new System.Collections.Generic.List<string> { "5330920da38116ac180009d2", "532caae9a38116ac18000846" },
                StatusId = (int)Status.Met,
                SourceId = "53f51afed4332316fcdbeba3",
                ClosedDate = DateTime.UtcNow
            };
            PutInsertToDoDataRequest request = new PutInsertToDoDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                ToDoData = data,
                UserId = userId,
                Version = version
            };

            ISchedulingDataManager cm = new StubToDoDataManager { Factory = new StubSchedulingRepositoryFactory() };
            PutInsertToDoDataResponse response = cm.InsertToDo(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UpdateToDo_Test()
        {

            ToDoData data = new ToDoData
            {
                Id = "5400b215d433231f60c94119",
                StatusId = (int)Status.Met,
                Description = "updated desc",
                Title = "updated title",
                DueDate = DateTime.UtcNow,
                StartTime = System.DateTime.UtcNow,
                Duration = 20,
                ClosedDate = DateTime.UtcNow
            };
            PutUpdateToDoDataRequest request = new PutUpdateToDoDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                ToDoData = data,
                UserId = userId,
                Version = version
            };

            ISchedulingDataManager cm = new StubToDoDataManager { Factory = new StubSchedulingRepositoryFactory() };
            PutUpdateToDoDataResponse response = cm.UpdateToDo(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void RemoveProgramInToDos_Test()
        {

            RemoveProgramInToDosDataRequest request = new RemoveProgramInToDosDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                ProgramId = "535ad70ad2d8e912e857525c",
                UserId = userId,
                Version = version
            };

            ISchedulingDataManager cm = new StubToDoDataManager { Factory = new StubSchedulingRepositoryFactory() };
            RemoveProgramInToDosDataResponse response = cm.RemoveProgramInToDos(request);

            Assert.IsNotNull(response);
        }
        
    }
}
