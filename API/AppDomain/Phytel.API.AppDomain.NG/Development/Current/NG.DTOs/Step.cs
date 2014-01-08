﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class Step : PlanElement
    {
        public int Type { get; set; }
        public string Question { get; set; }
        public string T { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Text { get; set; }
        public string Ex { get; set; }
        public int Status { get; set; }

        public string ActionId { get; set; }
        public int StepTypeId { get; set; }
        public string Instruction { get; set; }
        public string Header { get; set; }
        public int SelectedResponseId { get; set; }
        public int ControlType { get; set; }
        public List<Response> Responses { get; set; }
    }
}
