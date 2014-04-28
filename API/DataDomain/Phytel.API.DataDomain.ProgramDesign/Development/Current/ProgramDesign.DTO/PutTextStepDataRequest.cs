using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    [Api(Description = "A Request object to get a Text Step by it's ID from the API.")]
    [Route("/{Context}/{Version}/{ContractNumber}/ProgramDesign/TextStep/Insert", "PUT")]
    public class PutTextStepDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Title", Description = "Title of the Text Step being inserted", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Title { get; set; }

        [ApiMember(Name = "Description", Description = "Description of the Text Step being inserted", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Description { get; set; }

        [ApiMember(Name = "Text", Description = "Text of the Text Step being inserted", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Text { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context inserting the Step", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}
