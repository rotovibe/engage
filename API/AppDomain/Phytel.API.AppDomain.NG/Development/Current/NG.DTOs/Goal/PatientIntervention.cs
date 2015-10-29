﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PatientIntervention
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string PatientGoalId { get; set; }
        public string GoalName { get; set; }
        public string CreatedById { get; set; }
        public string TemplateId { get; set; }
        public string CategoryId { get; set; }
        public string AssignedToId { get; set; }
        public List<string> BarrierIds { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public PatientDetails PatientDetails { get; set; }
        public bool DeleteFlag { get; set; }
        public string Details { get; set; }
    }
}
