
-- add new columns
if not exists(select * from sys.columns where Name = N'MongoPatientId' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info ADD [MongoPatientId] [varchar](50) NULL;
	END
GO
if not exists(select * from sys.columns where Name = N'MongoPatientProgramId' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info ADD [MongoPatientProgramId] [varchar](50) NULL;
	END
GO
if not exists(select * from sys.columns where Name = N'LastStateUpdateDate' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info ADD [LastStateUpdateDate] DATETIME NULL;
	END
GO
if not exists(select * from sys.columns where Name = N'GraduatedFlag' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info ADD [GraduatedFlag] [varchar](10) NULL;
	END
GO
if not exists(select * from sys.columns where Name = N'Eligibility' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info ADD [Eligibility] [varchar](100) NULL;
	END
GO
if not exists(select * from sys.columns where Name = N'Enrollment' and Object_ID = Object_ID(N'RPT_Patient_PCM_Program_Info'))
	BEGIN
		ALTER TABLE RPT_Patient_PCM_Program_Info ADD [Enrollment] [varchar](100) NULL;
	END
GO