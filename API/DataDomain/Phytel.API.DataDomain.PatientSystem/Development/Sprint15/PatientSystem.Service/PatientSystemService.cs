using System;
using System.Net;
using Phytel.API.DataDomain.PatientSystem;
using Phytel.API.DataDomain.PatientSystem.DTO;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Common.Format;
using System.Configuration;
using System.Web;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.PatientSystem.Service
{
    public partial class PatientSystemService : ServiceStack.ServiceInterface.Service
    {
        public ICommonFormatterUtil CommonFormatterUtil { get; set; }
        public IPatientSystemDataManager PatientSystemDataManager { get; set; }
        public IHelpers Helpers { get; set; }

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
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
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
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
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
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
    }
}