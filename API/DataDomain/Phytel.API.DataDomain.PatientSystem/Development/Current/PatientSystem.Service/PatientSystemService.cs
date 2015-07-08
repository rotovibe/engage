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
    public class PatientSystemService : ServiceBase
    {
        public IPatientSystemDataManager Manager { get; set; }

        #region Gets
        public GetPatientSystemDataResponse Get(GetPatientSystemDataRequest request)
        {
            GetPatientSystemDataResponse response = new GetPatientSystemDataResponse();
            try
            {
                RequireUserId(request); 
                response = Manager.GetPatientSystem(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public GetPatientSystemsDataResponse Get(GetPatientSystemsDataRequest request)
        {
            GetPatientSystemsDataResponse response = new GetPatientSystemsDataResponse();
            try
            {
                RequireUserId(request); 
                response = Manager.GetPatientSystems(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
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
                RequireUserId(request); 
                response = Manager.InsertPatientSystem(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public PutUpdatePatientSystemDataResponse Put(PutUpdatePatientSystemDataRequest request)
        {
            PutUpdatePatientSystemDataResponse response = new PutUpdatePatientSystemDataResponse();
            try
            {
                RequireUserId(request);
                response = Manager.UpdatePatientSystem(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
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
                RequireUserId(request); 
                response = Manager.DeletePatientSystemByPatientId(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public UndoDeletePatientSystemsDataResponse Put(UndoDeletePatientSystemsDataRequest request)
        {
            UndoDeletePatientSystemsDataResponse response = new UndoDeletePatientSystemsDataResponse();
            try
            {
                RequireUserId(request); 
                response = Manager.UndoDeletePatientSystems(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        } 
        #endregion
    }
}