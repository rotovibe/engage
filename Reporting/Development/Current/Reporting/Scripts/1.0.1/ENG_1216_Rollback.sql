/****** Object:  Table [dbo].[RPT_ProcessAudit]    Script Date: 05/12/2015 09:56:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RPT_ProcessAudit]') AND type in (N'U'))
	DROP TABLE [RPT_ProcessAudit]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Initialize_Flat_Tables]    Script Date: 05/11/2015 10:11:29 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spPhy_RPT_Initialize_Flat_Tables]') AND type in (N'P'))
	DROP PROCEDURE [spPhy_RPT_Initialize_Flat_Tables]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Initialize_Flat_Tables]
AS
BEGIN
	EXECUTE [spPhy_RPT_ProgramResponse_Flat];
	EXECUTE [spPhy_RPT_Flat_BSHSI_HW2]
	EXECUTE [spPhy_RPT_Flat_CareBridge];
	EXECUTE [spPhy_RPT_Flat_Engage];
END

/**** ADDING RECOMMENDED NON-CLUSTERED INDEXES *****/
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = 'IX_RPT_PatientProgramResponse_StepidSelected')
	DROP INDEX IX_RPT_PatientProgramResponse_StepidSelected ON RPT_PatientProgramResponse;
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramModule]') AND name = 'IX_RPT_PatientProgramModule_MongoId')
	DROP INDEX IX_RPT_PatientProgramModule_MongoId ON RPT_PatientProgramModule;
GO


IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND name = 'IX_RPT_ProgramResponse_Flat_PatientId_PatientProgramId_ActionSourceId')
	DROP INDEX [IX_RPT_ProgramResponse_Flat_PatientId_PatientProgramId_ActionSourceId] ON [RPT_ProgramResponse_Flat];
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = 'IX_RPT_PatientProgramStep_SourceId')
	DROP INDEX [IX_RPT_PatientProgramStep_SourceId] ON [RPT_PatientProgramStep];
GO


/******************** patientprogramresponse *******************************/

