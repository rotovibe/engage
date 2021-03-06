SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientProgramModuleActionObjective](
	[ActionObjectiveId] [int] IDENTITY(1,1) NOT NULL,
	[ActionId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Value] [varchar](50) NULL,
	[Units] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
 CONSTRAINT [PK_PatientProgramModuleActionObjective] PRIMARY KEY CLUSTERED 
(
	[ActionObjectiveId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientProgramModuleActionObjective]  WITH CHECK ADD  CONSTRAINT [FK_PatientProgramModuleActionObjective_PatientProgramModuleAction] FOREIGN KEY([ActionId])
REFERENCES [RPT_PatientProgramAction] ([ActionId])
GO
ALTER TABLE [RPT_PatientProgramModuleActionObjective] CHECK CONSTRAINT [FK_PatientProgramModuleActionObjective_PatientProgramModuleAction]
GO
