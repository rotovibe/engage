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

DROP TABLE [dbo].[RPT_NoteDurationLookUp];
GO