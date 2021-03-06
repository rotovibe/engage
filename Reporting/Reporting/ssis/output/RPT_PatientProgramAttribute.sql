/****** Object:  Table [dbo].[RPT_PatientProgramAttribute]    Script Date: 05/04/2015 12:02:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAttribute]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientProgramAttribute]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAttribute]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_PatientProgramAttribute](
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
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAttribute]') AND name = N'IX_RPT_PatientProgramAttribute_MongoPlanElementId')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramAttribute_MongoPlanElementId] ON [dbo].[RPT_PatientProgramAttribute] 
(
	[MongoPlanElementId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAttribute]') AND name = N'IX_RPT_PatientProgramAttribute_PlanElementId')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramAttribute_PlanElementId] ON [dbo].[RPT_PatientProgramAttribute] 
(
	[PlanElementId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
