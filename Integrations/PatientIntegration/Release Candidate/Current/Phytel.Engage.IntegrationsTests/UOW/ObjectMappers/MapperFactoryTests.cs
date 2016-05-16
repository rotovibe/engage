using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.Engage.Integrations.UOW.ObjectMappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.Engage.Integrations.UOW.Notes;

namespace Phytel.Engage.Integrations.UOW.ObjectMappers.Tests
{
    [TestClass()]
    public class MapperFactoryTests
    {
        [TestMethod()]
        public void NoteMapperTest()
        {
            Type t = typeof(OrlandoHealth001_NoteMapper);
            var map = MapperFactory.NoteMapper("OrlandoHealth001");

            Assert.AreEqual(t, map.GetType());
        }
    }
}
