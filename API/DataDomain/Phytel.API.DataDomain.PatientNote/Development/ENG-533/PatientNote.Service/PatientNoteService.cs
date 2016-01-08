using System;
using System.Net;
using Phytel.API.Common;
using Phytel.API.DataDomain.PatientNote;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.Common.Format;
using System.Configuration;

namespace Phytel.API.DataDomain.PatientNote.Service
{
    public class PatientNoteService : ServiceBase
    {
        public IPatientNoteDataManager Manager { get; set; }
        
        public InsertPatientNoteDataResponse Post(InsertPatientNoteDataRequest request)
        {
            InsertPatientNoteDataResponse response = new InsertPatientNoteDataResponse();
            try
            {
                RequireUserId(request); 
                response.Id = Manager.InsertPatientNote(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public InsertBatchPatientNotesDataResponse Post(InsertBatchPatientNotesDataRequest request)
        {
            InsertBatchPatientNotesDataResponse response = new InsertBatchPatientNotesDataResponse();
            try
            {
                RequireUserId(request);
                response.Responses = Manager.InsertBatchPatientNotes(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public UpdatePatientNoteDataResponse Put(UpdatePatientNoteDataRequest request)
        {
            UpdatePatientNoteDataResponse response = new UpdatePatientNoteDataResponse();
            try
            {
                RequireUserId(request);
                response.PatientNoteData = Manager.UpdatePatientNote(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public GetPatientNoteDataResponse Get(GetPatientNoteDataRequest request)
        {
            GetPatientNoteDataResponse response = new GetPatientNoteDataResponse();
            try
            {
                RequireUserId(request);
                response.PatientNote = Manager.GetPatientNote(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public GetAllPatientNotesDataResponse Get(GetAllPatientNotesDataRequest request)
        {
            GetAllPatientNotesDataResponse response = new GetAllPatientNotesDataResponse();
            try
            {
                RequireUserId(request); 
                response.PatientNotes = Manager.GetAllPatientNotes(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public DeletePatientNoteDataResponse Delete(DeletePatientNoteDataRequest request)
        {
            DeletePatientNoteDataResponse response = new DeletePatientNoteDataResponse();
            try
            {
                RequireUserId(request); 
                response.Deleted = Manager.DeletePatientNote(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }


        public DeleteNoteByPatientIdDataResponse Delete(DeleteNoteByPatientIdDataRequest request)
        {
            DeleteNoteByPatientIdDataResponse response = new DeleteNoteByPatientIdDataResponse();
            try
            {
                RequireUserId(request);
                response = Manager.DeleteNoteByPatientId(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public UndoDeletePatientNotesDataResponse Put(UndoDeletePatientNotesDataRequest request)
        {
            UndoDeletePatientNotesDataResponse response = new UndoDeletePatientNotesDataResponse();
            try
            {
                RequireUserId(request); 
                response = Manager.UndoDeletePatientNotes(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public RemoveProgramInPatientNotesDataResponse Put(RemoveProgramInPatientNotesDataRequest request)
        {
            RemoveProgramInPatientNotesDataResponse response = new RemoveProgramInPatientNotesDataResponse();
            try
            {
                RequireUserId(request);
                response = Manager.RemoveProgramInPatientNotes(request);
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