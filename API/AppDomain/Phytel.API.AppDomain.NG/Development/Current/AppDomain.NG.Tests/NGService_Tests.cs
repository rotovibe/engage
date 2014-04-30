﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Service.Tests.Factories;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.Common.Audit;
using Phytel.API.AppDomain.NG.Test.Stubs;
using Phytel.API.Common.Format;

namespace Phytel.API.AppDomain.NG.Service.Tests
{
    [TestClass()]
    public class NGService_Tests
    {
        [TestMethod()]
        public void Post_Test()
        {
            ISecurityManager ism = SecurityManagerFactory.Get();
            INGManager ingm = NGManagerFactory.Get();

            NGService ngs = new NGService{ Security = ism, NGManager = ingm };

            GetPatientRequest request = new GetPatientRequest { ContractNumber="NG", PatientID="", Token="dsafgsdfgdafg", UserId="", Version= 1.0 };
            //((ServiceStack.ServiceInterface.Service)ngs).
            GetPatientResponse response = ngs.Get(request);

            Assert.Fail();
        }

        [TestClass()]
        public class GetPatientProgramDetailsRequest_Method
        {
            [TestMethod()]
            public void Get_Valid_Response_Test()
            {
                IAuditUtil audit = new StubAuditUtil();
                INGManager ngm = new StubNGManager();
                ISecurityManager sm = new StubSecurityManager();
                ICommonFormatterUtil cf = new StubCommonFormatterUtil();

                NGService ngs = new NGService { AuditUtil = audit, NGManager = ngm, Security = sm, CommonFormatterUtil = cf };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest { ContractNumber = "NG", PatientId = "", Token = "dsafgsdfgdafg", UserId = "", Version = 1.0 };
                GetPatientProgramDetailsSummaryResponse response = ngs.Get(request);

                Assert.IsNotNull(response);
            }

            [TestMethod()]
            public void Get_WithAttributes_Test()
            {
                IAuditUtil audit = new StubAuditUtil();
                INGManager ngm = new StubNGManager();
                ISecurityManager sm = new StubSecurityManager();
                ICommonFormatterUtil cf = new StubCommonFormatterUtil();

                NGService ngs = new NGService { AuditUtil = audit, NGManager = ngm, Security = sm, CommonFormatterUtil = cf };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest { ContractNumber = "NG", PatientId = "", Token = "dsafgsdfgdafg", UserId = "", Version = 1.0 };
                GetPatientProgramDetailsSummaryResponse response = ngs.Get(request);

                Assert.IsNotNull(response.Program.Attributes);
            }

            [TestMethod()]
            public void Get_With_Objectives_Test()
            {
                IAuditUtil audit = new StubAuditUtil();
                INGManager ngm = new StubNGManager();
                ISecurityManager sm = new StubSecurityManager();
                ICommonFormatterUtil cf = new StubCommonFormatterUtil();

                NGService ngs = new NGService { AuditUtil = audit, NGManager = ngm, Security = sm, CommonFormatterUtil = cf };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest { ContractNumber = "NG", PatientId = "", Token = "dsafgsdfgdafg", UserId = "", Version = 1.0 };
                GetPatientProgramDetailsSummaryResponse response = ngs.Get(request);

                Assert.IsNotNull(response.Program.Objectives);
            }

            [TestMethod()]
            public void Get_With_Module_Attributes_Test()
            {
                string desc = "BSHSI - Outreach & Enrollment";
                IAuditUtil audit = new StubAuditUtil();
                INGManager ngm = new StubNGManager();
                ISecurityManager sm = new StubSecurityManager();
                ICommonFormatterUtil cf = new StubCommonFormatterUtil();

                NGService ngs = new NGService { AuditUtil = audit, NGManager = ngm, Security = sm, CommonFormatterUtil = cf };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest { ContractNumber = "NG", PatientId = "", Token = "dsafgsdfgdafg", UserId = "", Version = 1.0 };
                GetPatientProgramDetailsSummaryResponse response = ngs.Get(request);

                Module module = response.Program.Modules.Find(m => m.SourceId == "532b5585a381168abe00042c");
                string mDesc = module.Description.Trim();
                Assert.AreEqual(desc, mDesc, true);
            }
        }
    }
}
