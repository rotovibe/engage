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
                        Objectives = new List<DTO.ObjectiveInfoData> { 
                            new DTO.ObjectiveInfoData{ 
                                Id ="123456789012345678901234", 
                                Value = "testing", 
                                Unit = "lbs", 
                                Status = 1} },
                        Actions = new List<ActionsDetail>(){ 
                            new ActionsDetail{ 
                                Id = "000000000000000000000000", 
                                SourceId="123456789012345678901234", 
                                ElementState = 4, 
                                Name ="test action from stub", 
                                Text = "test action 1",
                                AssignBy = "123456789011111111112233",
                                AssignTo = "123456789011111111112232",
                                AssignDate = Convert.ToDateTime("1/1/1899"),
                                AttrStartDate = Convert.ToDateTime("1/1/1800"),
                                AttrEndDate = Convert.ToDateTime("1/1/1801"),
                                Description = "BSHSI - Outreach & Enrollment action description",
                                Objectives = new List<DTO.ObjectiveInfoData> 
                                { new DTO.ObjectiveInfoData{ 
                                    Id ="123456789012345678901234",
                                    Value = "Action testing",
                                    Unit = "lbs",
                                    Status = 1
                                },
                                new DTO.ObjectiveInfoData{ 
                                    Id ="123456789012345678904567",
                                    Value = "Action testing 2",
                                    Unit = "lbs",
                                    Status = 2
                                } }
                            } } ,
                             AttrStartDate = Convert.ToDateTime("1/1/1900"),
                             AttrEndDate = Convert.ToDateTime("1/1/1901"),
                             AssignDate = Convert.ToDateTime("1/1/1999"),
                             AssignTo = "123456789011111111112222",
                             AssignBy = "123456789011111111112223"
                    }
                    }
                },
                Version = 1.0
            };
            return response;
        }

        public DTO.GetPatientProgramsDataResponse GetPatientPrograms(DTO.GetPatientProgramsDataRequest request)
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
            DTO.PutProgramToPatientResponse response = new PutProgramToPatientResponse
            {
                Outcome = new DTO.Outcome
                {
                    Reason = "SUccess!",
                    Result = 1
                },
                program = new ProgramInfo
                {
                    ElementState = 1,
                    Id = request.ContractProgramId,
                    Name = "Test program",
                    PatientId = request.PatientId,
                    ProgramState = 1,
                    ShortName = "short program",
                    Status = 1
                },
                Version = 1.0
            };

            return response;
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

        public DeletePatientProgramByPatientIdDataResponse DeletePatientProgramByPatientId(DeletePatientProgramByPatientIdDataRequest request)
        {
            throw new NotImplementedException();
        }


        public UndoDeletePatientProgramDataResponse UndoDeletePatientPrograms(UndoDeletePatientProgramDataRequest request)
        {
            throw new NotImplementedException();
        }


        public DeletePatientProgramDataResponse DeletePatientProgram(DeletePatientProgramDataRequest request)
        {
            throw new NotImplementedException();
        }


        public PutInsertResponseResponse PutInsertResponse(PutInsertResponseRequest r)
        {
            throw new NotImplementedException();
        }
    }
}
