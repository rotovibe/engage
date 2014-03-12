using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.DataDomain.PatientNote
{
    public static class PatientNoteDataManager
    {
        public static string InsertPatientNote(PutPatientNoteDataRequest request)
        {
            string noteId = string.Empty;
            try
            {
                IPatientNoteRepository<PutPatientNoteDataResponse> repo = PatientNoteRepositoryFactory<PutPatientNoteDataResponse>.GetPatientNoteRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;
                noteId = (string)repo.Insert(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return noteId;
        }

        public static PatientNoteData GetPatientNote(GetPatientNoteDataRequest request)
        {
            try
            {
                PatientNoteData response = null;
                IPatientNoteRepository<PatientNoteData> repo = PatientNoteRepositoryFactory<PatientNoteData>.GetPatientNoteRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;
                response = repo.FindByID(request.Id) as PatientNoteData;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<PatientNoteData> GetAllPatientNotes(GetAllPatientNotesDataRequest request)
        {
            try
            {
                List<PatientNoteData> response = null;
                IPatientNoteRepository<List<PatientNoteData>> repo = PatientNoteRepositoryFactory<List<PatientNoteData>>.GetPatientNoteRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;
                response = repo.FindByPatientId(request) as List<PatientNoteData>;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeletePatientNote(DeletePatientNoteDataRequest request)
        {
            try
            {
                IPatientNoteRepository<DeletePatientNoteDataResponse> repo = PatientNoteRepositoryFactory<DeletePatientNoteDataResponse>.GetPatientNoteRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;
                repo.Delete(request);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
    }
}   
