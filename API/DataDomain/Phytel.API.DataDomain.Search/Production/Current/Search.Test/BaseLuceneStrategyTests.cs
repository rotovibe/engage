using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Search.DTO;

namespace DataDomain.Search.Repo.LuceneStrategy.Tests
{
    [TestClass()]
    public class BaseLuceneStrategyTests
    {
        [TestClass()]
        public class ParseSpecialCharactersTest
        {
            [TestMethod()]
            public void ParseSpecialCharacters_SquareBracket()
            {
                var strat = new MedNameLuceneStrategy<MedNameSearchDocData, TextValuePair>();
                var val = "testing[";
                var result = strat.ParseSpecialCharacters(val);
                Assert.AreEqual("testing", result);
            }

            [TestMethod()]
            public void ParseSpecialCharacters_Asterisk()
            {
                var strat = new MedNameLuceneStrategy<MedNameSearchDocData, TextValuePair>();
                var val = "testing*";
                var result = strat.ParseSpecialCharacters(val);
                Assert.AreEqual("testing", result);
            }
        }
    }
}
