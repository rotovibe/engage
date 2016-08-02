using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.MongoDB.DataManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.MongoDB.DataManagement.Procedures;
namespace Phytel.API.DataDomain.Program.MongoDB.DataManagement.Tests
{
    [TestClass()]
    public class ProceduresManager_Test
    {
        [TestClass()]
        public class GetProceduresList_Test
        {
            [TestMethod()]
            public void GetProcedures_List_With_Proc_Info()
            {
                IProceduresManager pm = new ProceduresManager();
                Program.DTO.GetMongoProceduresListResponse response =  pm.GetProceduresList(new Program.DTO.GetMongoProceduresListRequest());

                Assert.IsNotNull(response.Procedures);
            }
        }

        [TestClass()]
        public class GetProcedureConstValues_Test
        {
            [TestMethod()]
            public void Get_Info_For_Proc()
            {
                IProceduresManager pm = new ProceduresManager();
               List<string> list =  pm.GetProcedureConstValues<string>(typeof (MoveProgramAttributeStartDateValue));
                Assert.AreEqual(2, list.Count);
            }
        }
    }
}
