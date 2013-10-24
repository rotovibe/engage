using Phytel.API.AppDomain.Security.DTOs;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace User.Services
{
    public class SecurityService : Service
    {
        public object Any(AuthenticateRequest request)
        {
            string aPIKey = base.Request.Headers["APIKey"];
            request.APIKey = aPIKey;

            // get token and save to APIusers
            // register token to global asp.net cache
            // return token

            return new AuthenticateResponse { Token = "Test123" };
        }
    }
}
