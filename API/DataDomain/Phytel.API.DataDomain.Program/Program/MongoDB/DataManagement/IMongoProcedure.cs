using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Program.MongoDB.DataManagement
{
    public interface IMongoProcedure
    {
        void Execute();
        IDataDomainRequest Request { get; set; }
        List<Result> Results { get; set; }
    }
}
