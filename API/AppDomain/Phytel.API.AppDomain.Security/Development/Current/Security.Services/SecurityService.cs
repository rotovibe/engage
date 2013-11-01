﻿using Phytel.API.AppDomain.Security.DTO;
using System;

namespace Phytel.API.AppDomain.Security.Service
{
    public class SecurityService : ServiceStack.ServiceInterface.Service
    {
        public object Any(AuthenticateRequest request)
        {
            AuthenticateResponse response = new AuthenticateResponse();
            try
            {
                // validate user against apiuser datastore
                response = SecurityManager.ValidateCredentials(request.Token, request.APIKey, request.Product);
            }
            catch(Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Excepton", ex.Message);
            }
            return response;
        }
    }
}
