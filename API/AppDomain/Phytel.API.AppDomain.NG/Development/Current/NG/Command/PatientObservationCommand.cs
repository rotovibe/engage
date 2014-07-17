﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;
using ServiceStack.Service;

namespace Phytel.API.AppDomain.NG
{
    public class PatientObservationCommand : INGCommand
    {
        private PostDeletePatientRequest request;
        private List<string> deletedIds;  
        private IRestClient client;
        private static readonly string DDPatientObservationsServiceUrl = ConfigurationManager.AppSettings["DDPatientObservationUrl"];

        public PatientObservationCommand(PostDeletePatientRequest req, IRestClient restClient)
        {
            request = req as PostDeletePatientRequest;
            client = restClient;
        }

        public void Execute()
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/PatientObservation/Patient/{PatientId}/Delete", "DELETE")]
            string poUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientObservation/Patient/{4}/Delete",
                                                    DDPatientObservationsServiceUrl,
                                                    "NG",
                                                    request.Version,
                                                    request.ContractNumber,
                                                    request.Id), request.UserId);
            DeletePatientObservationByPatientIdDataResponse poDDResponse = client.Delete<DeletePatientObservationByPatientIdDataResponse>(poUrl);
            if (poDDResponse != null && poDDResponse.Success)
            {
                deletedIds = poDDResponse.DeletedIds;
            }
        }

        public void Undo()
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/PatientObservation/UndoDelete", "PUT")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientObservation/UndoDelete",
                                        DDPatientObservationsServiceUrl,
                                        "NG",
                                        request.Version,
                                        request.ContractNumber), request.UserId);
            UndoDeletePatientObservationsDataResponse response = client.Put<UndoDeletePatientObservationsDataResponse>(url, new UndoDeletePatientObservationsDataRequest
            {
                Ids = deletedIds,
                Context = "NG",
                ContractNumber = request.ContractNumber,
                UserId = request.UserId,
                Version = request.Version
            } as object);
        }
    }
}
