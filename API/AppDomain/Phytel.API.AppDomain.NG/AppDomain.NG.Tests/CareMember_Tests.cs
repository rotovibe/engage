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
                PatientId = "55df7b6184ac070e74d95c1a",
                ContactId = "5325c821072ef705080d3488",
                Primary = true,
                TypeId = "530cd571d433231ed4ba969b"
            };
            
            PostCareMemberRequest request = new PostCareMemberRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "5310a9bdd6a4850e0477cade"; 
            request.Version = 1;
            request.PatientId = "55df7b6184ac070e74d95c1a";
            request.Token = "5310a9bdd6a4850e0477cade";
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
                Id = "5310d3eed6a4850e046e10ed",
                PatientId = "52f55895072ef709f84e7551",
                ContactId = "530fcad1d4332320e0336a6a",
                Primary = true,
                TypeId = "530cd576d433231ed4ba969c"
            };

            PostUpdateCareMemberRequest request = new PostUpdateCareMemberRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "AD_TestHarness";
            request.Version = 1;
            request.PatientId = "52f55895072ef709f84e7551";
            request.Token = "5310a9bdd6a4850e0477cade";
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
            request.Version = 1;
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
            request.Version = 1;
            request.PatientId = "52f55899072ef709f84e7637";
            request.UserId = "Snehal";

            CareMembersManager gManager = new CareMembersManager();
            List<CareMember> response = gManager.GetAllCareMembers(request);

            Assert.IsNotNull(response);
        }
    }
}
