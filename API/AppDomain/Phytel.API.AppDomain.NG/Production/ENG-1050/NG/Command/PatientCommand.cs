using System;
using System.Collections.Generic;
using System.Configuration;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG
{
    public class PatientCommand : INGCommand
    {
        private PostDeletePatientRequest request;
        private string deletedId;  
        private IRestClient client;

        protected static readonly string DDPatientServiceURL = ConfigurationManager.AppSettings["DDPatientServiceUrl"];

        public PatientCommand(PostDeletePatientRequest req, IRestClient restClient)
        {
            request = req as PostDeletePatientRequest;
            client = restClient;
        }

        public void Execute()
        {
            try
            { 
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{Id}/Delete", "DELETE")]
                string patientUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Delete",
                                                        DDPatientServiceURL,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.Id), request.UserId);
                DeletePatientDataResponse patientDDResponse = client.Delete<DeletePatientDataResponse>(patientUrl);
                if (patientDDResponse != null && patientDDResponse.Success)
                {
                    deletedId = patientDDResponse.DeletedId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD: PatientCommand Execute::" + ex.Message, ex.InnerException);
            }
        }

        public void Undo()
        {
            try
            { 
                // [Route("/{Context}/{Version}/{ContractNumber}/Patient/UndoDelete", "PUT")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/UndoDelete",
                                         DDPatientServiceURL,
                                         "NG",
                                         request.Version,
                                         request.ContractNumber), request.UserId);
                UndoDeletePatientDataResponse response = client.Put<UndoDeletePatientDataResponse>(url, new UndoDeletePatientDataRequest
                {
                    Id = deletedId,
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    UserId = request.UserId,
                    Version = request.Version
                }as object);
            }
            catch (Exception ex)
            {
                throw new Exception("AD: PatientCommand Undo::" + ex.Message, ex.InnerException);
            }
        }
    }

}
