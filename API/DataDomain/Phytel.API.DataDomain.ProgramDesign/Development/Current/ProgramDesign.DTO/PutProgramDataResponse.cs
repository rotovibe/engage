using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    public class PutProgramDataResponse : IDomainResponse
    {
        public bool Result { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
