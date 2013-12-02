using Phytel.API.DataDomain.Action.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Action;

namespace Phytel.API.DataDomain.Action
{
    public static class ActionDataManager
    {
        public static ActionResponse GetActionByID(ActionRequest request)
        {
            ActionResponse result = new ActionResponse();

            IActionRepository<ActionResponse> repo = ActionRepositoryFactory<ActionResponse>.GetActionRepository(request.ContractNumber, request.Context);
            result = repo.FindByID(request.ActionID) as ActionResponse;
            
            return (result != null ? result : new ActionResponse());
        }

        public static ActionListResponse GetActionList(ActionListRequest request)
        {
            ActionListResponse result = new ActionListResponse();

            IActionRepository<ActionListResponse> repo = ActionRepositoryFactory<ActionListResponse>.GetActionRepository(request.ContractNumber, request.Context);

            return result;
        }
    }
}   
