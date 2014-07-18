using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using System;
namespace Phytel.API.DataDomain.Program
{
    public interface IProgramDataManager
    {
        IDTOUtility DTOUtility { get; set; }
        IProgramRepositoryFactory Factory { get; set; }
        GetPatientActionDetailsDataResponse GetActionDetails(Phytel.API.DataDomain.Program.DTO.GetPatientActionDetailsDataRequest request);
        GetAllActiveProgramsResponse GetAllActiveContractPrograms(Phytel.API.DataDomain.Program.DTO.GetAllActiveProgramsRequest request);
        GetProgramDetailsSummaryResponse GetPatientProgramDetailsById(Phytel.API.DataDomain.Program.DTO.GetProgramDetailsSummaryRequest request);
        GetPatientProgramsResponse GetPatientPrograms(Phytel.API.DataDomain.Program.DTO.GetPatientProgramsRequest request);
        GetProgramAttributeResponse GetProgramAttributes(Phytel.API.DataDomain.Program.DTO.GetProgramAttributeRequest request);
        GetProgramResponse GetProgramByID(Phytel.API.DataDomain.Program.DTO.GetProgramRequest request);
        GetProgramByNameResponse GetProgramByName(Phytel.API.DataDomain.Program.DTO.GetProgramByNameRequest request);
        GetStepResponseListResponse GetStepResponse(Phytel.API.DataDomain.Program.DTO.GetStepResponseListRequest request);
        GetStepResponseResponse GetStepResponse(Phytel.API.DataDomain.Program.DTO.GetStepResponseRequest request);
        PutProgramAttributesResponse InsertProgramAttributes(Phytel.API.DataDomain.Program.DTO.PutProgramAttributesRequest request);
        PutProgramToPatientResponse PutPatientToProgram(Phytel.API.DataDomain.Program.DTO.PutProgramToPatientRequest request);
        PutProgramActionProcessingResponse PutProgramActionUpdate(Phytel.API.DataDomain.Program.DTO.PutProgramActionProcessingRequest request);
        PutUpdateProgramAttributesResponse PutUpdateProgramAttributes(Phytel.API.DataDomain.Program.DTO.PutUpdateProgramAttributesRequest request);
        PutUpdateResponseResponse PutUpdateResponse(Phytel.API.DataDomain.Program.DTO.PutUpdateResponseRequest r);
        DeletePatientProgramByPatientIdDataResponse DeletePatientProgramByPatientId(DeletePatientProgramByPatientIdDataRequest request);
        UndoDeletePatientProgramDataResponse UndoDeletePatientPrograms(UndoDeletePatientProgramDataRequest request);
    }
}
