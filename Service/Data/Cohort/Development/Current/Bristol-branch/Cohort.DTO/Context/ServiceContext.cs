using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Cohort.DTO.Context
{
    public class ServiceContext : IServiceContext
    {
        public string Contract { get; set; }
        public double Version { get; set; }
        public string UserId { get; set; }
        public string Context { get; set; }
        public object Tag { get; set; }
    }
}
