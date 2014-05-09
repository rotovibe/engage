using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    [Api(Description = "A Request object to get a Text Step by it's ID from the API.")]
    [Route("/{Context}/{Version}/{ContractNumber}/ProgramDesign/Step/{StepId}/Update", "PUT")]
    public class PutUpdateStepDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "StepId", Description = "Step ID of the Text Step being updated", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string StepId { get; set; }

        [ApiMember(Name = "Title", Description = "Title of the Text Step being updated", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Title { get; set; }

        [ApiMember(Name = "Description", Description = "Description of the Text Step being updated", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Description { get; set; }

        [ApiMember(Name = "Text", Description = "Text of the Text Step being updated", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Text { get; set; }

        [ApiMember(Name = "Question", Description = "Question of the YesNo Step being inserted", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Question { get; set; }

        [ApiMember(Name = "Notes", Description = "Notes of the YesNo Step being inserted", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Notes { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context updating the Step", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}
