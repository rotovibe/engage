using Phytel.API.AppDomain.Security.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;

namespace Phytel.API.AppDomain.Security
{
    public static class SecurityManager
    {
        // add validation checks
        public static AuthenticateResponse ValidateCredentials(string token, string securityToken, string apiKey, string productName)
        {
            try
            {
                //First, call the C3UserRepository class to get the user information based on the token
                //Next, call the APIRepository class to identify if the apiKey/productName combination is valid
                //create AuthenticationResponse object and populate
                //write APISession object, with TTL information 

                ISecurityRepository<AuthenticateResponse> userRepo = SecurityRepositoryFactory<AuthenticateResponse>.GetUserRepository(productName);
                ISecurityRepository<AuthenticateResponse> securityRepo = SecurityRepositoryFactory<AuthenticateResponse>.GetSecurityRepository(productName);

                AuthenticateResponse userResponse = userRepo.LoginUser(token, securityToken);
                if (string.IsNullOrEmpty(userResponse.SQLUserID) == false)
                    userResponse = securityRepo.LoginUser(userResponse, securityToken, apiKey, productName);
                else
                    throw new UnauthorizedAccessException("Login Failed! Unknown token and security token.");

                return userResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static UserAuthenticateResponse ValidateCredentials(string userName, string password, string securityToken, string apiKey, string productName, string contractNumber)
        {
            try
            {
                ISecurityRepository<UserAuthenticateResponse> securityRepo = SecurityRepositoryFactory<UserAuthenticateResponse>.GetSecurityRepository(productName);

                UserAuthenticateResponse response = securityRepo.LoginUser(userName, password, securityToken, apiKey, productName, contractNumber);

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static ValidateTokenResponse ValidateToken(ValidateTokenRequest request, string securityToken)
        {
            try
            {
                ISecurityRepository<AuthenticateResponse> securityRepo = SecurityRepositoryFactory<AuthenticateResponse>.GetSecurityRepository(request.Context);

                return securityRepo.Validate(request, securityToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static LogoutResponse Logout(LogoutRequest request, string securityToken)
        {
            ISecurityRepository<AuthenticateResponse> securityRepo = SecurityRepositoryFactory<AuthenticateResponse>.GetSecurityRepository(request.Context);

            return securityRepo.Logout(request.Token, securityToken, request.Context, request.ContractNumber);
        }
    }
}
