using System;
using System.Net;
using Phytel.API.Common;
using Phytel.API.DataDomain.PatientNote;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.Common.Format;
using System.Configuration;

namespace Phytel.API.DataDomain.PatientNote.Service
{
    public class PatientNoteService : ServiceStack.ServiceInterface.Service
    {
        public IPatientNoteDataManager Manager { get; set; }
        public ICommonFormatterUtil FormatUtil { get; set; }
        public IHelpers Helpers { get; set; }
        
        public PutPatientNoteDataResponse Put(PutPatientNoteDataRequest request)
        {
            PutPatientNoteDataResponse response = new PutPatientNoteDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientNoteDD:Put()::Unauthorized Access");

                response.Id = Manager.InsertPatientNote(request);
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
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientNoteDD:Get()::Unauthorized Access");

                response.PatientNote = Manager.GetPatientNote(request);
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
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientNoteDD:Get()::Unauthorized Access");

                response.PatientNotes = Manager.GetAllPatientNotes(request);
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
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientNoteDD:Delete()");

                response.Deleted = Manager.DeletePatientNote(request);
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


        public DeleteNoteByPatientIdDataResponse Delete(DeleteNoteByPatientIdDataRequest request)
        {
            DeleteNoteByPatientIdDataResponse response = new DeleteNoteByPatientIdDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientNoteDD:PatientNoteDelete()::Unauthorized Access");

                response = Manager.DeleteNoteByPatientId(request);
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

        public UndoDeletePatientNotesDataResponse Put(UndoDeletePatientNotesDataRequest request)
        {
            UndoDeletePatientNotesDataResponse response = new UndoDeletePatientNotesDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientNoteDD:PatientNoteUndoDelete()::Unauthorized Access");

                response = Manager.UndoDeletePatientNotes(request);
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

        public RemoveProgramInPatientNotesDataResponse Put(RemoveProgramInPatientNotesDataRequest request)
        {
            RemoveProgramInPatientNotesDataResponse response = new RemoveProgramInPatientNotesDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientNoteDD:RemoveProgramInPatientNotes()::Unauthorized Access");

                response = Manager.RemoveProgramInPatientNotes(request);
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