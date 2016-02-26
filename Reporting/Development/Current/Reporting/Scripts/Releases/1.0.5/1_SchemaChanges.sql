----------------------------------------------------------------------------------------------------------------------------------
--ENG-936
----------------------------------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_PatientTaskAttributeValue')
	DROP TABLE [dbo].[RPT_PatientTaskAttributeValue];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_PatientTaskAttribute')
	DROP TABLE [dbo].[RPT_PatientTaskAttribute];
GO

IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'PatientTaskAttributeName' AND Object_ID = Object_ID(N'RPT_PatientGoalMetrics'))
	BEGIN
		ALTER TABLE RPT_PatientGoalMetrics
		DROP COLUMN [PatientTaskAttributeName];
	END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'PatientTaskAttributeValue' AND Object_ID = Object_ID(N'RPT_PatientGoalMetrics'))
	BEGIN
		ALTER TABLE RPT_PatientGoalMetrics
		DROP COLUMN [PatientTaskAttributeValue];
	END
GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1354
----------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_Transition_Info')
	DROP TABLE [dbo].[RPT_Flat_Transition_Info]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[RPT_Flat_Transition_Info](
		PatientId [int] NULL,
		MongoPatientId [varchar](50) NULL,
		PatientProgramId [int] NULL,
		MongoPatientProgramId [varchar](50) NULL,
		Enrollment_Status [varchar](100) NULL,
		Enrolled_Date [date] NULL,
		Did_Not_Enroll_Date [date] NULL,
		Did_Not_Enroll_Reason [varchar](1000) NULL,
		Disenroll_Date [date] NULL,
		Disenroll_Reason [varchar](1000) NULL,
		Program_Completed_Date [date] NULL,
		Call_Within_48Hours_PostDischarge [varchar](100) NULL,
		Discharge_Type [varchar](100) NULL,
		Total_LACE_Score [varchar](1000) NULL,
		High_Risk_For_Readmission [varchar](100) NULL,
		Program_Status [varchar](100) NULL,
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1507
----------------------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'DueDate' AND object_id = Object_ID(N'[dbo].RPT_PatientIntervention'))
	BEGIN
		ALTER TABLE RPT_PatientIntervention
		ADD [DueDate] [datetime] NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'PatientInterventionDueDate' AND object_id = Object_ID(N'[dbo].RPT_PatientGoalMetrics'))
	BEGIN
		ALTER TABLE RPT_PatientGoalMetrics
		ADD [PatientInterventionDueDate] [datetime] NULL
	END
GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1559
----------------------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'Details' AND object_id = Object_ID(N'[dbo].RPT_PatientGoal'))
	BEGIN
		ALTER TABLE RPT_PatientGoal
		ADD [Details] [varchar](max) NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'Details' AND object_id = Object_ID(N'[dbo].RPT_PatientBarrier'))
	BEGIN
		ALTER TABLE RPT_PatientBarrier
		ADD [Details] [varchar](max) NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'Details' AND object_id = Object_ID(N'[dbo].RPT_PatientIntervention'))
	BEGIN
		ALTER TABLE RPT_PatientIntervention
		ADD [Details] [varchar](max) NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'Details' AND object_id = Object_ID(N'[dbo].RPT_PatientTask'))
	BEGIN
		ALTER TABLE RPT_PatientTask
		ADD [Details] [varchar](max) NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name in (N'PatientGoalDetails',N'PatientBarrierDetails', N'PatientInterventionDetails', N'PatientTaskDetails' ) AND object_id = Object_ID(N'[dbo].RPT_PatientGoalMetrics'))
	BEGIN
		ALTER TABLE RPT_PatientGoalMetrics
		ADD [PatientGoalDetails] [varchar](max) NULL, 
			[PatientBarrierDetails] [varchar](max) NULL,
			[PatientInterventionDetails] [varchar](max) NULL,
			[PatientTaskDetails] [varchar](max) NULL
	END
GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1560
----------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'MongoDurationId' AND Object_ID = Object_ID(N'RPT_PatientNote'))
	BEGIN
		ALTER TABLE RPT_PatientNote
		DROP COLUMN [MongoDurationId];
	END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'DurationId' AND Object_ID = Object_ID(N'RPT_PatientNote'))
	BEGIN
		ALTER TABLE RPT_PatientNote
		DROP COLUMN [DurationId];
	END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'Duration' AND Object_ID = Object_ID(N'RPT_PatientNotes_Dim'))
	BEGIN
		ALTER TABLE RPT_PatientNotes_Dim
		DROP COLUMN [Duration];
	END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'Duration' AND Object_ID = Object_ID(N'RPT_TouchPoint_Dim'))
	BEGIN
		ALTER TABLE RPT_TouchPoint_Dim
		DROP COLUMN [Duration];
	END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_NoteDurationLookUp')
	DROP TABLE [dbo].[RPT_NoteDurationLookUp];
GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1589
----------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_GoalStatistics')
	DROP TABLE [dbo].[RPT_Flat_GoalStatistics]
GO

CREATE TABLE [dbo].[RPT_Flat_GoalStatistics](
	MongoGoalId [varchar](50) NOT NULL,
	MongoPatientId [varchar](50) NULL,
	Confidence [varchar](50) NULL,
	Importance [varchar](50) NULL,
	StageofChange [varchar](50) NULL,
	CreatedOn DateTime NULL,
	CreatedBy [varchar](100) NULL,
	UpdatedOn DateTime NULL,
	UpdatedBy [varchar](100) NULL,
	Name [varchar](500) NULL,
	Details [varchar](max) NULL,
	TemplateId [varchar](50) NULL,
	[Source] [varchar](max) NULL,
	TargetDate DateTime NULL,
	TargetValue [varchar](300) NULL,
	[Status] [varchar](50) NULL,
	StartDate DateTime NULL,
	EndDate DateTime NULL,
	FocusAreas [varchar](max) NULL,
	[Type] [varchar](50) NULL,
	PrimaryCareManagerMongoId [varchar](50) NULL,
	PrimaryCareManagerPreferredName [varchar](100) NULL,
  CONSTRAINT [PK_MongoId] PRIMARY KEY CLUSTERED 
	(
		[MongoGoalId] ASC
	)
) ON [PRIMARY]
GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1590
----------------------------------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_InterventionStatistics')
	DROP TABLE [dbo].[RPT_Flat_InterventionStatistics]
GO

CREATE TABLE [dbo].[RPT_Flat_InterventionStatistics](
		MongoInterventionId [varchar](50) NOT NULL,
		MongoPatientId [varchar](50) NULL,
		MongoGoalId [varchar](50) NULL,
		CreatedOn DateTime NULL,
		CreatedBy [varchar](100) NULL,
		UpdatedOn DateTime NULL,
		UpdatedBy [varchar](100) NULL,
		ClosedDate [datetime] NULL,
		[Status] [varchar](50) NULL,
		StartDate [datetime] NULL,
		DueDate [datetime] NULL,
		[Description] [varchar](max) NULL,
		Details [varchar](max) NULL,
		CategoryName [varchar](max) NULL,
		TemplateId [varchar](50) NULL,
		AssignedTo [varchar](100) NULL,
		PrimaryCareManagerMongoId [varchar](50) NULL,
		PrimaryCareManagerPreferredName [varchar](100) NULL,
		InterventionBarriers [varchar](max) NULL,
) ON [PRIMARY]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Flat_InterventionStatistics]') AND name = N'CIDX_RPT_Flat_InterventionStatistics_MongoPatientId_MongoGoalId')
DROP INDEX [CIDX_RPT_Flat_InterventionStatistics_MongoPatientId_MongoGoalId] ON [dbo].[RPT_Flat_InterventionStatistics] WITH ( ONLINE = OFF )
GO

CREATE CLUSTERED INDEX [CIDX_RPT_Flat_InterventionStatistics_MongoPatientId_MongoGoalId] ON [dbo].[RPT_Flat_InterventionStatistics] 
(
	MongoPatientId ASC,
	MongoGoalId ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1591
----------------------------------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_TaskStatistics')
	DROP TABLE [dbo].[RPT_Flat_TaskStatistics]
GO

CREATE TABLE [dbo].[RPT_Flat_TaskStatistics](
		MongoTaskId [varchar](50) NOT NULL,
		MongoPatientId [varchar](50) NULL,
		MongoGoalId [varchar](50) NULL,
		CreatedOn DateTime NULL,
		CreatedBy [varchar](100) NULL,
		UpdatedOn DateTime NULL,
		UpdatedBy [varchar](100) NULL,
		ClosedDate [datetime] NULL,
		[Status] [varchar](50) NULL,
		StartDate [datetime] NULL,
		TargetDate [datetime] NULL,
		TargetValue [varchar](50) NULL,
		[Description] [varchar](max) NULL,
		Details [varchar](max) NULL,
		TemplateId [varchar](50) NULL,
		PrimaryCareManagerMongoId [varchar](50) NULL,
		PrimaryCareManagerPreferredName [varchar](100) NULL,
		TaskBarriers [varchar](max) NULL
) ON [PRIMARY]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Flat_TaskStatistics]') AND name = N'CIDX_RPT_Flat_TaskStatistics_MongoPatientId_MongoGoalId')
DROP INDEX [CIDX_RPT_Flat_TaskStatistics_MongoPatientId_MongoGoalId] ON [dbo].[RPT_Flat_TaskStatistics] WITH ( ONLINE = OFF )
GO

