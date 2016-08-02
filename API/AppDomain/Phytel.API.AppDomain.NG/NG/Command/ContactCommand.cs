using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using ServiceStack.Service;

namespace Phytel.API.AppDomain.NG
{
    public class ContactCommand : INGCommand
    {
        private PostDeletePatientRequest request;
        private string deletedId;
        private List<ContactWithUpdatedRecentList> contactWithUpdatedRecentLists;
        private IRestClient client;

        protected static readonly string DDContactServiceUrl = ConfigurationManager.AppSettings["DDContactServiceUrl"];

        public ContactCommand(PostDeletePatientRequest req, IRestClient restClient)
        {
            request = req as PostDeletePatientRequest;
            client = restClient;
        }

        public void Execute()
        {
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/Contact/Patient/{PatientId}/Delete", "DELETE")]
                string cUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contact/Patient/{4}/Delete",
                                                        DDContactServiceUrl,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.Id), request.UserId);
                DeleteContactByPatientIdDataResponse cDDResponse = client.Delete<DeleteContactByPatientIdDataResponse>(cUrl);
                if (cDDResponse != null && cDDResponse.Success)
                {
                    deletedId = cDDResponse.DeletedId;
                    contactWithUpdatedRecentLists = cDDResponse.ContactWithUpdatedRecentLists;
                }
            }     
            catch (Exception ex)
            {
                throw new Exception("AD: ContactCommand Execute::" + ex.Message, ex.InnerException);
            }
        }

        public void Undo()
        {
            try
            { 
                //[Route("/{Context}/{Version}/{ContractNumber}/Contact/UndoDelete", "PUT")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contact/UndoDelete",
                                            DDContactServiceUrl,
                                            "NG",
                                            request.Version,
                                            request.ContractNumber), request.UserId);
                UndoDeleteContactDataResponse response = client.Put<UndoDeleteContactDataResponse>(url, new UndoDeleteContactDataRequest
                {
                    Id = deletedId,
                    ContactWithUpdatedRecentLists = contactWithUpdatedRecentLists,
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    Version = request.Version,
                    UserId = request.UserId
                }as object);
            }
            catch (Exception ex)
            {
                throw new Exception("AD: ContactCommand Undo::" + ex.Message, ex.InnerException);
            }
        }
    }
}
