using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Notes.Visitors.Tests
{
    [TestClass()]
    public class TakeModifierTests
    {
        [TestMethod()]
        public void Modify_Test()
        {
            var takemod = new TakeModifier(2);
            var list = new List<PatientNote>()
            {
                new PatientNote {Id = "123456789012345678901234", Text = "test1"},
                new PatientNote {Id = "123456789012345678901222", Text = "test2"},
                new PatientNote {Id = "123456789012345678901233", Text = "test3"}
            };

            var result = takemod.Modify(ref list);

            Assert.AreEqual(2, result.Count);
        }
    }
}
