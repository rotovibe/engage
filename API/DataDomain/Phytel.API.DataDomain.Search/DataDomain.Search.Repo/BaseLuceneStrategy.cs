using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Contrib.Regex;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Search.Spans;
using Lucene.Net.Store;
using Phytel.API.DataDomain.Search.DTO;

namespace DataDomain.Search.Repo.LuceneStrategy
{
    public abstract class BaseLuceneStrategy<T, TT>
    {
        public abstract string LuceneDir { get; }
        public abstract void AddToLuceneIndex(T sampleData, IndexWriter writer);
        public abstract List<TT> ExecuteSearch(string searchQuery, string searchField = "");
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
                //var lockFilePath = Path.Combine(LuceneDir, "write.lock");
                //if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                return DirectoryTemp;
            }
        }

        public FSDirectory GetDirectory(string dir)
        {
            FSDirectory directoryTemp = null;

            if (!System.IO.Directory.Exists(dir))
            {
                var filePath = new FileInfo(dir);
                filePath.Directory.Create();
            }

            directoryTemp = FSDirectory.Open(new DirectoryInfo(dir));
            if (IndexWriter.IsLocked(directoryTemp)) IndexWriter.Unlock(directoryTemp);
            //var lockFilePath = Path.Combine(LuceneDir, "write.lock");
            //if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
            return directoryTemp;
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

        public List<TT> MapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(MapLuceneDocumentToData).ToList();
        }

        public List<TT> MapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => MapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }

        public List<MedNameSearchDocData> MapLuceneToDeepDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => MapDeepLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }

        public MedNameSearchDocData MapDeepLuceneDocumentToData(Document doc)
        {
            return AutoMapper.Mapper.Map<MedNameSearchDocData>(doc);
        }

        public TT MapLuceneDocumentToData(Document doc)
        {
            return AutoMapper.Mapper.Map<TT>(doc);
        }

        public Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query = new PhraseQuery();
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

        public Query ParseQueryWc(string searchQuery, string[] fields, QueryParser parser)
        {
            BooleanQuery mq = new BooleanQuery();
            try
            {
                var qrArr = searchQuery.Split(null);

                for (var i = 0; i < qrArr.Length; i++)
                {
                    mq.Add(new SpanRegexQuery(new Term(fields[0], qrArr[i] + ".*")), Occur.SHOULD);
                    mq.Add(new SpanRegexQuery(new Term(fields[1], qrArr[i] + ".*")), Occur.SHOULD);
                }
            }
            catch (ParseException ex)
            {
                throw new ArgumentException("BaseLuceneStrategy:ParseQueryWc():" + ex.Message);
            }
            return mq;
        }

        public BooleanQuery ParseWholeQueryWc(string searchQuery, string[] fields, QueryParser parser)
        {
            BooleanQuery mq = new BooleanQuery();

            try
            {
                var qrArr = searchQuery.Split(null);
                SpanQuery[] compNmQ = new SpanQuery[qrArr.Length];
                SpanQuery[] subsNmQ = new SpanQuery[qrArr.Length];

                for (var i = 0; i < qrArr.Length; i++)
                {
                    compNmQ[i] = new SpanRegexQuery(new Term(fields[0], qrArr[i] + ".*"));
                    subsNmQ[i] = new SpanRegexQuery(new Term(fields[1], qrArr[i] + ".*"));
                }

                SpanQuery compNameQ = new SpanNearQuery(compNmQ, 1, false);
                SpanQuery subsNameQ = new SpanNearQuery(subsNmQ, 1, false);

                mq.Add(compNameQ, Occur.SHOULD);
                mq.Add(subsNameQ, Occur.SHOULD);
            }
            catch (ParseException ex)
            {
                throw new ArgumentException("BaseLuceneStrategy:ParseWholeQueryWc():" + ex.Message);
            }
            return mq;
        }

        public Query ParseWholeQuery(string searchQuery, QueryParser parser)
        {
            Query query = new PhraseQuery();
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                //query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        public List<TT> SearchComplex(string input, string fieldName = "")
        {
            try
            {
                if (string.IsNullOrEmpty(input)) return new List<TT>();

                // added to remove symbol
                input = input.Replace("^", " ");

                var terms = input.Trim().Replace("-", " ").Split(' ')
                    .Where(x => !string.IsNullOrEmpty(x)).Select(x => "*" + x.Trim() + "*");

                input = string.Join(" ", terms);

                return ExecuteSearch(input, fieldName);
            }
            catch (Exception ex)
            {
                throw new Exception("SearchComplex():"+ ex.Message + ex.StackTrace);
            }
        }

        public List<TT> Search(string input, string fieldName = "")
        {
            try
            {
                return string.IsNullOrEmpty(input) ? new List<TT>() : ExecuteSearch(input, fieldName);
            }
            catch (Exception ex)
            {
                throw new Exception("Search():" + ex.Message + ex.StackTrace);
            }
        }

        public string ParseSpecialCharacters(string value)
        {
            try
            {
                var specialCharacters = new char[] {'.', ',', '/', '-', '(', ')', ';', '%','[','*'};
                var str = specialCharacters.Aggregate(value, (current, c) => current.Replace(c, ' '));
                // parse consecutive white spaces to one
                str = Regex.Replace(str, @"\s+", " ").Trim();
                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("ParseSpecialCharacters():" + ex.Message + ex.StackTrace);
            }
        }
    }
}
