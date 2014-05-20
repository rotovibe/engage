﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AD = Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Program.DTO;
namespace Phytel.API.AppDomain.NG.Tests
{
    [TestClass()]
    public class PlanElementUtil_Tests
    {
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
    }
}
