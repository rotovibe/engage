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
    public class MedFieldsLuceneStrategy<T, TT> : BaseLuceneStrategy<T, TT>, IMedFieldsLuceneStrategy<T, TT> where T : MedFieldsSearchDoc where TT : MedFieldsSearchDoc
    {
        protected static readonly string SearchIndexPath = ConfigurationManager.AppSettings["SearchIndexPath"];
        public string Contract { get; set; }
        public KeywordAnalyzer Analyzer { get; set; }
        private IndexSearcher _searcher;
        private IndexWriter _writer;

        public override string LuceneDir
        {
            get { return SearchIndexPath + "\\" + Contract + "\\" + @"\MedFields_Index"; }
        }

        public MedFieldsLuceneStrategy()
        {
            Contract = "InHealth001";
            Analyzer = new KeywordAnalyzer();
            _writer = new IndexWriter(Directory, Analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            _searcher = new IndexSearcher(Directory, false);
        }

        public override void AddToLuceneIndex(T sampleData, IndexWriter writer)
        {
            try
            {
                if (writer == null) throw new Exception("IndexWriter is null.");
                if (sampleData == null) throw new Exception("sampledata is null.");

                var boolQuery = new BooleanQuery();
                var sq1 = new TermQuery(new Term("MongoId", sampleData.ProductId));
                var sq2 = new TermQuery(new Term("DosageFormname", sampleData.DosageFormname));
                var sq3 = new TermQuery(new Term("ProprietaryName", sampleData.ProprietaryName));
                var sq4 = new TermQuery(new Term("RouteName", sampleData.RouteName));
                var sq5 = new TermQuery(new Term("SubstanceName", sampleData.SubstanceName));
                var sq6 = new TermQuery(new Term("Strength", sampleData.Strength));
                var sq8 = new TermQuery(new Term("CompositeName", sampleData.CompositeName));
                var sq9 = new TermQuery(new Term("PackageId", sampleData.ProductId));
                var sq7 = new TermQuery(new Term("Unit", string.Empty));
                boolQuery.Add(sq1, Occur.MUST);
                boolQuery.Add(sq2, Occur.MUST);
                boolQuery.Add(sq3, Occur.MUST);
                boolQuery.Add(sq4, Occur.MUST);
                boolQuery.Add(sq5, Occur.MUST);
                boolQuery.Add(sq6, Occur.MUST);
                boolQuery.Add(sq7, Occur.MUST);
                boolQuery.Add(sq8, Occur.MUST);
                boolQuery.Add(sq9, Occur.MUST);
                try
                {
                    writer.DeleteDocuments(boolQuery);
                }
                catch (Exception ex)
                {
                    throw new Exception("_writer.DeleteDocument() Failed" + ex.Message, ex.InnerException);
                }

                var doc = new Document();
                try
                {

                    if (sampleData.Id == null)
                        doc.Add(new Field("MongoId", string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    else doc.Add(new Field("MongoId", sampleData.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));
                }
                catch (Exception ex)
                {
                    throw new Exception("MongoId field creation Failed" + sampleData.Id + ex.Message, ex.InnerException);
                }

                try
                {
                    if (sampleData.ProductId == null) doc.Add(new Field("PackageId", string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    else doc.Add(new Field("PackageId", sampleData.ProductId, Field.Store.YES, Field.Index.NOT_ANALYZED));
                }
                catch (Exception ex)
                {
                    throw new Exception("PackageId field creation Failed - " + sampleData.ProductId + ex.Message,
                        ex.InnerException);
                }

                try
                {
                    if (sampleData.DosageFormname == null)
                        doc.Add(new Field("DosageFormname", string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    else
                        doc.Add(new Field("DosageFormname", sampleData.DosageFormname, Field.Store.YES,
                            Field.Index.NOT_ANALYZED));
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        "DosageFormname field creation Failed" + sampleData.DosageFormname + ex.Message,
                        ex.InnerException);
                }

                try
                {
                    if (sampleData.CompositeName == null)
                        doc.Add(new Field("CompositeName", string.Empty, Field.Store.YES, Field.Index.ANALYZED));
                    else
                        doc.Add(new Field("CompositeName", sampleData.CompositeName, Field.Store.YES, Field.Index.ANALYZED));
                }
                catch (Exception ex)
                {
                    throw new Exception("CompositeName field creation Failed" + sampleData.CompositeName + ex.Message,
                        ex.InnerException);
                }

                try
                {
                    if (sampleData.ProprietaryName == null)
                        doc.Add(new Field("ProprietaryName", string.Empty, Field.Store.YES, Field.Index.ANALYZED));
                    else
                        doc.Add(new Field("ProprietaryName", sampleData.ProprietaryName, Field.Store.YES,
                            Field.Index.ANALYZED));
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        "ProprietaryName field creation Failed" + sampleData.ProprietaryName + ex.Message,
                        ex.InnerException);
                }

                try
                {
                    if (sampleData.RouteName == null)
                        doc.Add(new Field("RouteName", string.Empty, Field.Store.YES, Field.Index.ANALYZED));
                    else doc.Add(new Field("RouteName", sampleData.RouteName, Field.Store.YES, Field.Index.ANALYZED));
                }
                catch (Exception ex)
                {
                    throw new Exception("RouteName field creation Failed" + sampleData.RouteName + ex.Message,
                        ex.InnerException);
                }

                try
                {
                    if(sampleData.SubstanceName == null)
                        doc.Add(new Field("SubstanceName", string.Empty, Field.Store.YES, Field.Index.ANALYZED));
                    else doc.Add(new Field("SubstanceName", sampleData.SubstanceName, Field.Store.YES, Field.Index.ANALYZED));
                }
                catch (Exception ex)
                {
                    throw new Exception("SubstanceName field creation Failed"+ sampleData.SubstanceName + ex.Message, ex.InnerException);
                }
                try
                {
                    if(sampleData.Strength == null)
                        doc.Add(new Field("Strength", string.Empty, Field.Store.YES, Field.Index.ANALYZED));
                    else doc.Add(new Field("Strength", sampleData.Strength, Field.Store.YES, Field.Index.ANALYZED));
                }
                catch (Exception ex)
                {
                    throw new Exception("Strength field creation Failed"+ sampleData.Strength + ex.Message, ex.InnerException);
                }
                try
                {
                    doc.Add(new Field("Unit", string.Empty, Field.Store.YES, Field.Index.ANALYZED));
                }
                catch (Exception ex)
                {
                    throw new Exception("Unit field creation Failed" + ex.Message, ex.InnerException);
                }

                try
                {
                    writer.AddDocument(doc);
                }
                catch (Exception ex)
                {
                    throw new Exception("_writer.AddDocument() Failed" + ex.Message, ex.InnerException);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "AD:MedFieldsLuceneStrategy:AddToLuceneIndex(T sampleData, IndexWriter writer)::" + ex.Message +
                    "::" + ex.StackTrace,
                    ex.InnerException);
            }
        }

        public override void AddUpdateLuceneIndex(IEnumerable<T> sampleDatas)
        {
            try
            {
                foreach (var sampleData in sampleDatas) AddToLuceneIndex(sampleData, _writer);
                try{_writer.Commit();}
                catch (Exception ex){throw new Exception("_writer.Commit() Failed " + ex.Message + ex.StackTrace, ex.InnerException); }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "AD:MedFieldsLuceneStrategy:AddUpdateLuceneIndex(IEnumerable<T> sampleDatas)::" + ex.Message,
                    ex.InnerException);
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
                throw new Exception("AD:MedFieldsLuceneStrategy:AddUpdateLuceneIndex()::" + ex.Message, ex.InnerException);
            }
        }


        public override List<TT> ExecuteSearch(string searchQuery, string searchField = "")
        {
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<TT>();

            try
            {
                searchQuery = "\"" + searchQuery + "\"";
                using (var searcher = new IndexSearcher(Directory, false))
                {
                    var hits_limit = 1000;
                    // ignore stop words
                    //var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30, CharArraySet.EMPTY_SET);
                    //var analyzer = new StandardAnalyzer();

                    if (!string.IsNullOrEmpty(searchField))
                    {
                        var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, searchField, Analyzer);
                        parser.PhraseSlop = 0;
                        var query = ParseWholeQuery(searchQuery, parser);
                        var hits = searcher.Search(query, hits_limit).ScoreDocs;
                        var results = MapLuceneToDataList(hits, searcher);
                        //analyzer.Close();
                        searcher.Dispose();
                        return results;
                    }
                    else
                    {
                        var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30,
                            new[] {"ProprietaryName", "SubstanceName"}, Analyzer);
                        parser.PhraseSlop = 0;
                        var query = ParseQuery(searchQuery, parser);
                        var hits = searcher.Search
                            (query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                        var results = MapLuceneToDataList(hits, searcher);
                        //analyzer.Close();
                        searcher.Dispose();
                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:MedFieldsLuceneStrategy:ExecuteSearch()::" + ex.Message + ex.StackTrace, ex.InnerException);
            }
        }
    }
}
