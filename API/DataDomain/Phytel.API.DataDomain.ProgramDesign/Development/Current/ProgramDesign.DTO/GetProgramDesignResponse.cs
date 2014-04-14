using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    public class GetProgramDesignResponse : IDomainResponse
    {
        public ProgramDesign ProgramDesign { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class ProgramDesign
    {
        public string ProgramDesignID { get; set; }
        public double Version { get; set; }
    }
}
