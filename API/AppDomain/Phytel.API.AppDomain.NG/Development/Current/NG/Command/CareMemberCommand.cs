using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.CareMember.DTO;
using ServiceStack.Service;

namespace Phytel.API.AppDomain.NG
{
    public class CareMemberCommand : INGCommand
    {
        private PostDeletePatientRequest request;
        private List<string> deletedIds;  
        private IRestClient client;

        private static readonly string DDCareMemberUrl = ConfigurationManager.AppSettings["DDCareMemberUrl"];

        public CareMemberCommand(PostDeletePatientRequest req, IRestClient restClient)
        {
            request = req as PostDeletePatientRequest;
            client = restClient;
        }

        public void Execute()
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/CareMember/Patient/{PatientId}/Delete", "DELETE")]
            string cmUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/CareMember/Patient/{4}/Delete",
                                                    DDCareMemberUrl,
                                                    "NG",
                                                    request.Version,
                                                    request.ContractNumber,
                                                    request.Id), request.UserId);
            DeleteCareMemberByPatientIdDataResponse cmDDResponse = client.Delete<DeleteCareMemberByPatientIdDataResponse>(cmUrl);
            if (cmDDResponse != null && cmDDResponse.Success)
            {
                deletedIds = cmDDResponse.DeletedIds;
            }
        }

        public void Undo()
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/CareMember/UndoDelete", "PUT")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/CareMember/UndoDelete",
                                        DDCareMemberUrl,
                                        "NG",
                                        request.Version,
                                        request.ContractNumber), request.UserId);
             UndoDeleteCareMembersDataResponse response = client.Put<UndoDeleteCareMembersDataResponse>(url, new UndoDeleteCareMembersDataRequest
            {
                Ids = deletedIds,
                Context = "NG",
                ContractNumber = request.ContractNumber,
                UserId = request.UserId,
                Version = request.Version
            }as object);
        }
    }
}
