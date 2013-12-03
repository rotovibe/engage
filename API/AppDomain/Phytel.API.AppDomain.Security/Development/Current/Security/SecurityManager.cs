using Phytel.API.AppDomain.Security.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;

namespace Phytel.API.AppDomain.Security
{
    public static class SecurityManager
    {
        // add validation checks
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

            return userResponse;
        }

        public static UserAuthenticateResponse ValidateCredentials(string userName, string password, string apiKey, string productName)
        {
            ISecurityRepository<UserAuthenticateResponse> securityRepo = SecurityRepositoryFactory<UserAuthenticateResponse>.GetSecurityRepository(productName);

            UserAuthenticateResponse response = securityRepo.LoginUser(userName, password, apiKey, productName);

            return response;
        }

        public static ValidateTokenResponse ValidateToken(ValidateTokenRequest request)
        {
            ISecurityRepository<AuthenticateResponse> securityRepo = SecurityRepositoryFactory<AuthenticateResponse>.GetSecurityRepository(request.Context);

            return securityRepo.Validate(request.Token, request.Context);
        }
    }
}
