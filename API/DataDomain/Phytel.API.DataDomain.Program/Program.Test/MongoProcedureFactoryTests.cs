using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.Procs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.MongoDB.DataManagement;
using Phytel.API.DataDomain.Program.DTO;
namespace Phytel.API.DataDomain.Program.Procs.Tests
{
    [TestClass()]
    public class MongoProcedureFactoryTests
    {
        [TestClass()]
        public class GetProcedure_Method
        {
            [TestMethod()]
            public void Get_UpdateProgramStartDate_Proc()
            {
                executeMongoProcedure("mp_UpdateProgramStartDateToFirstActionStartDate");
            }

            [TestMethod()]
            public void Get_UpdatePatientProgram_HTNAndDiabetesText_Proc_Test()
            {
                executeMongoProcedure("mp_UpdatePatientProgram_HTNAndDiabetesText");
            }

            [TestMethod()]
            public void Get_MoveProgramAttributeStartDateValue_Test()
            {
                executeMongoProcedure("mp_MoveProgramAttributeStartDateValue");
            }

            [TestMethod()]
            public void UpdatePatientProgramAssignedAttributes_Test()
            {
                executeMongoProcedure("mp_UpdatePatientProgramAssignedAttributes");
            }

            [TestMethod()]
            public void UpdatePatientModuleAssignedAttributes_Test()
            {
                executeMongoProcedure("mp_UpdatePatientModuleAssignedAttributes");
            }

            [TestMethod()]
            public void UpdatePatientActionAssignedAttributes_Test()
            {
                executeMongoProcedure("mp_UpdatePatientActionAssignedAttributes");
            }

            [TestMethod()]
            public void UpdatePatientActionStateUpdateDate_Test()
            {
                executeMongoProcedure("mp_UpdatePatientActionStateUpdateDate");
            }

            [TestMethod()]
            public void UpdateStateForProgramsAndModules_Test()
            {
                executeMongoProcedure("mp_UpdateStateForProgramsAndModules");
            }

            private void executeMongoProcedure(string procName)
            {
                double version = 1.0;
                double docVersion = 1.0;
                string contract = "InHealth001";
                string context = "NG";
                string userId = "user";

                GetMongoProceduresRequest request = new GetMongoProceduresRequest { Version = version, Name = procName, DocumentVersion = docVersion, ContractNumber = contract, Context = context, UserId = userId };
                MongoProcedureFactory factory = new MongoProcedureFactory();
                IMongoProcedure proc = factory.GetProcedure(request);
                proc.Execute();
            }
        }
    }
}
