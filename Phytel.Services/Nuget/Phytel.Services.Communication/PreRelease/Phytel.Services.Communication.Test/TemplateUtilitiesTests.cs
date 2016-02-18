using NUnit.Framework;
using Phytel.Services.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Phytel.Services.Communication.Test
{
    [TestFixture]
    [Category("TemplateUtilites")]
    public class TemplateUtilitesTests
    {
        private TemplateUtilities _templateUtilities = new TemplateUtilities();

        [Test]
        public void TestProperCase()
        {
            Assert.AreEqual("Firstname", _templateUtilities.ProperCase("firstname"), "Expected Firstname");
            Assert.AreEqual("McDonald", _templateUtilities.ProperCase("mcdonald"), "Expected McDonald");
            Assert.AreEqual("Collins", _templateUtilities.ProperCase("collinS"), "Expected Collins");
            Assert.AreEqual("Collin's", _templateUtilities.ProperCase("collin'S"), "Expected Collin's");
        }

        [Test]
        public void TestMissingObjects()
        {
            Hashtable missingObjects = new Hashtable();

            string firstMissing = "missing1";
            string secondMissing = "missing2";

            missingObjects = _templateUtilities.AddMissingObjects(missingObjects, firstMissing);
            Assert.IsTrue(missingObjects.ContainsValue(firstMissing), string.Format("Expected {0}", firstMissing));
            Assert.IsTrue(missingObjects.Count == 1, "Expected 1 item");

            missingObjects = _templateUtilities.AddMissingObjects(missingObjects, firstMissing);
            Assert.IsTrue(missingObjects.ContainsValue(firstMissing), string.Format("Expected {0}", firstMissing));
            Assert.IsTrue(missingObjects.Count == 1, "Expected 1 item");

            missingObjects = _templateUtilities.AddMissingObjects(missingObjects, secondMissing);
            Assert.IsTrue(missingObjects.ContainsValue(secondMissing), string.Format("Expected {0}", secondMissing));
            Assert.IsTrue(missingObjects.Count == 2, "Expected 2 item");

        }

        [Test]
        public void TestSpecialCharacter()
        {
            Assert.DoesNotThrow(new TestDelegate(SpecialCharactersTestDelegate));
            IDictionary<string, string> characters = new Dictionary<string, string>()
                {
                    {"&", "&amp;"},
                    {"<", "&lt;"},
                    {">", "&gt;"},
                    {"\"", "&quot;"},
                    {"'", "&apos;"}
                };

            foreach (string key in characters.Keys)
            {
                string testString = string.Format("test{0}test", key);
                string expectedString = string.Format("test{0}test", characters[key]);
                testString = _templateUtilities.HandleXMlSpecialCharacters(testString);
                Assert.AreEqual(expectedString, testString, string.Format("Expected {0}", expectedString));
            }
        }

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

        [Test]
        public void TestTextTransform()
        {
            string actualBody = string.Empty;            
            string expectedBody = "You have an appt with Test Schedule on Nov. 20 2020 at 8:00 AM. Txt YES to confirm or NO to cancel. Txt STOP to opt-out. HELP 4 HELP. MSG data rates may apply.";

            string xmlBody = "<TEXT><SendID>3</SendID><ActivityID>10</ActivityID><ContractID>ABC001</ContractID>" +
                              "<Patient><PatientID>20</PatientID><FullName>what is this</FullName><FirstName>test</FirstName><LastName>last</LastName></Patient>" +
                              "<Schedule><ScheduleID>30</ScheduleID><FullName>Test Schedule 1</FullName><DisplayName>Test Schedule</DisplayName></Schedule>" +
                              "<Facility><FacilityID>100</FacilityID><DisplayName /><Name>Test Facility</Name><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber>(214)555-1212</PhoneNumber></Facility>" +
                              "<Message><DayOfWeek>Friday</DayOfWeek><Month>Nov.</Month><Date>20</Date><Year>2020</Year><Time>8:00 AM</Time><DateTime />" +
                              "<TextFromNumber>8175551212</TextFromNumber><TextToNumber>4695551212</TextToNumber><TextHelpNumber>2145551212</TextHelpNumber><Body /></Message></TEXT>";
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlBody);
            string xslBody = "<xsl:stylesheet xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" version=\"1.0\">" +
                                "<xsl:output omit-xml-declaration=\"yes\" indent=\"yes\" />" +
                                "<xsl:template match=\"/\">" +
                                "<xsl:call-template name=\"Text\" />" +
                                "</xsl:template>" +
                                "<xsl:template name=\"Text\">" +
                                "You have an appt with <xsl:value-of select=\"substring(//Text/Schedule/DisplayName, 1, 13)\" /> on <xsl:value-of select=\"concat(//Text/Message/Month, ' ', //Text/Message/Date, ' ', //Text/Message/Year, ' at ', //Text/Message/Time)\" />. Txt YES to confirm or NO to cancel. Txt STOP to opt-out. HELP 4 HELP. MSG data rates may apply." +
                                "</xsl:template>" +
                                "</xsl:stylesheet>";
            TemplateDetail templateDetail = new TemplateDetail
            {
                TemplateXSLBody = xslBody
            };

            TemplateUtilities utilities = new TemplateUtilities();
            actualBody = utilities.Transform(xml, templateDetail, "Text");

            Assert.AreEqual(expectedBody, actualBody);            
        }

        private void SpecialCharactersTestDelegate()
        {
            string testString = null;
            testString = _templateUtilities.HandleXMlSpecialCharacters(testString);

            testString = string.Empty;
            testString = _templateUtilities.HandleXMlSpecialCharacters(testString);

            testString = " ";
            testString = _templateUtilities.HandleXMlSpecialCharacters(testString);        
        }       

    }
}
