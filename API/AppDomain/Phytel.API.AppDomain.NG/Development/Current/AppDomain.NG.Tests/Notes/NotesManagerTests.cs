using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.Notes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Notes.Visitors;

namespace Phytel.API.AppDomain.NG.Notes.Tests
{
    [TestClass()]
    public class NotesManagerTests
    {
        [TestMethod()]
        public void GetAllPatientNotesTest()
        {
            var ordMod = new OrderModifier();
            var list = new List<PatientNote>()
            {
                new PatientNote {Id = "123456789012345678901222", Text = "test2", CreatedOn = DateTime.Now.AddDays(2)},
                new PatientNote {Id = "123456789012345678901233", Text = "test3", CreatedOn = DateTime.Now.AddDays(1)},
                new PatientNote {Id = "123456789012345678901234", Text = "test1", CreatedOn = DateTime.Now.AddDays(3)}
            };

            var result = ordMod.Modify(ref list);
        }
    }
}
