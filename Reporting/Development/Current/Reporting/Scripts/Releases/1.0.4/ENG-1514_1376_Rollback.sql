ALTER TABLE [dbo].[RPT_PatientNote]
DROP COLUMN [DataSource],
	COLUMN [Duration]
GO

ALTER TABLE [dbo].[RPT_PatientNotes_Dim]
DROP COLUMN [DataSource],
	COLUMN [DurationInt]
GO

ALTER TABLE [dbo].[RPT_TouchPoint_Dim]
DROP COLUMN [DataSource],
	COLUMN [DurationInt]
GO