using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Program.MongoDB.DataManagement
{
    public abstract class MongoProcedure : IMongoProcedure
    {
        public void Execute()
        {
            Implementation();
        }

        public abstract void Implementation();

        public List<Result> Results { get; set; }
        public IDataDomainRequest Request { get; set; }
    }
}
