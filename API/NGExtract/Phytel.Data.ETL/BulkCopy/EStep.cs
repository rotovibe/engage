using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Data.ETL.BulkCopy
{
	public class EStep
	{
		public  string      MongoActionId           { get; set; }	
		public  int         ActionId                { get; set; }
		public  string      MongoId                 { get; set; }
		public  DateTime?   AttributeEndDate        { get; set; }
		public  DateTime?   AttributeStartDate      { get; set; }
		public  string      SourceId                { get; set; }
		public  string      Order                   { get; set; }
		public  string      Eligible                { get; set; }
		public  string      State                   { get; set; }
		public  string      Completed               { get; set; }
		public  DateTime?   EligibilityEndDate      { get; set; }
		public  string      Header                  { get; set; }
		public  string      SelectedResponseId      { get; set; }	
		public  string      ControlType             { get; set; }
		public  string      SelectType              { get; set; }
		public  string      IncludeTime             { get; set; }
		public  string      Question                { get; set; }
		public  string      Title                   { get; set; }
		public  string      Description             { get; set; }
		public  string      Notes                   { get; set; }
		public  string      Text                    { get; set; }
		public  string      Status                  { get; set; }
		public  string      Response                { get; set; }
		public  string      StepTypeId              { get; set; }
		public  string      Enabled                 { get; set; }
		public  DateTime?   StateUpdatedOn          { get; set; }
		public  string      MongoCompletedBy        { get; set; }	
		public  int         CompletedBy             { get; set; }
		public  DateTime?   DateCompleted           { get; set; }
		public  string      MongoNext               { get; set; }
		public  int         Next                    { get; set; }
		public  int         Previous                { get; set; }
		public  string      EligibilityRequirements { get; set; }	
		public  DateTime?   EligibilityStartDate    { get; set; }	
		public  string      MongoPrevious           { get; set; }
		public  string      Version                 { get; set; }
		public  string      MongoUpdatedBy          { get; set; }
		public  int         UpdatedBy               { get; set; }
		public  DateTime?   LastUpdatedOn           { get; set; }
		public  string      MongoRecordCreatedBy    { get; set; }
		public  int         RecordCreatedBy         { get; set; }
		public  DateTime?   RecordCreatedOn         { get; set; }
		public  DateTime?   TTLDate                 { get; set; }
		public  string      Delete                  { get; set; }
	}
}
