using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class GetMongoProceduresListResponse : IDomainResponse
    {
        public double Version { get; set; }
        public List<MongoProcedure> Procedures { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class MongoProcedure
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