/****** Object:  ForeignKey [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]    Script Date: 05/17/2015 12:23:37 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]    Script Date: 05/17/2015 12:23:37 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramResponse]    Script Date: 05/17/2015 12:23:37 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientProgramResponse]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramResponse]    Script Date: 05/17/2015 12:23:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_PatientProgramResponse](
	[ResponseId] [int] IDENTITY(1,1) NOT NULL,
	[MongoStepId] [varchar](50) NULL,
	[StepId] [int] NULL,
	[MongoNextStepId] [varchar](50) NULL,
	[NextStepId] [int] NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoActionId] [varchar](50) NULL,
	[Order] [varchar](50) NULL,
	[Text] [varchar](5000) NULL,
	[Value] [varchar](5000) NULL,
	[Nominal] [varchar](50) NULL,
	[Required] [varchar](50) NULL,
	[Selected] [varchar](50) NULL,
	[Version] [float] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime2](7) NULL,
	[Delete] [varchar](50) NULL,
	[MongoStepSourceId] [varchar](50) NULL,
	[StepSourceId] [int] NULL,
	[ActionId] [int] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime2](7) NULL,
	[TTLDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PatientProgramModuleActionStepResponse] PRIMARY KEY CLUSTERED 
(
	[ResponseId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse] ON [dbo].[RPT_PatientProgramResponse] 
(
	[MongoStepId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse_1')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_1] ON [dbo].[RPT_PatientProgramResponse] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse_2')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_2] ON [dbo].[RPT_PatientProgramResponse] 
(
	[MongoStepSourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse_Composite')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_Composite] ON [dbo].[RPT_PatientProgramResponse] 
(
	[StepId] ASC,
	[Selected] ASC
)
INCLUDE ( [Text],
[Delete]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse_Delete')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_Delete] ON [dbo].[RPT_PatientProgramResponse] 
(
	[Delete] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse_State')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_State] ON [dbo].[RPT_PatientProgramResponse] 
(
	[Selected] ASC,
	[Delete] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = N'IX_RPT_PatientProgramResponse_StepidSelected')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_StepidSelected] ON [dbo].[RPT_PatientProgramResponse] 
(
	[StepId] ASC,
	[Selected] ASC
)
INCLUDE ( [Text],
[Delete]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]    Script Date: 05/17/2015 12:23:37 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep] FOREIGN KEY([NextStepId])
REFERENCES [dbo].[RPT_PatientProgramStep] ([StepId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] CHECK CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]    Script Date: 05/17/2015 12:23:37 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep] FOREIGN KEY([StepId])
REFERENCES [dbo].[RPT_PatientProgramStep] ([StepId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] CHECK CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]
GO


/****** Object:  Table [dbo].[RPT_PatientProgramStep]    Script Date: 05/17/2015 17:52:06 ******/
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]    Script Date: 05/17/2015 17:52:06 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep] DROP CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramStep]    Script Date: 05/17/2015 17:52:06 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep] DROP CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT [FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]'))
ALTER TABLE [dbo].[RPT_PatientProgramResponse] DROP CONSTRAINT FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientProgramStep]
GO
/****** Object:  Table [dbo].[RPT_PatientProgramStep]    Script Date: 05/17/2015 17:52:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
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
SET ANSI_PADDING OFF
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
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = N'IX_RPT_PatientProgramStep_Sourceid')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramStep_Sourceid] ON [dbo].[RPT_PatientProgramStep] 
(
	[SourceId] ASC
)
INCLUDE ( [MongoActionId],
[MongoId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]    Script Date: 05/17/2015 17:52:06 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep]  WITH NOCHECK ADD  CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction] FOREIGN KEY([ActionId])
REFERENCES [dbo].[RPT_PatientProgramAction] ([ActionId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientProgramModuleActionStep_PatientProgramModuleAction]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]'))
ALTER TABLE [dbo].[RPT_PatientProgramStep] CHECK CONSTRAINT [FK_PatientProgramModuleActionStep_PatientProgramModuleAction]
GO


/******************* RESTORE RPT_PatientNoteProgram ************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientNoteProgram_PatientNote]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientNoteProgram]'))
ALTER TABLE [dbo].[RPT_PatientNoteProgram] DROP CONSTRAINT [FK_PatientNoteProgram_PatientNote]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientNoteProgram_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientNoteProgram]'))
ALTER TABLE [dbo].[RPT_PatientNoteProgram] DROP CONSTRAINT [FK_PatientNoteProgram_UserMongoRecordCreatedBy]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientNoteProgram_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientNoteProgram]'))
ALTER TABLE [dbo].[RPT_PatientNoteProgram] DROP CONSTRAINT [FK_PatientNoteProgram_UserMongoUpdatedBy]
GO

/****** Object:  Table [dbo].[RPT_PatientNoteProgram]    Script Date: 05/22/2015 13:10:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientNoteProgram]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientNoteProgram]
GO


/****** Object:  Table [dbo].[RPT_PatientNoteProgram]    Script Date: 05/22/2015 13:10:42 ******/
CREATE TABLE [dbo].[RPT_PatientNoteProgram](
	[PatientNoteProgramId] [int] IDENTITY(1,1) NOT NULL,
	[MongoPatientNoteId] [varchar](50) NOT NULL,
	[PatientNoteId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_PatientNoteProgram] PRIMARY KEY CLUSTERED 
(
	[PatientNoteProgramId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[RPT_PatientNoteProgram]  WITH CHECK ADD  CONSTRAINT [FK_PatientNoteProgram_PatientNote] FOREIGN KEY([PatientNoteId])
REFERENCES [dbo].[RPT_PatientNote] ([PatientNoteId])
GO

ALTER TABLE [dbo].[RPT_PatientNoteProgram] CHECK CONSTRAINT [FK_PatientNoteProgram_PatientNote]
GO

ALTER TABLE [dbo].[RPT_PatientNoteProgram]  WITH CHECK ADD  CONSTRAINT [FK_PatientNoteProgram_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO

ALTER TABLE [dbo].[RPT_PatientNoteProgram] CHECK CONSTRAINT [FK_PatientNoteProgram_UserMongoRecordCreatedBy]
GO

ALTER TABLE [dbo].[RPT_PatientNoteProgram]  WITH CHECK ADD  CONSTRAINT [FK_PatientNoteProgram_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO

ALTER TABLE [dbo].[RPT_PatientNoteProgram] CHECK CONSTRAINT [FK_PatientNoteProgram_UserMongoUpdatedBy]
GO


/*********************** RPT_ContactAddress ***************************/
ALTER TABLE RPT_ContactAddress
ALTER COLUMN [ContactId] [int] NOT NULL

ALTER TABLE RPT_ContactAddress
ALTER COLUMN [TypeId] [int] NOT NULL

ALTER TABLE RPT_ContactAddress
ALTER COLUMN [StateId] [int] NOT NULL

ALTER TABLE RPT_ContactAddress
ALTER COLUMN [StateId] [int] NOT NULL


/***************************** RPT_ContactEmail ***************************************/
ALTER TABLE RPT_ContactEmail
ALTER COLUMN [ContactId] [int] NOT NULL

ALTER TABLE RPT_ContactEmail
ALTER COLUMN [TypeId] [int] NOT NULL


/***************************** RPT_ContactLanguage ***************************************/
ALTER TABLE RPT_ContactLanguage
ALTER COLUMN [ContactId] [int] NOT NULL

ALTER TABLE RPT_ContactLanguage
ALTER COLUMN [LanguageLookUpId] [int] NOT NULL

/***************************** RPT_ContactPhone ***************************************/
ALTER TABLE RPT_ContactPhone
ALTER COLUMN [ContactId] [int] NOT NULL

ALTER TABLE RPT_ContactPhone
ALTER COLUMN [TypeId] [int] NOT NULL

/***************************** RPT_ContactTOD ***************************************/
DELETE [RPT_ContactTimeOfDay]
ALTER TABLE [RPT_ContactTimeOfDay]
ALTER COLUMN [ContactId] [int] NOT NULL
GO

DELETE [RPT_ContactTimeOfDay]
ALTER TABLE [RPT_ContactTimeOfDay]
ALTER COLUMN [TimeOfDayLookUpId] [int] NOT NULL
GO

/***************************** RPT_ContactWeekDay ***************************************/
ALTER TABLE [RPT_ContactWeekDay]
ALTER COLUMN [ContactId] [int] NOT NULL

/***************************** RPT_ContactWeekDay ***************************************/
ALTER TABLE [RPT_ContactRecentList]
ALTER COLUMN [ContactId] [int] NOT NULL


/********************************* [RPT_UserRecentList] ********************************/
IF NOT EXISTS(SELECT * FROM SYS.COLUMNS WHERE NAME = N'MongoUserId' AND OBJECT_ID = OBJECT_ID(N'RPT_UserRecentList'))
BEGIN
DELETE [RPT_UserRecentList]
ALTER TABLE [RPT_UserRecentList]
ADD [MongoUserId] VARCHAR(50) NOT NULL
END
GO

ALTER TABLE [RPT_UserRecentList]
ALTER COLUMN [UserId] [int] NOT NULL
GO

/********************************* [RPT_Observation] ********************************/
ALTER TABLE [RPT_Observation]
ALTER COLUMN [HighValue] [int] NULL
GO

ALTER TABLE [RPT_Observation]
ALTER COLUMN [LowValue] [int] NULL
GO

/************** RPT_SaveObservation *********************/
ALTER PROCEDURE [dbo].[spPhy_RPT_SaveObservation] 
	@MongoID varchar(50),
	@Code varchar(50),
	@CodingSystemId varchar(50),
	@Delete varchar(50),
	@Description varchar(MAX),
	@ExtraElements varchar(MAX),
	@HighValue INT,
	@LastUpdatedOn datetime,
	@LowValue INT,
	@ObservationTypeMongoId varchar(50),
	@Order INT,
	@Source varchar(50),
	@Standard varchar(50),
	@Status varchar(50),
	@TimeToLive datetime,
	@Units varchar(50),
	@UpdatedBy varchar(50),
	@Version float,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@CommonName varchar(100),
	@GroupId varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @ObservationTypeId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @ObservationTypeId = ObservationTypeId From RPT_ObservationTypeLookUp Where MongoId = @ObservationTypeMongoId
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId from [RPT_User] Where MongoId = @UpdatedBy
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId from [RPT_User] Where MongoId = @RecordCreatedBy
	
	
	If (@HighValue = -1)
	Select @HighValue = NULL
	
	If (@LowValue = -1)
	Select @LowValue = NULL
	
	If (@Order = -1)
	Select @Order = NULL
	
	If Exists(Select Top 1 1 From RPT_Observation Where MongoId = @MongoID)
	Begin
		Update RPT_Observation
		Set Code = @Code,
			CodingSystemId = @CodingSystemId,
			[Delete] = @Delete,
			[Description] = @Description,
			ExtraElements = @ExtraElements,
			HighValue = @HighValue,
			LastUpdatedOn = @LastUpdatedOn,
			LowValue = @LowValue,
			ObservationTypeId = @ObservationTypeId,
			MongoObservationLookUpId = @ObservationTypeMongoId,
			[Order] = @Order,
			[Source] = @Source,
			[Standard] = @Standard,
			[Status] = @Status,
			TTLDate = @TimeToLive,
			Units = @Units,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			[Version] = @Version,
			CommonName = @CommonName,
			GroupId = @GroupId
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_Observation(CommonName, GroupId, Code, CodingSystemId, [Delete], [Description], ExtraElements, HighValue, LastUpdatedOn, LowValue, ObservationTypeId, MongoObservationLookUpId, [Order], [Source], [Standard], [Status], TTLDate, Units, MongoUpdatedBy, UpdatedById, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, [Version], MongoId) values 
		(@CommonName, @GroupId, @Code, @CodingSystemId, @Delete, @Description, @ExtraElements, @HighValue, @LastUpdatedOn, @LowValue, @ObservationTypeId, @ObservationTypeMongoId, @Order, @Source, @Standard, @Status, @TimeToLive, @Units, @UpdatedBy, @UpdatedById, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn, @Version, @MongoID)
	End
END
GO
