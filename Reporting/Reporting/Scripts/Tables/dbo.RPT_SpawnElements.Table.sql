SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_SpawnElements](
	[SpawnId] [int] IDENTITY(1,1) NOT NULL,
	[PlanElementId] [int] NOT NULL,
	[MongoPlanElementId] [varchar](50) NOT NULL,
	[MongoSpawnId] [varchar](50) NOT NULL,
	[Tag] [varchar](100) NULL,
	[TypeId] [int] NOT NULL,
 CONSTRAINT [PK_PatientProgramModuleActionStepSpawn] PRIMARY KEY CLUSTERED 
(
	[SpawnId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_SpawnElements]  WITH CHECK ADD  CONSTRAINT [FK_RPT_SpawnElements_RPT_SpawnElementTypeCode] FOREIGN KEY([TypeId])
REFERENCES [RPT_SpawnElementTypeCode] ([TypeId])
GO
ALTER TABLE [RPT_SpawnElements] CHECK CONSTRAINT [FK_RPT_SpawnElements_RPT_SpawnElementTypeCode]
GO
