﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Scheduling.DTO
{
    public class ToDoData : IAppData
    {
        public string Id { get; set; }
        public string CreatedById { get; set; }
        public string AssignedToId { get; set; }
        public string PatientId { get; set; }
        public int StatusId { get; set; }
        public string CategoryId { get; set; }
        public int PriorityId { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? StartTime { get; set; }
        public int? Duration { get; set; } 
        public DateTime? ClosedDate { get; set; }
        public List<string> ProgramIds { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string SourceId { get; set; }
        public bool DeleteFlag { get; set; }
        public string ExternalRecordId { get; set; }
    }
}