/****** Object:  Table [dbo].[RPT_PatientProgramAction]    Script Date: 05/04/2015 12:02:10 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleAction_PatientProgramModule]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]'))
ALTER TABLE [dbo].[RPT_PatientProgramAction] DROP CONSTRAINT [FK_PatientProgramModuleAction_PatientProgramModule]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleAction_PatientProgramModule]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]'))
ALTER TABLE [dbo].[RPT_PatientProgramAction] DROP CONSTRAINT [FK_PatientProgramModuleAction_PatientProgramModule]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientProgramAction]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]') AND type in (N'U'))
BEGIN
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
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]') AND name = N'IX_RPT_PatientProgramAction')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramAction] ON [dbo].[RPT_PatientProgramAction] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]') AND name = N'IX_RPT_PatientProgramAction_1')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramAction_1] ON [dbo].[RPT_PatientProgramAction] 
(
	[SourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleAction_PatientProgramModule]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]'))
ALTER TABLE [dbo].[RPT_PatientProgramAction]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModuleAction_PatientProgramModule] FOREIGN KEY([PatientProgramModuleId])
REFERENCES [dbo].[RPT_PatientProgramModule] ([PatientProgramModuleId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleAction_PatientProgramModule]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramAction]'))
ALTER TABLE [dbo].[RPT_PatientProgramAction] CHECK CONSTRAINT [FK_PatientProgramModuleAction_PatientProgramModule]
GO
