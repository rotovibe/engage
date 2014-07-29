using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;

namespace Phytel.API.AppDomain.NG
{
    public class PatientProgramCommand : INGCommand
    {
        private PostRemovePatientProgramRequest request;
        private List<DeletedPatientProgram> deletedPatientPrograms; 
        private IRestClient client;
        private static readonly string DDProgramServiceUrl = ConfigurationManager.AppSettings["DDProgramServiceUrl"];

        public PatientProgramCommand(PostRemovePatientProgramRequest req, IRestClient restClient)
        {
            request = req as PostRemovePatientProgramRequest;
            client = restClient;
        }

        public void Execute()
        {
            try
            {
                // [Route("/{Context}/{Version}/{ContractNumber}/Program/{Id}/Delete", "DELETE")]
                string ppUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Program/{4}/Delete",
                                                        DDProgramServiceUrl,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.Id), request.UserId);
                DeletePatientProgramDataResponse ppDDResponse = client.Delete<DeletePatientProgramDataResponse>(ppUrl);
                if (ppDDResponse != null)
                {
                    DeletedPatientProgram pp = ppDDResponse.DeletedPatientProgram;
                    deletedPatientPrograms = new List<DeletedPatientProgram>() { pp };
                    if (!ppDDResponse.Success)
                    {
                        Undo();
                        throw new Exception("Error occurred during AppDomain: PatientProgramCommand Execute method");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD: PatientProgramCommand Execute::" + ex.Message, ex.InnerException);
            }
        }

        public void Undo()
        {
            try 
            { 
                //[Route("/{Context}/{Version}/{ContractNumber}/Program/UndoDelete", "PUT")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Program/UndoDelete",
                                            DDProgramServiceUrl,
                                            "NG",
                                            request.Version,
                                            request.ContractNumber), request.UserId);
                UndoDeletePatientProgramDataResponse response = client.Put<UndoDeletePatientProgramDataResponse>(url, new UndoDeletePatientProgramDataRequest
                {
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    Ids = deletedPatientPrograms,
                    UserId = request.UserId,
                    Version = request.Version
                });
            }
            catch (Exception ex)
            {
                throw new Exception("AD: PatientProgramCommand Undo::" + ex.Message, ex.InnerException);
            }
        }
    }
}
