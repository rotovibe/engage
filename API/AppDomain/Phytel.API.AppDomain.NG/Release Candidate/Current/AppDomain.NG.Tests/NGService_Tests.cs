using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Service.Tests.Factories;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Notes;
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

            NGService ngs = new NGService {Security = ism, NGManager = ingm};

            GetPatientRequest request = new GetPatientRequest
            {
                ContractNumber = "NG",
                PatientID = "",
                Token = "dsafgsdfgdafg",
                UserId = "",
                Version = 1.0
            };
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

                NGService ngs = new NGService
                {
                    AuditUtil = audit,
                    NGManager = ngm,
                    Security = sm,
                    CommonFormatterUtil = cf
                };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    ContractNumber = "NG",
                    PatientId = "",
                    Token = "dsafgsdfgdafg",
                    UserId = "",
                    Version = 1.0
                };
                GetPatientProgramDetailsSummaryResponse response = ngs.Get(request);

                Assert.IsNotNull(response);
            }

            [TestMethod()]
            [TestCategory("NIGHT-832")]
            [TestProperty("TFS", "11159")]
            [TestProperty("Layer", "NGService")]
            public void Get_AssignById_Test()
            {
                IAuditUtil audit = new StubAuditUtil();
                INGManager ngm = new StubNGManager();
                ISecurityManager sm = new StubSecurityManager();
                ICommonFormatterUtil cf = new StubCommonFormatterUtil();
                string userid = "000000000000000000000000";

                NGService ngs = new NGService
                {
                    AuditUtil = audit,
                    NGManager = ngm,
                    Security = sm,
                    CommonFormatterUtil = cf
                };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    ContractNumber = "NG",
                    PatientId = "",
                    Token = "dsafgsdfgdafg",
                    UserId = userid,
                    Version = 1.0
                };

                GetPatientProgramDetailsSummaryResponse response = ngs.Get(request);
                string result = response.Program.AssignById;
                Assert.AreEqual(userid, result);
            }

            [TestMethod()]
            [TestCategory("NIGHT-831")]
            [TestProperty("TFS", "11172")]
            [TestProperty("Layer", "NGService")]
            public void Get_AssignDate_Test()
            {
                IAuditUtil audit = new StubAuditUtil();
                INGManager ngm = new StubNGManager();
                ISecurityManager sm = new StubSecurityManager();
                ICommonFormatterUtil cf = new StubCommonFormatterUtil();
                string userid = "000000000000000000000000";
                DateTime now = System.DateTime.UtcNow.Date;

                NGService ngs = new NGService
                {
                    AuditUtil = audit,
                    NGManager = ngm,
                    Security = sm,
                    CommonFormatterUtil = cf
                };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    ContractNumber = "NG",
                    PatientId = "",
                    Token = "dsafgsdfgdafg",
                    UserId = userid,
                    Version = 1.0
                };

                GetPatientProgramDetailsSummaryResponse response = ngs.Get(request);
                DateTime assignbyDate = ((DateTime) response.Program.AssignDate).Date;
                Assert.AreEqual(now, assignbyDate);
            }

            [TestMethod()]
            [TestCategory("NIGHT-868")]
            [TestProperty("TFS", "11270")]
            [TestProperty("Layer", "NGService")]
            public void Get_StateChangeDate_Test()
            {
                IAuditUtil audit = new StubAuditUtil();
                INGManager ngm = new StubNGManager();
                ISecurityManager sm = new StubSecurityManager();
                ICommonFormatterUtil cf = new StubCommonFormatterUtil();
                string userid = "000000000000000000000000";
                DateTime now = System.DateTime.UtcNow.Date;

                NGService ngs = new NGService
                {
                    AuditUtil = audit,
                    NGManager = ngm,
                    Security = sm,
                    CommonFormatterUtil = cf
                };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    ContractNumber = "NG",
                    PatientId = "",
                    Token = "dsafgsdfgdafg",
                    UserId = userid,
                    Version = 1.0
                };

                GetPatientProgramDetailsSummaryResponse response = ngs.Get(request);
                DateTime statechangedate = ((DateTime) response.Program.StateUpdatedOn).Date;
                Assert.AreEqual(now, statechangedate);
            }

            [TestMethod()]
            public void Get_WithAttributes_Test()
            {
                IAuditUtil audit = new StubAuditUtil();
                INGManager ngm = new StubNGManager();
                ISecurityManager sm = new StubSecurityManager();
                ICommonFormatterUtil cf = new StubCommonFormatterUtil();

                NGService ngs = new NGService
                {
                    AuditUtil = audit,
                    NGManager = ngm,
                    Security = sm,
                    CommonFormatterUtil = cf
                };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    ContractNumber = "NG",
                    PatientId = "",
                    Token = "dsafgsdfgdafg",
                    UserId = "",
                    Version = 1.0
                };
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

                NGService ngs = new NGService
                {
                    AuditUtil = audit,
                    NGManager = ngm,
                    Security = sm,
                    CommonFormatterUtil = cf
                };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    ContractNumber = "NG",
                    PatientId = "",
                    Token = "dsafgsdfgdafg",
                    UserId = "",
                    Version = 1.0
                };
                GetPatientProgramDetailsSummaryResponse response = ngs.Get(request);

                Assert.IsNotNull(response.Program.Objectives);
            }

            [TestMethod()]
            [TestCategory("NIGHT-917")]
            [TestProperty("TFS", "1886")]
            public void AD_Get_With_Module_Description_Test()
            {
                string desc = "BSHSI - Outreach & Enrollment";
                IAuditUtil audit = new StubAuditUtil();
                INGManager ngm = new StubNGManager();
                ISecurityManager sm = new StubSecurityManager();
                ICommonFormatterUtil cf = new StubCommonFormatterUtil();

                NGService ngs = new NGService
                {
                    AuditUtil = audit,
                    NGManager = ngm,
                    Security = sm,
                    CommonFormatterUtil = cf
                };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest
                {
                    ContractNumber = "NG",
                    PatientId = "",
                    Token = "dsafgsdfgdafg",
                    UserId = "",
                    Version = 1.0
                };
                GetPatientProgramDetailsSummaryResponse response = ngs.Get(request);

                Module module = response.Program.Modules.Find(m => m.SourceId == "532b5585a381168abe00042c");
                string mDesc = module.Description.Trim();
                Assert.AreEqual(desc, mDesc, true);
            }
        }

        [TestClass()]
        public class PostProgramAttributesChange_Method
        {
            [TestMethod()]
            [TestCategory("NIGHT-937")]
            [TestProperty("TFS", "12107")]
            [TestProperty("Layer", "AD.NGService")]
            public void Post()
            {
                IAuditUtil audit = new StubAuditUtil();
                INGManager ngm = new StubNGManager();
                ISecurityManager sm = new StubSecurityManager();
                ICommonFormatterUtil cf = new StubCommonFormatterUtil();

                NGService ngs = new NGService
                {
                    AuditUtil = audit,
                    NGManager = ngm,
                    Security = sm,
                    CommonFormatterUtil = cf
                };

                PostProgramAttributesChangeRequest request = new PostProgramAttributesChangeRequest
                {
                    ContractNumber = "NG",
                    PatientId = "",
                    Token = "dsafgsdfgdafg",
                    UserId = "",
                    Version = 1.0
                };

                PostProgramAttributesChangeResponse response =  ngs.Post(request);
                Assert.IsNotNull(response);
            }
        }

        [TestMethod()]
        public void PostTest()
        {
            var service = new NGService
            {
                AuditUtil = new StubAuditUtil(),
                CommonFormatterUtil = new StubCommonFormatterUtil(),
                NotesManager = new StubNotesManager {UtilManager = new StubUtilsManager()},
                Security = new StubSecurityManager {}
            };

            var req = new PostPatientNoteRequest
            {
                ContractNumber = "InHealth001",
                PatientId = "111111111111111111111214",
                UserId = "1234",
                Version = 1,
                Note = new PatientNote {DataSource = "Engage"}
            };

            var resp = service.Post(req);
            Assert.IsNotNull(resp);
        }
    }
}
