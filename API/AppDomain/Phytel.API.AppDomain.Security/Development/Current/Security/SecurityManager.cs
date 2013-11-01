using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.AppDomain.Security.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;

namespace Phytel.API.AppDomain.Security
{
    public static class SecurityManager
    {
        public static AuthenticateResponse ValidateCredentials(string token, string apiKey, string productName)
        {
            //First, call the C3UserRepository class to get the user information based on the token
            //Next, call the APIRepository class to identify if the apiKey/productName combination is valid
            //create AuthenticationResponse object and populate
            //write APISession object, with TTL information 

            ISecurityRepository<AuthenticateResponse> userRepo = SecurityRepositoryFactory<AuthenticateResponse>.GetUserRepository(productName);
            ISecurityRepository<AuthenticateResponse> securityRepo = SecurityRepositoryFactory<AuthenticateResponse>.GetSecurityRepository(productName);

            AuthenticateResponse userResponse = userRepo.LoginUser(token);
            if(userResponse.UserID != Guid.Empty)
                userResponse = securityRepo.LoginUser(userResponse, apiKey, productName);

            throw new Exception("I just failed, help!!!");

            return userResponse;
        }

        public static ValidateTokenResponse ValidateToken(string token, string productName)
        {
            ISecurityRepository<AuthenticateResponse> securityRepo = SecurityRepositoryFactory<AuthenticateResponse>.GetSecurityRepository(productName);

            return securityRepo.Validate(token);
        }
    }
}
