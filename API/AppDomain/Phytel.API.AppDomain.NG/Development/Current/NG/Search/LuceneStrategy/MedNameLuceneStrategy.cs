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
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.AppDomain.NG.Search.LuceneStrategy
{
    public class MedNameLuceneStrategy<T, TT> : BaseLuceneStrategy<T, TT> where T : MedNameSearchDoc where TT : TextValuePair
    {
        protected static readonly string SearchIndexPath = ConfigurationManager.AppSettings["SearchIndexPath"];
        public string Contract { get; set; }

        public override string LuceneDir
        {
            get { return SearchIndexPath + "\\" + Contract + "\\" + @"\MedNames_Index"; }
        }

        public override void AddToLuceneIndex(T sampleData, IndexWriter writer)
        {
            var searchQuery = new TermQuery(new Term("MongoId", sampleData.ProductId));
            writer.DeleteDocuments(searchQuery);
            var doc = new Document();
            doc.Add(new Field("MongoId", sampleData.Id == null ? string.Empty : sampleData.Id.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            //doc.Add(new Field("PackageId", sampleData.ProductId.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            //doc.Add(new Field("ProductNDC", sampleData.ProductNDC.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("CompositeName", sampleData.CompositeName == null ? string.Empty : sampleData.CompositeName.Trim(), Field.Store.YES, Field.Index.ANALYZED));
            //doc.Add(new Field("ProprietaryName", sampleData.ProprietaryName.Trim(), Field.Store.YES, Field.Index.ANALYZED));
            //doc.Add(new Field("ProprietaryNameSuffix", sampleData.ProprietaryNameSuffix.Trim(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("SubstanceName", sampleData.SubstanceName == null ? string.Empty : sampleData.SubstanceName.Trim(), Field.Store.YES, Field.Index.ANALYZED));

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


        public override List<TT> ExecuteSearch(string searchQuery, string searchField = "")
        {
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<TT>();

            using (var searcher = new IndexSearcher(Directory, false))
            {
                var hits_limit = 1000;
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, searchField, analyzer);
                    parser.AllowLeadingWildcard = true;
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
                    var fields = new[] {"CompositeName", "SubstanceName"};
                    var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, fields, analyzer);
                    parser.AllowLeadingWildcard = true;
                    parser.PhraseSlop = 0;
                    var query = ParseWholeQueryWc(searchQuery, fields, parser);
                    var hits = searcher.Search(query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
            }
        }
    }
}
