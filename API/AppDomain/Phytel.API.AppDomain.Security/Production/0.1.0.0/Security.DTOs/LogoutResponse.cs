using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.Security.DTO
{
    [Api(Description = "Response posted back when the user attempts to log out of the application.")]
    public class LogoutResponse: IDomainResponse
    {
        [ApiMember(DataType = "bool", Description = "A boolean value to indicate if the user is logged out or not.", IsRequired = true, Name = "SuccessfulLogout", ParameterType = "body")]
        public bool SuccessfulLogout { get; set; }

        [ApiMember(DataType = "ResponseStatus", Description = "HTTP(S) Response Status identifying the result of the request.  This will come in the form of standard HTTP(S) responses (200, 401, 500, etc...)", IsRequired = true, Name = "Status", ParameterType = "body")]
        public ResponseStatus Status { get; set; }

        [ApiMember(DataType = "double", Description = "The specific version of the Response object being returned to support backward compatibility", IsRequired = true, Name = "Version", ParameterType = "body")]
        public double Version { get; set; }
    }
}
