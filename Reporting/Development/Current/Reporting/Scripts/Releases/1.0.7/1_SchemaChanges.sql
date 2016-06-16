----------------------------------------------------------------------------------------------------------------------------------
--ENG-2010
----------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_ContactTypeLookUp')
	DROP TABLE [dbo].[RPT_ContactTypeLookUp]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [dbo].[RPT_ContactTypeLookUp](
	[ContactTypeLookUpId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Name] [varchar](200) NULL,
	[Role] [varchar](200) NULL,
	[ParentId] [varchar](50) NULL,
	[Group] [varchar](50) NULL,
	[Active] [bit] NULL,
	[Version] [float] NULL,
	[DeleteFlag] [bit] NULL,
	[TTLDate] [datetime] NULL,
	[UpdatedBy] [varchar](50) NULL,
	[LastUpdatedOn] [datetime] NULL,
	[RecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedOn] [datetime] NULL
 CONSTRAINT [PK_CareTeamFrequency] PRIMARY KEY CLUSTERED 
(
	[ContactTypeLookUpId] ASC
)
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_CareTeamFrequency')
	DROP TABLE [dbo].[RPT_CareTeamFrequency]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [dbo].[RPT_CareTeamFrequency](
	[CareTeamFrequencyId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Name] [varchar](200) NULL,
 CONSTRAINT [PK_ContactTypeLookUp] PRIMARY KEY CLUSTERED 
(
	[CareTeamFrequencyId] ASC
)
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_CareTeam')
	DROP TABLE [dbo].[RPT_CareTeam]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [dbo].[RPT_CareTeam](
	[CareTeamId] [int] IDENTITY(1,1) NOT NULL,
	[MongoCareTeamId] [varchar](50) NOT NULL,
	[MongoContactIdForPatient] [varchar](50) NOT NULL,
	[MongoCareMemberId] [varchar](50) NOT NULL,
	[MongoContactIdForCareMember] [varchar](50) NOT NULL,
	[RoleId] [varchar](50) NULL,
	[CustomRoleName] [varchar](200) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Core] [bit] NULL,
	[Notes] [varchar](max) NULL,
	[FrequencyId] [varchar](50) NULL,
	[Distance] [float] NULL,
	[DistanceUnit] [varchar](50) NULL,
	[DataSource] [varchar](200) NULL,
	[ExternalRecordId] [varchar](200) NULL,
	[Status] [varchar](50) NULL,
	[Version] [float] NULL,
	[DeleteFlag] [bit] NULL,
	[TTLDate] [datetime] NULL,
	[UpdatedBy] [varchar](50) NULL,
	[LastUpdatedOn] [datetime] NULL,
	[RecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedOn] [datetime] NULL
 CONSTRAINT [PK_CareTeam] PRIMARY KEY CLUSTERED 
(
	[CareTeamId] ASC
)
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_Assigned_PCM')
	DROP TABLE [dbo].[RPT_Flat_Assigned_PCM]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [dbo].[RPT_Flat_Assigned_PCM](
	[MongoPatientId] [varchar](50) NOT NULL,
	[MongoPCMContactId] [varchar](50) NOT NULL,
	[MongoCareTeamId] [varchar](50) NOT NULL,
	[FirstName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[PreferredName] [varchar](100) NULL,
	[PrimaryCareManagerMongoID] [varchar](50) NULL, 
 CONSTRAINT [PK_RPT_Flat_Assigned_PCM] PRIMARY KEY CLUSTERED 
(
	[MongoPatientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


SET ANSI_PADDING OFF
GO