using Phytel.API.DataDomain.Security.DTO;
using Phytel.Services;

namespace Phytel.API.DataDomain.Security
{
    public static class SecurityDataManager
    {
        public static bool LoginUser(ValidateRequest request)
        {
            bool result = false;

            ISecurityRepository<APIUser> repo = SecurityRepositoryFactory<APIUser>.GetAPISessionRepository(request.Product);
            APIUser apiUser = repo.GetUser(request.UserName, request.APIKey, request.Product);

            if (null != apiUser)
            {
                DataProtector dataProtector = new DataProtector(DataProtector.Store.USE_SIMPLE_STORE);
                string apiUserPassword = dataProtector.Decrypt(apiUser.Password);
                if (apiUserPassword.ToLower().Equals(request.Password.ToLower()))
                    result = true;
            }
            return result;
        }
    }
}   
