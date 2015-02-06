using System.Collections.Generic;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Phytel.API.AppDomain.NG.DTO.Search;

namespace Phytel.API.AppDomain.NG.Search.LuceneStrategy
{
    public interface IMedFieldsLuceneStrategy<T, TT> where T : MedFieldsSearchDoc where TT : MedFieldsSearchDoc
    {
        string Contract { get; set; }
        string LuceneDir { get; }
        FSDirectory Directory { get; }
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
    }
}