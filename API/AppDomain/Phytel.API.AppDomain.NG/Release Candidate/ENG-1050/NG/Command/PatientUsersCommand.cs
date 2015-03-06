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
    public class PatientUsersCommand : INGCommand
    {
        private PostDeletePatientRequest request;
        private IRestClient client;
        private List<string> deletedIds; 

        protected static readonly string DDPatientServiceURL = ConfigurationManager.AppSettings["DDPatientServiceUrl"];

        public PatientUsersCommand(PostDeletePatientRequest req, IRestClient restClient)
        {
            request = req;
            client = restClient;
        }

        public void Execute()
        {
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientUser/Patient/{PatientId}/Delete", "DELETE")]
                string patientUserUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientUser/Patient/{4}/Delete",
                                                        DDPatientServiceURL,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.Id), request.UserId);
                DeletePatientUserByPatientIdDataResponse patientUserDDResponse = client.Delete<DeletePatientUserByPatientIdDataResponse>(patientUserUrl);
                if (patientUserDDResponse != null && patientUserDDResponse.Success)
                {
                    deletedIds = patientUserDDResponse.DeletedIds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD: PatientUserCommand Execute::" + ex.Message, ex.InnerException);
            }
        }

        public void Undo()
        {
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/PatientUser/UndoDelete", "PUT")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientUser/UndoDelete",
                                DDPatientServiceURL,
                                "NG",
                                request.Version,
                                request.ContractNumber), request.UserId);

                UndoDeletePatientUsersDataResponse response = client.Put<UndoDeletePatientUsersDataResponse>(url, new UndoDeletePatientUsersDataRequest
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
                throw new Exception("AD: PatientUserCommand Undo::" + ex.Message, ex.InnerException);
            }
        }
    }
}
