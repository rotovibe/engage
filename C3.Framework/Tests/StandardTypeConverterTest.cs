using System;
using NUnit.Framework;


namespace C3.Framework.Tests
{
    [TestFixture]
    public class StandardTypeConverterTest
    {
        [Test]
        public void TestEmpty()
        {
            Assert.IsTrue( StandardTypeConverter.IsEmpty(null) );
            Assert.IsTrue( StandardTypeConverter.IsEmpty(DBNull.Value) );
            Assert.IsTrue( StandardTypeConverter.IsEmpty("") );
            Assert.IsFalse( StandardTypeConverter.IsEmpty( new object() ) );
            Assert.IsFalse( StandardTypeConverter.IsEmpty( "hello world" ) );
        }

        [Test]
        public void TestBool()
        {
            StandardTypeConverter converter = new StandardTypeConverter();

            Assert.IsTrue(converter.ToBool(true));
            Assert.IsTrue(converter.ToBool(1) );
            Assert.IsTrue(converter.ToBool("true") );
            Assert.IsTrue(converter.ToBool("yes"));
            Assert.IsTrue(converter.ToBool("eligible"));

            Assert.IsFalse(converter.ToBool(false));
            Assert.IsFalse(converter.ToBool(0) );
            Assert.IsFalse(converter.ToBool(""));
            Assert.IsFalse(converter.ToBool("false"));
            Assert.IsFalse(converter.ToBool("no"));
            Assert.IsFalse(converter.ToBool("ineligible"));
        }
    }
}
