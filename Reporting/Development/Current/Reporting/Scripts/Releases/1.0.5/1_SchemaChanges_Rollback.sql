----------------------------------------------------------------------------------------------------------------------------------
--ENG-936
----------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[RPT_PatientTaskAttribute](
	[PatientTaskAttributeId] [int] IDENTITY(1,1) NOT NULL,
	[MongoPatientTaskId] [varchar](50) NULL,
	[PatientTaskId] [int] NOT NULL,
	[MongoGoalAttributeId] [varchar](50) NOT NULL,
	[GoalAttributeId] [int] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_PatientTaskAttribute] PRIMARY KEY CLUSTERED 
(
	[PatientTaskAttributeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttribute_GoalAttribute] FOREIGN KEY([GoalAttributeId])
REFERENCES [dbo].[RPT_GoalAttribute] ([GoalAttributeID])
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute] CHECK CONSTRAINT [FK_PatientTaskAttribute_GoalAttribute]
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttribute_PatientTask] FOREIGN KEY([PatientTaskId])
REFERENCES [dbo].[RPT_PatientTask] ([PatientTaskId])
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute] CHECK CONSTRAINT [FK_PatientTaskAttribute_PatientTask]
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttribute_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute] CHECK CONSTRAINT [FK_PatientTaskAttribute_UserMongoRecordCreatedBy]
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttribute_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute] CHECK CONSTRAINT [FK_PatientTaskAttribute_UserMongoUpdatedBy]
GO

CREATE TABLE [dbo].[RPT_PatientTaskAttributeValue](
	[ValueId] [int] IDENTITY(1,1) NOT NULL,
	[PatientTaskAttributeId] [int] NOT NULL,
	[Value] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_PatientTaskAttributeValue] PRIMARY KEY CLUSTERED 
(
	[ValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttributeValue_PatientTaskAttribute] FOREIGN KEY([PatientTaskAttributeId])
REFERENCES [dbo].[RPT_PatientTaskAttribute] ([PatientTaskAttributeId])
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttributeValue] CHECK CONSTRAINT [FK_PatientTaskAttributeValue_PatientTaskAttribute]
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttributeValue_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttributeValue] CHECK CONSTRAINT [FK_PatientTaskAttributeValue_UserMongoRecordCreatedBy]
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttributeValue_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttributeValue] CHECK CONSTRAINT [FK_PatientTaskAttributeValue_UserMongoUpdatedBy]
GO


IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'PatientTaskAttributeName' AND object_id = Object_ID(N'[dbo].RPT_PatientGoalMetrics'))
	BEGIN
		ALTER TABLE RPT_PatientGoalMetrics
		ADD [PatientTaskAttributeName] [varchar] (100) NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'PatientTaskAttributeValue' AND object_id = Object_ID(N'[dbo].RPT_PatientGoalMetrics'))
	BEGIN
		ALTER TABLE RPT_PatientGoalMetrics
		ADD [PatientTaskAttributeValue] [varchar] (50) NULL
	END
GO
----------------------------------------------------------------------------------------------------------------------------------
--ENG-1354
----------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_Transition_Info')
	DROP TABLE [dbo].[RPT_Flat_Transition_Info]
GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1507
----------------------------------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'DueDate' AND Object_ID = Object_ID(N'RPT_PatientIntervention'))
	BEGIN
		ALTER TABLE RPT_PatientIntervention
		DROP COLUMN [DueDate];
	END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'PatientInterventionDueDate' AND Object_ID = Object_ID(N'RPT_PatientGoalMetrics'))
	BEGIN
		ALTER TABLE RPT_PatientGoalMetrics
		DROP COLUMN [PatientInterventionDueDate];
	END
GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1559
----------------------------------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'Details' AND Object_ID = Object_ID(N'RPT_PatientGoal'))
	BEGIN
		ALTER TABLE RPT_PatientGoal
		DROP COLUMN [Details];
	END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'Details' AND Object_ID = Object_ID(N'RPT_PatientBarrier'))
	BEGIN
		ALTER TABLE RPT_PatientBarrier
		DROP COLUMN [Details];
	END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'Details' AND Object_ID = Object_ID(N'RPT_PatientIntervention'))
	BEGIN
		ALTER TABLE RPT_PatientIntervention
		DROP COLUMN [Details];
	END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'Details' AND Object_ID = Object_ID(N'RPT_PatientTask'))
	BEGIN
		ALTER TABLE RPT_PatientTask
		DROP COLUMN [Details];
	END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE Name in (N'PatientGoalDetails',N'PatientBarrierDetails', N'PatientInterventionDetails', N'PatientTaskDetails' ) AND Object_ID = Object_ID(N'RPT_PatientGoalMetrics'))
	BEGIN
		ALTER TABLE RPT_PatientGoalMetrics
		DROP COLUMN [PatientGoalDetails],[PatientBarrierDetails],[PatientInterventionDetails],[PatientTaskDetails];
	END
GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1560
----------------------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'MongoDurationId' AND object_id = Object_ID(N'[dbo].RPT_PatientNote'))
	BEGIN
		ALTER TABLE RPT_PatientNote
		ADD [MongoDurationId] [varchar] (50) NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'DurationId' AND object_id = Object_ID(N'[dbo].RPT_PatientNote'))
	BEGIN
		ALTER TABLE RPT_PatientNote
		ADD [DurationId] [int] NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'Duration' AND object_id = Object_ID(N'[dbo].RPT_PatientNotes_Dim'))
	BEGIN
		ALTER TABLE RPT_PatientNotes_Dim
		ADD [Duration] [varchar] (100) NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'Duration' AND object_id = Object_ID(N'[dbo].RPT_TouchPoint_Dim'))
	BEGIN
		ALTER TABLE RPT_TouchPoint_Dim
		ADD [Duration] [varchar] (100) NULL
	END
GO

CREATE TABLE [dbo].[RPT_NoteDurationLookUp](
	[NoteDurationId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[LookUpType] [varchar](max) NULL,
	[Name] [varchar](max) NULL,
	[Default] [varchar](50) NULL,
	[Active] [varchar](50) NULL,
 CONSTRAINT [PK_NoteDurationLookUp] PRIMARY KEY CLUSTERED 
(
	[NoteDurationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] 

GO
----------------------------------------------------------------------------------------------------------------------------------
--ENG-1589
----------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_GoalStatistics')
	DROP TABLE [dbo].[RPT_Flat_GoalStatistics]
GO
----------------------------------------------------------------------------------------------------------------------------------
--ENG-1590
----------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_InterventionStatistics')
	DROP TABLE [dbo].[RPT_Flat_InterventionStatistics]
GO
----------------------------------------------------------------------------------------------------------------------------------
--ENG-1591
----------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_TaskStatistics')
	DROP TABLE [dbo].[RPT_Flat_TaskStatistics]
GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1592
----------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_BarrierStatistics')
	DROP TABLE [dbo].[RPT_Flat_BarrierStatistics]
GO


----------------------------------------------------------------------------------------------------------------------------------
--ENG-1663
----------------------------------------------------------------------------------------------------------------------------------

IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'HTN' and Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
    ALTER TABLE RPT_Engage_Enrollment_Info
	DROP COLUMN HTN 
END

IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Heart_Failure' and Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
    ALTER TABLE RPT_Engage_Enrollment_Info
	DROP COLUMN Heart_Failure 
END

IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'COPD' and Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
    ALTER TABLE RPT_Engage_Enrollment_Info
	DROP COLUMN COPD 
END

IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Diabetes' and Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
    ALTER TABLE RPT_Engage_Enrollment_Info
	DROP COLUMN Diabetes 
END

IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Asthma' and Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
    ALTER TABLE RPT_Engage_Enrollment_Info
	DROP COLUMN Asthma 
END

IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Comorbid_Disease' and Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
BEGIN
    ALTER TABLE RPT_Engage_Enrollment_Info
	DROP COLUMN Comorbid_Disease 
END
---------------------------------------------------------------------------------------------------------------------------------
--ENG-1506
----------------------------------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_Latest_PatientObservations')
	DROP TABLE [dbo].[RPT_Flat_Latest_PatientObservations]
GO