/****** Object:  Table [dbo].[RPT_PatientProgramStep]    Script Date: 05/04/2015 12:02:19 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep] DROP CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep] DROP CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientProgramStep]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_PatientProgramStep](
	[StepId] [int] IDENTITY(1,1) NOT NULL,
	[MongoActionId] [varchar](50) NOT NULL,
	[ActionId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[AttributeEndDate] [datetime] NULL,
	[AttributeStartDate] [datetime] NULL,
	[SourceId] [varchar](50) NULL,
	[Order] [varchar](50) NULL,
	[Eligible] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[Completed] [varchar](50) NULL,
	[EligibilityEndDate] [datetime] NULL,
	[Header] [varchar](100) NULL,
	[SelectedResponseId] [varchar](50) NULL,
	[ControlType] [varchar](50) NULL,
	[SelectType] [varchar](50) NULL,
	[IncludeTime] [varchar](50) NULL,
	[Question] [varchar](max) NULL,
	[Title] [varchar](2000) NULL,
	[Description] [varchar](max) NULL,
	[Notes] [varchar](max) NULL,
	[Text] [varchar](max) NULL,
	[Status] [varchar](50) NULL,
	[Response] [varchar](50) NULL,
	[StepTypeId] [varchar](50) NULL,
	[Enabled] [varchar](50) NULL,
	[StateUpdatedOn] [datetime] NULL,
	[MongoCompletedBy] [varchar](50) NULL,
	[CompletedBy] [int] NULL,
	[DateCompleted] [datetime] NULL,
	[MongoNext] [varchar](50) NULL,
	[Next] [int] NULL,
	[Previous] [int] NULL,
	[EligibilityRequirements] [varchar](50) NULL,
	[EligibilityStartDate] [datetime] NULL,
	[MongoPrevious] [varchar](50) NULL,
	[Version] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
 CONSTRAINT [PK_PatientProgramModuleActionStep] PRIMARY KEY CLUSTERED 
(
	[StepId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = N'IX_RPT_PatientProgramStep')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramStep] ON [dbo].[RPT_PatientProgramStep] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = N'IX_RPT_PatientProgramStep_1')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramStep_1] ON [dbo].[RPT_PatientProgramStep] 
(
	[SourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = N'IX_RPT_PatientProgramStep_2')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramStep_2] ON [dbo].[RPT_PatientProgramStep] 
(
	[StepId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = N'IX_RPT_PatientProgramStep_ActionId')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramStep_ActionId] ON [dbo].[RPT_PatientProgramStep] 
(
	[ActionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction] FOREIGN KEY([ActionId])
REFERENCES [dbo].[RPT_PatientProgramAction] ([ActionId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep] CHECK CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]
GO
