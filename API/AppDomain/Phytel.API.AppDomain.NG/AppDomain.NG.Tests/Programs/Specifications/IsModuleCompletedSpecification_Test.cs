using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Phytel.API.AppDomain.NG.Specifications.Tests
{
    [TestClass()]
    public class IsModuleCompletedSpecification_Test
    {
        [TestClass()]
        public class IsSatisfiedBy
        {
            [TestMethod()]
            public void IsSatisfiedBy_True()
            {
                ISpecification<Module> spec = new IsModuleCompletedSpecification<Module>();
                var result = spec.IsSatisfiedBy(new Module {Completed = true});
                Assert.AreEqual(true, result);
            }
        }
    }
}
