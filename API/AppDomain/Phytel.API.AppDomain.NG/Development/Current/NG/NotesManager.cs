using System;
using System.Collections.Generic;
using System.Configuration;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientNote.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG
{
    public class NotesManager : ManagerBase
    {
        #region endpoint addresses
        protected static readonly string DDPatientNoteUrl = ConfigurationManager.AppSettings["DDPatientNoteUrl"];
        #endregion

        public PatientNote GetPatientNote(GetPatientNoteRequest request)
        {
            try
            {
                PatientNote result = null;
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Note/{Id}", "GET")]
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Note/{5}",
                                                        DDPatientNoteUrl,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.PatientId,
                                                        request.Id), request.UserId);

                GetPatientNoteDataResponse ddResponse = client.Get<GetPatientNoteDataResponse>(url);

                if (ddResponse != null && ddResponse.PatientNote != null)
                {
                    PatientNoteData n = ddResponse.PatientNote;
                    result = new PatientNote
                    {
                        Id = n.Id,
                        PatientId = n.PatientId,
                        Text = n.Text,
                        ProgramIds = n.ProgramIds,
                        CreatedOn = n.CreatedOn,
                        CreatedById = n.CreatedById,
                        TypeId = n.TypeId,
                        MethodId = n.MethodId,
                        OutcomeId = n.OutcomeId,
                        WhoId = n.WhoId,
                        SourceId = n.SourceId,
                        DurationId = n.DurationId,
                        ValidatedIdentity = n.ValidatedIdentity,
                        ContactedOn = n.ContactedOn
                    };
                }
                return result;
            }
            catch (WebServiceException ex) { throw ex; }
        }

        public List<PatientNote> GetAllPatientNotes(GetAllPatientNotesRequest request)
        {
            try
            {
                List<PatientNote> result = null;
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Notes/{Count}", "GET")]
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Notes/{5}",
                    DDPatientNoteUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Count), request.UserId);

                GetAllPatientNotesDataResponse ddResponse = client.Get<GetAllPatientNotesDataResponse>(url);

                if (ddResponse != null && ddResponse.PatientNotes != null && ddResponse.PatientNotes.Count > 0)
                {
                    result = new List<PatientNote>();
                    List<PatientNoteData> dataList = ddResponse.PatientNotes;
                    foreach (PatientNoteData n in dataList)
                    {
                        result.Add(new PatientNote
                        {
                            Id = n.Id,
                            PatientId = n.PatientId,
                            Text = n.Text,
                            ProgramIds = n.ProgramIds,
                            CreatedOn = n.CreatedOn,
                            CreatedById = n.CreatedById,
                            TypeId = n.TypeId,
                            MethodId = n.MethodId,
                            OutcomeId = n.OutcomeId,
                            WhoId = n.WhoId,
                            SourceId = n.SourceId,
                            DurationId = n.DurationId,
                            ValidatedIdentity = n.ValidatedIdentity,
                            ContactedOn = n.ContactedOn
                        }
                        );
                    }
                }
                return result;
            }
            catch (WebServiceException ex) { throw ex; }
        }

        public PostPatientNoteResponse InsertPatientNote(PostPatientNoteRequest request)
        {
            try
            {
                if (request.Note == null)
                    throw new Exception("The Note property is null in the request.");
                else if (string.IsNullOrEmpty(request.Note.Text))
                    throw new Exception("Note text is a required field.");

                PostPatientNoteResponse response = new PostPatientNoteResponse();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Note/Insert", "PUT")]
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Note", "POST")]
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/patient/{4}/note",
                                                                        DDPatientNoteUrl,
                                                                        "NG",
                                                                        request.Version,
                                                                        request.ContractNumber,
                                                                        request.PatientId), request.UserId);

                PatientNoteData noteData = new PatientNoteData {
                    Text  = request.Note.Text,
                    ProgramIds  = request.Note.ProgramIds,
                    CreatedById = request.UserId,
                    CreatedOn = request.Note.CreatedOn,
                    PatientId = request.Note.PatientId,
                    TypeId = request.Note.TypeId,
                    MethodId = request.Note.MethodId,
                    OutcomeId = request.Note.OutcomeId,
                    WhoId = request.Note.WhoId,
                    SourceId = request.Note.SourceId,
                    DurationId = request.Note.DurationId,
                    ContactedOn = request.Note.ContactedOn,
                    ValidatedIdentity = request.Note.ValidatedIdentity
                };
                InsertPatientNoteDataResponse dataDomainResponse =
                    client.Post<InsertPatientNoteDataResponse>(url, new InsertPatientNoteDataRequest
                                                                                {
                                                                                    PatientNote = noteData,
                                                                                    Context = "NG",
                                                                                    ContractNumber = request.ContractNumber,
                                                                                    Version = request.Version,
                                                                                    UserId = request.UserId,
                                                                                    PatientId = request.PatientId
                                                                                } as object);
                if(dataDomainResponse != null && !(string.IsNullOrEmpty(dataDomainResponse.Id)))
                {
                    response.Id = dataDomainResponse.Id;
                    response.Version = dataDomainResponse.Version;   
                }
                
                return response;
            }
            catch (WebServiceException ex) { throw ex; }
        }

        public PostDeletePatientNoteResponse DeletePatientNote(PostDeletePatientNoteRequest request)
        {
            PostDeletePatientNoteResponse response = new PostDeletePatientNoteResponse();
            try
            {
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Note/{5}/Delete",
                                                        DDPatientNoteUrl,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.PatientId,
                                                        request.Id), request.UserId);

                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Note/{Id}/Delete", "DELETE")]
                DeletePatientNoteDataResponse ddResponse = client.Delete<DeletePatientNoteDataResponse>(url);

                if (ddResponse != null && ddResponse.Deleted)
                {
                    response.Version = ddResponse.Version; 
                }
                return response;
            }
            catch (WebServiceException ex) { throw ex; }
        }

        public UpdatePatientNoteResponse UpdatePatientNote(UpdatePatientNoteRequest request)
        {
            try
            {
                if (request.PatientNote == null)
                    throw new Exception("The Note property is null in the request.");
                else if (string.IsNullOrEmpty(request.PatientNote.Text))
                    throw new Exception("Note text is a required field.");

                UpdatePatientNoteResponse response = new UpdatePatientNoteResponse();
                if (request.PatientNote != null)
                {
                    PatientNote pn = request.PatientNote;
                    //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Note/{PatientNoteId}", "PUT")]
                    IRestClient client = new JsonServiceClient();
                    string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Note/{5}",
                                                                            DDPatientNoteUrl,
                                                                            "NG",
                                                                            request.Version,
                                                                            request.ContractNumber,
                                                                            pn.PatientId,
                                                                            pn.Id), request.UserId);

                    PatientNoteData pnData = new PatientNoteData
                    {
                        Text = pn.Text,
                        ProgramIds = pn.ProgramIds,
                        CreatedById = request.UserId,
                        CreatedOn = pn.CreatedOn,
                        PatientId = pn.PatientId,
                        TypeId = pn.TypeId,
                        MethodId = pn.MethodId,
                        OutcomeId = pn.OutcomeId,
                        WhoId = pn.WhoId,
                        SourceId = pn.SourceId,
                        DurationId = pn.DurationId,
                        ContactedOn = pn.ContactedOn,
                        ValidatedIdentity = pn.ValidatedIdentity
                    };
                    UpdatePatientNoteDataResponse dataDomainResponse =
                        client.Put<UpdatePatientNoteDataResponse>(url, new UpdatePatientNoteDataRequest
                        {
                            Context = "NG",
                            ContractNumber = request.ContractNumber,
                            Version = request.Version,
                            UserId = request.UserId,
                            PatientNoteData = pnData
                        } as object);
                }
                return response;
            }
            catch (WebServiceException ex) { throw ex; }
        }
    }
}
