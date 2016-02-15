using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Phytel.Services.Communication.Test
{
    [TestFixture]
    public class CommTemplateUtilitiesTests
    {
        [Test]
        public void TestModeCaseValidation()
        {
            XmlDocument doc = new XmlDocument();
            TemplateUtilities utilities = new TemplateUtilities();
            string mode = string.Empty;

            //Send an incorrect mode
            doc.AppendChild(doc.CreateNode("element", "Email", ""));
            mode = "Text";
            Assert.AreEqual(utilities.ValidateModeCase(doc, ref mode), false);
            Assert.AreEqual(mode, "Text");

            //Send the correct mode in title case
            doc.RemoveAll();
            doc.AppendChild(doc.CreateNode("element", "Text", ""));
            mode = "text";
            Assert.AreEqual(utilities.ValidateModeCase(doc, ref mode), true);
            Assert.AreEqual(mode, "Text");

            //Send the correct mode in upper case
            doc.RemoveAll();
            doc.AppendChild(doc.CreateNode("element", "TEXT", ""));
            mode = "text";
            Assert.AreEqual(utilities.ValidateModeCase(doc, ref mode), true);
            Assert.AreEqual(mode, "TEXT");

            //Send the correct mode in lower case
            doc.RemoveAll();
            doc.AppendChild(doc.CreateNode("element", "text", ""));
            mode = "TEXT";
            Assert.AreEqual(utilities.ValidateModeCase(doc, ref mode), true);
            Assert.AreEqual(mode, "text");
        }
    }
}
