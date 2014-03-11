using System;
using System.Collections.Generic;
using System.Net;
using Phytel.API.DataDomain.PatientProblem;
using Phytel.API.DataDomain.PatientProblem.DTO;
using System.Configuration;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.PatientProblem.Service
{
    public class PatientProblemService : ServiceStack.ServiceInterface.Service
    {
        public GetAllPatientProblemsDataResponse Get(GetAllPatientProblemsDataRequest request)
        {
            GetAllPatientProblemsDataResponse response = new GetAllPatientProblemsDataResponse();
            try
            {
                response = DataPatientProblemManager.GetAllPatientProblem(request);
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
                response = DataPatientProblemManager.GetPatientProblem(request);
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
                response = DataPatientProblemManager.PutPatientProblem(request);
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
                response = DataPatientProblemManager.PutUpdatePatientProblem(request);
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