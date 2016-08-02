using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.Security
{
    public interface ISecurityRepository<T> : IRepository<T>
    {
        AuthenticateResponse LoginUser(string token, string securityToken);
        AuthenticateResponse LoginUser(AuthenticateResponse existingReponse, string securityToken, string apiKey, string productName);
        UserAuthenticateResponse LoginUser(string userName, string password, string securityToken, string apiKey, string productName, string contractNumber);
        ValidateTokenResponse Validate(ValidateTokenRequest request, string securityToken);
        LogoutResponse Logout(string token, string securityToken, string context, string contractNumber);
    }
}