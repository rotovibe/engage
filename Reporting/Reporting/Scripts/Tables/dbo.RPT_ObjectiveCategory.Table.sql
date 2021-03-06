SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_ObjectiveCategory](
	[ObjectiveCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[ObjectiveId] [int] NOT NULL,
	[MongoObjectiveLookUpId] [varchar](50) NULL,
	[CategoryId] [int] NOT NULL,
	[MongoCategoryLookUpId] [varchar](50) NULL,
 CONSTRAINT [PK_ObjectiveCategory] PRIMARY KEY CLUSTERED 
(
	[ObjectiveCategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_ObjectiveCategory]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveCategory_ObjectiveLookUp] FOREIGN KEY([ObjectiveId])
REFERENCES [RPT_ObjectiveLookUp] ([ObjectiveId])
GO
ALTER TABLE [RPT_ObjectiveCategory] CHECK CONSTRAINT [FK_ObjectiveCategory_ObjectiveLookUp]
GO
ALTER TABLE [RPT_ObjectiveCategory]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveCategoryMongoCategoryLookUp] FOREIGN KEY([CategoryId])
REFERENCES [RPTMongoCategoryLookUp] ([CategoryId])
GO
ALTER TABLE [RPT_ObjectiveCategory] CHECK CONSTRAINT [FK_ObjectiveCategoryMongoCategoryLookUp]
GO
