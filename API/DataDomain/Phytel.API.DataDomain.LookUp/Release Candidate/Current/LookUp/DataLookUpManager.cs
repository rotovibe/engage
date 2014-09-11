using System.Collections.Generic;
using System.Linq;
using Phytel.API.Interface;
using System;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.LookUp
{
    public static class LookUpDataManager
    {
        #region Problems
        public static GetProblemDataResponse GetProblem(GetProblemDataRequest request)
        {
            GetProblemDataResponse response = new GetProblemDataResponse();

            ILookUpRepository<GetProblemDataResponse> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<GetProblemDataResponse>.GetLookUpRepository(request.ContractNumber, request.Context, request.UserId);
            
            response.Problem = repo.FindProblemByID(request.ProblemID) as ProblemData;
            return response;
        }

        public static GetAllProblemsDataResponse GetAllProblems(GetAllProblemsDataRequest request)
        {
            GetAllProblemsDataResponse response = new GetAllProblemsDataResponse();

            ILookUpRepository<ProblemData> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<ProblemData>.GetLookUpRepository(request.ContractNumber, request.Context, request.UserId);

            List<ProblemData> problems = repo.GetAllProblems();

            if (problems != null)
            {
                response.Problems = problems;
            }
            return response;
        }

        public static SearchProblemsDataResponse SearchProblem(SearchProblemsDataRequest request)
        {
            SearchProblemsDataResponse response = new SearchProblemsDataResponse();

            ILookUpRepository<ProblemData> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<ProblemData>.GetLookUpRepository(request.ContractNumber, request.Context, request.UserId);

            List<ProblemData> problems = repo.SearchProblem(request);

            if (problems != null)
            {
                response.Problems = problems;
            }
            return response;
        } 
        #endregion

        #region Objective
        public static GetObjectiveDataResponse GetObjectiveByID(GetObjectiveDataRequest request)
        {
            GetObjectiveDataResponse result = new GetObjectiveDataResponse();

            ILookUpRepository<GetObjectiveDataResponse> repo = LookUpRepositoryFactory<GetObjectiveDataResponse>.GetLookUpRepository(request.ContractNumber, request.Context, request.UserId);

            result.Objective = repo.FindObjectiveByID(request.ObjectiveID) as IdNamePair;

            return (result != null ? result : new GetObjectiveDataResponse());
        } 
        #endregion

        #region Category
        public static GetCategoryDataResponse GetCategoryByID(GetCategoryDataRequest request)
        {
            GetCategoryDataResponse result = new GetCategoryDataResponse();

            ILookUpRepository<GetCategoryDataResponse> repo = LookUpRepositoryFactory<GetCategoryDataResponse>.GetLookUpRepository(request.ContractNumber, request.Context, request.UserId);

            result.Category = repo.FindCategoryByID(request.CategoryID) as IdNamePair;

            return (result != null ? result : new GetCategoryDataResponse());
        } 
        #endregion

        #region Contact Related LookUps
        public static GetAllCommModesDataResponse GetAllCommModes(GetAllCommModesDataRequest request)
        {
            GetAllCommModesDataResponse response = new GetAllCommModesDataResponse();

            ILookUpRepository<IdNamePair> repo = LookUpRepositoryFactory<IdNamePair>.GetLookUpRepository(request.ContractNumber, request.Context, request.UserId);

            List<IdNamePair> data = repo.GetAllCommModes();

            if (data != null)
            {
                response.CommModes = data;
            }
            return response;
        }

        public static GetAllStatesDataResponse GetAllStates(GetAllStatesDataRequest request)
        {
            GetAllStatesDataResponse response = new GetAllStatesDataResponse();

            ILookUpRepository<StateData> repo = LookUpRepositoryFactory<StateData>.GetLookUpRepository(request.ContractNumber, request.Context, request.UserId);

            List<StateData> data = repo.GetAllStates();

            if (data != null)
            {
                response.States = data;
            }
            return response;
        }

        public static GetAllTimesOfDaysDataResponse GetAllTimesOfDays(GetAllTimesOfDaysDataRequest request)
        {
            GetAllTimesOfDaysDataResponse response = new GetAllTimesOfDaysDataResponse();

            ILookUpRepository<IdNamePair> repo = LookUpRepositoryFactory<IdNamePair>.GetLookUpRepository(request.ContractNumber, request.Context, request.UserId);

            List<IdNamePair> data = repo.GetAllTimesOfDays();

            if (data != null)
            {
                response.TimesOfDays = data;
            }
            return response;
        }

        public static GetAllTimeZonesDataResponse GetAllTimeZones(GetAllTimeZonesDataRequest request)
        {
            GetAllTimeZonesDataResponse response = new GetAllTimeZonesDataResponse();

            ILookUpRepository<TimeZoneData> repo = LookUpRepositoryFactory<TimeZoneData>.GetLookUpRepository(request.ContractNumber, request.Context, request.UserId);

            List<TimeZoneData> data = repo.GetAllTimeZones();

            if (data != null)
            {
                response.TimeZones = data;
            }
            return response;
        }

        public static GetAllCommTypesDataResponse GetAllCommTypes(GetAllCommTypesDataRequest request)
        {
            GetAllCommTypesDataResponse response = new GetAllCommTypesDataResponse();

            ILookUpRepository<CommTypeData> repo = LookUpRepositoryFactory<CommTypeData>.GetLookUpRepository(request.ContractNumber, request.Context, request.UserId);

            List<CommTypeData> data = repo.GetAllCommTypes();

            if (data != null)
            {
                response.CommTypes = data;
            }
            return response;
        }

        public static GetAllLanguagesDataResponse GetAllLanguages(GetAllLanguagesDataRequest request)
        {
            GetAllLanguagesDataResponse response = new GetAllLanguagesDataResponse();

            ILookUpRepository<LanguageData> repo = LookUpRepositoryFactory<LanguageData>.GetLookUpRepository(request.ContractNumber, request.Context, request.UserId);

            List<LanguageData> data = repo.GetAllLanguages();

            if (data != null)
            {
                response.Languages = data;
            }
            return response;
        }

        public static GetTimeZoneDataResponse GetDefaultTimeZone(GetTimeZoneDataRequest request)
        {
            GetTimeZoneDataResponse response = new GetTimeZoneDataResponse();

            ILookUpRepository<TimeZoneData> repo = LookUpRepositoryFactory<TimeZoneData>.GetLookUpRepository(request.ContractNumber, request.Context, request.UserId);

            TimeZoneData data = repo.GetDefaultTimeZone();

            if (data != null)
            {
                response.TimeZone = data;
            }
            return response;
        }
        #endregion

        #region Refatored LookUps
        public static GetLookUpsDataResponse GetLookUpsByType(GetLookUpsDataRequest request)
        {
            GetLookUpsDataResponse response = new GetLookUpsDataResponse();

            ILookUpRepository<IdNamePair> repo = LookUpRepositoryFactory<IdNamePair>.GetLookUpRepository(request.ContractNumber, request.Context, request.UserId);

            List<IdNamePair> data = repo.GetLookps(request.Name);

            if (data != null)
            {
                response.LookUpsData = data;
            }
            return response;
        }

        public static GetLookUpDetailsDataResponse GetLookUpDetails(GetLookUpDetailsDataRequest request)
        {
            GetLookUpDetailsDataResponse response = new GetLookUpDetailsDataResponse();

            ILookUpRepository<IdNamePair> repo = LookUpRepositoryFactory<IdNamePair>.GetLookUpRepository(request.ContractNumber, request.Context, request.UserId);

            List<LookUpDetailsData> data = repo.GetLookUpDetails(request.Name);

            if (data != null)
            {
                response.LookUpDetailsData = data;
            }
            return response;
        }
        #endregion  

        #region Program
        public static GetAllObjectivesDataResponse GetAllObjectives(GetAllObjectivesDataRequest request)
        {
            GetAllObjectivesDataResponse response = new GetAllObjectivesDataResponse();

            ILookUpRepository<ObjectiveData> repo = LookUpRepositoryFactory<ObjectiveData>.GetLookUpRepository(request.ContractNumber, request.Context, request.UserId);

            List<ObjectiveData> data = repo.GetAllObjectives();

            if (data != null)
            {
                response.ObjectivesData = data;
            }
            return response;
        } 
        #endregion
    }
}   
