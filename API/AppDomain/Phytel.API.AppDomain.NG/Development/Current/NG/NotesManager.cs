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
                GetPatientNoteDataResponse ddResponse = client.Get<GetPatientNoteDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Note/{5}?UserId={6}",
                    DDPatientNoteUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Id,
                    request.UserId));

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
                        CreatedById = n.CreatedById
                    };
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetPatientNote()" + ex.Message, ex.InnerException);
            }
        }

        public List<PatientNote> GetAllPatientNotes(GetAllPatientNotesRequest request)
        {
            try
            {
                List<PatientNote> result = null;
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Notes/{Count}", "GET")]
                IRestClient client = new JsonServiceClient();
                GetAllPatientNotesDataResponse ddResponse = client.Get<GetAllPatientNotesDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Notes/{5}?UserId={6}",
                    DDPatientNoteUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Count,
                    request.UserId));

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
                            CreatedById = n.CreatedById
                        }
                        );
                    }
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAllPatientNotes()" + ex.Message, ex.InnerException);
            }
        }

        public PostPatientNoteResponse InsertPatientNote(PostPatientNoteRequest request, string createdBy)
        {
            try
            {
                if (request.Note == null)
                    throw new Exception("The Note property is null in the request.");
                else if (string.IsNullOrEmpty(request.Note.Text))
                    throw new Exception("Note text is a required field.");

                PostPatientNoteResponse response = new PostPatientNoteResponse();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Note/Insert", "PUT")]
                IRestClient client = new JsonServiceClient();
                PatientNoteData noteData = new PatientNoteData {
                    Text  = request.Note.Text,
                    ProgramIds  = request.Note.ProgramIds,
                    CreatedById = createdBy,
                    CreatedOn = request.Note.CreatedOn,
                    PatientId = request.Note.PatientId
                };
                PutPatientNoteDataResponse dataDomainResponse =
                    client.Put<PutPatientNoteDataResponse>(string.Format("{0}/{1}/{2}/{3}/patient/{4}/note/insert?UserId={5}",
                                                                                DDPatientNoteUrl,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber,
                                                                                request.PatientId,
                                                                                request.UserId), new PutPatientNoteDataRequest
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
                }
                response.Version = request.Version;   
                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        //public PostDeletePatientGoalResponse DeletePatientNote(PostDeletePatientGoalRequest request)
        //{
        //    try
        //    {
        //        PostDeletePatientGoalResponse pgr = new PostDeletePatientGoalResponse();
        //        bool result = false;

        //        // we can thread here if we want!
        //        // save goals
        //        GoalsEndpointUtil.DeleteGoalRequest(request);

        //        // save barriers
        //        GoalsUtil.DeletePatientGoalBarriers(request);

        //        // save tasks
        //        GoalsUtil.DeletePatientGoalTasks(request);

        //        // save interventions
        //        GoalsUtil.DeletePatientGoalInterventions(request);

        //        pgr.Result = result;

        //        return pgr;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("AD:SavePatientGoal:" + ex.Message, ex.InnerException);
        //    }
        //}
    }
}
