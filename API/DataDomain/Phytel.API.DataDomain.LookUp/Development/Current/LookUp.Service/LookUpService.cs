using System;
using System.Collections.Generic;
using System.Net;
using Phytel.API.DataDomain.LookUp;
using Phytel.API.DataDomain.LookUp.DTO;

namespace Phytel.API.DataDomain.Patient.Service
{
    public class LookUpService : ServiceStack.ServiceInterface.Service
    {
        #region Problems
        public GetProblemDataResponse Get(GetProblemDataRequest request)
        {
            GetProblemDataResponse response = new GetProblemDataResponse();
            try
            {
                response = LookUpDataManager.GetProblem(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public GetAllProblemsDataResponse Get(GetAllProblemsDataRequest request)
        {
            GetAllProblemsDataResponse response = new GetAllProblemsDataResponse();
            try
            {
                response = LookUpDataManager.GetAllProblems(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public SearchProblemsDataResponse Post(SearchProblemsDataRequest request)
        {
            SearchProblemsDataResponse response = new SearchProblemsDataResponse();
            try
            {
                response = LookUpDataManager.SearchProblem(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
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
                response = LookUpDataManager.GetObjectiveByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
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
                response = LookUpDataManager.GetCategoryByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        } 
        #endregion
    }
}