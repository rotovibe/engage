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
    public class RemoveSelectedResponseSpecification_Test
    {
        [TestClass()]
        public class IsSatisFiedBy
        {
            [TestMethod()]
            public void IsSatisfiedBy_Type2()
            {
                ISpecification<Step> spec = new RemoveSelectedResponseSpecification<Step>();
                var resp = spec.IsSatisfiedBy(new Step {StepTypeId = 2});
                Assert.AreEqual(true, resp);
            }

            [TestMethod()]
            public void IsSatisfiedBy_Type10()
            {
                ISpecification<Step> spec = new RemoveSelectedResponseSpecification<Step>();
                var resp = spec.IsSatisfiedBy(new Step {StepTypeId = 10});
                Assert.AreEqual(true, resp);
            }

            [TestMethod()]
            public void IsSatisfiedBy_Type3()
            {
                ISpecification<Step> spec = new RemoveSelectedResponseSpecification<Step>();
                var resp = spec.IsSatisfiedBy(new Step {StepTypeId = 3});
                Assert.AreEqual(true, resp);
            }

            [TestMethod()]
            public void IsSatisfiedBy_Type9()
            {
                ISpecification<Step> spec = new RemoveSelectedResponseSpecification<Step>();
                var resp = spec.IsSatisfiedBy(new Step {StepTypeId = 9});
                Assert.AreEqual(true, resp);
            }

            [TestMethod()]
            public void IsSatisfiedBy_Type6()
            {
                ISpecification<Step> spec = new RemoveSelectedResponseSpecification<Step>();
                var resp = spec.IsSatisfiedBy(new Step {StepTypeId = 6});
                Assert.AreEqual(true, resp);
            }

            [TestMethod()]
            public void IsSatisfiedBy_Type8()
            {
                ISpecification<Step> spec = new RemoveSelectedResponseSpecification<Step>();
                var resp = spec.IsSatisfiedBy(new Step {StepTypeId = 8});
                Assert.AreEqual(true, resp);
            }

            [TestMethod()]
            public void IsSatisfiedBy_TypeNotAllowed()
            {
                ISpecification<Step> spec = new RemoveSelectedResponseSpecification<Step>();
                var resp = spec.IsSatisfiedBy(new Step {StepTypeId = 1});

                Assert.AreEqual(false, resp);
            }
        }
    }
}
