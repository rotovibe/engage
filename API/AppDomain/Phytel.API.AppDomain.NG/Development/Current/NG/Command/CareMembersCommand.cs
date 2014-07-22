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
    public class CareMembersCommand : INGCommand
    {
        private PostDeletePatientRequest request;
        private List<string> deletedIds;  
        private IRestClient client;

        private static readonly string DDCareMemberUrl = ConfigurationManager.AppSettings["DDCareMemberUrl"];

        public CareMembersCommand(PostDeletePatientRequest req, IRestClient restClient)
        {
            request = req as PostDeletePatientRequest;
            client = restClient;
        }

        public void Execute()
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("AD: CareMemberCommand Execute::" + ex.Message, ex.InnerException);
            }
        }

        public void Undo()
        {
            try
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
              } as object);
            }
            catch (Exception ex)
            {
                throw new Exception("AD: CareMemberCommand Undo::" + ex.Message, ex.InnerException);
            }
        }
    }
}
