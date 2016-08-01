using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
namespace Phytel.API.DataDomain.Program.Tests
{
    [TestClass()]
    public class ProgramRepositoryFactoryTests
    {
        [TestClass()]
        public class GetRepository_Method
        {
            [TestMethod()]
            public void Get_Program()
            {
                MongoProgramRepository cRepo = new MongoProgramRepository("InHealth001");
                ProgramRepositoryFactory factory = new ProgramRepositoryFactory();
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest
                {
                    ContractNumber = "InHealth001",
                    Context = "NG",
                    UserId = "NGUser"
                };

                IProgramRepository repo = factory.GetRepository(request, RepositoryType.Program);

                Assert.AreEqual(cRepo.GetType(), repo.GetType());
            }

            [TestMethod()]
            public void Get_PatientProgram()
            {
                MongoPatientProgramRepository cRepo = new MongoPatientProgramRepository("InHealth001");
                ProgramRepositoryFactory factory = new ProgramRepositoryFactory();
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest
                {
                    ContractNumber = "InHealth001",
                    Context = "NG",
                    UserId = "NGUser"
                };

                IProgramRepository repo = factory.GetRepository(request, RepositoryType.PatientProgram);

                Assert.AreEqual(cRepo.GetType(), repo.GetType());
            }

            [TestMethod()]
            public void Get_PatientProgramAttributes()
            {
                MongoPatientProgramAttributeRepository cRepo = new MongoPatientProgramAttributeRepository("InHealth001");
                ProgramRepositoryFactory factory = new ProgramRepositoryFactory();
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest
                {
                    ContractNumber = "InHealth001",
                    Context = "NG",
                    UserId = "NGUser"
                };

                IProgramRepository repo = factory.GetRepository(request, RepositoryType.PatientProgramAttribute);

                Assert.AreEqual(cRepo.GetType(), repo.GetType());
            }

            [TestMethod()]
            public void Get_PatientProgramResponse()
            {
                MongoPatientProgramResponseRepository cRepo = new MongoPatientProgramResponseRepository("InHealth001");
                ProgramRepositoryFactory factory = new ProgramRepositoryFactory();
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest
                {
                    ContractNumber = "InHealth001",
                    Context = "NG",
                    UserId = "NGUser"
                };

                IProgramRepository repo = factory.GetRepository(request, RepositoryType.PatientProgramResponse);

                Assert.AreEqual(cRepo.GetType(), repo.GetType());
            }

            [TestMethod()]
            public void Get_ContractProgram()
            {
                MongoContractProgramRepository cRepo = new MongoContractProgramRepository("InHealth001");
                ProgramRepositoryFactory factory = new ProgramRepositoryFactory();
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest
                {
                    ContractNumber = "InHealth001",
                    Context = "NG",
                    UserId = "NGUser"
                };

                IProgramRepository repo = factory.GetRepository(request, RepositoryType.ContractProgram);

                Assert.AreEqual(cRepo.GetType(), repo.GetType());
            }

            [TestMethod()]
            public void Get_Response()
            {
                MongoResponseRepository cRepo = new MongoResponseRepository("InHealth001");
                ProgramRepositoryFactory factory = new ProgramRepositoryFactory();
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest
                {
                    ContractNumber = "InHealth001",
                    Context = "NG",
                    UserId = "NGUser"
                };

                IProgramRepository repo = factory.GetRepository(request, RepositoryType.Response);

                Assert.AreEqual(cRepo.GetType(), repo.GetType());
            }
        }
    }
}
