using Phytel.API.DataDomain.Step.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Step;

namespace Phytel.API.DataDomain.Step
{
    public static class StepDataManager
    {
        public static StepResponse GetStepByID(StepRequest request)
        {
            StepResponse result = new StepResponse();

            IStepRepository<StepResponse> repo = StepRepositoryFactory<StepResponse>.GetStepRepository(request.ContractNumber, request.Context);
            result = repo.FindByID(request.StepID) as StepResponse;
            
            return (result != null ? result : new StepResponse());
        }

        public static StepListResponse GetStepList(StepListRequest request)
        {
            StepListResponse result = new StepListResponse();

            IStepRepository<StepListResponse> repo = StepRepositoryFactory<StepListResponse>.GetStepRepository(request.ContractNumber, request.Context);

            return result;
        }
    }
}   
