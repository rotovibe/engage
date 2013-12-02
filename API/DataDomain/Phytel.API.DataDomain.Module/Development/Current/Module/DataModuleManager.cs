using Phytel.API.DataDomain.Module.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Module;

namespace Phytel.API.DataDomain.Module
{
    public static class ModuleDataManager
    {
        public static ModuleResponse GetModuleByID(ModuleRequest request)
        {
            ModuleResponse result = new ModuleResponse();

            IModuleRepository<ModuleResponse> repo = ModuleRepositoryFactory<ModuleResponse>.GetModuleRepository(request.ContractNumber, request.Context);
            result = repo.FindByID(request.ModuleID) as ModuleResponse;
            
            return (result != null ? result : new ModuleResponse());
        }

        public static ModuleListResponse GetModuleList(ModuleListRequest request)
        {
            ModuleListResponse result = new ModuleListResponse();

            IModuleRepository<ModuleListResponse> repo = ModuleRepositoryFactory<ModuleListResponse>.GetModuleRepository(request.ContractNumber, request.Context);

            return result;
        }
    }
}   
