using Phytel.API.AppDomain.Security.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG
{
    public abstract class ManagerBase
    {
        public bool IsUserValidated(string token)
        {
            bool result = false;
            IRestClient client = new JsonServiceClient();

            ValidateTokenResponse response = client.Post<ValidateTokenResponse>("http://localhost:999/api/security/Token",
                new ValidateTokenRequest { Token = token } as object);

            if (response.IsValid) result = true;

            return result;
        }
    }
}
