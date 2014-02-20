using Phytel.API.DataDomain.Module.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Module;

namespace Phytel.API.DataDomain.Module
{
    public static class ModuleDataManager
    {
        public static GetModuleResponse GetModuleByID(GetModuleRequest request)
        {
            GetModuleResponse result = new GetModuleResponse();

            IModuleRepository<GetModuleResponse> repo = ModuleRepositoryFactory<GetModuleResponse>.GetModuleRepository(request.ContractNumber, request.Context);
            var module = repo.FindByID(request.ModuleID);
            result.Module = module as DTO.Module;
            
            return (result != null ? result : new GetModuleResponse());
        }

        public static GetAllModulesResponse GetModuleList(GetAllModulesRequest request)
        {
            GetAllModulesResponse result = new GetAllModulesResponse();

            IModuleRepository<GetAllModulesResponse> repo = ModuleRepositoryFactory<GetAllModulesResponse>.GetModuleRepository(request.ContractNumber, request.Context);

            return result;
        }
    }
}   
