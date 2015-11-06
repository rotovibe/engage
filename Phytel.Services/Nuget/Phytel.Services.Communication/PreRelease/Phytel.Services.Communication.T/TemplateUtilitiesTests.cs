using NUnit.Framework;
using Phytel.Services.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Service.Communication.Test
{
    [TestFixture]
    [Category("TemplateUtilites")]
    public class TemplateUtilitesTests
    {
        private TemplateUtilites _templateUtilities = new TemplateUtilites();

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
