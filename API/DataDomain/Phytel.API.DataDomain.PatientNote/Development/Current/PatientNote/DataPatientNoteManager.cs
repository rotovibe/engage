using Phytel.API.DataDomain.PatientNote.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientNote;
using System;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.PatientNote
{
    public static class PatientNoteDataManager
    {
        public static GetPatientNoteResponse GetPatientNoteByID(GetPatientNoteRequest request)
        {
            try
            {
                GetPatientNoteResponse result = new GetPatientNoteResponse();

                IPatientNoteRepository<GetPatientNoteResponse> repo = PatientNoteRepositoryFactory<GetPatientNoteResponse>.GetPatientNoteRepository(request.ContractNumber, request.Context);
                result = repo.FindByID(request.PatientNoteID) as GetPatientNoteResponse;

                // if cross-domain service call has error
                //if (result.Status != null)
                //{
                //    throw new ArgumentException(result.Status.Message, new Exception() { Source = result.Status.StackTrace });
                //}

                return (result != null ? result : new GetPatientNoteResponse());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static GetAllPatientNotesResponse GetPatientNoteList(GetAllPatientNotesRequest request)
        {
            try
            {
                GetAllPatientNotesResponse result = new GetAllPatientNotesResponse();

                IPatientNoteRepository<GetAllPatientNotesResponse> repo = PatientNoteRepositoryFactory<GetAllPatientNotesResponse>.GetPatientNoteRepository(request.ContractNumber, request.Context);
               

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
