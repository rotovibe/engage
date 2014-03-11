using System;
using System.Net;
using Phytel.API.DataDomain.PatientNote;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.Common.Format;
using System.Configuration;

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
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetPatientNoteDataResponse Get(GetPatientNoteDataRequest request)
        {
            GetPatientNoteDataResponse response = new GetPatientNoteDataResponse();
            try
            {
                response.PatientNote = PatientNoteDataManager.GetPatientNote(request);
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

        public GetAllPatientNotesDataResponse Get(GetAllPatientNotesDataRequest request)
        {
            GetAllPatientNotesDataResponse response = new GetAllPatientNotesDataResponse();
            try
            {
                response.PatientNotes = PatientNoteDataManager.GetAllPatientNotes(request);
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

        public DeletePatientNoteDataResponse Delete(DeletePatientNoteDataRequest request)
        {
            DeletePatientNoteDataResponse response = new DeletePatientNoteDataResponse();
            try
            {
                response.Deleted = PatientNoteDataManager.DeletePatientNote(request);
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