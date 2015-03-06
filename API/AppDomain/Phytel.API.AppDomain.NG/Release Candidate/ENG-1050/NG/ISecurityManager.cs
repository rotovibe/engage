using Phytel.API.AppDomain.Security.DTO;
using System;
namespace Phytel.API.AppDomain.NG
{
    public interface ISecurityManager
    {
        ValidateTokenResponse IsUserValidated(double version, string token, string contractNumber);
    }
}
