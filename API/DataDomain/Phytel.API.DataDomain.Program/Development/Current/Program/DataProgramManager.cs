using Phytel.API.DataDomain.Program.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Program;
using Phytel.API.Interface;
using System.Collections.Generic;
using System;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Configuration;
using Phytel.API.DataDomain.Program.MongoDB.DTO;

namespace Phytel.API.DataDomain.Program
{
    public static class ProgramDataManager
    {
        public static GetProgramResponse GetProgramByID(GetProgramRequest request)
        {
            GetProgramResponse programResponse = new GetProgramResponse();
            DTO.Program result;

            IProgramRepository<GetProgramResponse> repo = ProgramRepositoryFactory<GetProgramResponse>.GetProgramRepository(request.ContractNumber, request.Context);
            result = repo.FindByID(request.ProgramID) as DTO.Program;

            programResponse.Program = result;
            return (programResponse != null ? programResponse : new GetProgramResponse());
        }

        public static GetAllActiveProgramsResponse GetAllActiveContractPrograms(GetAllActiveProgramsRequest request)
        {
            GetAllActiveProgramsResponse response = new GetAllActiveProgramsResponse();
            List<ProgramInfo> result;

            IProgramRepository<GetAllActiveProgramsResponse> repo =
                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<GetAllActiveProgramsResponse>.GetProgramRepository(request.ContractNumber, request.Context);

            result = repo.GetActiveProgramsInfoList(request);
            response.Programs = result;

            return response;
        }

        public static PutProgramToPatientResponse PutPatientToProgram(PutProgramToPatientRequest request)
        {
            PutProgramToPatientResponse response = new PutProgramToPatientResponse();

            if (!IsValidPatientId(request))
            {
                return FormatExceptionResponse(response, "Patient does not exist or has an invalid id.", "500");
            }

            if (!IsValidContractProgramId(request))
            {
                return FormatExceptionResponse(response, "ContractProgram does not exist or has an invalid identifier.", "500");
            }

            if (!IsContractProgramAssignable(request))
            {
                return FormatExceptionResponse(response, "ContractProgram is not currently active.", "500");
            }

            IProgramRepository<PutProgramToPatientResponse> patProgRepo =
                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramToPatientResponse>
                .GetPatientProgramRepository(request.ContractNumber, request.Context);

            object resp = patProgRepo.Insert((object)request);
            response = (PutProgramToPatientResponse)resp;

            return response;
        }

        private static PutProgramToPatientResponse FormatExceptionResponse(PutProgramToPatientResponse response, string reason, string errorcode)
        {
            response.Status = new ResponseStatus(errorcode, reason);
            response.Outcome = new Outcome() { Reason = reason, Result = 0 };
            return response;
        }

        private static bool IsContractProgramAssignable(PutProgramToPatientRequest p)
        {
            bool result = false;

            IProgramRepository<ContractProgram> contractProgRepo =
                            Phytel.API.DataDomain.Program.ProgramRepositoryFactory<ContractProgram>
                            .GetContractProgramRepository(p.ContractNumber, p.Context);

            ContractProgram c = contractProgRepo.FindByID(p.ContractProgramId) as ContractProgram;

            if (c != null)
            {
                if (c.Status == 1 && c.Delete != true)
                    result = true;
            }

            return result;
        }

        private static bool IsValidPatientId(PutProgramToPatientRequest request)
        {

            bool result = false;
            string path = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            IRestClient client = new JsonServiceClient();
            GetPatientDataResponse response = client.Get<GetPatientDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/patient/{4}", 
                path,
                context,
                version,
                contractNumber,
                request.PatientId));

            if (response.Patient != null)
            {
                result = true;
            }

            return result;
        }

        private static bool IsValidContractProgramId(PutProgramToPatientRequest request)
        {
            bool result = false;
            IProgramRepository<PutProgramToPatientResponse> contractProgRepo =
                            Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramToPatientResponse>
                            .GetContractProgramRepository(request.ContractNumber, request.Context);

            object contractProgram = contractProgRepo.FindByID(request.ContractProgramId);
            if (contractProgram != null)
            {
                result = true;
            }

            return result;
        }

        public static GetProgramDetailsSummaryResponse GetPatientProgramDetailsById(GetProgramDetailsSummaryRequest request)
        {
            try
            {
                GetProgramDetailsSummaryResponse response = null;

                IProgramRepository<GetProgramDetailsSummaryResponse> repo =
                    Phytel.API.DataDomain.Program.ProgramRepositoryFactory<GetProgramDetailsSummaryResponse>
                    .GetPatientProgramRepository(request.ContractNumber, request.Context);

                response = repo.FindByID(request.ProgramId) as GetProgramDetailsSummaryResponse;

                return response;
            }
            catch
            {
                throw;
            }
        }
    }
}   
