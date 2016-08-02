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
        GetPatientProgramsDataResponse GetPatientPrograms(Phytel.API.DataDomain.Program.DTO.GetPatientProgramsDataRequest request);
        GetProgramAttributeResponse GetProgramAttributes(Phytel.API.DataDomain.Program.DTO.GetProgramAttributeRequest request);
        GetProgramResponse GetProgramByID(Phytel.API.DataDomain.Program.DTO.GetProgramRequest request);
        GetProgramByNameResponse GetProgramByName(Phytel.API.DataDomain.Program.DTO.GetProgramByNameRequest request);
        GetStepResponseListResponse GetStepResponse(Phytel.API.DataDomain.Program.DTO.GetStepResponseListRequest request);
        GetStepResponseResponse GetStepResponse(GetStepResponseRequest request);
        PutProgramAttributesResponse InsertProgramAttributes(PutProgramAttributesRequest request);
        PutProgramToPatientResponse PutPatientToProgram(PutProgramToPatientRequest request);
        PutProgramActionProcessingResponse PutProgramActionUpdate(PutProgramActionProcessingRequest request);
        PutUpdateProgramAttributesResponse PutUpdateProgramAttributes(PutUpdateProgramAttributesRequest request);
        PutUpdateResponseResponse PutUpdateResponse(PutUpdateResponseRequest r);
        PutInsertResponseResponse PutInsertResponse(PutInsertResponseRequest r);
        DeletePatientProgramByPatientIdDataResponse DeletePatientProgramByPatientId(DeletePatientProgramByPatientIdDataRequest request);
        UndoDeletePatientProgramDataResponse UndoDeletePatientPrograms(UndoDeletePatientProgramDataRequest request);
        DeletePatientProgramDataResponse DeletePatientProgram(DeletePatientProgramDataRequest request);
    }
}
