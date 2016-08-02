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

