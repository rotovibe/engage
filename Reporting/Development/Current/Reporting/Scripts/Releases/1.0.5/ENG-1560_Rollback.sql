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