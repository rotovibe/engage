using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EModule
    {
        public  int         PatientProgramId            { get; set; }
        public  string      MongoId                     { get; set; }	
        public  string      MongoProgramId              { get; set; }
        public  DateTime?   AttributeStartDate          { get; set; }
        public  DateTime?   AttributeEndDate            { get; set; }
        public  string      SourceId                    { get; set; }
        public  string      Order                       { get; set; }	
        public  string      Eligible                    { get; set; }
        public  string      State                       { get; set; }
        public  DateTime?   AssignedOn                  { get; set; }	
        public  string      MongoAssignedBy             { get; set; }
        public  int         AssignedBy                  { get; set; }
        public  string      MongoAssignedTo             { get; set; }
        public  int         AssignedTo                  { get; set; }
        public  string      Completed                   { get; set; }
        public  DateTime?   EligibilityEndDate          { get; set; }
        public  string      Name                        { get; set; }
        public  string      Description                 { get; set; }
        public  string      Status                      { get; set; }
        public  DateTime?   StateUpdatedOn              { get; set; }
        public  string      Enabled                     { get; set; }
        public  string      MongoCompletedBy            { get; set; }
        public  int         CompletedBy                 { get; set; }
        public  DateTime?   DateCompleted               { get; set; }
        public  string      MongoNext                   { get; set; }
        public  int         Next                        { get; set; }
        public  string      MongoPrevious               { get; set; }
        public  int         Previous                    { get; set; }
        public  string      EligibilityRequirements     { get; set; }	
        public  DateTime?   EligibilityStartDate        { get; set; }
        public  DateTime?   LastupdatedOn               { get; set; }
        public  string      RecordCreatedBy             { get; set; }
        public  DateTime?   RecordCreatedOn             { get; set; }
        public  DateTime?   TTLDate                     { get; set; }
        public  string      Delete                      { get; set; }
    }
}
