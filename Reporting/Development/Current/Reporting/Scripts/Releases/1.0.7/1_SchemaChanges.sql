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
	[CareTeamFrequencyId] [int] IDENTITY(1,1) NOT NULL,
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
	[CareTeamFrequencyId] ASC
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
	[ContactTypeLookUpId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Name] [varchar](200) NULL,
 CONSTRAINT [PK_ContactTypeLookUp] PRIMARY KEY CLUSTERED 
(
	[ContactTypeLookUpId] ASC
)
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO