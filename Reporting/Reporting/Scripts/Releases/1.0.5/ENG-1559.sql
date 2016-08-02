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