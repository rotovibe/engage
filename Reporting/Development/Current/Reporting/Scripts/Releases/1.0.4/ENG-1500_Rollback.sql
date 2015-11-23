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

DROP INDEX [IX_RPT_Medication_FamilyId] ON [dbo].[RPT_Medication]
GO

ALTER TABLE [dbo].[RPT_Medication]
ALTER COLUMN StartDate [varchar](100) NULL
GO

ALTER TABLE [dbo].[RPT_Medication]
ALTER COLUMN EndDate [varchar](100) NULL
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