-- [RPT_TouchPoint_Dim]
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MongoPatientId' AND Object_ID = Object_ID(N'RPT_TouchPoint_Dim'))
	BEGIN
		ALTER TABLE [dbo].[RPT_TouchPoint_Dim] ADD [MongoPatientId] [varchar](50) NULL
	END
GO

-- [RPT_Engage_Enrollment_Info]
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MongoPatientId' AND Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
	BEGIN
		ALTER TABLE [dbo].RPT_Engage_Enrollment_Info ADD [MongoPatientId] [varchar](50) NULL
	END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MongoPatientProgramId' AND Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
	BEGIN
		ALTER TABLE [dbo].RPT_Engage_Enrollment_Info ADD [MongoPatientProgramId] [varchar](50) NULL
	END
GO

-- [RPT_CareBridge_Enrollment_Info]
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MongoPatientId' AND Object_ID = Object_ID(N'RPT_CareBridge_Enrollment_Info'))
	BEGIN
		ALTER TABLE [dbo].RPT_CareBridge_Enrollment_Info ADD [MongoPatientId] [varchar](50) NULL
	END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MongoPatientProgramId' AND Object_ID = Object_ID(N'RPT_CareBridge_Enrollment_Info'))
	BEGIN
		ALTER TABLE [dbo].RPT_CareBridge_Enrollment_Info ADD [MongoPatientProgramId] [varchar](50) NULL
	END
GO