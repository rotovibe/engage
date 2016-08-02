IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'ClinicalBackGround' AND Object_ID = Object_ID(N'RPT_Patient'))
	BEGIN
		ALTER TABLE RPT_Patient
		DROP COLUMN [ClinicalBackGround];
	END
GO


IF EXISTS (SELECT * FROM sys.columns WHERE Name IN (N'ClinicalBackGround', N'Background') AND Object_ID = Object_ID(N'RPT_PatientInformation'))
	BEGIN
		ALTER TABLE RPT_PatientInformation
		DROP COLUMN [Background], COLUMN [ClinicalBackGround]
	END
GO