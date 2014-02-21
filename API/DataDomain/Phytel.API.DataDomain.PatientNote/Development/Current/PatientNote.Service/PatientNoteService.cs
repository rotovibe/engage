using System;
using System.Net;
using Phytel.API.DataDomain.PatientNote;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.PatientNote.Service
{
    public class PatientNoteService : ServiceStack.ServiceInterface.Service
    {
        public PutPatientNoteDataResponse Put(PutPatientNoteDataRequest request)
        {
            PutPatientNoteDataResponse response = new PutPatientNoteDataResponse();
            try
            {
                response.Id = PatientNoteDataManager.InsertPatientNote(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        //public GetPatientNoteResponse Get(GetPatientNoteRequest request)
        //{
        //    GetPatientNoteResponse response = new GetPatientNoteResponse();
        //    try
        //    {
        //        response = PatientNoteDataManager.GetPatientNoteByID(request);
        //        response.Version = request.Version;
        //    }
        //    catch (Exception ex)
        //    {
        //        //TODO: Log this to C3 database via ASE
        //        base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
        //    }
        //    return response;
        //}

        //public GetAllPatientNotesResponse Post(GetAllPatientNotesRequest request)
        //{
        //    GetAllPatientNotesResponse response = new GetAllPatientNotesResponse();
        //    try
        //    {
        //        response = PatientNoteDataManager.GetPatientNoteList(request);
        //        response.Version = request.Version;
        //    }
        //    catch (Exception ex)
        //    {
        //        //TODO: Log this to C3 database via ASE
        //        base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
        //    }
        //    return response;
        //}
    }
}