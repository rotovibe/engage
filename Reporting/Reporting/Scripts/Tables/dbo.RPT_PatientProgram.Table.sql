SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientProgram](
	[PatientProgramId] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Name] [varchar](100) NULL,
	[ShortName] [varchar](100) NULL,
	[Description] [varchar](max) NULL,
	[AttributeStartDate] [datetime] NULL,
	[AttributeEndDate] [datetime] NULL,
	[SourceId] [varchar](50) NULL,
	[Order] [varchar](50) NULL,
	[Eligible] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[AssignedOn] [datetime] NULL,
	[MongoAssignedBy] [varchar](50) NULL,
	[AssignedBy] [int] NULL,
	[MongoAssignedToId] [varchar](50) NULL,
	[AssignedToId] [int] NULL,
	[Completed] [varchar](50) NULL,
	[EligibilityStartDate] [datetime] NULL,
	[EligibilityReason] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Status] [varchar](50) NULL,
	[ContractProgramId] [varchar](50) NULL,
	[Version] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[Enabled] [varchar](50) NULL,
	[StateUpdatedOn] [datetime] NULL,
	[CompletedBy] [int] NULL,
	[MongoCompletedBy] [varchar](50) NULL,
	[DateCompleted] [datetime] NULL,
	[EligibilityRequirements] [varchar](max) NULL,
	[EligibilityEndDate] [datetime] NULL,
	[TTLDate] [datetime] NULL,
 CONSTRAINT [PK_PatientProgram] PRIMARY KEY CLUSTERED 
(
	[PatientProgramId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientProgram]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgram_Patient] FOREIGN KEY([PatientId])
REFERENCES [RPT_Patient] ([PatientId])
GO
ALTER TABLE [RPT_PatientProgram] CHECK CONSTRAINT [FK_PatientProgram_Patient]
GO
