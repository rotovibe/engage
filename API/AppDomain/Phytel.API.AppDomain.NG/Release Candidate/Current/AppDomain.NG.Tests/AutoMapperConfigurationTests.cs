using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Service.Mappers;
using Phytel.API.DataDomain.Contact.DTO;
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
            ContactTypeLookUpsMappers.Build();
        }

        [TestMethod]
        public void AssertAutoMapperConfigurationIsValid()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void ContactIsuserShouldBeTrue_Test()
        {
            ContactData cData = new ContactData()
            {
                Id = "56ea0c2c64e91cf53bbfca5f",
                UserId = "testUserId",
            };
            var ct =  Mapper.Map<DTO.Contact>(cData);
            
            Assert.IsTrue(ct.IsUser);

        }
        [TestMethod]
        public void ContactIsuserShouldBeFalse_Test()
        {
            ContactData cData = new ContactData()
            {
                Id = "56ea0c2c64e91cf53bbfca5f",               
            };
            var ct = Mapper.Map<DTO.Contact>(cData);

            Assert.IsFalse(ct.IsUser);

        }

        [TestMethod]
        public void ContactIsPatientShouldBeTrue_Test()
        {
            ContactData cData = new ContactData()
            {
                Id = "56ea0c2c64e91cf53bbfca5f",
                PatientId = "testPatientId",
            };
            var ct = Mapper.Map<DTO.Contact>(cData);

            Assert.IsTrue(ct.IsPatient);

        }
        [TestMethod]
        public void ContactIsPatientShouldBeFalse_Test()
        {
            ContactData cData = new ContactData()
            {
                Id = "56ea0c2c64e91cf53bbfca5f",
            };
            var ct = Mapper.Map<DTO.Contact>(cData);

            Assert.IsFalse(ct.IsPatient);

        }

        [TestMethod]
        public void ContactTypeLookUpParentIdShouldBeNull_Test()
        {
            ContactTypeLookUpData cData = new ContactTypeLookUpData()
            {
                Id = "56ea0c2c64e91cf53bbfca5f",
                Name = "Doctor",
                Role = "Doctor(M.D)",
                CreatedOn = DateTime.UtcNow,
                Group = ContactLookUpGroupType.CareTeam,
                ParentId = "000000000000000000000000"
            };
            var ct = Mapper.Map<ContactTypeLookUp>(cData);

            Assert.IsNull(ct.ParentId);

        }

    }
}
