﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class AttributeData
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ControlType { get; set; }
        public int Order { get; set; } 
    }
}
