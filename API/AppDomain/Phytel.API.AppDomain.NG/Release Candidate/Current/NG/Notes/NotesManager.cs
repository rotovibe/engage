using System;
using System.Collections.Generic;
using System.Configuration;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Utilization;
using Phytel.API.AppDomain.NG.Notes.Visitors;
using Phytel.API.DataDomain.PatientNote.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO.Context;
using Phytel.API.AppDomain.NG.DTO.Note.Context;

namespace Phytel.API.AppDomain.NG.Notes
{
    public class NotesManager : ManagerBase, INotesManager
    {
        #region endpoint addresses
        protected static readonly string DDPatientNoteUrl = ConfigurationManager.AppSettings["DDPatientNoteUrl"];
        #endregion

        public IUtilizationManager UtilManager { get; set; }

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
                        Duration = n.Duration,
                        ValidatedIdentity = n.ValidatedIdentity,
                        ContactedOn = n.ContactedOn,
                        UpdatedById = n.UpdatedById,
                        UpdatedOn = n.UpdatedOn,
                        DataSource = n.DataSource
                    };
                }
                return result;
            }
            catch (WebServiceException ex) { throw ex; }
        }

        public List<PatientNote> GetAllPatientNotes(IServiceContext context)
        {
            try
            {
                var hlist = new HistoryListVisitable(context);
                hlist.Accept(new PatientNoteVisitor());
                hlist.Accept(new PatientUtilizationVisitor());
                hlist.Modify(new OrderModifier());
                hlist.Modify(new TakeModifier(((PatientNoteContext)context.Tag).Count));
                return hlist.GetList();
            }
            catch (WebServiceException ex)
            {
                throw ex;
            }
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
                    Duration = request.Note.Duration,
                    ContactedOn = request.Note.ContactedOn,
                    ValidatedIdentity = request.Note.ValidatedIdentity,
                    DataSource = request.Note.DataSource
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
                    //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Note/{Id}", "PUT")]
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
                        Id = pn.Id,
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
                        Duration = pn.Duration,
                        ContactedOn = pn.ContactedOn,
                        ValidatedIdentity = pn.ValidatedIdentity,
                        DataSource = pn.DataSource
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
                    if (dataDomainResponse != null & dataDomainResponse.PatientNoteData != null)
                    {
                        response.PatientNote = Mapper.Map<PatientNote>(dataDomainResponse.PatientNoteData); ;
                    }
                }
                return response;
            }
            catch (WebServiceException ex) { throw ex; }
        }
    }
}
