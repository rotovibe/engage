﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.Test.Stubs;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.DataDomain.Program.Service.Tests
{
    [TestClass()]
    public class ProgramService_Tests
    {

        [TestMethod()]
        public void GET_GetProgramDetailsSummaryResponse_Test()
        {
            ProgramService ps = new ProgramService
            {
                ProgramDataManager = new StubProgramDataManager(),
                Helpers = new StubHelper(),
                CommonFormatterUtil = new StubCommonFormatterUtil()
            };

            GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
            {
                Context = "NG",
                ContractNumber = "InHealth001",
                PatientId = "",
                ProgramId = "",
                UserId = "nguser",
                Version = 1.0
            };

            GetProgramDetailsSummaryResponse response =  ps.Get(request);
            Assert.IsNotNull(response.Program);
        }
    }
}
