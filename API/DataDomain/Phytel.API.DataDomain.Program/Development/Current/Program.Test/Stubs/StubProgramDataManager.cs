using Phytel.API.DataDomain.Program.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Program.Test.Stubs
{
    public class StubProgramDataManager : IProgramDataManager
    {
        public MongoDB.DTO.IDTOUtility DTOUtility { get; set; }
        public IProgramRepositoryFactory Factory { get; set; }

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
                    EligibilityEndDate = System.DateTime.UtcNow,
                    ObjectivesData = new List<DTO.ObjectiveInfoData> { new DTO.ObjectiveInfoData{ Id ="123456789012345678901234",
                         Value = "testing",
                          Unit = "lbs",
                           Status = 1
                    } },
                    Modules = new List<ModuleDetail>() { 
                    new ModuleDetail { Id = "000000000000000000000000", 
                        Name = "Test stub module 1",
                         Description = "BSHSI - Outreach & Enrollment",
                          SourceId ="532b5585a381168abe00042c",
                        Actions = new List<ActionsDetail>(){ 
                            new ActionsDetail{ Id = "000000000000000000000000", ElementState = 4, Name ="test action from stub", Text = "test action 1"} } ,
                             AttrStartDate = Convert.ToDateTime("1/1/1900"),
                             AttrEndDate = Convert.ToDateTime("1/1/1901"),
                             AssignedOn = Convert.ToDateTime("1/1/1999"),
                             AssignTo = "123456789011111111112222",
                             AssignBy = "123456789011111111112223"
                    }
                    }
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
