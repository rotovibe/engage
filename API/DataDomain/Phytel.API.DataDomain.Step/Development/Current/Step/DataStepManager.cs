using Phytel.API.DataDomain.Step.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Step;

namespace Phytel.API.DataDomain.Step
{
    public static class StepDataManager
    {
        public static GetStepResponse GetStepByID(GetStepRequest request)
        {
            GetStepResponse result = new GetStepResponse();

            IStepRepository<GetStepResponse> repo = StepRepositoryFactory<GetStepResponse>.GetStepRepository(request.ContractNumber, request.Context);
            result = repo.FindByID(request.StepID) as GetStepResponse;
            
            return (result != null ? result : new GetStepResponse());
        }

        public static GetAllStepsResponse GetStepList(GetAllStepsRequest request)
        {
            GetAllStepsResponse result = new GetAllStepsResponse();

            IStepRepository<GetAllStepsResponse> repo = StepRepositoryFactory<GetAllStepsResponse>.GetStepRepository(request.ContractNumber, request.Context);

            return result;
        }
    }
}   
