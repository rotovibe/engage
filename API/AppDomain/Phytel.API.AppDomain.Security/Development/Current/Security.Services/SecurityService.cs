using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Format;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Net;
using System.Web;

namespace Phytel.API.AppDomain.Security.Service
{
    public class SecurityService : ServiceStack.ServiceInterface.Service
    {
        private const string _phytelSecurityToken = "x-Phytel-Security";

        public AuthenticateResponse Post(AuthenticateRequest request)
        {
            AuthenticateResponse response = new AuthenticateResponse();
            try
            {
                string securityToken = HttpContext.Current.Request.Headers.Get(_phytelSecurityToken);

                // validate user against apiuser datastore
                response = SecurityManager.ValidateCredentials(request.Token, securityToken, request.APIKey, request.Context);
                return response;
            }
            catch(Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            return response;
        }
        }

        public UserAuthenticateResponse Post(UserAuthenticateRequest request)
        {
            UserAuthenticateResponse response = new UserAuthenticateResponse();
            try
            {
                string securityToken = HttpContext.Current.Request.Headers.Get(_phytelSecurityToken);

                // validate user against apiuser datastore
                response = SecurityManager.ValidateCredentials(request.UserName, request.Password, securityToken, request.APIKey, request.Context);
                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            return response;
        }
        }

        public ValidateTokenResponse Post(ValidateTokenRequest request)
        {
            ValidateTokenResponse response = new ValidateTokenResponse();
            try
            {
                string securityToken = HttpContext.Current.Request.Headers.Get(_phytelSecurityToken);

                // validate user against apiuser datastore
                response = SecurityManager.ValidateToken(request, securityToken);
                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }

        public LogoutResponse Post(LogoutRequest request)
        {
            LogoutResponse response = new LogoutResponse();
            try
            {
                string securityToken = HttpContext.Current.Request.Headers.Get(_phytelSecurityToken);

                response = SecurityManager.Logout(request, securityToken);
                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }
    }
}
