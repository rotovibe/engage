using Phytel.API.AppDomain.Security.DTO;
using ServiceStack;
using System;
using System.Net;

namespace Phytel.API.AppDomain.Security.Service
{
    public class SecurityService : ServiceStack.Service
    {
        public DTO.AuthenticateResponse Post(AuthenticateRequest request)
        {
            DTO.AuthenticateResponse response = new DTO.AuthenticateResponse();
            try
            {
                // validate user against apiuser datastore
                response = SecurityManager.ValidateCredentials(request.Token, request.APIKey, request.Product);
            }
            catch(Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
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
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
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
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ResponseStatus("Exception", ex.Message);
            }
            return response;
        }
    }
}
