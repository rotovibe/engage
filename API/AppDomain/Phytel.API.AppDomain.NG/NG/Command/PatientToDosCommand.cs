using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Scheduling.DTO;
using ServiceStack.Service;

namespace Phytel.API.AppDomain.NG
{
    public class PatientToDosCommand : INGCommand
    {
        private PostDeletePatientRequest request;
        private List<string> deletedIds;  
        private IRestClient client;

        protected static readonly string DDSchedulingUrl = ConfigurationManager.AppSettings["DDSchedulingUrl"];

        public PatientToDosCommand(PostDeletePatientRequest req, IRestClient restClient)
        {
            request = req as PostDeletePatientRequest;
            client = restClient;
        }

        public void Execute()
        {
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/Scheduling/ToDo/Patient/{PatientId}/Delete", "DELETE")]
                string pnUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Scheduling/ToDo/Patient/{4}/Delete",
                                                        DDSchedulingUrl,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.Id), request.UserId);
                DeleteToDoByPatientIdDataResponse pnDDResponse = client.Delete<DeleteToDoByPatientIdDataResponse>(pnUrl);
                if (pnDDResponse != null && pnDDResponse.Success)
                {
                    deletedIds = pnDDResponse.DeletedIds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD: PatientToDosCommand Execute::" + ex.Message, ex.InnerException);
            }
        }

        public void Undo()
        {
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/Scheduling/ToDo/UndoDelete", "PUT")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Scheduling/ToDo/UndoDelete",
                                            DDSchedulingUrl,
                                            "NG",
                                            request.Version,
                                            request.ContractNumber), request.UserId);
                UndoDeletePatientToDosDataResponse response = client.Put<UndoDeletePatientToDosDataResponse>(url, new UndoDeletePatientToDosDataRequest
                {
                    Ids = deletedIds,
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    UserId = request.UserId,
                    Version = request.Version
                } as object);
            }
            catch (Exception ex)
            {
                throw new Exception("AD: PatientToDosCommand Undo::" + ex.Message, ex.InnerException);
            }
        }
    }
}
