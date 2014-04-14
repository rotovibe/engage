using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    public class GetProgramAttributeResponse : IDomainResponse
    {
        public ProgramAttribute ProgramAttribute { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
