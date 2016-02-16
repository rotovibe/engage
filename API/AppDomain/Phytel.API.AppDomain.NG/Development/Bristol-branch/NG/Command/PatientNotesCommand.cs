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
    public class PatientNotesCommand : INGCommand
    {
        private PostDeletePatientRequest request;
        private List<string> deletedIds;  
        private IRestClient client;

        private static readonly string DDPatientNoteUrl = ConfigurationManager.AppSettings["DDPatientNoteUrl"];

        public PatientNotesCommand(PostDeletePatientRequest req, IRestClient restClient)
        {
            request = req as PostDeletePatientRequest;
            client = restClient;
        }

        public void Execute()
        {
            try
            { 
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientNote/Patient/{PatientId}/Delete", "DELETE")]
                string pnUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientNote/Patient/{4}/Delete",
                                                        DDPatientNoteUrl,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.Id), request.UserId);
                DeleteNoteByPatientIdDataResponse pnDDResponse = client.Delete<DeleteNoteByPatientIdDataResponse>(pnUrl);
                if (pnDDResponse != null && pnDDResponse.Success)
                {
                    deletedIds = pnDDResponse.DeletedIds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD: PatientNoteCommand Execute::" + ex.Message, ex.InnerException);
            }
        }

        public void Undo()
        {
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientNote/UndoDelete", "PUT")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientNote/UndoDelete",
                                            DDPatientNoteUrl,
                                            "NG",
                                            request.Version,
                                            request.ContractNumber), request.UserId);
                UndoDeletePatientNotesDataResponse response = client.Put<UndoDeletePatientNotesDataResponse>(url, new UndoDeletePatientNotesDataRequest
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
                throw new Exception("AD: PatientNoteCommand Undo::" + ex.Message, ex.InnerException);
            }
        }
    }
}
