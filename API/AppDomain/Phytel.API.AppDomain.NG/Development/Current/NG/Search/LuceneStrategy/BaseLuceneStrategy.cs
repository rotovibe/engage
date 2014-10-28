using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.AppDomain.NG.Search.LuceneStrategy
{
    public abstract class BaseLuceneStrategy<T>
    {
        public abstract string LuceneDir { get; }
        public abstract void AddToLuceneIndex(T sampleData, IndexWriter writer);
        public abstract List<IdNamePair> ExecuteSearch(string searchQuery, string searchField = "");
        public abstract void AddUpdateLuceneIndex(IEnumerable<T> sampleDatas);
        public abstract void AddUpdateLuceneIndex(T sampleData);
        public FSDirectory DirectoryTemp;


        public FSDirectory Directory
        {
            get
            {
                if (!System.IO.Directory.Exists(LuceneDir))
                {
                    var filePath = new FileInfo(LuceneDir);
                    filePath.Directory.Create();
                }

                if (DirectoryTemp == null) DirectoryTemp = FSDirectory.Open(new DirectoryInfo(LuceneDir));
                if (IndexWriter.IsLocked(DirectoryTemp)) IndexWriter.Unlock(DirectoryTemp);
                var lockFilePath = Path.Combine(LuceneDir, "write.lock");
                if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                return DirectoryTemp;
            }
        }

        public void ClearLuceneIndexRecord(int recordId)
        {
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var searchQuery = new TermQuery(new Term("Id", recordId.ToString()));
                writer.DeleteDocuments(searchQuery);

                analyzer.Close();
                writer.Dispose();
            }
        }

        public bool ClearLuceneIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                using (var writer = new IndexWriter(Directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.DeleteAll();
                    analyzer.Close();
                    writer.Dispose();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void Optimize()
        {
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
        }

        public List<IdNamePair> MapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(MapLuceneDocumentToData).ToList();
        }

        public List<IdNamePair> MapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => MapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }

        public IdNamePair MapLuceneDocumentToData(Document doc)
        {
            return AutoMapper.Mapper.Map<IdNamePair>(doc);
        }

        public Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        public List<IdNamePair> SearchComplex(string input, string fieldName = "")
        {
            if (string.IsNullOrEmpty(input)) return new List<IdNamePair>();

            // added to remove symbol
            input = input.Replace("^", " ");

            var terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);

            return ExecuteSearch(input, fieldName);
        }

        public List<IdNamePair> Search(string input, string fieldName = "")
        {
            return string.IsNullOrEmpty(input) ? new List<IdNamePair>() : ExecuteSearch(input, fieldName);
        }
    }
}
