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
    public class PatientProgramsCommand : INGCommand
    {
        private PostDeletePatientRequest request;
        private List<DeletedPatientProgram> deletedPatientPrograms; 
        private IRestClient client;
        private static readonly string DDProgramServiceUrl = ConfigurationManager.AppSettings["DDProgramServiceUrl"];

        public PatientProgramsCommand(PostDeletePatientRequest req, IRestClient restClient)
        {
            request = req as PostDeletePatientRequest;
            client = restClient;
        }

        public void Execute()
        {
            try
            {
                //  [Route("/{Context}/{Version}/{ContractNumber}/Program/Patient/{PatientId}/Delete", "DELETE")]
                string ppUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Program/Patient/{4}/Delete",
                                                        DDProgramServiceUrl,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.Id), request.UserId);
                DeletePatientProgramByPatientIdDataResponse ppDDResponse = client.Delete<DeletePatientProgramByPatientIdDataResponse>(ppUrl);
                if (ppDDResponse != null && ppDDResponse.Success)
                {
                    deletedPatientPrograms = ppDDResponse.DeletedPatientPrograms;
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
                //    [Route("/{Context}/{Version}/{ContractNumber}/Program/UndoDelete", "PUT")]
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
