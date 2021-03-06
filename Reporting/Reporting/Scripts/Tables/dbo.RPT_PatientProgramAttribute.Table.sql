SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientProgramAttribute](
	[PatientProgramAttributeId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NULL,
	[Completed] [varchar](50) NULL,
	[DidNotEnrollReason] [varchar](50) NULL,
	[Eligibility] [varchar](50) NULL,
	[Enrollment] [varchar](50) NULL,
	[GraduatedFlag] [varchar](50) NULL,
	[InelligibleReason] [varchar](50) NULL,
	[Lock] [varchar](50) NULL,
	[OptOut] [varchar](50) NULL,
	[OverrideReason] [varchar](50) NULL,
	[MongoPlanElementId] [varchar](50) NULL,
	[PlanElementId] [int] NULL,
	[Population] [varchar](50) NULL,
	[RemovedReason] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [varchar](50) NULL,
	[Delete] [varchar](50) NULL,
	[DateCompleted] [datetime] NULL,
	[CompletedBy] [int] NULL,
	[MongoCompletedBy] [varchar](50) NULL,
	[TTLDate] [datetime] NULL,
 CONSTRAINT [PK_PatientProgramAttribute] PRIMARY KEY CLUSTERED 
(
	[PatientProgramAttributeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
