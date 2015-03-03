using System.Collections.Generic;
using System.Linq;
using System;
using DataDomain.Search.Repo;
using DataDomain.Search.Repo.LuceneStrategy;
using DataDomain.Search.Repo.Search;
using Phytel.API.DataDomain.Search.DTO;

namespace Phytel.API.DataDomain.Search
{
    public class SearchDataManager : ISearchDataManager
    {
        public IMedNameLuceneStrategy<MedNameSearchDoc, TextValuePair> MedNameStrategy { get; set; }

        public List<TextValuePair> GetTermSearchResults(GetSearchRequest request)
        {
            try
            {
                List<TextValuePair> result = null;
                result = MedNameStrategy.Search(request.Term).Select(s => s).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SearchDD:GetSearchByID()::" + ex.Message, ex.InnerException);
            }
        }

        public List<DTO.Search> GetSearchList(GetAllSearchsRequest request)
        {
            try
            {
                List<DTO.Search> result = null;
                var repo = SearchRepositoryFactory.GetSearchRepository(request, RepositoryType.Search);
                result = repo.SelectAll().Cast<DTO.Search>().ToList<DTO.Search>();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SearchDD:GetSearchList()::" + ex.Message, ex.InnerException);
            }
        }
    }
}   
