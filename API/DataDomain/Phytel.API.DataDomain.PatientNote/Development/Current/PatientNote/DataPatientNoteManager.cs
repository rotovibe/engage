using Phytel.API.DataDomain.PatientNote.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientNote;
using System;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.PatientNote
{
    public static class PatientNoteDataManager
    {
        public static bool InsertPatientNote(PutPatientNoteDataRequest request)
        {
            bool isInserted = false;
            try
            {
                IPatientNoteRepository<PutPatientNoteDataResponse> repo = PatientNoteRepositoryFactory<PutPatientNoteDataResponse>.GetPatientNoteRepository(request.ContractNumber, request.Context);
                isInserted = (bool)repo.Insert(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isInserted;
        }


        //public static GetPatientNoteDataResponse GetPatientNoteByID(GetPatientNoteDataRequest request)
        //{
        //    try
        //    {
        //        GetPatientNoteResponse result = new GetPatientNoteResponse();

        //        IPatientNoteRepository<GetPatientNoteResponse> repo = PatientNoteRepositoryFactory<GetPatientNoteResponse>.GetPatientNoteRepository(request.ContractNumber, request.Context);
        //        result = repo.FindByID(request.PatientNoteID) as GetPatientNoteResponse;

        //        // if cross-domain service call has error
        //        //if (result.Status != null)
        //        //{
        //        //    throw new ArgumentException(result.Status.Message, new Exception() { Source = result.Status.StackTrace });
        //        //}

        //        return (result != null ? result : new GetPatientNoteResponse());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static GetAllPatientNotesDataResponse GetPatientNoteList(GetAllPatientNotesDataRequest request)
        //{
        //    try
        //    {
        //        GetAllPatientNotesDataResponse result = new GetAllPatientNotesDataResponse();

        //        IPatientNoteRepository<GetAllPatientNotesDataResponse> repo = PatientNoteRepositoryFactory<GetAllPatientNotesDataResponse>.GetPatientNoteRepository(request.ContractNumber, request.Context);
               

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}   
