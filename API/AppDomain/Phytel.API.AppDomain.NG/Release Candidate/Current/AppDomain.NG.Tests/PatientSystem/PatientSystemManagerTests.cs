using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Phytel.API.AppDomain.NG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Context;
using Phytel.API.AppDomain.NG.DTO.Internal;
using Phytel.API.AppDomain.NG.Test.Stubs;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.AppDomain.NG.Tests
{
    [TestClass()]
    public class PatientSystemManagerTests
    {
        private string _patientId;
        private IServiceContext _context;

        [TestInitialize]
        public void TestInitialize()
        {
            Mapper.CreateMap<PatientSystemData, DTO.PatientSystem>();
            Mapper.CreateMap<DTO.PatientSystem, PatientSystemData>();
            Mapper.CreateMap<SystemData, DTO.System>();
            Mapper.CreateMap<DTO.System, SystemData>();
            Mapper.CreateMap<UpdatePatientsAndSystemsRequest, GetActiveSystemsRequest>();

            _patientId = ObjectId.GenerateNewId().ToString();

            _context = new ServiceContext
            {
                Contract = "InHealth001",
                Token = "123456",
                UserId = "1234",
                Version = 1.0,
                Tag = new List<DTO.PatientSystem>
                {
                    new DTO.PatientSystem
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        PatientId = _patientId,
                        Primary = true
                    },
                    new DTO.PatientSystem
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        PatientId = _patientId,
                        Primary = true
                    },
                    new DTO.PatientSystem
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        PatientId = _patientId,
                        Primary = false
                    }
                }
            };
        }



        [TestMethod()]
        public void SetPrimaryPatientSystemTest()
        {
            var psm = new PatientSystemManager {EndpointUtil = new StubPatientSystemEndpointUtil()};
            psm.SetPrimaryPatientSystem(_context, _patientId, (List<DTO.PatientSystem>)_context.Tag);
            Assert.IsTrue(true);
        }
    }
}
