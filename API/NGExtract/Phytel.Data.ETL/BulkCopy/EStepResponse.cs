using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EStepResponse
    {
        public string _StepId { get; set; }
        public int StepId { get; set; }
        public string _nextStepId { get; set; }
        public int NextStepId { get; set; }
        public string _id { get; set; }
        public string _actionId { get; set; }
        public string Order { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string Nominal { get; set; }
        public string Required { get; set; }
        public string Selected { get; set; }
        public int Version { get; set; }
        public string _recordCreatedBy { get; set; }
        public int RecordCreatedBy { get; set; }
        public DateTime? RecordCreatedOn { get; set; }
        public string Delete { get; set; }
        public string _stepSourceId { get; set; }
        public int StepSourceId { get; set; }
        public int ActionId { get; set; }
        public string _updatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public DateTime? TTLDate { get; set; }
    }
}
