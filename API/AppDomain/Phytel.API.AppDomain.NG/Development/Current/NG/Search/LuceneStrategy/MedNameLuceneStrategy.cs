using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public class MedNameLuceneStrategy<T, TT> : BaseLuceneStrategy<T, TT>, IMedNameLuceneStrategy<T, TT> where T : MedNameSearchDoc where TT : TextValuePair
    {
        protected static readonly string SearchIndexPath = ConfigurationManager.AppSettings["SearchIndexPath"];
        public override string LuceneDir{ get { return SearchIndexPath + "\\" + Contract + "\\" + @"\MedNames_Index"; } }
        public string Contract { get; set; }
        public StandardAnalyzer Analyzer { get; set; }
        private IndexWriter _writer;

        public MedNameLuceneStrategy()
        {
            Contract = "InHealth001";
            Analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30, new HashSet<string>());
            _writer = new IndexWriter(Directory, Analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
        }

        public override void AddToLuceneIndex(T sampleData, IndexWriter writer)
        {
            try
            {
                var searchQuery = new TermQuery(new Term("MongoId", sampleData.Id.Trim()));
                writer.DeleteDocuments(searchQuery);
                var doc = new Document();
                doc.Add(new Field("MongoId", sampleData.Id == null ? string.Empty : sampleData.Id.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("CompositeName", sampleData.CompositeName == null ? string.Empty : sampleData.CompositeName.Trim(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("SubstanceName", sampleData.SubstanceName == null ? string.Empty : sampleData.SubstanceName.Trim(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("RouteName", sampleData.RouteName == null ? string.Empty : sampleData.RouteName.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("DosageFormName", sampleData.DosageFormname == null ? string.Empty : sampleData.DosageFormname.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("Strength", sampleData.Strength == null ? string.Empty : sampleData.Strength.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("Unit", sampleData.Unit == null ? string.Empty : sampleData.Unit.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));

                writer.AddDocument(doc);
                writer.Commit();
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

            using (var searcher = new IndexSearcher(Directory, true))
            {
                BooleanQuery.MaxClauseCount = 1024 * 32;
                var hits_limit = 1000;
                //var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30, new HashSet<string>());

                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, searchField, Analyzer)
                    {
                        AllowLeadingWildcard = true,
                        PhraseSlop = 0
                    };
                    var query = ParseWholeQuery(searchQuery, parser);
                    var hits = searcher.Search(query, hits_limit).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    //_analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
                else
                {
                    var fields = new[] {"CompositeName", "SubstanceName"};
                    var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, fields, Analyzer)
                    {
                        DefaultOperator = QueryParser.Operator.AND,
                        PhraseSlop = 0
                    };
                    var query = ParseWholeQueryWc(searchQuery.ToLower(), fields, parser);
                    var hits = searcher.Search(query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;

                    var results = MapLuceneToDataList(hits, searcher);
                    //Analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
            }
        }
    }
}
