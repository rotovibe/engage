using System;
using System.Collections.Generic;
using System.Configuration;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Phytel.API.AppDomain.NG.DTO.Search;
using Version = Lucene.Net.Util.Version;

namespace Phytel.API.AppDomain.NG.Search.LuceneStrategy
{
    public class MedNameLuceneStrategy<T, TT> : BaseLuceneStrategy<T, TT>, IMedNameLuceneStrategy<T, TT> where T : MedNameSearchDoc where TT : TextValuePair
    {
        protected static readonly string SearchIndexPath = ConfigurationManager.AppSettings["SearchIndexPath"];
        public override string LuceneDir{ get { return SearchIndexPath + "\\" + Contract + "\\" + @"\MedNames_Index"; } }
        public string Contract { get; set; }
        public StandardAnalyzer Analyzer { get; set; }
        private IndexWriter _writer;

        public MedNameLuceneStrategy()
        {
            Contract = ConfigurationManager.AppSettings["SearchContractName"]; //"InHealth001";
            Analyzer = new StandardAnalyzer(Version.LUCENE_30, new HashSet<string>());
            _writer = new IndexWriter(Directory, Analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
        }

        public override void AddToLuceneIndex(T sampleData, IndexWriter writer)
        {
            try
            {
                var searchQuery = new TermQuery(new Term("MongoId", sampleData.Id.Trim()));
                writer.DeleteDocuments(searchQuery);
                var doc = new Document();

                doc.Add(new Field("CompositeName", sampleData.CompositeName == null ? string.Empty : sampleData.CompositeName.Trim(), Field.Store.YES, Field.Index.ANALYZED) { Boost = 10 });
                doc.Add(new Field("MongoId", sampleData.Id == null ? string.Empty : sampleData.Id.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("SubstanceName", sampleData.SubstanceName == null ? string.Empty : sampleData.SubstanceName.Trim(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("RouteName", sampleData.RouteName == null ? string.Empty : sampleData.RouteName.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("DosageFormName", sampleData.DosageFormname == null ? string.Empty : sampleData.DosageFormname.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("Strength", sampleData.Strength == null ? string.Empty : sampleData.Strength.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("Unit", sampleData.Unit == null ? string.Empty : sampleData.Unit.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));

                writer.AddDocument(doc);
                writer.Commit();
                writer.Optimize();
            }
            catch (Exception ex)
            {
                throw new Exception("AD:MedNameLuceneStrategy:AddToLuceneIndex(T sampleData, IndexWriter writer)::" + ex.Message, ex.InnerException);
            }
        }

        public override void AddUpdateLuceneIndex(IEnumerable<T> sampleDatas)
        {
            try
            {
                foreach (var sampleData in sampleDatas) AddToLuceneIndex(sampleData, _writer);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:MedNameLuceneStrategy:AddToLuceneIndex()::" + ex.Message, ex.InnerException);
            }
            finally
            {
                IndexWriter.Unlock(Directory);
            }
        }

        public override void AddUpdateLuceneIndex(T sampleData)
        {
            try
            {
                AddUpdateLuceneIndex(new List<T> {sampleData});
            }
            catch (Exception ex)
            {
                throw new Exception("AD:MedNameLuceneStrategy:AddUpdateLuceneIndex()::" + ex.Message, ex.InnerException);
            }
        }


        public override List<TT> ExecuteSearch(string searchQuery, string searchField = "")
        {
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<TT>();

            searchQuery = ParseSpecialCharacters(searchQuery);

            IndexReader rdr = IndexReader.Open(Directory, true);
            IndexSearcher searcher = new IndexSearcher(rdr);
            BooleanQuery.MaxClauseCount = 1024*32;
            const int hitsLimit = 1000;

            if (!string.IsNullOrEmpty(searchField))
            {
                var parser = new QueryParser(Version.LUCENE_30, searchField, Analyzer)
                {
                    AllowLeadingWildcard = true,
                    PhraseSlop = 0
                };
                var query = ParseWholeQuery(searchQuery, parser);
                var hits = searcher.Search(query, hitsLimit).ScoreDocs;
                var results = MapLuceneToDataList(hits, searcher);
                searcher.Dispose();
                return results;
            }
            else
            {
                var fields = new[] {"CompositeName", "SubstanceName"};
                var parser = new MultiFieldQueryParser(Version.LUCENE_30, fields, Analyzer)
                {
                    DefaultOperator = QueryParser.Operator.AND,
                    PhraseSlop = 0
                };

                var query = ParseWholeQueryWc(searchQuery.ToLower(), fields, parser);
                var hits = searcher.Search(query, null, hitsLimit, Sort.RELEVANCE).ScoreDocs;

                var results = MapLuceneToDataList(hits, searcher);
                searcher.Dispose();
                return results;
            }
        }
    }
}
