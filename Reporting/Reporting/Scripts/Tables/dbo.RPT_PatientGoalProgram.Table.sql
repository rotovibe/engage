SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientGoalProgram](
	[PatientGoalProgramId] [int] IDENTITY(1,1) NOT NULL,
	[MongoPatientGoalId] [varchar](50) NOT NULL,
	[PatientGoalId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_PatientGoalProgram] PRIMARY KEY CLUSTERED 
(
	[PatientGoalProgramId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientGoalProgram]  WITH CHECK ADD  CONSTRAINT [FK_PatientGoalProgram_PatientGoal] FOREIGN KEY([PatientGoalId])
REFERENCES [RPT_PatientGoal] ([PatientGoalId])
GO
ALTER TABLE [RPT_PatientGoalProgram] CHECK CONSTRAINT [FK_PatientGoalProgram_PatientGoal]
GO
ALTER TABLE [RPT_PatientGoalProgram]  WITH CHECK ADD  CONSTRAINT [FK_PatientGoalProgram_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientGoalProgram] CHECK CONSTRAINT [FK_PatientGoalProgram_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientGoalProgram]  WITH CHECK ADD  CONSTRAINT [FK_PatientGoalProgram_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientGoalProgram] CHECK CONSTRAINT [FK_PatientGoalProgram_UserMongoUpdatedBy]
GO
