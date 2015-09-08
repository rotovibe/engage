using System;
using System.Collections.Generic;
using System.Net;
using Phytel.API.DataDomain.LookUp;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.Common.Format;
using System.Configuration;
using System.Web;

namespace Phytel.API.DataDomain.Patient.Service
{
    public class LookUpService : ServiceStack.ServiceInterface.Service
    {
        public ILookUpDataManager LookUpDataManager { get; set; }
        
        #region Lookups
        #region Problems
        public GetProblemDataResponse Get(GetProblemDataRequest request)
        {
            GetProblemDataResponse response = new GetProblemDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Get()::Unauthorized Access");

                response = LookUpDataManager.GetProblem(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAllProblemsDataResponse Get(GetAllProblemsDataRequest request)
        {
            GetAllProblemsDataResponse response = new GetAllProblemsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Get()::Unauthorized Access");

                response = LookUpDataManager.GetAllProblems(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public SearchProblemsDataResponse Post(SearchProblemsDataRequest request)
        {
            SearchProblemsDataResponse response = new SearchProblemsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Post()::Unauthorized Access");

                response = LookUpDataManager.SearchProblem(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
        #endregion

        #region Objective
        public GetObjectiveDataResponse Get(GetObjectiveDataRequest request)
        {
            GetObjectiveDataResponse response = new GetObjectiveDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Get()::Unauthorized Access");

                response = LookUpDataManager.GetObjectiveByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
        #endregion

        #region Category
        public GetCategoryDataResponse Get(GetCategoryDataRequest request)
        {
            GetCategoryDataResponse response = new GetCategoryDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Get()::Unauthorized Access");

                response = LookUpDataManager.GetCategoryByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
        #endregion

        #region Contact Related LookUps
        public GetAllCommModesDataResponse Get(GetAllCommModesDataRequest request)
        {
            GetAllCommModesDataResponse response = new GetAllCommModesDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Get()::Unauthorized Access");

                response = LookUpDataManager.GetAllCommModes(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAllStatesDataResponse Get(GetAllStatesDataRequest request)
        {
            GetAllStatesDataResponse response = new GetAllStatesDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Get()::Unauthorized Access");

                response = LookUpDataManager.GetAllStates(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAllTimesOfDaysDataResponse Get(GetAllTimesOfDaysDataRequest request)
        {
            GetAllTimesOfDaysDataResponse response = new GetAllTimesOfDaysDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Get()::Unauthorized Access");

                response = LookUpDataManager.GetAllTimesOfDays(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAllTimeZonesDataResponse Get(GetAllTimeZonesDataRequest request)
        {
            GetAllTimeZonesDataResponse response = new GetAllTimeZonesDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Get()::Unauthorized Access");

                response = LookUpDataManager.GetAllTimeZones(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAllCommTypesDataResponse Get(GetAllCommTypesDataRequest request)
        {
            GetAllCommTypesDataResponse response = new GetAllCommTypesDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Get()::Unauthorized Access");

                response = LookUpDataManager.GetAllCommTypes(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAllLanguagesDataResponse Get(GetAllLanguagesDataRequest request)
        {
            GetAllLanguagesDataResponse response = new GetAllLanguagesDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Get()::Unauthorized Access");

                response = LookUpDataManager.GetAllLanguages(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetTimeZoneDataResponse Get(GetTimeZoneDataRequest request)
        {
            GetTimeZoneDataResponse response = new GetTimeZoneDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Get()::Unauthorized Access");

                response = LookUpDataManager.GetDefaultTimeZone(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }


        #endregion

        #region Refactored LookUps
        public GetLookUpsDataResponse Get(GetLookUpsDataRequest request)
        {
            GetLookUpsDataResponse response = new GetLookUpsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Get()::Unauthorized Access");

                response = LookUpDataManager.GetLookUpsByType(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetLookUpDetailsDataResponse Get(GetLookUpDetailsDataRequest request)
        {
            GetLookUpDetailsDataResponse response = new GetLookUpDetailsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Get()::Unauthorized Access");

                response = LookUpDataManager.GetLookUpDetails(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
        #endregion

        #region Program
        public GetAllObjectivesDataResponse Get(GetAllObjectivesDataRequest request)
        {
            GetAllObjectivesDataResponse response = new GetAllObjectivesDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Get()::Unauthorized Access");

                response = LookUpDataManager.GetAllObjectives(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
        #endregion 
        #endregion

        #region Settings
        public GetAllSettingsDataResponse Get(GetAllSettingsDataRequest request)
        {
            GetAllSettingsDataResponse response = new GetAllSettingsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("LookUpDD:Get()::Unauthorized Access");

                response = LookUpDataManager.GetAllSettings(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
        #endregion  
    }
}