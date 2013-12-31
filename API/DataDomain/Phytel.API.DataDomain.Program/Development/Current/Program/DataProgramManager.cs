using Phytel.API.DataDomain.Program.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Program;
using Phytel.API.Interface;
using System.Collections.Generic;
using System;
using ServiceStack.ServiceInterface.ServiceModel;

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
            PutProgramToPatientResponse response;

            //IProgramRepository<PutProgramToPatientResponse> patRepo =
            //    Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramToPatientResponse>
            //    .GetPatientProgramRepository(request.ContractNumber, request.Context);


            if (IsValidContractProgramId(request))
            {
                response = new PutProgramToPatientResponse();
                response.Status = new ResponseStatus("", "ContractProgram does not exist.");
            }

            IProgramRepository<PutProgramToPatientResponse> patProgRepo =
                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramToPatientResponse>
                .GetPatientProgramRepository(request.ContractNumber, request.Context);

            response = patProgRepo.InsertPatientToProgramAssignment(request);

            return response;
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
            GetProgramDetailsSummaryResponse response;

            IProgramRepository<GetProgramDetailsSummaryResponse> repo =
                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<GetProgramDetailsSummaryResponse>
                .GetPatientProgramRepository(request.ContractNumber, request.Context);

            response = repo.GetPatientProgramDocumentDetailsById(request);

            return response;
        }
    }
}   
