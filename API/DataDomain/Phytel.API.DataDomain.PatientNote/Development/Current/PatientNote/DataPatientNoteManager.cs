using Phytel.API.DataDomain.PatientNote.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientNote;
using System;
using Phytel.API.Common.Format;

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
                noteId = (string)repo.Insert(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return noteId;
        }

        //public static PatientNoteData GetPatientNote(GetPatientNoteDataRequest request)
        //{
        //    try
        //    {
        //        GetPatientNoteDataResponse response = null;

        //        IPatientNoteRepository<GetPatientNoteDataResponse> repo = PatientNoteRepositoryFactory<GetPatientNoteDataResponse>.GetPatientNoteRepository(request.ContractNumber, request.Context);
        //        response = repo.FindByID(request.Id) as GetPatientNoteDataResponse;
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static List<PatientNoteData> GetAllPatientNotes(GetAllPatientNotesDataRequest request)
        //{
        //    try
        //    {
        //        GetAllPatientNotesDataResponse response = new GetAllPatientNotesDataResponse();
        //        IPatientNoteRepository<GetAllPatientNotesDataResponse> repo = PatientNoteRepositoryFactory<GetAllPatientNotesDataResponse>.GetPatientNoteRepository(request.ContractNumber, request.Context);


        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}   
