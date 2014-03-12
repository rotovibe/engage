using Phytel.API.DataDomain.Action.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Action;

namespace Phytel.API.DataDomain.Action
{
    public static class ActionDataManager
    {
        public static GetActionDataResponse GetActionByID(GetActionDataRequest request)
        {
            GetActionDataResponse result = new GetActionDataResponse();

            IActionRepository<GetActionDataResponse> repo = ActionRepositoryFactory<GetActionDataResponse>.GetActionRepository(request.ContractNumber, request.Context);
            repo.UserId = request.UserId;
            result = repo.FindByID(request.ActionID) as GetActionDataResponse;
            
            return (result != null ? result : new GetActionDataResponse());
        }
    }
}   
