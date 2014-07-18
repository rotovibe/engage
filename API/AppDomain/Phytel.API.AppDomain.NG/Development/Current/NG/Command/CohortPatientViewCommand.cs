using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG
{
    public class CohortPatientViewCommand : INGCommand
    {
        private PostDeletePatientRequest request;
        private IRestClient client;
        private string deletedId;

        protected static readonly string DDPatientServiceURL = ConfigurationManager.AppSettings["DDPatientServiceUrl"];

        public CohortPatientViewCommand(PostDeletePatientRequest req, IRestClient restClient)
        {
            request = req;
            client = restClient;
        }

        public void Execute()
        {
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/CohortPatientView/Patient/{PatientId}/Delete", "DELETE")]
                string cpvUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/CohortPatientView/Patient/{4}/Delete",
                                                        DDPatientServiceURL,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.Id), request.UserId);
                DeleteCohortPatientViewDataResponse cpvDDResponse = client.Delete<DeleteCohortPatientViewDataResponse>(cpvUrl);
                if (cpvDDResponse != null && cpvDDResponse.Success)
                {
                    deletedId = cpvDDResponse.DeletedId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD: CohortPatientViewCommand Execute::" + ex.Message, ex.InnerException);
            }
        }

        public void Undo()
        {
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/CohortPatientView/UndoDelete", "PUT")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/CohortPatientView/UndoDelete",
                                            DDPatientServiceURL,
                                            "NG",
                                            request.Version,
                                            request.ContractNumber), request.UserId);
                UndoDeleteCohortPatientViewDataResponse response = client.Put<UndoDeleteCohortPatientViewDataResponse>(url, new UndoDeleteCohortPatientViewDataRequest
                {
                    Id = deletedId,
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    UserId = request.UserId,
                    Version = request.Version
                }
                as object);
            }
            catch (Exception ex)
            {
                throw new Exception("AD: CohortPatientViewCommand Undo::" + ex.Message, ex.InnerException);
            }
        }
    }
}
