using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.Test.Stubs;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using Phytel.API.DataDomain.Program.DTO;
namespace Phytel.API.DataDomain.Program.Tests
{
    [TestClass()]
    public class MongoPatientProgramRepository_Test
    {
        [TestMethod()]
        public void MongoPatientProgramRepository()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Insert()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InsertAll()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Delete()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteAll()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FindByEntityExistsID()
        {
            Assert.Fail();
        }

        [TestClass()]
        public class FindByID
        {
            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.Repository")]
            public void DD_FindByID_AttrStartDate()
            {
                DateTime? time = Convert.ToDateTime("1/1/1900");
                StubProgramRepositoryFactory factory = new StubProgramRepositoryFactory();
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest
                {
                    ContractNumber = "InHealth001",
                    Context = "NG",
                    UserId = "000000000000000000000000"
                };

                IProgramRepository repo = factory.GetRepository(request, RepositoryType.PatientProgram);

                MEPatientProgram mep = repo.FindByID("000000000000000000000000") as MEPatientProgram;
                DateTime? tDate = mep.Modules[0].AttributeStartDate;
                Assert.AreEqual(time, tDate);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.Repository")]
            public void DD_FindByID_AttrEndDate()
            {
                DateTime? time = Convert.ToDateTime("1/1/1901");
                StubProgramRepositoryFactory factory = new StubProgramRepositoryFactory();
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest
                {
                    ContractNumber = "InHealth001",
                    Context = "NG",
                    UserId = "000000000000000000000000"
                };

                IProgramRepository repo = factory.GetRepository(request, RepositoryType.PatientProgram);

                MEPatientProgram mep = repo.FindByID("000000000000000000000000") as MEPatientProgram;
                DateTime? tDate = mep.Modules[0].AttributeEndDate;
                Assert.AreEqual(time, tDate);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.Repository")]
            public void DD_FindByID_AssignedOn()
            {
                DateTime? time = Convert.ToDateTime("1/1/1999");
                StubProgramRepositoryFactory factory = new StubProgramRepositoryFactory();
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest
                {
                    ContractNumber = "InHealth001",
                    Context = "NG",
                    UserId = "000000000000000000000000"
                };

                IProgramRepository repo = factory.GetRepository(request, RepositoryType.PatientProgram);

                MEPatientProgram mep = repo.FindByID("000000000000000000000000") as MEPatientProgram;
                DateTime? tDate = mep.Modules[0].AssignedOn;
                Assert.AreEqual(time, tDate);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.Repository")]
            public void DD_FindByID_AssignedTo()
            {
                string ctrl = "123456789011111111112222";
                StubProgramRepositoryFactory factory = new StubProgramRepositoryFactory();
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest
                {
                    ContractNumber = "InHealth001",
                    Context = "NG",
                    UserId = "000000000000000000000000"
                };

                IProgramRepository repo = factory.GetRepository(request, RepositoryType.PatientProgram);

                MEPatientProgram mep = repo.FindByID("000000000000000000000000") as MEPatientProgram;
                string sample = mep.Modules[0].AssignedTo.ToString();
                Assert.AreEqual(ctrl, sample);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.Repository")]
            public void DD_FindByID_AssignedBy()
            {
                string ctrl = "123456789011111111112223";
                StubProgramRepositoryFactory factory = new StubProgramRepositoryFactory();
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest
                {
                    ContractNumber = "InHealth001",
                    Context = "NG",
                    UserId = "000000000000000000000000"
                };

                IProgramRepository repo = factory.GetRepository(request, RepositoryType.PatientProgram);

                MEPatientProgram mep = repo.FindByID("000000000000000000000000") as MEPatientProgram;
                string sample = mep.Modules[0].AssignedBy.ToString();
                Assert.AreEqual(ctrl, sample);
            }
        }

        [TestMethod()]
        public void Select()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SelectAll()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Save()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Update()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetActiveProgramsInfoList()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CacheByID()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FindByName()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Find()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FindByPlanElementID()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetLimitedProgramFields()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InsertAsBatch()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Find1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Update_Test()
        {
            PutProgramActionProcessingRequest request = new PutProgramActionProcessingRequest
            {
                ProgramId = "5330920da38116ac180009d2",
                Program = new ProgramDetail {Id = "5330920da38116ac180009d2", AssignTo = null}
            };

            MongoPatientProgramRepository repo = new MongoPatientProgramRepository("InHealth001")
            {
                UserId = "123456789012345678901234"
            };
            repo.Update(request);
        }
    }
}
