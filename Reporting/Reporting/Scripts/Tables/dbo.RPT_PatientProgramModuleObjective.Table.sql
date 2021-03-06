SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientProgramModuleObjective](
	[ModuleObjectiveId] [int] IDENTITY(1,1) NOT NULL,
	[PatientProgramModuleId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Value] [varchar](50) NULL,
	[Units] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
 CONSTRAINT [PK_PatientProgramModuleObjective] PRIMARY KEY CLUSTERED 
(
	[ModuleObjectiveId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientProgramModuleObjective]  WITH CHECK ADD  CONSTRAINT [FK_PatientProgramModuleObjective_PatientProgramModule] FOREIGN KEY([PatientProgramModuleId])
REFERENCES [RPT_PatientProgramModule] ([PatientProgramModuleId])
GO
ALTER TABLE [RPT_PatientProgramModuleObjective] CHECK CONSTRAINT [FK_PatientProgramModuleObjective_PatientProgramModule]
GO
