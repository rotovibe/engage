----------------------------------------------------------------------------------------------------------------------------------
--ENG-2010
----------------------------------------------------------------------------------------------------------------------------------

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
 CONSTRAINT [PK_CareTeamFrequency] PRIMARY KEY CLUSTERED 
(
	[CareTeamFrequencyId] ASC
)
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
