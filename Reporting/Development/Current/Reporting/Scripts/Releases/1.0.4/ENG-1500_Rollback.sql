ALTER TABLE [dbo].[RPT_PatientNotes_Dim]
ALTER COLUMN Text [varchar](5000) NULL
GO

ALTER TABLE [dbo].[RPT_TouchPoint_Dim]
ALTER COLUMN Text [varchar](5000) NULL
GO

ALTER TABLE [dbo].[RPT_Flat_PatientMedSup_Dim]
ALTER COLUMN StartDate [varchar](25) NULL
GO

ALTER TABLE [dbo].[RPT_Flat_PatientMedSup_Dim]
ALTER COLUMN EndDate [varchar](25) NULL
GO

ALTER TABLE [dbo].[RPT_Flat_MedicationMap_Dim]
ALTER COLUMN StartDate [varchar](25) NULL
GO

ALTER TABLE [dbo].[RPT_Flat_MedicationMap_Dim]
ALTER COLUMN EndDate [varchar](25) NULL
GO