SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_ReactionLookUp](
	[ReactionId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](100) NOT NULL,
	[lookUpType] [varchar](200) NULL,
	[Name] [varchar](300) NULL,
	[CodeSystem] [varchar](100) NULL,
	[Code] [varchar](100) NULL,
 CONSTRAINT [PK_RPT_ReactionLookUp] PRIMARY KEY CLUSTERED 
(
	[ReactionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
