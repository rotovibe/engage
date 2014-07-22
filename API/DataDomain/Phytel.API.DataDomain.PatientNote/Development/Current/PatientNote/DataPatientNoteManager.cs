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
                IPatientNoteRepository<PutPatientNoteDataResponse> repo = PatientNoteRepositoryFactory<PutPatientNoteDataResponse>.GetPatientNoteRepository(request.ContractNumber, request.Context, request.UserId);

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
                IPatientNoteRepository<PatientNoteData> repo = PatientNoteRepositoryFactory<PatientNoteData>.GetPatientNoteRepository(request.ContractNumber, request.Context, request.UserId);

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
                IPatientNoteRepository<List<PatientNoteData>> repo = PatientNoteRepositoryFactory<List<PatientNoteData>>.GetPatientNoteRepository(request.ContractNumber, request.Context, request.UserId);

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
                IPatientNoteRepository<DeletePatientNoteDataResponse> repo = PatientNoteRepositoryFactory<DeletePatientNoteDataResponse>.GetPatientNoteRepository(request.ContractNumber, request.Context, request.UserId);

                repo.Delete(request);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DeleteNoteByPatientIdDataResponse DeleteNoteByPatientId(DeleteNoteByPatientIdDataRequest request)
        {
            DeleteNoteByPatientIdDataResponse response = null;
            try
            {
                response = new DeleteNoteByPatientIdDataResponse();

                IPatientNoteRepository<DeleteNoteByPatientIdDataResponse> repo = PatientNoteRepositoryFactory<DeleteNoteByPatientIdDataResponse>.GetPatientNoteRepository(request.ContractNumber, request.Context, request.UserId);
                GetAllPatientNotesDataRequest getAllPatientNotesDataRequest = new GetAllPatientNotesDataRequest 
                {
                     Context = request.Context,
                      ContractNumber = request.ContractNumber, 
                      PatientId = request.PatientId,
                      UserId = request.UserId,
                      Version = request.Version
                };
                List<PatientNoteData> patientNotes = repo.FindByPatientId(getAllPatientNotesDataRequest) as List<PatientNoteData>;
                List<string> deletedIds = null;
                if (patientNotes != null)
                {
                    deletedIds = new List<string>();
                    patientNotes.ForEach(u =>
                    {
                        DeletePatientNoteDataRequest deletePatientNoteDataRequest = new DeletePatientNoteDataRequest
                        { 
                            Context = request.Context, 
                            ContractNumber = request.ContractNumber,
                            Id = u.Id,
                            PatientId = request.PatientId,
                            UserId = request.UserId, 
                            Version =  request.Version
                        };
                        repo.Delete(deletePatientNoteDataRequest);
                        deletedIds.Add(deletePatientNoteDataRequest.Id);
                    });
                    response.DeletedIds = deletedIds;
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public static UndoDeletePatientNotesDataResponse UndoDeletePatientNotes(UndoDeletePatientNotesDataRequest request)
        {
            UndoDeletePatientNotesDataResponse response = null;
            try
            {
                response = new UndoDeletePatientNotesDataResponse();

                IPatientNoteRepository<DeleteNoteByPatientIdDataResponse> repo = PatientNoteRepositoryFactory<DeleteNoteByPatientIdDataResponse>.GetPatientNoteRepository(request.ContractNumber, request.Context, request.UserId);
                if (request.Ids != null && request.Ids.Count > 0)
                {
                    request.Ids.ForEach(u =>
                    {
                        request.PatientNoteId = u;
                        repo.UndoDelete(request);
                    });
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public static RemoveProgramInPatientNotesDataResponse RemoveProgramInPatientNotes(RemoveProgramInPatientNotesDataRequest request)
        {
            RemoveProgramInPatientNotesDataResponse response = null;
            try
            {
                response = new RemoveProgramInPatientNotesDataResponse();

                IPatientNoteRepository<DeleteNoteByPatientIdDataResponse> repo = PatientNoteRepositoryFactory<DeleteNoteByPatientIdDataResponse>.GetPatientNoteRepository(request.ContractNumber, request.Context, request.UserId);
                if (request.ProgramId != null)
                {
                    List<PatientNoteData> notes = repo.FindNotesWithAProgramId(request.ProgramId) as List<PatientNoteData>;
                    if (notes != null && notes.Count > 0)
                    {
                        notes.ForEach(u =>
                        {
                            request.NoteId = u.Id;
                            if (u.ProgramIds != null && u.ProgramIds.Remove(request.ProgramId))
                            {
                                repo.RemoveProgram(request, u.ProgramIds);
                            }
                        });
                    }
                }
                return response;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}   
