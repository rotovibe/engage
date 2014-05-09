using System;
using System.Net;
using Phytel.API.DataDomain.PatientSystem;
using Phytel.API.DataDomain.PatientSystem.DTO;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Common.Format;
using System.Configuration;
using System.Web;

namespace Phytel.API.DataDomain.PatientSystem.Service
{
    public class PatientSystemService : ServiceStack.ServiceInterface.Service
    {
        public GetPatientSystemDataResponse Get(GetPatientSystemDataRequest request)
        {
            GetPatientSystemDataResponse response = new GetPatientSystemDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientSystemDD:Get()::Unauthorized Access");

                response = PatientSystemDataManager.GetPatientSystem(request);
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

        public GetAllPatientSystemsDataResponse Post(GetAllPatientSystemsDataRequest request)
        {
            GetAllPatientSystemsDataResponse response = new GetAllPatientSystemsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientSystemDD:Post()::Unauthorized Access");

                response = PatientSystemDataManager.GetAllPatientSystems(request);
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

        public PutPatientSystemDataResponse Put(PutPatientSystemDataRequest request)
        {
            PutPatientSystemDataResponse response = new PutPatientSystemDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientSystemDD:Put()::Unauthorized Access");

                response = PatientSystemDataManager.PutPatientSystem(request);
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