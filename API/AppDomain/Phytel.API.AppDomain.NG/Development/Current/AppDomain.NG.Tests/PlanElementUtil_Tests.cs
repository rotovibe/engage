using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AD = Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Program.DTO;
using MongoDB.Bson;
namespace Phytel.API.AppDomain.NG.Tests
{
    [TestClass()]
    public class PlanElementUtil_Tests
    {
        public const string SystemId = "5368ff2ad4332316288f3e3e";

        [TestMethod()]
        public void ResponseSpawnAllowed_Test()
        {
            DTO.Step s = new DTO.Step { StepTypeId = 15 };
            DTO.Response r = new DTO.Response {  Id = "000000000000000000000000", Value=""};
            new PlanElementUtils().ResponseSpawnAllowed(s, r);
            Assert.Fail();
        }

        [TestClass()]
        public class SetProgramAttributes_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-868")]
            [TestProperty("TFS", "11270")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Set_Element_State_Change_Date()
            {
                IPlanElementUtils peUtil = new PlanElementUtils { };
                AD.SpawnElement spwn = new AD.SpawnElement { ElementId = "123456789012345678901234", ElementType =  12, Tag = "3"};
                AD.Program program = new AD.Program { ElementState = 1 };
                ProgramAttributeData progAttr = new ProgramAttributeData();
                peUtil.SetProgramAttributes(spwn, program, "UserId", progAttr);

                DateTime control = DateTime.UtcNow.Date;
                DateTime sample = ((DateTime)program.StateUpdatedOn).Date;
                Assert.AreEqual(control, sample);
            }
        }

        [TestClass()]
        public class CloneProgram_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-868")]
            [TestProperty("TFS", "11270")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Clone_With_StateChangeDate()
            {
                DateTime control = DateTime.UtcNow.Date;
                IPlanElementUtils peUtil = new PlanElementUtils { };
                AD.Program program = new AD.Program { ElementState = 1, StateUpdatedOn = control };
                peUtil.CloneProgram(program);

                DateTime sample = ((DateTime)program.StateUpdatedOn).Date;
                Assert.AreEqual(control, sample);
            }
        }

        [TestClass()]
        public class SetInitialProperties_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-948")]
            [TestProperty("TFS", "11495")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Get_Assigned_By()
            {
                DateTime control = DateTime.UtcNow.Date;
                IPlanElementUtils peUtil = new PlanElementUtils { };
                AD.Module mod = new AD.Module();
                AD.Program prog = new AD.Program();

                peUtil.SetInitialProperties(prog, mod);

                Assert.AreEqual(SystemId, mod.AssignById);
            }

            [TestMethod()]
            [TestCategory("NIGHT-949")]
            [TestProperty("TFS", "11444")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Get_Assigned_Date()
            {
                DateTime control = DateTime.UtcNow.Date;
                IPlanElementUtils peUtil = new PlanElementUtils {};
                AD.Module mod = new AD.Module();
                AD.Program prog = new AD.Program();

                peUtil.SetInitialProperties(prog, mod);

                Assert.AreEqual(DateTime.UtcNow.Date, ((DateTime)mod.AssignDate).Date);
            }

            [TestMethod()]
            [TestCategory("NIGHT-950")]
            [TestProperty("TFS", "11456")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Get_Assigned_To()
            {
                ObjectId? assignedTO = new ObjectId();
                IPlanElementUtils peUtil = new PlanElementUtils { };
                AD.Module mod = new AD.Module();
                AD.Program prog = new AD.Program{ AssignToId = assignedTO.ToString()};

                peUtil.SetInitialProperties(prog, mod);

                Assert.AreEqual(assignedTO.ToString(), mod.AssignToId);
            }

            [TestMethod()]
            [TestCategory("NIGHT-950")]
            [TestProperty("TFS", "11456")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Get_Assigned_To_Null()
            {
                string assignedTO = null;
                IPlanElementUtils peUtil = new PlanElementUtils { };
                AD.Module mod = new AD.Module();
                AD.Program prog = new AD.Program { AssignToId = assignedTO };

                peUtil.SetInitialProperties(prog, mod);

                Assert.AreEqual(assignedTO, mod.AssignToId);
            }
        }
    }
}
