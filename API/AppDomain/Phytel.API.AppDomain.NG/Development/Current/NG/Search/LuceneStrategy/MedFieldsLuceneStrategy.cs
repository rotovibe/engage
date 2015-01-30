using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.AppDomain.NG.Search.LuceneStrategy
{
    public class MedFieldsLuceneStrategy<T, TT> : BaseLuceneStrategy<T, TT>
        where T : MedFieldsSearchDoc
        where TT : MedFieldsSearchDoc
    {
        protected static readonly string SearchIndexPath = ConfigurationManager.AppSettings["SearchIndexPath"];

        public string Contract { get; set; }

        public override string LuceneDir
        {
            get { return SearchIndexPath + "\\" + Contract + "\\" + @"\MedFields_Index"; }
        }

        public override void AddToLuceneIndex(T sampleData, IndexWriter writer)
        {
            var boolQuery = new BooleanQuery();
            var sq1 = new TermQuery(new Term("MongoId", sampleData.ProductId));
            var sq2 = new TermQuery(new Term("DosageFormname", sampleData.DosageFormname));
            var sq3 = new TermQuery(new Term("ProprietaryName", sampleData.ProprietaryName));
            var sq4 = new TermQuery(new Term("RouteName", sampleData.RouteName));
            var sq5 = new TermQuery(new Term("SubstanceName", sampleData.SubstanceName));
            var sq6 = new TermQuery(new Term("Strength", sampleData.Strength));
            var sq7 = new TermQuery(new Term("Unit", sampleData.Unit));
            var sq8 = new TermQuery(new Term("CompositeName", sampleData.CompositeName));
            var sq9 = new TermQuery(new Term("PackageId", sampleData.ProductId));
            boolQuery.Add(sq1, Occur.MUST);
            boolQuery.Add(sq2, Occur.MUST);
            boolQuery.Add(sq3, Occur.MUST);
            boolQuery.Add(sq4, Occur.MUST);
            boolQuery.Add(sq5, Occur.MUST);
            boolQuery.Add(sq6, Occur.MUST);
            boolQuery.Add(sq7, Occur.MUST);
            boolQuery.Add(sq8, Occur.MUST);
            boolQuery.Add(sq9, Occur.MUST);
            writer.DeleteDocuments(boolQuery);

            var doc = new Document();
            doc.Add(new Field("MongoId", sampleData.Id.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("PackageId", sampleData.ProductId, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("DosageFormname", sampleData.DosageFormname.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("CompositeName", sampleData.CompositeName.Trim(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("ProprietaryName", sampleData.ProprietaryName.Trim(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("RouteName", sampleData.RouteName.Trim(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("SubstanceName", sampleData.SubstanceName.Trim(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Strength", sampleData.Strength.Trim(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Unit", sampleData.Unit, Field.Store.YES, Field.Index.ANALYZED));

            writer.AddDocument(doc);
        }

        public override void AddUpdateLuceneIndex(IEnumerable<T> sampleDatas)
        {
            //var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            var analyzer = new KeywordAnalyzer();
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var sampleData in sampleDatas) AddToLuceneIndex(sampleData, writer);

                analyzer.Close();
                //writer.Dispose();
            }
        }

        public override void AddUpdateLuceneIndex(T sampleData)
        {
            AddUpdateLuceneIndex(new List<T> { sampleData });
        }


        public override List<TT> ExecuteSearch(string searchQuery, string searchField = "")
        {
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<TT>();

            searchQuery = "\"" + searchQuery + "\"";
            using (var searcher = new IndexSearcher(Directory, false))
            {
                var hits_limit = 1000;
                // ignore stop words
                //var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30, CharArraySet.EMPTY_SET);
                var analyzer = new KeywordAnalyzer();

                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, searchField, analyzer);
                    parser.PhraseSlop = 0;
                    var query = ParseWholeQuery(searchQuery, parser);
                    var hits = searcher.Search(query, hits_limit).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
                else
                {
                    var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, new[] { "ProprietaryName", "SubstanceName" }, analyzer);
                    parser.PhraseSlop = 0;
                    var query = ParseQuery(searchQuery, parser);
                    var hits = searcher.Search
                    (query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
            }
        }
    }
}
