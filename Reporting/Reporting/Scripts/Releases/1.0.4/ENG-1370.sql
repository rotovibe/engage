ALTER TABLE [dbo].[RPT_TouchPoint_Dim]
ADD [RecordUpdatedOn] datetime NULL,
	[RecordUpdatedBy] [varchar](200) NULL
GO

ALTER TABLE [dbo].[RPT_PatientNotes_Dim]
ADD [RecordUpdatedOn] datetime NULL,
	[RecordUpdatedBy] [varchar](200) NULL
GO