IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'MongoDurationId' AND object_id = Object_ID(N'[dbo].RPT_PatientNote'))
	BEGIN
		ALTER TABLE RPT_PatientNote
		ADD [MongoDurationId] [varchar] (50) NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'DurationId' AND object_id = Object_ID(N'[dbo].RPT_PatientNote'))
	BEGIN
		ALTER TABLE RPT_PatientNote
		ADD [DurationId] [int] NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'Duration' AND object_id = Object_ID(N'[dbo].RPT_PatientNotes_Dim'))
	BEGIN
		ALTER TABLE RPT_PatientNotes_Dim
		ADD [Duration] [varchar] (100) NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'Duration' AND object_id = Object_ID(N'[dbo].RPT_TouchPoint_Dim'))
	BEGIN
		ALTER TABLE RPT_TouchPoint_Dim
		ADD [Duration] [varchar] (100) NULL
	END
GO

CREATE TABLE [dbo].[RPT_NoteDurationLookUp](
	[NoteDurationId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[LookUpType] [varchar](max) NULL,
	[Name] [varchar](max) NULL,
	[Default] [varchar](50) NULL,
	[Active] [varchar](50) NULL,
 CONSTRAINT [PK_NoteDurationLookUp] PRIMARY KEY CLUSTERED 
(
	[NoteDurationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] 

GO