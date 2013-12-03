using Phytel.API.DataDomain.Action.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Action;

namespace Phytel.API.DataDomain.Action
{
    public static class ActionDataManager
    {
        public static GetActionResponse GetActionByID(GetActionRequest request)
        {
            GetActionResponse result = new GetActionResponse();

            IActionRepository<GetActionResponse> repo = ActionRepositoryFactory<GetActionResponse>.GetActionRepository(request.ContractNumber, request.Context);
            result = repo.FindByID(request.ActionID) as GetActionResponse;
            
            return (result != null ? result : new GetActionResponse());
        }

        public static GetAllActionsResponse GetActionList(GetAllActionsRequest request)
        {
            GetAllActionsResponse result = new GetAllActionsResponse();

            IActionRepository<GetAllActionsResponse> repo = ActionRepositoryFactory<GetAllActionsResponse>.GetActionRepository(request.ContractNumber, request.Context);

            return result;
        }
    }
}   
