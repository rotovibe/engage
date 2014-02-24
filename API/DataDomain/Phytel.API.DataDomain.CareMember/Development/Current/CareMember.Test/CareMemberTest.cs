using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.CareMember.DTO;

namespace Phytel.API.DataDomain.CareMember.Test
{
    [TestClass]
    public class CareMemberTest
    {
        [TestMethod]
        public void GetCareMemberByID()
        {
            GetCareMemberRequest request = new GetCareMemberRequest{ CareMemberID = "5"};

            GetCareMemberResponse response = CareMemberDataManager.GetCareMemberByID(request);

            Assert.IsTrue(response.CareMember.CareMemberID == "Tony");
        }
    }
}
