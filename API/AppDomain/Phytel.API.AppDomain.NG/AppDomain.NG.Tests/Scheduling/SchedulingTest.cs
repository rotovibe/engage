using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Test.Stubs;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Test.Scheduling
{
    /// <summary>
    /// This class tests all the methods that are related to ToDo domain in INGManager class.
    /// </summary>
    [TestClass]
    public class SchedulingTest
    {
        string userId = "000000000000000000000000";
        string contractNumber = "InHealth001";
        double version = 1.0;
        string context = "NG";

        [TestMethod()]
        public void GetToDosTest()
        {
            SchedulingManager ngm = new SchedulingManager();

            GetToDosRequest request = new GetToDosRequest
            {
                Version = version,
                ContractNumber = contractNumber,
                UserId = userId,
                AssignedToId = "5325c821072ef705080d3488",
               // FromDate = DateTime.Parse("9/1/2014 3:47:01 PM"),
                StatusIds = new List<int> { 1, 3}
            };

            GetToDosResponse response = ngm.GetToDos(request);
            List<AppDomain.NG.DTO.ToDo> list = response.ToDos;
            Assert.IsNotNull(list);
        }

        [TestMethod()]
        public void InsertToDoTest()
        {
            SchedulingManager ngm = new SchedulingManager();

           PostInsertToDoRequest request = new PostInsertToDoRequest
            {
                Version = version,
                ContractNumber = contractNumber,
                UserId = userId,
                ToDo = new AppDomain.NG.DTO.ToDo
                {
                    AssignedToId = "5325c821072ef705080d3489",
                    CategoryId = "53f7964cd433231edc7d24ce",
                    Description = "app domain desc 2",
                    DueDate = DateTime.UtcNow,
                    PatientId = "5325db87d6a4850adcbba986",
                    PriorityId = 0,
                    ProgramIds = new List<string>{"532caae9a38116ac18000846"},
                    Title = "app domain title 2",
                }
            };

            PostInsertToDoResponse response = ngm.InsertToDo(request);
            Assert.IsNotNull(response);
        }
    }
}
