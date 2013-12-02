using Phytel.API.DataDomain.Module;
using Phytel.API.DataDomain.Module.DTO;

namespace Phytel.API.DataDomain.Module.Service
{
    public class ModuleService : ServiceStack.ServiceInterface.Service
    {
        public ModuleResponse Post(ModuleRequest request)
        {
            ModuleResponse response = ModuleDataManager.GetModuleByID(request);
            response.Version = request.Version;
            return response;
        }

        public ModuleResponse Get(ModuleRequest request)
        {
            ModuleResponse response = ModuleDataManager.GetModuleByID(request);
            response.Version = request.Version;
            return response;
        }

        public ModuleListResponse Post(ModuleListRequest request)
        {
            ModuleListResponse response = ModuleDataManager.GetModuleList(request);
            response.Version = request.Version;
            return response;
        }
    }
}