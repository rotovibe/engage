using System;
using System.Collections.Generic;
using System.Net;
using Phytel.API.DataDomain.PatientProblem;
using Phytel.API.DataDomain.PatientProblem.DTO;
using System.Configuration;
using Phytel.API.Common.Format;
using System.Web;

namespace Phytel.API.DataDomain.PatientProblem.Service
{
    public class PatientProblemService : ServiceStack.ServiceInterface.Service
    {
        private const string _phytelUserIDToken = "x-Phytel-UserID";

        public GetAllPatientProblemsDataResponse Get(GetAllPatientProblemsDataRequest request)
        {
            GetAllPatientProblemsDataResponse response = new GetAllPatientProblemsDataResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientProblemDD:Get()");

                response = DataPatientProblemManager.GetAllPatientProblem(request);
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

        public GetPatientProblemsDataResponse Get(GetPatientProblemsDataRequest request)
        {
            GetPatientProblemsDataResponse response = new GetPatientProblemsDataResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientProblemDD:Get()");

                response = DataPatientProblemManager.GetPatientProblem(request);
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

        public PutNewPatientProblemResponse Put(PutNewPatientProblemRequest request)
        {
            PutNewPatientProblemResponse response = new PutNewPatientProblemResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientProblemDD:Put()");

                response = DataPatientProblemManager.PutPatientProblem(request);
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

        public PutUpdatePatientProblemResponse Put(PutUpdatePatientProblemRequest request)
        {
            PutUpdatePatientProblemResponse response = new PutUpdatePatientProblemResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientProblemDD:Put()");

                response = DataPatientProblemManager.PutUpdatePatientProblem(request);
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
    }
}