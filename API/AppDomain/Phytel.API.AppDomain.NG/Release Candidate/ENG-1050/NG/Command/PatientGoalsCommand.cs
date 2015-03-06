using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientGoal.DTO;
using ServiceStack.Service;

namespace Phytel.API.AppDomain.NG
{
    public class PatientGoalsCommand : INGCommand
    {
        private PostDeletePatientRequest request;
        private List<DeletedPatientGoal> deletedPatientGoals;  
        private IRestClient client;
        private static readonly string DDPatientGoalsServiceUrl = ConfigurationManager.AppSettings["DDPatientGoalUrl"];

        public PatientGoalsCommand(PostDeletePatientRequest req, IRestClient restClient)
        {
            request = req as PostDeletePatientRequest;
            client = restClient;
        }

        public void Execute()
        {
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientGoal/Patient/{PatientId}/Delete", "DELETE")]
                string pgUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientGoal/Patient/{4}/Delete",
                                                        DDPatientGoalsServiceUrl,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.Id), request.UserId);
                DeletePatientGoalByPatientIdDataResponse pgDDResponse = client.Delete<DeletePatientGoalByPatientIdDataResponse>(pgUrl);
                if (pgDDResponse != null && pgDDResponse.Success)
                {
                    deletedPatientGoals = pgDDResponse.DeletedPatientGoals;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD: PatientGoalCommand Execute::" + ex.Message, ex.InnerException);
            }
        }

        public void Undo()
        {
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientGoal/UndoDelete", "PUT")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientGoal/UndoDelete",
                                            DDPatientGoalsServiceUrl,
                                            "NG",
                                            request.Version,
                                            request.ContractNumber), request.UserId);
                UndoDeletePatientGoalDataResponse response = client.Put<UndoDeletePatientGoalDataResponse>(url, new UndoDeletePatientGoalDataRequest
                {
                    Ids = deletedPatientGoals,
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    UserId = request.UserId,
                    Version = request.Version
                } as object);
            }     
            catch (Exception ex)
            {
                throw new Exception("AD: PatientGoalCommand Undo::" + ex.Message, ex.InnerException);
            }
        }
    }
}
