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
        public IPatientSystemDataManager Manager { get; set; }

        #region Gets
        public GetPatientSystemDataResponse Get(GetPatientSystemDataRequest request)
        {
            GetPatientSystemDataResponse response = new GetPatientSystemDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientSystemDD:Get()::Unauthorized Access");

                response = Manager.GetPatientSystem(request);
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

        public GetPatientSystemsDataResponse Get(GetPatientSystemsDataRequest request)
        {
            GetPatientSystemsDataResponse response = new GetPatientSystemsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientSystemDD:Get()::Unauthorized Access");

                response = Manager.GetPatientSystems(request);
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

        #region Insert & Update
        public PutPatientSystemDataResponse Put(PutPatientSystemDataRequest request)
        {
            PutPatientSystemDataResponse response = new PutPatientSystemDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientSystemDD:Put()::Unauthorized Access");

                response = Manager.InsertPatientSystem(request);
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

        public PutUpdatePatientSystemDataResponse Put(PutUpdatePatientSystemDataRequest request)
        {
            PutUpdatePatientSystemDataResponse response = new PutUpdatePatientSystemDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientSystemDD:Put()::Unauthorized Access");

                response = Manager.UpdatePatientSystem(request);
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

        #region Delete & UndoDelete
        public DeletePatientSystemByPatientIdDataResponse Delete(DeletePatientSystemByPatientIdDataRequest request)
        {
            DeletePatientSystemByPatientIdDataResponse response = new DeletePatientSystemByPatientIdDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientSystemDD:PatientSystemDelete()::Unauthorized Access");

                response = Manager.DeletePatientSystemByPatientId(request);
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

        public UndoDeletePatientSystemsDataResponse Put(UndoDeletePatientSystemsDataRequest request)
        {
            UndoDeletePatientSystemsDataResponse response = new UndoDeletePatientSystemsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientSystemDD:PatientSystemUndoDelete()::Unauthorized Access");

                response = Manager.UndoDeletePatientSystems(request);
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