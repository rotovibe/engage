using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class CareMember_Tests
    {
        [TestMethod]
        public void InsertCareMember_Test()
        {
            CareMember cm = new CareMember { 
                Id = "-2",
                PatientId = "52f55899072ef709f84e7637",
                ContactId = "53043e5fd433231f48de8a7b",
                Primary = false,
                TypeId = "530cd576d433231ed4ba969c"
            };
            
            PostCareMemberRequest request = new PostCareMemberRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "AD_TestHarness"; 
            request.Version = "v1";
            request.PatientId = "52f55899072ef709f84e7637";
            request.Token = "5307bcf5d6a4850cd4abe0dd";
            request.CareMember = cm;

            CareMembersManager nManager = new CareMembersManager();
            PostCareMemberResponse response = nManager.InsertCareMember(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UpdateCareMember_Test()
        {
            CareMember cm = new CareMember
            {
                Id = "530d0fb7d433232bd830c520",
                PatientId = "52f55899072ef709f84e7637",
                ContactId = "53043e53d433231f48de8a7a",
                Primary = false,
                TypeId = "530cd576d433231ed4ba969c"
            };

            PostUpdateCareMemberRequest request = new PostUpdateCareMemberRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "AD_TestHarness";
            request.Version = "v1";
            request.PatientId = "52f55899072ef709f84e7637";
            request.Token = "5307bcf5d6a4850cd4abe0dd";
            request.CareMember = cm;

            CareMembersManager nManager = new CareMembersManager();
            PostUpdateCareMemberResponse response = nManager.UpdateCareMember(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetCareMember_Test()
        {
            GetCareMemberRequest request = new GetCareMemberRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "AD_TestHarness";
            request.Version = "v1";
            request.Id = "530d0fb7d433232bd830c520";
            request.PatientId = "52f55899072ef709f84e7637";
            request.UserId = "Snehal";

            CareMembersManager gManager = new CareMembersManager();
            CareMember response = gManager.GetCareMember(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetAllCareMembers_Test()
        {
            GetAllCareMembersRequest request = new GetAllCareMembersRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "AD_TestHarness";
            request.Version = "v1";
            request.PatientId = "52f55899072ef709f84e7637";
            request.UserId = "Snehal";

            CareMembersManager gManager = new CareMembersManager();
            List<CareMember> response = gManager.GetAllCareMembers(request);

            Assert.IsNotNull(response);
        }
    }
}
