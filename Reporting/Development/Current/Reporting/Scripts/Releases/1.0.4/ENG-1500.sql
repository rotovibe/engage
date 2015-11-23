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

DROP INDEX [IX_RPT_Medication_FamilyId] ON [dbo].[RPT_Medication]
GO

UPDATE [dbo].[RPT_Medication]
SET StartDate = null
WHERE isdate(StartDate) = 0
GO

ALTER TABLE [dbo].[RPT_Medication]
ALTER COLUMN StartDate datetime NULL
GO

ALTER TABLE [dbo].[RPT_Medication]
ALTER COLUMN EndDate datetime NULL
GO

CREATE NONCLUSTERED INDEX [IX_RPT_Medication_FamilyId] ON [dbo].[RPT_Medication]
(
	[FamilyId] ASC,
	[ProductId] ASC,
	[NDC] ASC,
	[ProprietaryName] ASC,
	[StartDate] ASC,
	[EndDate] ASC,
	[Version] ASC,
	[DeleteFlag] ASC,
	[TTLDate] ASC,
	[LastUpdatedOn] ASC,
	[MongoRecordCreatedBy] ASC,
	[RecordCreatedOn] ASC,
	[MongoUpdatedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO