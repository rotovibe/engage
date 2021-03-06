-- [RPT_TouchPoint_Dim]
IF EXISTS(SELECT * FROM sys.columns WHERE Name = N'MongoPatientId' AND Object_ID = Object_ID(N'RPT_TouchPoint_Dim'))
	BEGIN
		ALTER TABLE [dbo].[RPT_TouchPoint_Dim] DROP COLUMN [MongoPatientId]
	END

IF EXISTS(SELECT * FROM sys.columns WHERE Name = N'MongoPatientProgramId' AND Object_ID = Object_ID(N'RPT_TouchPoint_Dim'))
	BEGIN
		ALTER TABLE [dbo].[RPT_TouchPoint_Dim] DROP COLUMN [MongoPatientProgramId]
	END

-- [RPT_Engage_Enrollment_Info]
IF EXISTS(SELECT * FROM sys.columns WHERE Name = N'MongoPatientId' AND Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
	BEGIN
		ALTER TABLE [dbo].RPT_Engage_Enrollment_Info DROP COLUMN [MongoPatientId]
	END

IF EXISTS(SELECT * FROM sys.columns WHERE Name = N'MongoPatientProgramId' AND Object_ID = Object_ID(N'RPT_Engage_Enrollment_Info'))
	BEGIN
		ALTER TABLE [dbo].RPT_Engage_Enrollment_Info DROP COLUMN [MongoPatientProgramId]
	END


-- [RPT_CareBridge_Enrollment_Info]
IF EXISTS(SELECT * FROM sys.columns WHERE Name = N'MongoPatientId' AND Object_ID = Object_ID(N'RPT_CareBridge_Enrollment_Info'))
	BEGIN
		ALTER TABLE [dbo].RPT_CareBridge_Enrollment_Info DROP COLUMN [MongoPatientId]
	END

IF EXISTS(SELECT * FROM sys.columns WHERE Name = N'MongoPatientProgramId' AND Object_ID = Object_ID(N'RPT_CareBridge_Enrollment_Info'))
	BEGIN
		ALTER TABLE [dbo].RPT_CareBridge_Enrollment_Info DROP COLUMN [MongoPatientProgramId]
	END
