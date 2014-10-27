using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.AppDomain.NG.Search.LuceneStrategy
{
    public class MedsLuceneStrategy<T> : BaseLuceneStrategy<T> where T : IdNamePair
    {
        protected static readonly string SearchIndexPath = ConfigurationManager.AppSettings["SearchIndexPath"];

        public override string LuceneDir
        {
            get { return SearchIndexPath + @"\Meds_Index"; }
        }

        public override void AddToLuceneIndex(T sampleData, IndexWriter writer)
        {
            var searchQuery = new TermQuery(new Term("Id", sampleData.Id));
            writer.DeleteDocuments(searchQuery);
            var doc = new Document();
            doc.Add(new Field("Id", sampleData.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Name", sampleData.Name, Field.Store.YES, Field.Index.ANALYZED));

            writer.AddDocument(doc);
        }

        public override void AddUpdateLuceneIndex(IEnumerable<T> sampleDatas)
        {
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var sampleData in sampleDatas) AddToLuceneIndex(sampleData, writer);

                analyzer.Close();
                writer.Dispose();
            }
        }

        public override void AddUpdateLuceneIndex(T sampleData)
        {
            AddUpdateLuceneIndex(new List<T> { sampleData });
        }


        public override List<IdNamePair> ExecuteSearch(string searchQuery, string searchField = "")
        {
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<IdNamePair>();

            using (var searcher = new IndexSearcher(Directory, false))
            {
                var hits_limit = 1000;
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, searchField, analyzer);
                    var query = ParseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, hits_limit).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
                else
                {
                    var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, new[] { "Id", "Name" }, analyzer);
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
