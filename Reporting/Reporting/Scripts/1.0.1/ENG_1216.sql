/**** CREATE PROCESS AUDIT LOG TABLE ****/
/****** Object:  Table [dbo].[RPT_ProcessAudit]    Script Date: 05/12/2015 09:56:42 ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RPT_ProcessAudit]') AND type in (N'U'))
BEGIN
CREATE TABLE [RPT_ProcessAudit](
	[aid] [int] IDENTITY(1,1) NOT NULL,
	[Statement] [varchar](200) NOT NULL,
	[Start] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[Contract] [varchar](200) NOT NULL,
	[Time] [time](6) NOT NULL
 CONSTRAINT [PK_ProcessAudit] PRIMARY KEY CLUSTERED 
(
	[aid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[RPT_ProcessAudit]') AND name = N'IX_RPT_ProcessAudit_StartEndDate')
CREATE NONCLUSTERED INDEX [IX_RPT_ProcessAudit_StartEndDate] ON [RPT_ProcessAudit] 
(
	[Start] ASC,
	[End] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/***** ADD LOGGING TO THE FLAT TABLE INITIALIZAION ******/
/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Initialize_Flat_Tables]    Script Date: 05/11/2015 10:11:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Initialize_Flat_Tables]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Initialize_Flat_Tables]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Initialize_Flat_Tables]    Script Date: 05/15/2015 13:34:55 ******/
CREATE PROCEDURE [dbo].[spPhy_RPT_Initialize_Flat_Tables]
AS
BEGIN
	DECLARE @StartTime Datetime;
	
	SET @StartTime = GETDATE();
	EXECUTE [spPhy_RPT_ProgramResponse_Flat];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_ProgramResponse_Flat', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));

	SET @StartTime = GETDATE();
	EXECUTE [spPhy_RPT_Flat_BSHSI_HW2]
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract],[Time]) VALUES ('spPhy_RPT_Flat_BSHSI_HW2', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));

	SET @StartTime = GETDATE();	
	EXECUTE [spPhy_RPT_Flat_CareBridge];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_Flat_CareBridge', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));
	
	SET @StartTime = GETDATE();	
	EXECUTE [spPhy_RPT_Flat_Engage];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_Flat_Engage', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));	
	
	SET @StartTime = GETDATE();	
	EXECUTE [spPhy_RPT_Flat_Observations_Dim];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_Flat_Observations_Dim', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));		

	SET @StartTime = GETDATE();	
	EXECUTE [spPhy_RPT_Flat_TouchPoint_Dim];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_Flat_TouchPoint_Dim', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));	
	
	SET @StartTime = GETDATE();	
	EXECUTE [spPhy_RPT_SavePatientInfo];
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES ('spPhy_RPT_SavePatientInfo', @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));		
END
GO

/**** ADDING RECOMMENDED NON-CLUSTERED INDEXES *****/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramResponse]') AND name = 'IX_RPT_PatientProgramResponse_StepidSelected')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramResponse_StepidSelected]
ON [dbo].[RPT_PatientProgramResponse] ([StepId],[Selected])
INCLUDE ([Text],[Delete])
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramModule]') AND name = 'IX_RPT_PatientProgramModule_MongoId')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramModule_MongoId]
ON [dbo].[RPT_PatientProgramModule] ([MongoId])
INCLUDE ([MongoProgramId])
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND name = 'IX_RPT_ProgramResponse_Flat_PatientId_PatientProgramId_ActionSourceId')
CREATE NONCLUSTERED INDEX [IX_RPT_ProgramResponse_Flat_PatientId_PatientProgramId_ActionSourceId]
ON [dbo].[RPT_ProgramResponse_Flat] ([PatientId],[PatientProgramId],[ActionSourceId])
INCLUDE ([ActionArchivedDate])
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientProgramStep]') AND name = 'IX_RPT_PatientProgramStep_SourceId')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientProgramStep_SourceId]
ON [dbo].[RPT_PatientProgramStep] ([SourceId])
INCLUDE ([MongoActionId],[MongoId])
GO


/**************** patientprogramresponse alter ****************************/
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


/*********************************************************/


ALTER TABLE dbo.RPT_PatientProgramStep
	DROP CONSTRAINT FK_PatientProgramModuleActionStep_PatientProgramModuleAction
GO
ALTER TABLE dbo.RPT_PatientProgramAction SET (LOCK_ESCALATION = TABLE)
GO

