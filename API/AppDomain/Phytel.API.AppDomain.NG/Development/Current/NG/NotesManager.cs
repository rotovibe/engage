using System;
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

        //public GetPatientGoalResponse GetPatientNote(GetPatientGoalRequest request)
        //{
        //    try
        //    {
        //        GetPatientGoalResponse response = new GetPatientGoalResponse();
        //        response.Goal = GoalsEndpointUtil.GetPatientGoal(request);
        //        response.Version = request.Version;
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public GetAllPatientGoalsResponse GetAllPatientNotes(GetAllPatientGoalsRequest request)
        //{
        //    try
        //    {
        //        GetAllPatientGoalsResponse response = new GetAllPatientGoalsResponse();
        //        response.Goals = GoalsEndpointUtil.GetAllPatientGoals(request);
        //        response.Version = request.Version;
        //        return response;
        //    }
        //    catch (Exception ex)C:\TFS2013\PhytelCode\Phytel.Net\Services\API\AppDomain\Phytel.API.AppDomain.NG\Development\Current\NG.DTOs\Goal\GetAllPatientGoalsRequest.cs
        //    {
        //        throw ex;
        //    }
        //}

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
                IRestClient client = new JsonServiceClient();
                PatientNoteData noteData = new PatientNoteData {
                    Text  = request.Note.Text,
                    ProgramIds  = request.Note.ProgramIds,
                    CreatedBy = request.Note.CreatedBy,
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
