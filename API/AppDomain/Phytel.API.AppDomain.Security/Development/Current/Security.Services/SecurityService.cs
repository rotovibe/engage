﻿using Phytel.API.AppDomain.Security.DTO;
using ServiceStack.ServiceInterface.ServiceModel;
using System;

namespace Phytel.API.AppDomain.Security.Service
{
    public class SecurityService : ServiceStack.ServiceInterface.Service
    {
        public AuthenticateResponse Post(AuthenticateRequest request)
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
                response.Status = new ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public UserAuthenticateResponse Post(UserAuthenticateRequest request)
        {
            UserAuthenticateResponse response = new UserAuthenticateResponse();
            try
            {
                // validate user against apiuser datastore
                response = SecurityManager.ValidateCredentials(request.UserName, request.Password, request.APIKey, request.Product);
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                response.Status = new ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public ValidateTokenResponse Post(ValidateTokenRequest request)
        {
            ValidateTokenResponse response = new ValidateTokenResponse();
            try
            {
                // validate user against apiuser datastore
                response = SecurityManager.ValidateToken(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                response.Status = new ResponseStatus("Exception", ex.Message);
            }
            return response;
        }
    }
}
