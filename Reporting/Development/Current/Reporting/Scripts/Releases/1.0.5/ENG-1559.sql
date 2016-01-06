IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'Details' AND object_id = Object_ID(N'[dbo].RPT_PatientGoal'))
	BEGIN
		ALTER TABLE RPT_PatientGoal
		ADD [Details] [varchar](5000) NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'Details' AND object_id = Object_ID(N'[dbo].RPT_PatientBarrier'))
	BEGIN
		ALTER TABLE RPT_PatientBarrier
		ADD [Details] [varchar](5000) NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'Details' AND object_id = Object_ID(N'[dbo].RPT_PatientIntervention'))
	BEGIN
		ALTER TABLE RPT_PatientIntervention
		ADD [Details] [varchar](5000) NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'Details' AND object_id = Object_ID(N'[dbo].RPT_PatientTask'))
	BEGIN
		ALTER TABLE RPT_PatientTask
		ADD [Details] [varchar](5000) NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name in (N'PatientGoalDetails',N'PatientBarrierDetails', N'PatientInterventionDetails', N'PatientTaskDetails' ) AND object_id = Object_ID(N'[dbo].RPT_PatientGoalMetrics'))
	BEGIN
		ALTER TABLE RPT_PatientGoalMetrics
		ADD [PatientGoalDetails] [varchar](5000) NULL, 
			[PatientBarrierDetails] [varchar](5000) NULL,
			[PatientInterventionDetails] [varchar](5000) NULL,
			[PatientTaskDetails] [varchar](5000) NULL
	END
GO