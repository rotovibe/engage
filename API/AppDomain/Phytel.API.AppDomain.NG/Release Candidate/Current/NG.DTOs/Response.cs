﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class Response
    {
        public string Id { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public string StepId { get; set; }
        public string Value { get; set; }
        public bool Nominal { get; set; }
        public bool Required { get; set; }
        public string NextStepId { get; set; }
        public List<SpawnElement> SpawnElement { get; set; }
        public bool Selected { get; set; }
        public bool Delete { get; set; }
    }
}
