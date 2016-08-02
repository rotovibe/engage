using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Medication.DTO;
using ServiceStack.Service;

namespace Phytel.API.AppDomain.NG
{
    public class PatientMedSuppsCommand : INGCommand
    {
        private PostDeletePatientRequest request;
        private List<string> deletedIds;  
        private IRestClient client;

        private static readonly string DDMedicationUrl = ConfigurationManager.AppSettings["DDMedicationUrl"];

        public PatientMedSuppsCommand(PostDeletePatientRequest req, IRestClient restClient)
        {
            request = req as PostDeletePatientRequest;
            client = restClient;
        }

        public void Execute()
        {
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/Patient/{PatientId}/Delete", "DELETE")]
                string pnUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientMedSupp/Patient/{4}/Delete",
                                                        DDMedicationUrl,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.Id), request.UserId);
                DeleteMedSuppsByPatientIdDataResponse pnDDResponse = client.Delete<DeleteMedSuppsByPatientIdDataResponse>(pnUrl);
                if (pnDDResponse != null && pnDDResponse.Success)
                {
                    deletedIds = pnDDResponse.DeletedIds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD: PatientMedSuppsCommand Execute::" + ex.Message, ex.InnerException);
            }
        }

        public void Undo()
        {
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/UndoDelete", "PUT")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientMedSupp/UndoDelete",
                                            DDMedicationUrl,
                                            "NG",
                                            request.Version,
                                            request.ContractNumber), request.UserId);
                UndoDeletePatientMedSuppsDataResponse response = client.Put<UndoDeletePatientMedSuppsDataResponse>(url, new UndoDeletePatientMedSuppsDataRequest
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
                throw new Exception("AD: PatientMedSuppsCommand Undo::" + ex.Message, ex.InnerException);
            }
        }
    }
}
