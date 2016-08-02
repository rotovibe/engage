DROP TABLE [dbo].[RPT_PatientTaskAttributeValue];
GO

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


