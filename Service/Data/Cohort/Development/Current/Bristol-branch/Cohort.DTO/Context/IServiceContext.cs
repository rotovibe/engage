using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.DataDomain.Cohort.DTO.Context
{
    public interface IServiceContext
    {
        string Contract { get; set; }
        double Version { get; set; }
        string UserId { get; set; }
        string Context { get; set; }
        object Tag { get; set; }
    }
}
