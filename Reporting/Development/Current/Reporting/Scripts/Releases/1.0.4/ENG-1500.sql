ALTER TABLE [dbo].[RPT_PatientNotes_Dim]
ALTER COLUMN Text [varchar](max) NULL
GO

ALTER TABLE [dbo].[RPT_TouchPoint_Dim]
ALTER COLUMN Text [varchar](max) NULL
GO

ALTER TABLE [dbo].[RPT_Flat_PatientMedSup_Dim]
ALTER COLUMN StartDate datetime NULL
GO

ALTER TABLE [dbo].[RPT_Flat_PatientMedSup_Dim]
ALTER COLUMN EndDate datetime NULL
GO

ALTER TABLE [dbo].[RPT_Flat_MedicationMap_Dim]
ALTER COLUMN StartDate datetime NULL
GO

ALTER TABLE [dbo].[RPT_Flat_MedicationMap_Dim]
ALTER COLUMN EndDate datetime NULL
GO

ALTER TABLE [dbo].[RPT_Medication]
ALTER COLUMN StartDate datetime NULL
GO

ALTER TABLE [dbo].[RPT_Medication]
ALTER COLUMN EndDate datetime NULL
GO