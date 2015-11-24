ALTER TABLE [dbo].[RPT_PatientNote]
ADD [DataSource] [varchar](50) NULL,
	[Duration] [int] NULL
GO

ALTER TABLE [dbo].[RPT_PatientNotes_Dim]
ADD [DataSource] [varchar](50) NULL,
	[DurationInt] [int] NULL
GO

ALTER TABLE [dbo].[RPT_TouchPoint_Dim]
ADD [DataSource] [varchar](50) NULL,
	[DurationInt] [int] NULL
GO