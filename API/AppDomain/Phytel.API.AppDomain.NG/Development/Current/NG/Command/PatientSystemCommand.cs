﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using ServiceStack.Service;

namespace Phytel.API.AppDomain.NG
{
    public class PatientSystemCommand : INGCommand
    {
        private PostDeletePatientRequest request;
        private List<string> deletedIds;  
        private IRestClient client;

        private static readonly string DDPatientSystemUrl = ConfigurationManager.AppSettings["DDPatientSystemUrl"];

        public PatientSystemCommand(PostDeletePatientRequest req, IRestClient restClient)
        {
            request = req as PostDeletePatientRequest;
            client = restClient;
        }

        public void Execute()
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/PatientSystem/Patient/{PatientId}/Delete", "DELETE")]
            string psUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientSystem/Patient/{4}/Delete",
                                                    DDPatientSystemUrl,
                                                    "NG",
                                                    request.Version,
                                                    request.ContractNumber,
                                                    request.Id), request.UserId);
            DeletePatientSystemByPatientIdDataResponse psDDResponse = client.Delete<DeletePatientSystemByPatientIdDataResponse>(psUrl);
            if (psDDResponse != null && psDDResponse.Success)
            {
                deletedIds = psDDResponse.DeletedIds;
            }
        }

        public void Undo()
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/PatientSystem/UndoDelete", "Put")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientSystem/UndoDelete",
                                        DDPatientSystemUrl,
                                        "NG",
                                        request.Version,
                                        request.ContractNumber), request.UserId);
            UndoDeletePatientSystemsDataResponse response = client.Put<UndoDeletePatientSystemsDataResponse>(url, new UndoDeletePatientSystemsDataRequest
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
