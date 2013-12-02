using Phytel.API.DataDomain.Step;
using Phytel.API.DataDomain.Step.DTO;

namespace Phytel.API.DataDomain.Step.Service
{
    public class StepService : ServiceStack.ServiceInterface.Service
    {
        public StepResponse Post(StepRequest request)
        {
            StepResponse response = StepDataManager.GetStepByID(request);
            response.Version = request.Version;
            return response;
        }

        public StepResponse Get(StepRequest request)
        {
            StepResponse response = StepDataManager.GetStepByID(request);
            response.Version = request.Version;
            return response;
        }

        public StepListResponse Post(StepListRequest request)
        {
            StepListResponse response = StepDataManager.GetStepList(request);
            response.Version = request.Version;
            return response;
        }
    }
}