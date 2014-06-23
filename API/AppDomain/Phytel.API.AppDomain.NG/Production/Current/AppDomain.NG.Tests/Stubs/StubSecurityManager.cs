using Phytel.API.AppDomain.Security.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubSecurityManager : ISecurityManager
    {
        public ValidateTokenResponse IsUserValidated(double version, string token, string contractNumber)
        {
            try
            {
                ValidateTokenResponse response = new ValidateTokenResponse();
                response.UserId = "000000000000000000000000";

                if (String.IsNullOrEmpty(response.UserId))
                    throw new Exception("token: " + token + " Userid is null.");

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:IsUserValidated()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