CREATE CLUSTERED INDEX [CIDX_RPT_Flat_TaskStatistics_MongoPatientId_MongoGoalId] ON [dbo].[RPT_Flat_TaskStatistics] 
(
	MongoPatientId ASC,
	MongoGoalId ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1592
----------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_BarrierStatistics')
	DROP TABLE [dbo].[RPT_Flat_BarrierStatistics]
GO

CREATE TABLE [dbo].[RPT_Flat_BarrierStatistics](
		MongoBarrierId [varchar](50) NOT NULL,
		MongoPatientId [varchar](50) NULL,
		MongoGoalId [varchar](50) NULL,
		CreatedOn DateTime NULL,
		CreatedBy [varchar](100) NULL,
		UpdatedOn DateTime NULL,
		UpdatedBy [varchar](100) NULL,
		[Status] [varchar](50) NULL,
		Name [varchar](500) NULL,
		Details [varchar](max) NULL,
		Category [varchar](50) NULL,
		PrimaryCareManagerMongoId [varchar](50) NULL,
		PrimaryCareManagerPreferredName [varchar](100) NULL,
) ON [PRIMARY]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Flat_BarrierStatistics]') AND name = N'CIDX_RPT_Flat_BarrierStatistics_MongoPatientId_MongoGoalId')
DROP INDEX [CIDX_RPT_Flat_BarrierStatistics_MongoPatientId_MongoGoalId] ON [dbo].[RPT_Flat_BarrierStatistics] WITH ( ONLINE = OFF )
GO

CREATE CLUSTERED INDEX [CIDX_RPT_Flat_BarrierStatistics_MongoPatientId_MongoGoalId] ON [dbo].[RPT_Flat_BarrierStatistics] 
(
	MongoPatientId ASC,
	MongoGoalId ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1663
----------------------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'HTN' and Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
	ALTER TABLE RPT_Engage_Enrollment_Info
	ADD HTN varchar(100) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Heart_Failure' and Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
	ALTER TABLE RPT_Engage_Enrollment_Info
	ADD Heart_Failure varchar(100) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'COPD' and Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
	ALTER TABLE RPT_Engage_Enrollment_Info
	ADD COPD varchar(100) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Diabetes' and Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
	ALTER TABLE RPT_Engage_Enrollment_Info
	ADD Diabetes varchar(100) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Asthma' and Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
	ALTER TABLE RPT_Engage_Enrollment_Info
	ADD Asthma varchar(100) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Comorbid_Disease' and Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
	ALTER TABLE RPT_Engage_Enrollment_Info
	ADD Comorbid_Disease varchar(500) NULL
END


----------------------------------------------------------------------------------------------------------------------------------
--ENG-1506
----------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_Latest_PatientObservations')
	DROP TABLE [dbo].[RPT_Flat_Latest_PatientObservations]
GO

CREATE TABLE [dbo].[RPT_Flat_Latest_PatientObservations](
	MongoPatientId [varchar](50) NULL,
	[ObservationType] [varchar](100) NULL,
	[Code] [varchar](100) NULL,
	[CommonName] [varchar](100) NULL,
	[Description] [varchar](max) NULL,
	[State] [varchar](50) NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
	[NumericValue] [float] NULL,
	[NonNumericValue] [varchar](50) NULL,
	PrimaryCareManagerMongoId [varchar](50) NULL,
	PrimaryCareManagerFirstName [varchar](100) NULL,
	PrimaryCareManagerLastName [varchar](100) NULL,
	PrimaryCareManagerPreferredName [varchar](100) NULL,
) ON [PRIMARY]
GO