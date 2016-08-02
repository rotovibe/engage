using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.Security.DTO
{
    [Api(Description = "Response posted back when the API authenticates the user requesting access")]
    public class AuthenticateResponse: IDomainResponse
    {
        [ApiMember(DataType = "string", Description = "Unique SQL ID of the user logged into the product with access to the API", IsRequired = true, Name = "SQLUserID", ParameterType = "body")]
        public string SQLUserID { get; set; }

        [ApiMember(DataType = "string", Description = "Users Contact ID of the user logged into the product", IsRequired = true, Name = "UserId", ParameterType = "body")]
        public string UserId { get; set; }

        [ApiMember(DataType = "string", Description = "User Name the user logged into the product with access to the API", IsRequired = true, Name = "UserName", ParameterType = "body")]
        public string UserName { get; set; }

        [ApiMember(DataType = "string", Description = "First Name of the user logged into the product with access to the API", IsRequired = true, Name = "FirstName", ParameterType = "body")]
        public string FirstName { get; set; }

        [ApiMember(DataType = "string", Description = "Last Name of the user logged into the product with access to the API", IsRequired = true, Name = "LastName", ParameterType = "body")]
        public string LastName { get; set; }

        [ApiMember(DataType = "Int", Description = "Timeout period (in minutes) to when the users API access will be removed after no activity", IsRequired = true, Name = "SessionTimeout", ParameterType = "body")]
        public int SessionTimeout { get; set; }

        [ApiMember(DataType = "string", Description = "Unique Token given to the user that is to be used each time a request is sent to the API for validation", IsRequired = true, Name = "APIToken", ParameterType = "body")]
        public string APIToken { get; set; }

        [ApiMember(DataType = "List<ContractInfo>", Description = "List of Contracts the user has access to from within the API Repository", IsRequired = true, Name = "Contracts", ParameterType = "body")]
        public List<ContractInfo> Contracts { get; set; }

        [ApiMember(DataType = "ResponseStatus", Description = "HTTP(S) Response Status identifying the result of the request.  This will come in the form of standard HTTP(S) responses (200, 401, 500, etc...)", IsRequired = true, Name = "Status", ParameterType = "body")]
        public ResponseStatus Status { get; set; }

        [ApiMember(DataType = "double", Description = "The specific version of the Response object being returned to support backward compatibility", IsRequired = true, Name = "Version", ParameterType = "body")]
        public double Version { get; set; }
    }
}
