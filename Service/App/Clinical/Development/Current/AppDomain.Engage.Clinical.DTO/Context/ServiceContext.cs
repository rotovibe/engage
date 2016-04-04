using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDomain.Engage.Clinical.DTO.Context
{
    public class ServiceContext : IServiceContext
    {
        public string Contract { get; set; }
        public double Version { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public object Tag { get; set; }
    }
}
