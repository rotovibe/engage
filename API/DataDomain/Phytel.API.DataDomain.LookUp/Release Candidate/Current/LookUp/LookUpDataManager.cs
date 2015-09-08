using System.Collections.Generic;
using System.Linq;
using Phytel.API.Interface;
using System;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.LookUp
{
    public class LookUpDataManager : ILookUpDataManager
    {
        public ILookUpRepositoryFactory Factory { get; set; }
        #region Lookups
        #region Problems
        public GetProblemDataResponse GetProblem(GetProblemDataRequest request)
        {
            GetProblemDataResponse response = new GetProblemDataResponse();
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.LookUp);
            response.Problem = repo.FindProblemByID(request.ProblemID) as ProblemData;
            return response;
        }

        public GetAllProblemsDataResponse GetAllProblems(GetAllProblemsDataRequest request)
        {
            GetAllProblemsDataResponse response = new GetAllProblemsDataResponse();
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.LookUp);
            List<ProblemData> problems = repo.GetAllProblems();

            if (problems != null)
            {
                response.Problems = problems;
            }
            return response;
        }

        public SearchProblemsDataResponse SearchProblem(SearchProblemsDataRequest request)
        {
            SearchProblemsDataResponse response = new SearchProblemsDataResponse(); 
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.LookUp);
            List<ProblemData> problems = repo.SearchProblem(request);

            if (problems != null)
            {
                response.Problems = problems;
            }
            return response;
        }
        #endregion

        #region Objective
        public GetObjectiveDataResponse GetObjectiveByID(GetObjectiveDataRequest request)
        {
            GetObjectiveDataResponse result = new GetObjectiveDataResponse();
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.LookUp);
            result.Objective = repo.FindObjectiveByID(request.ObjectiveID) as IdNamePair;

            return (result != null ? result : new GetObjectiveDataResponse());
        }
        #endregion

        #region Category
        public GetCategoryDataResponse GetCategoryByID(GetCategoryDataRequest request)
        {
            GetCategoryDataResponse result = new GetCategoryDataResponse();
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.LookUp);
            result.Category = repo.FindCategoryByID(request.CategoryID) as IdNamePair;

            return (result != null ? result : new GetCategoryDataResponse());
        }
        #endregion

        #region Contact Related LookUps
        public GetAllCommModesDataResponse GetAllCommModes(GetAllCommModesDataRequest request)
        {
            GetAllCommModesDataResponse response = new GetAllCommModesDataResponse();
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.LookUp);
            List<IdNamePair> data = repo.GetAllCommModes();

            if (data != null)
            {
                response.CommModes = data;
            }
            return response;
        }

        public GetAllStatesDataResponse GetAllStates(GetAllStatesDataRequest request)
        {
            GetAllStatesDataResponse response = new GetAllStatesDataResponse();
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.LookUp); 
            List<StateData> data = repo.GetAllStates();

            if (data != null)
            {
                response.States = data;
            }
            return response;
        }

        public GetAllTimesOfDaysDataResponse GetAllTimesOfDays(GetAllTimesOfDaysDataRequest request)
        {
            GetAllTimesOfDaysDataResponse response = new GetAllTimesOfDaysDataResponse();
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.LookUp);
            List<IdNamePair> data = repo.GetAllTimesOfDays();

            if (data != null)
            {
                response.TimesOfDays = data;
            }
            return response;
        }

        public GetAllTimeZonesDataResponse GetAllTimeZones(GetAllTimeZonesDataRequest request)
        {
            GetAllTimeZonesDataResponse response = new GetAllTimeZonesDataResponse();
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.LookUp);
            List<TimeZoneData> data = repo.GetAllTimeZones();

            if (data != null)
            {
                response.TimeZones = data;
            }
            return response;
        }

        public GetAllCommTypesDataResponse GetAllCommTypes(GetAllCommTypesDataRequest request)
        {
            GetAllCommTypesDataResponse response = new GetAllCommTypesDataResponse();
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.LookUp);
            List<CommTypeData> data = repo.GetAllCommTypes();

            if (data != null)
            {
                response.CommTypes = data;
            }
            return response;
        }

        public GetAllLanguagesDataResponse GetAllLanguages(GetAllLanguagesDataRequest request)
        {
            GetAllLanguagesDataResponse response = new GetAllLanguagesDataResponse();
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.LookUp);
            List<LanguageData> data = repo.GetAllLanguages();

            if (data != null)
            {
                response.Languages = data;
            }
            return response;
        }

        public GetTimeZoneDataResponse GetDefaultTimeZone(GetTimeZoneDataRequest request)
        {
            GetTimeZoneDataResponse response = new GetTimeZoneDataResponse();
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.LookUp);
            TimeZoneData data = repo.GetDefaultTimeZone();

            if (data != null)
            {
                response.TimeZone = data;
            }
            return response;
        }
        #endregion

        #region Refatored LookUps
        public GetLookUpsDataResponse GetLookUpsByType(GetLookUpsDataRequest request)
        {
            GetLookUpsDataResponse response = new GetLookUpsDataResponse();
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.LookUp);
            List<IdNamePair> data = repo.GetLookps(request.Name);

            if (data != null)
            {
                response.LookUpsData = data;
            }
            return response;
        }

        public GetLookUpDetailsDataResponse GetLookUpDetails(GetLookUpDetailsDataRequest request)
        {
            GetLookUpDetailsDataResponse response = new GetLookUpDetailsDataResponse();
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.LookUp);
            List<LookUpDetailsData> data = repo.GetLookUpDetails(request.Name);

            if (data != null)
            {
                response.LookUpDetailsData = data;
            }
            return response;
        }
        #endregion

        #region Program
        public GetAllObjectivesDataResponse GetAllObjectives(GetAllObjectivesDataRequest request)
        {
            GetAllObjectivesDataResponse response = new GetAllObjectivesDataResponse();
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.LookUp);
            List<ObjectiveData> data = repo.GetAllObjectives();

            if (data != null)
            {
                response.ObjectivesData = data;
            }
            return response;
        }
        #endregion 
        #endregion

        #region Settings
        public GetAllSettingsDataResponse GetAllSettings(GetAllSettingsDataRequest request)
        {
            GetAllSettingsDataResponse response = new GetAllSettingsDataResponse();
            ILookUpRepository repo = Factory.GetRepository(request, RepositoryType.Setting);
            List<MESetting> list = repo.SelectAll() as List<MESetting>;
            if (list != null && list.Count > 0)
            {
               Dictionary<string,string> data = new Dictionary<string,string>();
               list.ForEach(s => 
                   {
                       data.Add(s.Key, s.Value);
                   });

               response.SettingsData = data;    
            }
            return response;
        }

        #endregion
    }
}   
