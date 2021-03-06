SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientProgramSpawn](
	[PatientProgramSpawnId] [int] IDENTITY(1,1) NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Type] [varchar](50) NULL,
	[Tag] [varchar](max) NULL,
 CONSTRAINT [PK_PatientProgramSpawn] PRIMARY KEY CLUSTERED 
(
	[PatientProgramSpawnId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientProgramSpawn]  WITH CHECK ADD  CONSTRAINT [FK_PatientProgramSpawn_PatientProgram] FOREIGN KEY([PatientProgramId])
REFERENCES [RPT_PatientProgram] ([PatientProgramId])
GO
ALTER TABLE [RPT_PatientProgramSpawn] CHECK CONSTRAINT [FK_PatientProgramSpawn_PatientProgram]
GO
