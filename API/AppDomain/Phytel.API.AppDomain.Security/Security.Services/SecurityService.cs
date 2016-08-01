using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Format;
using Phytel.API.Interface;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Configuration;
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
                //Generate the new security token because this is a first time authentication request

                //build the token from the user authentication request remote machine for additional security
                //this will then be passed in from calling domains via the header for validation
                string securityToken = BuildSecurityToken();

                // validate user against apiuser datastore
                response = SecurityManager.ValidateCredentials(request.Token, securityToken, request.APIKey, request.Context);
                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }

        public UserAuthenticateResponse Get(UserAuthenticateRequest request)
        {
            UserAuthenticateResponse response = new UserAuthenticateResponse();
            try
            {
                //build the token from the user authentication request remote machine for additional security
                //this will then be passed in from calling domains via the header for validation
                string securityToken = BuildSecurityToken();

                request.UserName = Request.Headers["UserName"];
                request.Password = Request.Headers["Password"];
                request.APIKey = Request.Headers["APIKey"];
                request.Context = Request.Headers["Context"];

                // validate user against apiuser datastore
                response = SecurityManager.ValidateCredentials(request.UserName, request.Password, securityToken, request.APIKey, request.Context, request.ContractNumber);
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
                //pull token from request coming in to validate token
                string securityToken = HttpContext.Current.Request.Headers.Get(_phytelSecurityToken);

                // validate user against apiuser datastore
                response = SecurityManager.ValidateToken(request, securityToken);
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    LogException(ex);
            }
            return response;
        }

        public LogoutResponse Post(LogoutRequest request)
        {
            LogoutResponse response = new LogoutResponse();
            try
            {
                //build the token from the user authentication request remote machine for additional security
                //this will then be passed in from calling domains via the header for validation
                string securityToken = BuildSecurityToken();
                request.Token = base.Request.Headers["Token"] as string;
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

        public void LogException(Exception ex)
        {
            string _aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
            Common.Helper.LogException(int.Parse(_aseProcessID), ex);
        }

        private string BuildSecurityToken()
        {
            string securityToken = "Unknown";
            try
            {
#if(DEBUG)
                securityToken = "Engineer";
#else
                securityToken = HttpContext.Current.Request.UserAgent;
#endif
            }
            catch
            {
                securityToken = "Unknown";
            }
            return securityToken;
        }
    }
}
