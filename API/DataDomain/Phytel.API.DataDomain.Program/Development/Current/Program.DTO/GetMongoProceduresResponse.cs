using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class GetMongoProceduresResponse : IDomainResponse
    {
        public bool Success { get; set; }
        public double Version { get; set; }
        public List<Result> Results { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Result
    {
        public string Message { get; set; }
    }
}
