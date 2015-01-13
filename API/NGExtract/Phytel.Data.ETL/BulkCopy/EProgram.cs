using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EProgram
    {
        public int          PatientId               { get; set; }
        public string       MongoPatientId          { get; set; }
        public string       MongoId                 { get; set; }
        public string       Name                    { get; set; }
        public string       ShortName               { get; set; }
        public string       Description             { get; set; }
        public DateTime?    AttributeStartDate      { get; set; }
        public DateTime?    AttributeEndDate        { get; set; }
        public string       SourceId                { get; set; }
        public string       Order                   { get; set; }
        public string       Eligible                { get; set; }
        public string       State                   { get; set; }
        public DateTime?    AssignedOn              { get; set; }
        public string       MongoAssignedBy         { get; set; }
        public int          AssignedBy              { get; set; }
        public string       MongoAssignedToId       { get; set; }
        public int          AssignedToId            { get; set; }
        public string       Completed               { get; set; }
        public DateTime?    EligibilityStartDate    { get; set; }
        public string       EligibilityReason       { get; set; }
        public DateTime?    StartDate               { get; set; }
        public DateTime?    EndDate                 { get; set; }
        public string       Status                  { get; set; }
        public string       ContractProgramId       { get; set; }
        public string       Version                 { get; set; }
        public string       MongoUpdatedBy          { get; set; }
        public int          UpdatedBy               { get; set; }
        public DateTime?    LastUpdatedOn           { get; set; }
        public string       MongoRecordCreatedBy    { get; set; }
        public int          RecordCreatedBy         { get; set; }
        public DateTime?    RecordCreatedOn         { get; set; }
        public string       Delete                  { get; set; }
        public string       Enabled                 { get; set; }
        public DateTime?    StateUpdatedOn          { get; set; }
        public int          CompletedBy             { get; set; }
        public string       MongoCompletedBy        { get; set; }
        public DateTime?    DateCompleted           { get; set; }
        public string       EligibilityRequirements { get; set; }
        public DateTime?    EligibilityEndDate      { get; set; }
        public DateTime?    TTLDate                 { get; set; }
    }
}
