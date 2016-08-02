IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'ClinicalBackGround' AND object_id = Object_ID(N'[dbo].RPT_Patient'))
	BEGIN
		ALTER TABLE RPT_Patient
		ADD [ClinicalBackGround] [varchar](max) NULL
	END
GO


IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name IN (N'ClinicalBackGround', N'Background') AND object_id = Object_ID(N'[dbo].RPT_PatientInformation'))
	BEGIN
		ALTER TABLE RPT_PatientInformation
		ADD [Background] [varchar](max) NULL, [ClinicalBackGround] [varchar](max) NULL
		
	END
GO