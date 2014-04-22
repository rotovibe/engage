using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Program.Test.Stubs
{
    public class StubProgramDataManager : IProgramDataManager
    {
        public DTO.GetPatientActionDetailsDataResponse GetActionDetails(DTO.GetPatientActionDetailsDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetAllActiveProgramsResponse GetAllActiveContractPrograms(DTO.GetAllActiveProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetProgramDetailsSummaryResponse GetPatientProgramDetailsById(DTO.GetProgramDetailsSummaryRequest request)
        {
            DTO.GetProgramDetailsSummaryResponse response = new DTO.GetProgramDetailsSummaryResponse
            {
                Program = new DTO.ProgramDetail
                {
                    Name = "test patient program",
                    Description = "test program description",
                    Id = "000000000000000000000000",
                    EligibilityRequirements = "Test eligibility requirements",
                    EligibilityStartDate = System.DateTime.UtcNow,
                    EligibilityEndDate = System.DateTime.UtcNow
                },
                Version = 1.0
            };
            return response;
        }

        public DTO.GetPatientProgramsResponse GetPatientPrograms(DTO.GetPatientProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetProgramAttributeResponse GetProgramAttributes(DTO.GetProgramAttributeRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetProgramResponse GetProgramByID(DTO.GetProgramRequest request)
        {
            DTO.GetProgramResponse response = new DTO.GetProgramResponse { Program = new DTO.Program { }, Version = 1.0 };
            return response;
        }

        public DTO.GetProgramByNameResponse GetProgramByName(DTO.GetProgramByNameRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetStepResponseListResponse GetStepResponse(DTO.GetStepResponseListRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetStepResponseResponse GetStepResponse(DTO.GetStepResponseRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutProgramAttributesResponse InsertProgramAttributes(DTO.PutProgramAttributesRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutProgramToPatientResponse PutPatientToProgram(DTO.PutProgramToPatientRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutProgramActionProcessingResponse PutProgramActionUpdate(DTO.PutProgramActionProcessingRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutUpdateProgramAttributesResponse PutUpdateProgramAttributes(DTO.PutUpdateProgramAttributesRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutUpdateResponseResponse PutUpdateResponse(DTO.PutUpdateResponseRequest r)
        {
            throw new NotImplementedException();
        }
    }
}
