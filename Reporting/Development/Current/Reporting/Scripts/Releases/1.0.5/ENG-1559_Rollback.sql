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
