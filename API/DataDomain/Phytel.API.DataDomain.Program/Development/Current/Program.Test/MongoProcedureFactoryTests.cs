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
                string procName = "mp_UpdateProgramStartDateToFirstActionStartDate";
                double version =1.0 ;
                double docVersion = 1.0;
                string contract = "InHealth001";
                string context = "NG";
                string userId = "user";

                PostMongoProceduresRequest request = new PostMongoProceduresRequest { Version = version, Name = procName, DocumentVersion = docVersion, ContractNumber = contract, Context = context, UserId = userId };

                MongoProcedureFactory factory = new MongoProcedureFactory();
                IMongoProcedure proc = factory.GetProcedure(request);
                proc.Execute();
            }
        }
    }
}
