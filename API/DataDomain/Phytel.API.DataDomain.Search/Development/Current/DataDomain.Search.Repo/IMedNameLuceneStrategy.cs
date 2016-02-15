using System.Collections.Generic;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Phytel.API.DataDomain.Search.DTO;

namespace DataDomain.Search.Repo.LuceneStrategy
{
    public interface IMedNameLuceneStrategy<T, TT> where T : MedNameSearchDocData where TT : TextValuePair
    {
        //string LuceneDir { get; }
        string Contract { get; set; }
        StandardAnalyzer SAnalyzer { get; set; }
        //FSDirectory Directory { get; }
        void AddToLuceneIndex(T sampleData, IndexWriter writer);
        void AddUpdateLuceneIndex(IEnumerable<T> sampleDatas);
        void AddUpdateLuceneIndex(T sampleData);
        List<TT> ExecuteSearch(string searchQuery, string searchField = "");
        void ClearLuceneIndexRecord(int recordId);
        bool ClearLuceneIndex();
        void Optimize();
        List<TT> MapLuceneToDataList(IEnumerable<Document> hits);
        List<TT> MapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher);
        TT MapLuceneDocumentToData(Document doc);
        Query ParseQuery(string searchQuery, QueryParser parser);
        Query ParseQueryWc(string searchQuery, string[] fields, QueryParser parser);
        BooleanQuery ParseWholeQueryWc(string searchQuery, string[] fields, QueryParser parser);
        Query ParseWholeQuery(string searchQuery, QueryParser parser);
        List<TT> SearchComplex(string input, string fieldName = "");
        List<TT> Search(string input, string fieldName = "");
        string ParseSpecialCharacters(string value);
        void Delete(T sampleData, IndexWriter writer);
        void DeleteFromIndex(IEnumerable<T> sampleDatas);
    }
}