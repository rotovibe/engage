/* RPT_System */
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_System]') AND type in (N'U'))
	DROP TABLE [dbo].[RPT_System]
GO

CREATE TABLE [dbo].[RPT_System](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[DisplayLabel] [varchar](100) NULL,
	[Field] [varchar](100) NULL,
	[Name] [varchar](100) NULL,
	[Primary] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[Version] [float] NULL,
	[UpdatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedOn] [datetime] NULL,
	[DeleteFlag] [varchar](50) NULL,
	[TTLDate] [datetime] NULL,
	[LastUpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_RPT_System] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/* RPT_PatientSystem */
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Value' AND Object_ID = Object_ID(N'RPT_PatientSystem'))
	BEGIN
		ALTER TABLE [dbo].[RPT_PatientSystem] ADD [Value] VARCHAR(100) NULL;
	END
GO
	
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'DataSource' AND Object_ID = Object_ID(N'RPT_PatientSystem'))
	BEGIN
		ALTER TABLE [dbo].[RPT_PatientSystem] ADD [DataSource] VARCHAR(100) NULL;
	END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Status' AND Object_ID = Object_ID(N'RPT_PatientSystem'))
	BEGIN
		ALTER TABLE [dbo].[RPT_PatientSystem] ADD [Status] VARCHAR(100) NULL;
	END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Primary' AND Object_ID = Object_ID(N'RPT_PatientSystem'))
	BEGIN
		ALTER TABLE [dbo].[RPT_PatientSystem] ADD [Primary] VARCHAR(100) NULL;
	END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'SysId' AND Object_ID = Object_ID(N'RPT_PatientSystem'))
	BEGIN
		ALTER TABLE [dbo].[RPT_PatientSystem] ADD [SysId] VARCHAR(100) NULL;
	END
GO
	
/* RPT_PatientInformation */
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'PrimaryId' AND Object_ID = Object_ID(N'RPT_PatientInformation'))
	BEGIN
		ALTER TABLE [dbo].[RPT_PatientInformation] ADD [PrimaryId] VARCHAR(100) NULL;
	END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'PrimaryIdSystem' AND Object_ID = Object_ID(N'RPT_PatientInformation'))
	BEGIN
		ALTER TABLE [dbo].[RPT_PatientInformation] ADD [PrimaryIdSystem] VARCHAR(100) NULL;
	END
GO	

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'EngageId' AND Object_ID = Object_ID(N'RPT_PatientInformation'))
	BEGIN
		ALTER TABLE [dbo].[RPT_PatientInformation] ADD [EngageId] VARCHAR(100) NULL;
	END
GO		