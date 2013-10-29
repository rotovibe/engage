using Phytel.API.AppDomain.Security.DTOs;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.Security;
using System.Web;

namespace Phytel.API.AppDomain.Security.Services
{
    public class SecurityService : Service
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
