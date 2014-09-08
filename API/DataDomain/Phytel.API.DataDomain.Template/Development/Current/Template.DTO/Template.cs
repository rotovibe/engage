﻿using System;

namespace Phytel.API.DataDomain.Template.DTO
{
    public class Template
    {
        public string Id { get; set; }
        public double Version { get; set; }
        public string UpdatedBy { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime? TtlDate { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string RecordCreatedBy { get; set; }
        public DateTime RecordCreatedOn { get; set; }
    }
}
