using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EProgramAttribute
	{
        public  string      MongoId                 { get; set;}
        public  string      Completed               { get; set;}
        public  string      DidNotEnrollReason      { get; set;}
        public  string      Eligibility             { get; set;}
        public  string      Enrollment              { get; set;}
        public  string      GraduatedFlag           { get; set;}
        public  string      InelligibleReason       { get; set;}	
        public  string      Lock                    { get; set;}
        public  string      OptOut                  { get; set;}
        public  string      OverrideReason          { get; set;}
        public  string      MongoPlanElementId      { get; set;}
        public  int         PlanElementId           { get; set;}
        public  string      Population              { get; set;}
        public  string      RemovedReason           { get; set;}
        public  string      Status                  { get; set;}
        public  string      MongoUpdatedBy          { get; set;}
        public  int         UpdatedBy               { get; set;}
        public  DateTime?   LastUpdatedOn           { get; set;}
        public  string      MongoRecordCreatedBy    { get; set;}
        public  int         RecordCreatedBy         { get; set;}
        public  DateTime?   RecordCreatedOn         { get; set;}
        public  string      Version                 { get; set;}
        public  string      Delete                  { get; set;}
        public  DateTime?   DateCompleted           { get; set;}
        public  int         CompletedBy             { get; set;}
        public  string      MongoCompletedBy        { get; set;}
        public  DateTime?   TTLDate                 { get; set;}
	}
}
