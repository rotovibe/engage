﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.Interface
{
    public interface IDataDomainRequest
    {
        string ContractNumber { get; set; }
        string DataDomainName { get; set; }
    }
}
