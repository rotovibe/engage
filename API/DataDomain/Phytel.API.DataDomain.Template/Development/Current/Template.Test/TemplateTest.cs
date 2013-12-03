using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Template.DTO;

namespace Phytel.API.DataDomain.Template.Test
{
    [TestClass]
    public class TemplateTest
    {
        [TestMethod]
        public void GetTemplateByID()
        {
            GetTemplateRequest request = new GetTemplateRequest{ TemplateID = "5"};

            GetTemplateResponse response = TemplateDataManager.GetTemplateByID(request);

            Assert.IsTrue(response.Template.TemplateID == "Tony");
        }
    }
}
