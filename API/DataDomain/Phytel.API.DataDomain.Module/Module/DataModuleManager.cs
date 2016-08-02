using Phytel.API.DataDomain.Module.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Module;
using Phytel.API.DataDomain.Module.MongoDB.DTO;

namespace Phytel.API.DataDomain.Module
{
    public static class ModuleDataManager
    {
        public static GetModuleResponse GetModuleByID(GetModuleRequest request)
        {
            GetModuleResponse result = new GetModuleResponse();

            IModuleRepository<GetModuleResponse> repo = ModuleRepositoryFactory<GetModuleResponse>.GetModuleRepository(request.ContractNumber, request.Context, request.UserId);

            var module = repo.FindByID(request.ModuleID);
            result.Module = module as DTO.Module;
            
            return (result != null ? result : new GetModuleResponse());
        }

        public static GetAllModulesResponse GetModuleList(GetAllModulesRequest request)
        {
            GetAllModulesResponse result = new GetAllModulesResponse();

            MongoModuleRepository<GetAllModulesResponse> repo = new MongoModuleRepository<GetAllModulesResponse>(request.ContractNumber);
            repo.UserId = request.UserId;

            result = repo.SelectAll(request.Version, Status.Active);

            return result;
        }
    }
}   
