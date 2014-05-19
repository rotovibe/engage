using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.CareMember.DTO;

namespace Phytel.API.DataDomain.CareMember.Test
{
    [TestClass]
    public class CareMemberTest
    {
        [TestMethod]
        public void GetCareMember_Test_Passes()
        {
            GetCareMemberDataRequest request = new GetCareMemberDataRequest { Id = "531dedbbd6a485039854650b" };
            StubCareMemberDataManager cm = new StubCareMemberDataManager { Factory = new StubCareMemberRepositoryFactory() };
            CareMemberData response = cm.GetCareMember(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetAllCareMembers_Test_Passes()
        {
            GetAllCareMembersDataRequest request = new GetAllCareMembersDataRequest { PatientId = "52f55899072ef709f84e7637" };
            StubCareMemberDataManager cm = new StubCareMemberDataManager { Factory = new StubCareMemberRepositoryFactory() };
            List<CareMemberData> response = cm.GetAllCareMembers(request);

            Assert.IsTrue(response.Count > 0);
        }

        [TestMethod]
        public void InsertCareMember_Test()
        {
            CareMemberData n = new CareMemberData { ContactId = "53043e6cd433231f48de8a7c", Primary = true, TypeId = "530cd571d433231ed4ba969b", PatientId = "52f5589c072ef709f84e7798" };
            PutCareMemberDataRequest request = new PutCareMemberDataRequest
            {
                UserId = "DD_Harness",
                Version = 1,
                CareMember = n
            };
            StubCareMemberDataManager cm = new StubCareMemberDataManager { Factory = new StubCareMemberRepositoryFactory() };
            string id = cm.InsertCareMember(request);

            Assert.IsNotNull(id);
        }

        [TestMethod]
        public void UpdateCareMember_Test()
        {
            CareMemberData n = new CareMemberData { Id = "530cf7a4d43323130096eff9", ContactId = "53043e53d433231f48de8a7a", Primary = false, TypeId = "530cd576d433231ed4ba969c", PatientId = "52f5589c072ef709f84e7798" };
            PutUpdateCareMemberDataRequest request = new PutUpdateCareMemberDataRequest
            {
                UserId = "DD_Harness",
                Version = 1,
                CareMember = n
            };
            StubCareMemberDataManager cm = new StubCareMemberDataManager { Factory = new StubCareMemberRepositoryFactory() };
            bool updated = cm.UpdateCareMember(request);

            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void GetPrimaryCareManager_Test()
        {
            GetPrimaryCareManagerDataRequest request = new GetPrimaryCareManagerDataRequest { PatientId = "5325dafed6a4850adcbba7fa", Context = "NG", ContractNumber = "InHealth001", UserId = "000000000000000000000000", Version = 1.0 };
            StubCareMemberDataManager cm = new StubCareMemberDataManager { Factory = new StubCareMemberRepositoryFactory() };
            CareMemberData response = cm.GetPrimaryCareManager(request);

            Assert.IsNotNull(response);
        }
    }
}
