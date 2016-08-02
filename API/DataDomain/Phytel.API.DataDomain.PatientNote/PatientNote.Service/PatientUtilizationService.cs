using System;
using Phytel.API.DataDomain.PatientNote.DTO.Response.Utilization;
using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.DataDomain.PatientNote.Service
{
    public class PatientUtilizationService : ServiceBase
    {
        public IDataPatientUtilizationManager Manager { get; set; }

        public PostPatientUtilizationDataResponse Post(PostPatientUtilizationDataRequest request)
        {
            PostPatientUtilizationDataResponse response = new PostPatientUtilizationDataResponse();
            try
            {
                RequireUserId(request);
                response.Utilization = Manager.InsertPatientUtilization(request.PatientUtilization);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public PutPatientUtilizationDataResponse Put(PutPatientUtilizationDataRequest request)
        {
            PutPatientUtilizationDataResponse response = new PutPatientUtilizationDataResponse();
            try
            {
                RequireUserId(request);
                response.Utilization = Manager.UpdatePatientUtilization(request.PatientUtilization);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public GetAllPatientUtilizationDataResponse Get(GetAllPatientUtilizationDataRequest request)
        {
            GetAllPatientUtilizationDataResponse response = new GetAllPatientUtilizationDataResponse();
            try
            {
                RequireUserId(request);
                response.Utilizations = Manager.GetPatientUtilizations(request.PatientId);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public GetPatientUtilizationDataResponse Get(GetPatientUtilizationDataRequest request)
        {
            GetPatientUtilizationDataResponse response = new GetPatientUtilizationDataResponse();
            try
            {
                RequireUserId(request);
                response.Utilization = Manager.GetPatientUtilization(request.UtilId);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public DeletePatientUtilizationDataResponse Delete(DeletePatientUtilizationDataRequest request)
        {
            DeletePatientUtilizationDataResponse response = new DeletePatientUtilizationDataResponse();
            try
            {
                RequireUserId(request);
                Manager.DeletePatientUtilization(request.UtilId);
                response.Success = true;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public DeleteUtilizationByPatientIdDataResponse Delete(DeleteUtilizationsByPatientIdDataRequest request)
        {
            DeleteUtilizationByPatientIdDataResponse response = new DeleteUtilizationByPatientIdDataResponse();
            try
            {
                RequireUserId(request);
                response = Manager.DeletePatientUtilizationsByPatientId(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public UndoDeletePatientUtilizationsDataResponse Put(UndoDeletePatientUtilizationsDataRequest request)
        {
            UndoDeletePatientUtilizationsDataResponse response = new UndoDeletePatientUtilizationsDataResponse();
            try
            {
                RequireUserId(request);
                response = Manager.UndoDeletePatientPatientUtilizations(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
    }
}