-- rollback --
if not exists(select * from sys.columns where Name = N'FirstName' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info ADD [FirstName] [varchar](100) NULL;
	END
GO

if not exists(select * from sys.columns where Name = N'MiddleName' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info ADD [MiddleName] [varchar](100) NULL;
	END
GO

if not exists(select * from sys.columns where Name = N'LastName' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info ADD [LastName] [varchar](100) NULL;
	END
GO

if not exists(select * from sys.columns where Name = N'DateOfBirth' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info ADD [DateOfBirth] [varchar](50) NULL;
	END
GO

if not exists(select * from sys.columns where Name = N'Age' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info ADD [Age] [tinyint] NULL;
	END
GO

if not exists(select * from sys.columns where Name = N'Gender' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info ADD [Gender] [varchar](50) NULL;
	END
GO

if not exists(select * from sys.columns where Name = N'Priority' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info ADD [Priority] [varchar](50) NULL;
	END
GO

if not exists(select * from sys.columns where Name = N'SystemId' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info ADD [SystemId] [varchar](50) NULL;
	END
GO

if not exists(select * from sys.columns where Name = N'Assigned_PCM' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info ADD [Assigned_PCM] [varchar](100) NULL;
	END
GO

-- drop new columns
ALTER TABLE [RPT_Patient_PCM_Program_Info] DROP COLUMN 
	[MongoPatientId], 
	[MongoPatientProgramId], 
	[LastStateUpdateDate], 
	[GraduatedFlag], 
	[Eligibility], 
	[Enrollment]
GO