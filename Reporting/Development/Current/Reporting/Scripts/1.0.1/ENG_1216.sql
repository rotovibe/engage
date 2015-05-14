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
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spPhy_RPT_Initialize_Flat_Tables]') AND type in (N'P'))
	DROP PROCEDURE [spPhy_RPT_Initialize_Flat_Tables]
GO

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
END

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
