using Phytel.API.DataDomain.Action;
using Phytel.API.DataDomain.Action.DTO;

namespace Phytel.API.DataDomain.Action.Service
{
    public class ActionService : ServiceStack.ServiceInterface.Service
    {
        public ActionResponse Post(ActionRequest request)
        {
            ActionResponse response = ActionDataManager.GetActionByID(request);
            response.Version = request.Version;
            return response;
        }

        public ActionResponse Get(ActionRequest request)
        {
            ActionResponse response = ActionDataManager.GetActionByID(request);
            response.Version = request.Version;
            return response;
        }

        public ActionListResponse Post(ActionListRequest request)
        {
            ActionListResponse response = ActionDataManager.GetActionList(request);
            response.Version = request.Version;
            return response;
        }
    }
}