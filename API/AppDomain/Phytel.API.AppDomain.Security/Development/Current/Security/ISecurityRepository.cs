using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.Security
{
    public interface ISecurityRepository<T> : IRepository<T>
    {
        AuthenticateResponse LoginUser(string token);
        AuthenticateResponse LoginUser(AuthenticateResponse existingReponse, string apiKey, string productName);
        ValidateTokenResponse Validate(string token);
    }
}