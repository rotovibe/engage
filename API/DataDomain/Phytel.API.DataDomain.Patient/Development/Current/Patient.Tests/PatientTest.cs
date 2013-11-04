using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Phytel.API.DataDomain.Patient.Test
{
    [TestClass]
    public class PatientTest
    {
        [TestMethod]
        public void GetUserByToken()
        {
            DTO.PatientDataRequest newRequest = new DTO.PatientDataRequest { UserToken = "abcxyz" };

            DTO.PatientDataResponse response = DataDomain.Patient.PatientDataManager.LoginUser(newRequest);

            Assert.IsTrue(response.UserName == "tdigiorgio@phytel.com");
        }
    }
}
