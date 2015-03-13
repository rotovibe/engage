﻿using System;
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
using Phytel.API.DataDomain.Search.DTO;
using ServiceStack.Common;
using Version = Lucene.Net.Util.Version;

namespace DataDomain.Search.Repo.LuceneStrategy
{
    public class AllergyLuceneStrategy<T, TT> : BaseLuceneStrategy<T, TT>, IAllergyLuceneStrategy<T, TT> where T : IdNamePair where TT : IdNamePair
    {
        public string Contract { get; set; }

        protected readonly string SearchIndexPath = ConfigurationManager.AppSettings["SearchIndexPath"];
        private readonly string indexPath = ConfigurationManager.AppSettings["SearchIndexPath"] + "\\";
        private readonly string allergyIndex = @"\Allergy_Index";

        public StandardAnalyzer Analyzer { get; set; }
        private IndexWriter _writer;
        private readonly Dictionary<string, IndexWriter> _writerPool;

        public override string LuceneDir
        {
            get { return SearchIndexPath + "\\" + Contract + "\\" + @"\Allergy_Index"; }
        }

        public AllergyLuceneStrategy()
        {
            Analyzer = new StandardAnalyzer(Version.LUCENE_30, new HashSet<string>());
            Contract = HostContext.Instance.Items["Contract"].ToString();
        }


        public override void AddToLuceneIndex(T sampleData, IndexWriter writer)
        {
            var searchQuery = new TermQuery(new Term("Id", sampleData.Id));
            writer.DeleteDocuments(searchQuery);
            var doc = new Document();
            doc.Add(new Field("Id", sampleData.Id.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Name", sampleData.Name.Trim(), Field.Store.YES, Field.Index.ANALYZED));

            writer.AddDocument(doc);
        }

        public override void AddUpdateLuceneIndex(IEnumerable<T> sampleDatas)
        {
            try
            {
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    foreach (var sampleData in sampleDatas)
                        AddToLuceneIndex(sampleData, writer);

                    analyzer.Close();
                    writer.Dispose();
                }
            }
            catch (Exception ex)
            {
                FileLog.LogMessageToFile(ex.Message + " trace:" + ex.StackTrace);
                throw ex;
            }
        }

        public override void AddUpdateLuceneIndex(T sampleData)
        {
            AddUpdateLuceneIndex(new List<T> { sampleData });
        }


        public override List<TT> ExecuteSearch(string searchQuery, string searchField = "")
        {
            try
            {
                FileLog.LogMessageToFile(Directory.Directory.FullName);

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
                        var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, new[] {"Id", "Name"}, analyzer);
                        parser.AllowLeadingWildcard = true;
                        parser.PhraseSlop = 0;
                        var query = ParseWholeQuery(searchQuery, parser);
                        var hits = searcher.Search(query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                        var results = MapLuceneToDataList(hits, searcher);
                        analyzer.Close();
                        searcher.Dispose();
                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                FileLog.LogMessageToFile(ex.Message + " trace:" + ex.StackTrace);
                throw ex;
            }
        }

    }
}
