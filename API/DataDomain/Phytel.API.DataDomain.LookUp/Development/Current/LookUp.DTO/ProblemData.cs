﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class ProblemData: LookUpData
    {
        public bool Active { get; set; }
        public string Type { get; set; }
    }
}
