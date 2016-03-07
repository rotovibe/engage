using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Phytel.API.DataDomain.Search.DTO;
using ServiceStack.Common;
using ServiceStack.Common.Extensions;
using Version = Lucene.Net.Util.Version;

namespace DataDomain.Search.Repo.LuceneStrategy
{
    public class MedNameLuceneStrategy<T, TT> : BaseLuceneStrategy<T, TT>, IMedNameLuceneStrategy<T, TT> where T : MedNameSearchDocData where TT : TextValuePair
    {
        protected readonly string SearchIndexPath = ConfigurationManager.AppSettings["SearchIndexPath"];
        private readonly string indexPath = ConfigurationManager.AppSettings["SearchIndexPath"] + "\\";
        private readonly string mednameIndex = @"\MedNames_Index";

        public string Contract { get; set; }
        public StandardAnalyzer SAnalyzer { get; set; }
        private readonly Dictionary<string, IndexWriter> _writerPool;

        public MedNameLuceneStrategy()
        {
            SAnalyzer = new StandardAnalyzer(Version.LUCENE_30, new HashSet<string>());
            _writerPool = new Dictionary<string, IndexWriter>();
            ManageWriterPool(_writerPool, HostContext.Instance.Items["Contract"].ToString(), indexPath, mednameIndex);
        }

        public void ManageWriterPool(Dictionary<string, IndexWriter> pool, string contract, string path, string namedIndex)
        {
            var dirs = new DirectoryInfo(indexPath).GetDirectories();
            dirs.ForEach(r =>
            {
                if (!pool.ContainsKey(r.Name.ToLower()))
                {
                    pool.Add(r.Name.ToLower(), new IndexWriter(GetDirectory(path + r.Name + namedIndex), SAnalyzer, IndexWriter.MaxFieldLength.UNLIMITED));
                }
            });
        }

        public void Delete(T sampleData, IndexWriter writer)
        {
            try
            {
                List<Query> str = new List<Query>();
                str.Add(new TermQuery(new Term("MongoId", sampleData.Id == null ? string.Empty : sampleData.Id.Trim())));
                writer.DeleteDocuments(str.ToArray());
                writer.Commit();
                writer.Optimize();
            }
            catch (Exception ex)
            {
                throw new Exception("AD:MedNameLuceneStrategy:Delete(T sampleData, IndexWriter writer)::" + ex.Message,
                    ex.InnerException);
            }
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

        public void DeleteFromIndex(IEnumerable<T> sampleDatas)
        {
            try
            {
                foreach (var sampleData in sampleDatas) Delete(sampleData, _writerPool[HostContext.Instance.Items["Contract"].ToString().ToLower()]);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:MedNameLuceneStrategy:DeleteFromIndex()::" + ex.Message, ex.InnerException);
            }
            finally
            {
                IndexWriter.Unlock(_writerPool[HostContext.Instance.Items["Contract"].ToString().ToLower()].Directory);
            }
        }

        public override void AddUpdateLuceneIndex(IEnumerable<T> sampleDatas)
        {
            try
            {
                //foreach (var sampleData in sampleDatas) AddToLuceneIndex(sampleData, _writer);
                foreach (var sampleData in sampleDatas) AddToLuceneIndex(sampleData, _writerPool[HostContext.Instance.Items["Contract"].ToString().ToLower()]);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:MedNameLuceneStrategy:AddToLuceneIndex()::" + ex.Message, ex.InnerException);
            }
            finally
            {
                IndexWriter.Unlock(_writerPool[HostContext.Instance.Items["Contract"].ToString().ToLower()].Directory);
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
            try
            {
                if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<TT>();

                searchQuery = ParseSpecialCharacters(searchQuery);

                IndexReader rdr;
                try
                {
                    rdr = IndexReader.Open(_writerPool[HostContext.Instance.Items["Contract"].ToString().ToLower()].Directory,
                        true);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(
                        "ExecuteSearch(): Could not find contract name in the WriterPool." + ex.Message, ex.StackTrace);
                }

                IndexSearcher searcher = new IndexSearcher(rdr);
                BooleanQuery.MaxClauseCount = 1024*32;
                const int hitsLimit = 1000;

                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Version.LUCENE_30, searchField, SAnalyzer)
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
                    var parser = new MultiFieldQueryParser(Version.LUCENE_30, fields, SAnalyzer)
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
            catch (Exception ex)
            {
                throw new ArgumentException("ExecuteSearch():" + ex.Message, ex.StackTrace);
            }
        }

        public override string LuceneDir
        {
            get { throw new NotImplementedException(); }
        }
    }
}
