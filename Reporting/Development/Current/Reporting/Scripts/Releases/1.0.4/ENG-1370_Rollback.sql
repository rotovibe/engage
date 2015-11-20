ALTER TABLE [dbo].[RPT_TouchPoint_Dim]
DROP COLUMN [RecordUpdatedOn],
COLUMN [RecordUpdatedBy]
GO

ALTER TABLE  [dbo].[RPT_PatientNotes_Dim]
DROP COLUMN [RecordUpdatedOn],
COLUMN [RecordUpdatedBy]
GO