SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_LanguageLookUp](
	[LanguageId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[LookUpType] [varchar](max) NULL,
	[Name] [varchar](max) NULL,
	[Code] [varchar](50) NULL,
	[Active] [varchar](50) NULL,
 CONSTRAINT [PK_LanguageLookUp] PRIMARY KEY CLUSTERED 
(
	[LanguageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
