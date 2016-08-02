using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Scheduling.DTO;
using Phytel.API.DataDomain.Scheduling.Test.Stubs;

namespace Phytel.API.DataDomain.Scheduling.Test
{
    [TestClass]
    public class ToDoTest
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
               // AssignedToId = "5325c821072ef705080d348c",
                //PatientId  = "",
                StatusIds = new List<int> { 1, 3},
                CreatedById = "5325c821072ef705080d3488"
                //FromDate = DateTime.Parse("9/1/2014 5:36:54 PM")

            };

            ISchedulingDataManager cm = new SchedulingDataManager { Factory = new SchedulingRepositoryFactory() };
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
                StartTime = DateTime.UtcNow,
                Duration = 20,
                Title = "to do title 3.",
                PatientId = "5325da01d6a4850adcbba4fa",
                PriorityId = 0,
                ProgramIds = new System.Collections.Generic.List<string> { "5330920da38116ac180009d2", "532caae9a38116ac18000846" },
            };
            PutInsertToDoDataRequest request = new PutInsertToDoDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                ToDoData = data,
                UserId = userId,
                Version = version
            };

            ISchedulingDataManager cm = new SchedulingDataManager { Factory = new SchedulingRepositoryFactory() };
            PutInsertToDoDataResponse response = cm.InsertToDo(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UpdateToDo_Test()
        {

            ToDoData data = new ToDoData
            {
                Id = "5400b215d433231f60c94119",
                AssignedToId = "5325c821072ef705080d3488",
                //CategoryId = "53f51b0ed4332316fcdbeba4",
                //Description = "updated desc",
                //DueDate = System.DateTime.UtcNow,
                //StartTime = DateTime.UtcNow,
                //Duration = 20,
                Title = "this is my updated title",
               // PatientId = "5325d9efd6a4850adcbba4c2",
               // PriorityId = 3,
                StatusId = 1,
                //ProgramIds = new System.Collections.Generic.List<string> { "532caae9a38116ac18000846" },
                DeleteFlag = true
            };
            PutUpdateToDoDataRequest request = new PutUpdateToDoDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                ToDoData = data,
                UserId = userId,
                Version = version
            };

            ISchedulingDataManager cm = new SchedulingDataManager { Factory = new SchedulingRepositoryFactory() };
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
                ProgramId = "5400b12bd6a4850940b9f245",
                UserId = userId,
                Version = version
            };

            ISchedulingDataManager cm = new SchedulingDataManager { Factory = new SchedulingRepositoryFactory() };
            RemoveProgramInToDosDataResponse response = cm.RemoveProgramInToDos(request);

            Assert.IsNotNull(response);
        }
        
    }
}
