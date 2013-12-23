using Phytel.API.DataDomain.Program.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Program;
using Phytel.API.Interface;
using System.Collections.Generic;
using System;

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
            Outcome result;

            IProgramRepository<PutProgramToPatientResponse> repo =
                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramToPatientResponse>.GetProgramRepository(request.ContractNumber, request.Context);

            result = repo.InsertPatientToProgramAssignment(request);

            response.Result = result;

            return response;
        }
    }
}   
