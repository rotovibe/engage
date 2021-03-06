ALTER TABLE [dbo].[RPT_PatientProgramAction] DROP CONSTRAINT [FK_PatientProgramModuleAction_PatientProgramModule]
GO
ALTER TABLE [dbo].[RPT_PatientProgramAction] DROP CONSTRAINT [FK_PatientProgramModuleAction_PatientProgramModule]
GO
DROP TABLE [dbo].[RPT_PatientProgramAction]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RPT_PatientProgramAction](
	[ActionId] [int] IDENTITY(1,1) NOT NULL,
	[PatientProgramModuleId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoModuleId] [varchar](50) NOT NULL,
	[SourceId] [varchar](50) NULL,
	[Order] [varchar](50) NULL,
	[Eligible] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[StateUpdatedOn] [datetime] NULL,
	[AssignedOn] [datetime] NULL,
	[MongoAssignedBy] [varchar](50) NULL,
	[AssignedBy] [int] NULL,
	[MongoAssignedTo] [varchar](50) NULL,
	[AssignedTo] [int] NULL,
	[MongoCompletedBy] [varchar](50) NULL,
	[CompletedBy] [int] NULL,
	[Completed] [varchar](50) NULL,
	[EligibilityEndDate] [datetime] NULL,
	[Name] [varchar](100) NULL,
	[Description] [varchar](max) NULL,
	[Status] [varchar](50) NULL,
	[AttributeEndDate] [datetime] NULL,
	[AttributeStartDate] [datetime] NULL,
	[Enabled] [varchar](50) NULL,
	[DateCompleted] [datetime] NULL,
	[MongoNext] [varchar](50) NULL,
	[Next] [int] NULL,
	[MongoPrevious] [varchar](50) NULL,
	[Previous] [int] NULL,
	[EligibilityRequirements] [varchar](50) NULL,
	[EligibilityStartDate] [datetime] NULL,
	[version] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[Archived] [varchar](50) NULL,
	[ArchivedDate] [datetime] NULL,
	[MongoArchiveOriginId] [varchar](50) NULL,
 CONSTRAINT [PK_PatientProgramModuleAction] PRIMARY KEY CLUSTERED 
(
	[ActionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramAction] ON [dbo].[RPT_PatientProgramAction] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramAction_1] ON [dbo].[RPT_PatientProgramAction] 
(
	[SourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RPT_PatientProgramAction]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModuleAction_PatientProgramModule] FOREIGN KEY([PatientProgramModuleId])
REFERENCES [dbo].[RPT_PatientProgramModule] ([PatientProgramModuleId])
GO
ALTER TABLE [dbo].[RPT_PatientProgramAction] CHECK CONSTRAINT [FK_PatientProgramModuleAction_PatientProgramModule]
GO
