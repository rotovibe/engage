using System;
using System.Collections.Generic;
using System.Net;
using DataDomain.PatientNote.Repo;
using Phytel.API.Common;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientNote.Repo;

namespace Phytel.API.DataDomain.PatientNote
{
    public class PatientNoteDataManager : IPatientNoteDataManager
    {
        IPatientNoteRepositoryFactory Factory { get; set; }

        public PatientNoteDataManager(IPatientNoteRepositoryFactory repo)
        {
            Factory = repo;
        }

        public string InsertPatientNote(InsertPatientNoteDataRequest request)
        {
            string noteId = string.Empty;
            try
            {
                IMongoPatientNoteRepository repo = Factory.GetRepository(RepositoryType.PatientNote);
                noteId = (string)repo.Insert(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return noteId;
        }

        public InsertBatchPatientNotesDataResponse InsertBatchPatientNotes(InsertBatchPatientNotesDataRequest request)
        {
            InsertBatchPatientNotesDataResponse response = new InsertBatchPatientNotesDataResponse();
            if (request.PatientNotesData != null && request.PatientNotesData.Count > 0)
            {
                List<HttpObjectResponse<PatientNoteData>> list = new List<HttpObjectResponse<PatientNoteData>>();
                var repo = Factory.GetRepository(RepositoryType.PatientNote);
                request.PatientNotesData.ForEach(p =>
                {
                    InsertPatientNoteDataRequest insertReq = new InsertPatientNoteDataRequest
                    {
                        PatientId = p.PatientId,
                        Context = request.Context,
                        ContractNumber = request.ContractNumber,
                        PatientNote = p,
                        UserId = request.UserId,
                        Version = request.Version
                    };
                    HttpStatusCode code = HttpStatusCode.OK;
                    PatientNoteData pnData = null;
                    string message = string.Empty;
                    try
                    {
                        string id = (string)repo.Insert(insertReq);
                        if (!string.IsNullOrEmpty(id))
                        {
                            code = HttpStatusCode.Created;
                            pnData = new PatientNoteData { Id = id, AtmosphereId = p.AtmosphereId, PatientId = p.PatientId };
                        }
                    }
                    catch (Exception ex)
                    {
                        code = HttpStatusCode.InternalServerError;
                        message = string.Format("AtmosphereId: {0}, Message: {1}, StackTrace: {2}", p.AtmosphereId, ex.Message, ex.StackTrace);
                    }
                    list.Add(new HttpObjectResponse<PatientNoteData> { Code = code, Body = (PatientNoteData)pnData, Message = message });
                });
                response.Responses = list;
            }
            return response;
        }

        public PatientNoteData UpdatePatientNote(UpdatePatientNoteDataRequest request)
        {
            PatientNoteData result = null; 
            try
            {
                IMongoPatientNoteRepository repo = Factory.GetRepository(RepositoryType.PatientNote);
                if(request.PatientNoteData != null)
                { 
                    bool status = (bool)repo.Update(request);
                    if (status)
                    {
                        result = (PatientNoteData)repo.FindByID(request.PatientNoteData.Id);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PatientNoteData GetPatientNote(GetPatientNoteDataRequest request)
        {
            try
            {
                PatientNoteData response = null; 
                IMongoPatientNoteRepository repo = Factory.GetRepository(RepositoryType.PatientNote);
                response = repo.FindByID(request.Id) as PatientNoteData;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PatientNoteData> GetAllPatientNotes(GetAllPatientNotesDataRequest request)
        {
            try
            {
                List<PatientNoteData> response = null;
                IMongoPatientNoteRepository repo = Factory.GetRepository(RepositoryType.PatientNote);
                response = repo.FindByPatientId(request) as List<PatientNoteData>;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeletePatientNote(DeletePatientNoteDataRequest request)
        {
            try
            {
                IMongoPatientNoteRepository repo = Factory.GetRepository(RepositoryType.PatientNote);
                repo.Delete(request);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DeleteNoteByPatientIdDataResponse DeleteNoteByPatientId(DeleteNoteByPatientIdDataRequest request)
        {
            DeleteNoteByPatientIdDataResponse response = null;
            try
            {
                response = new DeleteNoteByPatientIdDataResponse();

                IMongoPatientNoteRepository repo = Factory.GetRepository(RepositoryType.PatientNote);
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

        public UndoDeletePatientNotesDataResponse UndoDeletePatientNotes(UndoDeletePatientNotesDataRequest request)
        {
            UndoDeletePatientNotesDataResponse response = null;
            try
            {
                response = new UndoDeletePatientNotesDataResponse();

                IMongoPatientNoteRepository repo = Factory.GetRepository(RepositoryType.PatientNote);
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

        public RemoveProgramInPatientNotesDataResponse RemoveProgramInPatientNotes(RemoveProgramInPatientNotesDataRequest request)
        {
            RemoveProgramInPatientNotesDataResponse response = null;
            try
            {
                response = new RemoveProgramInPatientNotesDataResponse();

                IMongoPatientNoteRepository repo = Factory.GetRepository(RepositoryType.PatientNote);
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
