SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_AllergySourceLookUp](
	[AllergySourceId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](100) NOT NULL,
	[lookUpType] [varchar](200) NULL,
	[Name] [varchar](300) NULL,
	[Active] [varchar](50) NULL,
	[Default] [varchar](50) NULL,
 CONSTRAINT [PK_RPT_AllergySourceLookUp] PRIMARY KEY CLUSTERED 
(
	[AllergySourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
