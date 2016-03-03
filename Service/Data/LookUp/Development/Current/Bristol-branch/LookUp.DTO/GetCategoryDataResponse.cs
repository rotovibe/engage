using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    [Api(Description = "Response posted back when a Category by it's ID is requested from the API.")]
    public class GetCategoryDataResponse : IDomainResponse
    {
        [ApiMember(DataType = "LookUpData", Description = "A single Category object which has an ID and a Name.", IsRequired = true, Name = "Category", ParameterType = "body")]
        public IdNamePair Category { get; set; }

        [ApiMember(DataType = "ResponseStatus", Description = "HTTP(S) Response Status identifying the result of the request.  This will come in the form of standard HTTP(S) responses (200, 401, 500, etc...)", IsRequired = true, Name = "Status", ParameterType = "body")]
        public ResponseStatus Status { get; set; }

        [ApiMember(DataType = "double", Description = "The specific version of the Response object being returned to support backward compatibility", IsRequired = true, Name = "Version", ParameterType = "body")]
        public double Version { get; set; }
    }
}
