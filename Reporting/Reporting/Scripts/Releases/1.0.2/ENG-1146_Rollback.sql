/*** RPT_Medication ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Medication')
	DROP TABLE [RPT_Medication];
GO

/*** [RPT_MedPharmClass] ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_MedPharmClass')
	DROP TABLE [RPT_MedPharmClass];
GO

/*** RPT_MedicationMap ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_MedicationMap')
	DROP TABLE [RPT_MedicationMap];
GO

/**** RPT_PatientMedSupp ****/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientMedSupp]') AND type in (N'U'))
	DROP TABLE [dbo].[RPT_PatientMedSupp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientMedSupp]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientMedSupp]
GO

/*** RPT_PatientMedFrequency ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientMedFrequency]') AND type in (N'U'))
	DROP TABLE [dbo].[RPT_PatientMedFrequency]
GO
