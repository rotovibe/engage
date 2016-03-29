using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Service.Mappers;
using Phytel.API.DataDomain.Patient.DTO;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class AutoMapperConfigurationTests
    {
        [TestInitialize]
        public void SetUp()
        {
            ContactMapper.Build();
        }

        [TestMethod]
        public void AssertAutoMapperConfigurationIsValid()
        {
            Mapper.AssertConfigurationIsValid();
        }
    }
}
