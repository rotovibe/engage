using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Allergy.DTO;
using ServiceStack.Service;

namespace Phytel.API.AppDomain.NG
{
    public class PatientAllergiesCommand : INGCommand
    {
        private PostDeletePatientRequest request;
        private List<string> deletedIds;  
        private IRestClient client;

        private static readonly string DDAllergyUrl = ConfigurationManager.AppSettings["DDAllergyUrl"];

        public PatientAllergiesCommand(PostDeletePatientRequest req, IRestClient restClient)
        {
            request = req as PostDeletePatientRequest;
            client = restClient;
        }

        public void Execute()
        {
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/Patient/{PatientId}/Delete", "DELETE")]
                string pnUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientAllergy/Patient/{4}/Delete",
                                                        DDAllergyUrl,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.Id), request.UserId);
                DeleteAllergiesByPatientIdDataResponse pnDDResponse = client.Delete<DeleteAllergiesByPatientIdDataResponse>(pnUrl);
                if (pnDDResponse != null && pnDDResponse.Success)
                {
                    deletedIds = pnDDResponse.DeletedIds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD: PatientAllergiesCommand Execute::" + ex.Message, ex.InnerException);
            }
        }

        public void Undo()
        {
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/UndoDelete", "PUT")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientAllergy/UndoDelete",
                                            DDAllergyUrl,
                                            "NG",
                                            request.Version,
                                            request.ContractNumber), request.UserId);
                UndoDeletePatientAllergiesDataResponse response = client.Put<UndoDeletePatientAllergiesDataResponse>(url, new UndoDeletePatientAllergiesDataRequest
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
                throw new Exception("AD: PatientAllergiesCommand Undo::" + ex.Message, ex.InnerException);
            }
        }
    }
}
