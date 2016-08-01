using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomain.Search.Repo.LuceneStrategy;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Phytel.API.AppDomain.NG.Search.LuceneStrategy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO.Search;

namespace Phytel.API.AppDomain.NG.Search.LuceneStrategy.Tests
{
    [TestClass()]
    public class BaseLuceneStrategyTests
    {
        [TestClass()]
        public class ParseWholeQueryWcTest
        {
            //[TestMethod()]
            //public void ParseWholeQueryWc_Med_Name_Whole_Words_Phrase()
            //{
            //    const string control = @"spanNear([SpanRegexQuery(compositename:Cat.*), SpanRegexQuery(compositename:Nip.*)], 20, False) spanNear([SpanRegexQuery(substancename:Cat.*), SpanRegexQuery(substancename:Nip.*)], 20, False)";
            //    const string phrase = "Cat Nip";

            //    var strat = new MedNameLuceneStrategy<MedNameSearchDoc, TextValuePair>();
            //    var fields = new string[2] {"compositename", "substancename"};
            //    var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30, new HashSet<string>());
            //    var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "", analyzer);

            //    var boolQ = strat.ParseWholeQueryWc(phrase, fields, parser);
            //    Assert.AreEqual(control, boolQ.ToString());
            //}

            //[TestMethod()]
            //public void ParseWholeQueryWc_Med_Name_Partial_Word_Phrase()
            //{
            //    const string control = @"spanNear([SpanRegexQuery(compositename:Cat.*), SpanRegexQuery(compositename:Ni.*)], 20, False) spanNear([SpanRegexQuery(substancename:Cat.*), SpanRegexQuery(substancename:Ni.*)], 20, False)";
            //    const string phrase = "Cat Ni";

            //    var strat = new MedNameLuceneStrategy<MedNameSearchDoc, TextValuePair>();
            //    var fields = new string[2] { "compositename", "substancename" };
            //    var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30, new HashSet<string>());
            //    var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "", analyzer);

            //    var boolQ = strat.ParseWholeQueryWc(phrase, fields, parser);
            //    Assert.AreEqual(control, boolQ.ToString());
            //}

            //[TestMethod()]
            //public void ParseWholeQueryWc_Med_Name_Whole_Word()
            //{
            //    const string control = @"spanNear([SpanRegexQuery(compositename:Cat.*)], 20, False) spanNear([SpanRegexQuery(substancename:Cat.*)], 20, False)";
            //    const string phrase = "Cat";

            //    var strat = new MedNameLuceneStrategy<MedNameSearchDoc, TextValuePair>();
            //    var fields = new string[2] { "compositename", "substancename" };
            //    var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30, new HashSet<string>());
            //    var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "", analyzer);

            //    var boolQ = strat.ParseWholeQueryWc(phrase, fields, parser);
            //    Assert.AreEqual(control, boolQ.ToString());
            //}

            //[TestMethod()]
            //public void ParseWholeQueryWc_Med_Name_Partial_Word()
            //{
            //    const string control = @"spanNear([SpanRegexQuery(compositename:Ca.*)], 20, False) spanNear([SpanRegexQuery(substancename:Ca.*)], 20, False)";
            //    const string phrase = "Ca";

            //    var strat = new MedNameLuceneStrategy<MedNameSearchDoc, TextValuePair>();
            //    var fields = new string[2] { "compositename", "substancename" };
            //    var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30, new HashSet<string>());
            //    var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "", analyzer);

            //    var boolQ = strat.ParseWholeQueryWc(phrase, fields, parser);
            //    Assert.AreEqual(control, boolQ.ToString());
            //}
        }
    }
}
