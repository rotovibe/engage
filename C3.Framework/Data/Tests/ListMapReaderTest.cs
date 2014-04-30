using System.Collections.Generic;
using NUnit.Framework;

namespace C3.Framework.Data.Tests
{
    [TestFixture]
    public class ListMapReaderTest
    {
        [Test]
        public void TestReadEmpty()
        {
            ListMapReader reader = new ListMapReader();

            Assert.That( reader.Read(), Is.False );
        }

        [Test]
        public void TestReadMultiple()
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            ListMapReader reader = new ListMapReader();
            reader.AddResult(values);
            reader.AddResult(values);

            Assert.That(reader.Read(), Is.True);
            Assert.That(reader.Read(), Is.True);
            Assert.That(reader.Read(), Is.False);
        }

        [Test]
        public void TestGetInt()
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values["test"] = "10";

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            data.Add(values);

            ListMapReader reader = new ListMapReader(data);

            Assert.That( reader.Read(), Is.True );
            Assert.That( reader.GetInt("test"), Is.EqualTo(10) );
        }
    }
}
