using Phytel.API.AppDomain.Security.DTO;
using System;

namespace Phytel.API.AppDomain.Security.Service
{
    public class SecurityService : ServiceStack.ServiceInterface.Service
    {
        public object Any(AuthenticateRequest request)
        {
            string aPIKey = base.Request.Headers["APIKey"];
            request.APIKey = aPIKey;

            // validate user against apiuser datastore
            AuthenticateResponse authResponse = SecurityManager.ValidateCredentials("mel", "bobadilla", "apikey1234");
            bool validated = Convert.ToBoolean(authResponse.Validated);
            
            // return token
            string currentToken = CacheModel.GetValidToken(request.UserName, request.Password, request.APIKey);

            return authResponse;
        }
    }
}
