using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Action.DTO
{
    [Api(Description = "Response posted back when an Action by it's ID is requested from the API.")]
    public class GetActionDataResponse : IDomainResponse
    {
        [ApiMember(DataType = "ActionData", Description = "A single Action object which has details like ID, Name, Description, CompletedBy, Objectives, Status", IsRequired = true, Name = "Action", ParameterType = "body")]
        public ActionData Action { get; set; }

        [ApiMember(DataType = "ResponseStatus", Description = "HTTP(S) Response Status identifying the result of the request.  This will come in the form of standard HTTP(S) responses (200, 401, 500, etc...)", IsRequired = true, Name = "Status", ParameterType = "body")]
        public ResponseStatus Status { get; set; }

        [ApiMember(DataType = "double", Description = "The specific version of the Response object being returned to support backward compatibility", IsRequired = true, Name = "Version", ParameterType = "body")]
        public double Version { get; set; }
    }

    public class ActionData
    {
        
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CompletedBy { get; set; }
        public List<string> Objectives { get; set; }
        public string Status { get; set; }

        //used to validate the CompletedBy property
        public bool ShouldSerializeCompletedBy()
        {
            return CompletedBy.Length > 5;
        }
    }
}
