using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.DataDomain.ProgramDesign.DTO;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public interface IProgramDesignDataManager
    {
        GetProgramDesignResponse GetProgramDesignByID(GetProgramDesignRequest request);
        GetAllProgramDesignsResponse GetProgramDesignList(GetAllProgramDesignsRequest request);
        GetProgramResponse GetProgramByID(GetProgramRequest request);
        GetProgramByNameResponse GetProgramByName(GetProgramByNameRequest request);
        GetAllActiveProgramsResponse GetAllActiveContractPrograms(GetAllActiveProgramsRequest request);
        GetModuleResponse GetModuleByID(GetModuleRequest request);
        GetAllModulesResponse GetModuleList(GetAllModulesRequest request);
        GetActionDataResponse GetActionByID(GetActionDataRequest request);
        GetAllActionsDataResponse GetActionsList(GetAllActionsDataRequest request);
        GetYesNoStepDataResponse GetYesNoStepByID(GetYesNoStepDataRequest request);
        GetAllYesNoStepDataResponse GetAllYesNoSteps(GetAllYesNoStepDataRequest request);
        GetTextStepDataResponse GetTextStepByID(GetTextStepDataRequest request);
        GetAllTextStepDataResponse GetAllTextSteps(GetAllTextStepDataRequest request);
    }
}
