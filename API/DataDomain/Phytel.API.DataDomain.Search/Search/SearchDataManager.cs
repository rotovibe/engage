using System.Collections.Generic;
using System.Linq;
using System;
using DataDomain.Search.Repo;
using DataDomain.Search.Repo.LuceneStrategy;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.Search.DTO;

namespace Phytel.API.DataDomain.Search
{
    public class SearchDataManager : ISearchDataManager
    {
        public IMedNameLuceneStrategy<MedNameSearchDocData, TextValuePair> MedNameStrategy { get; set; }
        public IAllergyLuceneStrategy<IdNamePair, IdNamePair> AllergyStrategy { get; set; }

        public bool InsertMedDocInIndex(PutMedRegistrationRequest request)
        {
            try
            {
                MedNameStrategy.AddUpdateLuceneIndex(request.MedDocument);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("SearchDD:GetSearchByID()::" + ex.Message, ex.InnerException);
            }
        }

        public bool DeleteMedDocs(List<MedNameSearchDocData> meds)
        {
            try
            {
                MedNameStrategy.DeleteFromIndex(meds);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("SearchDD:GetSearchByID()::" + ex.Message, ex.InnerException);
            }
        }

        public bool InsertAllergyDocInIndex(PutAllergyRegistrationRequest request)
        {
            try
            {
                AllergyStrategy.AddUpdateLuceneIndex(request.AllergyDocument);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("SearchDD:GetSearchByID()::" + ex.Message, ex.InnerException);
            }
        }

        public List<object> GetTermSearchResults(GetSearchRequest request, SearchEnum type)
        {
            try
            {
                List<object> result = null;

                switch (type)
                {

                    case SearchEnum.Medication:
                    {
                        result = MedNameStrategy.Search(request.Term).Select(s => s).ToList<object>();
                        break;
                    }
                    case SearchEnum.Allergy:
                    {
                        if (AllergyStrategy == null)
                            throw new ArgumentException("Allergystrategy is null.");

                        var matched = AllergyStrategy.SearchComplex(request.Term, "Name");
                        if (matched != null)
                        {
                            result = matched.Select(s => s).ToList<object>();
                        }

                        break;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                FileLog.LogMessageToFile("SearchDD:GetTermSearchResults()::" + ex.Message + " trace:" + ex.StackTrace);
                throw new Exception("SearchDD:GetTermSearchResults()::" + ex.Message, ex.InnerException);
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
