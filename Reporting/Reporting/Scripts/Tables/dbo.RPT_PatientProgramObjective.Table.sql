SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientProgramObjective](
	[PatientProgramObjectiveId] [int] IDENTITY(1,1) NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Value] [varchar](max) NULL,
	[Measurement] [varchar](max) NULL,
	[Status] [varchar](50) NULL,
 CONSTRAINT [PK_PatientProgramObjective] PRIMARY KEY CLUSTERED 
(
	[PatientProgramObjectiveId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientProgramObjective]  WITH CHECK ADD  CONSTRAINT [FK_PatientProgramObjective_PatientProgram] FOREIGN KEY([PatientProgramId])
REFERENCES [RPT_PatientProgram] ([PatientProgramId])
GO
ALTER TABLE [RPT_PatientProgramObjective] CHECK CONSTRAINT [FK_PatientProgramObjective_PatientProgram]
GO
