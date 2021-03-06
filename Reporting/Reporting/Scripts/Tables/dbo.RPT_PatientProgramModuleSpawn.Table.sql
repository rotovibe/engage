SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientProgramModuleSpawn](
	[ModuleSpawnId] [int] IDENTITY(1,1) NOT NULL,
	[PatientProgramModuleId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PatientProgramModuleSpawn] PRIMARY KEY CLUSTERED 
(
	[ModuleSpawnId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientProgramModuleSpawn]  WITH CHECK ADD  CONSTRAINT [FK_PatientProgramModuleSpawn_PatientProgramModule] FOREIGN KEY([PatientProgramModuleId])
REFERENCES [RPT_PatientProgramModule] ([PatientProgramModuleId])
GO
ALTER TABLE [RPT_PatientProgramModuleSpawn] CHECK CONSTRAINT [FK_PatientProgramModuleSpawn_PatientProgramModule]
GO