CREATE TABLE dbo.Tmp_RPT_PatientProgramStep
	(
	StepId int NOT NULL IDENTITY (1, 1),
	MongoActionId varchar(50) NOT NULL,
	ActionId int NOT NULL,
	MongoId varchar(50) NOT NULL,
	AttributeEndDate datetime NULL,
	AttributeStartDate datetime NULL,
	SourceId varchar(50) NULL,
	[Order] varchar(50) NULL,
	Eligible varchar(50) NULL,
	State varchar(50) NULL,
	Completed varchar(50) NULL,
	EligibilityEndDate datetime NULL,
	Header varchar(100) NULL,
	SelectedResponseId varchar(50) NULL,
	ControlType varchar(50) NULL,
	SelectType varchar(50) NULL,
	IncludeTime varchar(50) NULL,
	Question varchar(900) NULL,
	Title varchar(2000) NULL,
	Description varchar(MAX) NULL,
	Notes varchar(MAX) NULL,
	Text varchar(MAX) NULL,
	Status varchar(50) NULL,
	Response varchar(50) NULL,
	StepTypeId varchar(50) NULL,
	Enabled varchar(50) NULL,
	StateUpdatedOn datetime NULL,
	MongoCompletedBy varchar(50) NULL,
	CompletedBy int NULL,
	DateCompleted datetime NULL,
	MongoNext varchar(50) NULL,
	Next int NULL,
	Previous int NULL,
	EligibilityRequirements varchar(50) NULL,
	EligibilityStartDate datetime NULL,
	MongoPrevious varchar(50) NULL,
	Version varchar(50) NULL,
	MongoUpdatedBy varchar(50) NULL,
	UpdatedBy int NULL,
	LastUpdatedOn datetime NULL,
	MongoRecordCreatedBy varchar(50) NULL,
	RecordCreatedBy int NULL,
	RecordCreatedOn datetime NULL,
	TTLDate datetime NULL,
	[Delete] varchar(50) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_RPT_PatientProgramStep SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_RPT_PatientProgramStep ON
GO
IF EXISTS(SELECT * FROM dbo.RPT_PatientProgramStep)
	 EXEC('INSERT INTO dbo.Tmp_RPT_PatientProgramStep (StepId, MongoActionId, ActionId, MongoId, AttributeEndDate, AttributeStartDate, SourceId, [Order], Eligible, State, Completed, EligibilityEndDate, Header, SelectedResponseId, ControlType, SelectType, IncludeTime, Question, Title, Description, Notes, Text, Status, Response, StepTypeId, Enabled, StateUpdatedOn, MongoCompletedBy, CompletedBy, DateCompleted, MongoNext, Next, Previous, EligibilityRequirements, EligibilityStartDate, MongoPrevious, Version, MongoUpdatedBy, UpdatedBy, LastUpdatedOn, MongoRecordCreatedBy, RecordCreatedBy, RecordCreatedOn, TTLDate, [Delete])
		SELECT StepId, MongoActionId, ActionId, MongoId, AttributeEndDate, AttributeStartDate, SourceId, [Order], Eligible, State, Completed, EligibilityEndDate, Header, SelectedResponseId, ControlType, SelectType, IncludeTime, CONVERT(varchar(900), Question), Title, Description, Notes, Text, Status, Response, StepTypeId, Enabled, StateUpdatedOn, MongoCompletedBy, CompletedBy, DateCompleted, MongoNext, Next, Previous, EligibilityRequirements, EligibilityStartDate, MongoPrevious, Version, MongoUpdatedBy, UpdatedBy, LastUpdatedOn, MongoRecordCreatedBy, RecordCreatedBy, RecordCreatedOn, TTLDate, [Delete] FROM dbo.RPT_PatientProgramStep WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_RPT_PatientProgramStep OFF
GO
ALTER TABLE dbo.RPT_PatientProgramResponse
	DROP CONSTRAINT FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep
GO
ALTER TABLE dbo.RPT_PatientProgramResponse
	DROP CONSTRAINT FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep
GO
DROP TABLE dbo.RPT_PatientProgramStep
GO
EXECUTE sp_rename N'dbo.Tmp_RPT_PatientProgramStep', N'RPT_PatientProgramStep', 'OBJECT' 
GO
ALTER TABLE dbo.RPT_PatientProgramStep ADD CONSTRAINT
	PK_PatientProgramModuleActionStep PRIMARY KEY CLUSTERED 
	(
	StepId
	) WITH( PAD_INDEX = OFF, FILLFACTOR = 90, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_RPT_PatientProgramStep_Sourceid ON dbo.RPT_PatientProgramStep
	(
	SourceId
	) INCLUDE (MongoActionId, MongoId) 
 WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_RPT_PatientProgramStep ON dbo.RPT_PatientProgramStep
	(
	MongoId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_RPT_PatientProgramStep_1 ON dbo.RPT_PatientProgramStep
	(
	SourceId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_RPT_PatientProgramStep_2 ON dbo.RPT_PatientProgramStep
	(
	StepId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_RPT_PatientProgramStep_ActionId ON dbo.RPT_PatientProgramStep
	(
	ActionId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_RPT_PatientProgramStep_Optimized ON dbo.RPT_PatientProgramStep
	(
	StepId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.RPT_PatientProgramStep WITH NOCHECK ADD CONSTRAINT
	FK_PatientProgramModuleActionStep_PatientProgramModuleAction FOREIGN KEY
	(
	ActionId
	) REFERENCES dbo.RPT_PatientProgramAction
	(
	ActionId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO

ALTER TABLE dbo.RPT_PatientProgramResponse WITH NOCHECK ADD CONSTRAINT
	FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionNextStep FOREIGN KEY
	(
	NextStepId
	) REFERENCES dbo.RPT_PatientProgramStep
	(
	StepId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RPT_PatientProgramResponse WITH NOCHECK ADD CONSTRAINT
	FK_PatientProgramModuleActionStepResponse_PatientProgramModuleActionStep FOREIGN KEY
	(
	StepId
	) REFERENCES dbo.RPT_PatientProgramStep
	(
	StepId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RPT_PatientProgramResponse SET (LOCK_ESCALATION = TABLE)
GO



/*********************** RPT_PatientNotesProgram Alter  *******************************/
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
	[PatientNoteId] [int] NULL,
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


/*********************** RPT_ContactAddress ***************************/
ALTER TABLE RPT_ContactAddress
ALTER COLUMN [ContactId] [int] NULL

ALTER TABLE RPT_ContactAddress
ALTER COLUMN [TypeId] [int] NULL

ALTER TABLE RPT_ContactAddress
ALTER COLUMN [StateId] [int] NULL

ALTER TABLE RPT_ContactAddress
ALTER COLUMN [StateId] [int] NULL


/***************************** RPT_ContactEmail ***************************************/
ALTER TABLE RPT_ContactEmail
ALTER COLUMN [ContactId] [int] NULL

ALTER TABLE RPT_ContactEmail
ALTER COLUMN [TypeId] [int] NULL


/***************************** RPT_ContactLanguage ***************************************/
ALTER TABLE RPT_ContactLanguage
ALTER COLUMN [ContactId] [int] NULL

ALTER TABLE RPT_ContactLanguage
ALTER COLUMN [LanguageLookUpId] [int] NULL



/***************************** RPT_ContactPhone ***************************************/
ALTER TABLE RPT_ContactPhone
ALTER COLUMN [ContactId] [int] NULL

ALTER TABLE RPT_ContactPhone
ALTER COLUMN [TypeId] [int] NULL


/***************************** RPT_ContactTOD ***************************************/
ALTER TABLE [RPT_ContactTimeOfDay]
ALTER COLUMN [ContactId] [int] NULL

ALTER TABLE [RPT_ContactTimeOfDay]
ALTER COLUMN [TimeOfDayLookUpId] [int] NULL


/***************************** RPT_ContactWeekDay ***************************************/
ALTER TABLE [RPT_ContactWeekDay]
ALTER COLUMN [ContactId] [int] NULL


/***************************** RPT_ContactWeekDay ***************************************/
ALTER TABLE [RPT_ContactRecentList]
ALTER COLUMN [ContactId] [int] NULL


/********************************* [RPT_UserRecentList] ********************************/
IF NOT EXISTS ( SELECT *  FROM   sys.columns WHERE  object_id = OBJECT_ID(N'[dbo].[RPT_UserRecentList]') AND name = 'MongoUserId')
BEGIN
ALTER TABLE [RPT_UserRecentList]
ADD [MongoUserId] VARCHAR(50) NULL
END
GO

IF NOT EXISTS ( SELECT *  FROM   sys.columns WHERE  object_id = OBJECT_ID(N'[dbo].[RPT_UserRecentList]') AND name = 'UserId')
BEGIN
ALTER TABLE [RPT_UserRecentList]
ALTER COLUMN [UserId] [int] NULL
END
GO


/********************************* [RPT_Observation] ********************************/
ALTER TABLE [RPT_Observation]
ALTER COLUMN [HighValue] decimal(18,4) NULL
GO

ALTER TABLE [RPT_Observation]
ALTER COLUMN [LowValue] decimal(18,4) NULL
GO

/************** RPT_SaveObservation *********************/
ALTER PROCEDURE [dbo].[spPhy_RPT_SaveObservation] 
	@MongoID varchar(50),
	@Code varchar(50),
	@CodingSystemId varchar(50),
	@Delete varchar(50),
	@Description varchar(MAX),
	@ExtraElements varchar(MAX),
	@HighValue DECIMAL(18,4),
	@LastUpdatedOn datetime,
	@LowValue DECIMAL(18,4),
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
