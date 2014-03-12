using Phytel.API.DataDomain.Step.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Step;
using Phytel.API.Interface;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Phytel.API.DataDomain.Step
{
    public static class StepDataManager
    {
        private static readonly string yesnostep = "yesno";
        private static readonly string textstep = "text";
        
        public static GetYesNoStepDataResponse GetYesNoStepByID(GetYesNoStepDataRequest request)
        {
            GetYesNoStepDataResponse result = new GetYesNoStepDataResponse();

            IStepRepository<GetYesNoStepDataResponse> repo = StepRepositoryFactory<GetYesNoStepDataResponse>.GetStepRepository(request.ContractNumber, request.Context, yesnostep);
            repo.UserId = request.UserId;
            result = repo.FindByID(request.YesNoStepID) as GetYesNoStepDataResponse;

            return (result != null ? result : new GetYesNoStepDataResponse());
        }

        public static GetTextStepDataResponse GetTextStepByID(GetTextStepDataRequest request)
        {
            GetTextStepDataResponse result = new GetTextStepDataResponse();

            IStepRepository<GetTextStepDataResponse> repo = StepRepositoryFactory<GetTextStepDataResponse>.GetStepRepository(request.ContractNumber, request.Context, textstep);
            repo.UserId = request.UserId;
            result = repo.FindByID(request.TextStepID) as GetTextStepDataResponse;

            return (result != null ? result : new GetTextStepDataResponse());
        }
    }
}   
