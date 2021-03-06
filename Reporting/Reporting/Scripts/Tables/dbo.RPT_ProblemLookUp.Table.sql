SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_ProblemLookUp](
	[ProblemId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[LookUpType] [varchar](max) NULL,
	[Name] [varchar](max) NULL,
	[Type] [varchar](100) NULL,
	[Active] [varchar](50) NULL,
	[CodeSystem] [varchar](100) NULL,
	[Code] [varchar](100) NULL,
	[Default] [varchar](50) NULL,
	[DefaultLevel] [varchar](50) NULL,
 CONSTRAINT [PK_ProblemLookUp] PRIMARY KEY CLUSTERED 
(
	[ProblemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
