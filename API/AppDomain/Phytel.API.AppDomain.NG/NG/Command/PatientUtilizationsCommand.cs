using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientNote.DTO;
using ServiceStack.Service;

namespace Phytel.API.AppDomain.NG
{
    public class PatientUtilizationsCommand : INGCommand
    {
        private PostDeletePatientRequest request;
        private List<string> deletedIds;  
        private IRestClient client;

        private static readonly string DDPatientNoteUrl = ConfigurationManager.AppSettings["DDPatientNoteUrl"];

        public PatientUtilizationsCommand(PostDeletePatientRequest req, IRestClient restClient)
        {
            request = req as PostDeletePatientRequest;
            client = restClient;
        }

        public void Execute()
        {
            try
            { 
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientUtilization/Patient/{PatientId}/Delete", "DELETE")]
                string pnUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientUtilization/Patient/{4}/Delete",
                                                        DDPatientNoteUrl,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.Id), request.UserId);
                DeleteUtilizationByPatientIdDataResponse pnDDResponse = client.Delete<DeleteUtilizationByPatientIdDataResponse>(pnUrl);
                if (pnDDResponse != null && pnDDResponse.Success)
                {
                    deletedIds = pnDDResponse.DeletedIds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD: PatientUtilizationCommand Execute::" + ex.Message, ex.InnerException);
            }
        }

        public void Undo()
        {
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientUtilization/UndoDelete", "PUT")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientUtilization/UndoDelete",
                                            DDPatientNoteUrl,
                                            "NG",
                                            request.Version,
                                            request.ContractNumber), request.UserId);
                UndoDeletePatientUtilizationsDataResponse response = client.Put<UndoDeletePatientUtilizationsDataResponse>(url, new UndoDeletePatientUtilizationsDataRequest
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
                throw new Exception("AD: PatientUtilizationCommand Undo::" + ex.Message, ex.InnerException);
            }
        }
    }
}
