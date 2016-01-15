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

