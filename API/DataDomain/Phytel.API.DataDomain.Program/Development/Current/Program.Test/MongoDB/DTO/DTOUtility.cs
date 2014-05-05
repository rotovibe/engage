using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using MongoDB.Bson;
namespace Phytel.API.DataDomain.Program.MongoDB.DTO.Tests
{
    [TestClass()]
    public class DTOUtility_Test
    {
        [TestClass()]
        public class GetModules
        {
            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Module_StartDate()
            {
                DateTime? sD = Convert.ToDateTime("1/1/1900");
                DTOUtility util = new DTOUtility();
                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module> mods = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module>();

                mods.Add(new Phytel.API.DataDomain.Program.MongoDB.DTO.Module
                {
                    Id = ObjectId.Parse("000000000000000000000000"),
                    Name = "Test stub module 1",
                    Description = "BSHSI - Outreach & Enrollment",
                    SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                    Actions = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action>(){ 
                                            new Phytel.API.DataDomain.Program.MongoDB.DTO.Action{ 
                                                Id = ObjectId.Parse("000000000000000000000000"),  
                                                State = ElementState.InProgress, 
                                                Name ="test action from stub", 
                                                Description = "test action 1"} },
                    AttributeStartDate = Convert.ToDateTime("1/1/1900"),
                    AttributeEndDate = Convert.ToDateTime("1/1/1901")
                });

                List<ModuleDetail> mds = util.GetModules(mods, "InHealth001", "000000000000000000000000");
                DateTime? startD = mds[0].AttrStartDate;
                Assert.AreEqual(sD, startD);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Module_EndDate()
            {
                DateTime? eD = Convert.ToDateTime("1/1/1901");
                DTOUtility util = new DTOUtility();
                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module> mods = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module>();

                mods.Add(new Phytel.API.DataDomain.Program.MongoDB.DTO.Module
                {
                    Id = ObjectId.Parse("000000000000000000000000"),
                    Name = "Test stub module 1",
                    Description = "BSHSI - Outreach & Enrollment",
                    SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                    Actions = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action>(){ 
                                            new Phytel.API.DataDomain.Program.MongoDB.DTO.Action{ 
                                                Id = ObjectId.Parse("000000000000000000000000"),  
                                                State = ElementState.InProgress, 
                                                Name ="test action from stub", 
                                                Description = "test action 1"} },
                    AttributeStartDate = Convert.ToDateTime("1/1/1900"),
                    AttributeEndDate = Convert.ToDateTime("1/1/1901")
                });

                List<ModuleDetail> mds = util.GetModules(mods, "InHealth001", "000000000000000000000000");
                DateTime? endD = mds[0].AttrEndDate;
                Assert.AreEqual(eD, endD);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Module_AssignedOn()
            {
                DateTime? assOn = Convert.ToDateTime("1/1/1999");
                DTOUtility util = new DTOUtility();
                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module> mods = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module>();

                mods.Add(new Phytel.API.DataDomain.Program.MongoDB.DTO.Module
                {
                    Id = ObjectId.Parse("000000000000000000000000"),
                    Name = "Test stub module 1",
                    Description = "BSHSI - Outreach & Enrollment",
                    SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                    Actions = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action>(){ 
                                            new Phytel.API.DataDomain.Program.MongoDB.DTO.Action{ 
                                                Id = ObjectId.Parse("000000000000000000000000"),  
                                                State = ElementState.InProgress, 
                                                Name ="test action from stub", 
                                                Description = "test action 1"} },
                    AttributeStartDate = Convert.ToDateTime("1/1/1900"),
                    AttributeEndDate = Convert.ToDateTime("1/1/1901"),
                    AssignedOn = Convert.ToDateTime("1/1/1999")
                });

                List<ModuleDetail> mds = util.GetModules(mods, "InHealth001", "000000000000000000000000");
                DateTime? assD = mds[0].AssignDate;
                Assert.AreEqual(assOn, assD);
            }
            
            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Module_AssignedTo()
            {
                string asT = "123456789011111111112222";
                DTOUtility util = new DTOUtility();
                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module> mods = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module>();

                mods.Add(new Phytel.API.DataDomain.Program.MongoDB.DTO.Module
                {
                    Id = ObjectId.Parse("000000000000000000000000"),
                    Name = "Test stub module 1",
                    Description = "BSHSI - Outreach & Enrollment",
                    SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                    Actions = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action>(){ 
                                            new Phytel.API.DataDomain.Program.MongoDB.DTO.Action{ 
                                                Id = ObjectId.Parse("000000000000000000000000"),  
                                                State = ElementState.InProgress, 
                                                Name ="test action from stub", 
                                                Description = "test action 1"} },
                    AttributeStartDate = Convert.ToDateTime("1/1/1900"),
                    AttributeEndDate = Convert.ToDateTime("1/1/1901"),
                    AssignedOn = Convert.ToDateTime("1/1/1999"),
                    AssignedTo = ObjectId.Parse("123456789011111111112222")
                });

                List<ModuleDetail> mds = util.GetModules(mods, "InHealth001", "000000000000000000000000");
                string assT = mds[0].AssignTo;
                Assert.AreEqual(asT, assT);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Module_AssignedBy()
            {
                string asT = "123456789011111111112223";
                DTOUtility util = new DTOUtility();
                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module> mods = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module>();

                mods.Add(new Phytel.API.DataDomain.Program.MongoDB.DTO.Module
                {
                    Id = ObjectId.Parse("000000000000000000000000"),
                    Name = "Test stub module 1",
                    Description = "BSHSI - Outreach & Enrollment",
                    SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                    Actions = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action>(){ 
                                            new Phytel.API.DataDomain.Program.MongoDB.DTO.Action{ 
                                                Id = ObjectId.Parse("000000000000000000000000"),  
                                                State = ElementState.InProgress, 
                                                Name ="test action from stub", 
                                                Description = "test action 1"} },
                    AttributeStartDate = Convert.ToDateTime("1/1/1900"),
                    AttributeEndDate = Convert.ToDateTime("1/1/1901"),
                    AssignedOn = Convert.ToDateTime("1/1/1999"),
                    AssignedTo = ObjectId.Parse("123456789011111111112222"),
                    AssignedBy = ObjectId.Parse("123456789011111111112223")
                });

                List<ModuleDetail> mds = util.GetModules(mods, "InHealth001", "000000000000000000000000");
                string assT = mds[0].AssignBy;
                Assert.AreEqual(asT, assT);
            }
        }
    }
}
