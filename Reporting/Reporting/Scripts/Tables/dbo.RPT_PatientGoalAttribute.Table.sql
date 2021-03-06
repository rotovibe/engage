SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientGoalAttribute](
	[PatientGoalAttributeId] [int] IDENTITY(1,1) NOT NULL,
	[MongoPatientGoalId] [varchar](50) NOT NULL,
	[PatientGoalId] [int] NOT NULL,
	[MongoGoalAttributeId] [varchar](50) NOT NULL,
	[GoalAttributeID] [int] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_PatientGoalAttribute] PRIMARY KEY CLUSTERED 
(
	[PatientGoalAttributeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientGoalAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientGoalAttribute_GoalAttribute] FOREIGN KEY([GoalAttributeID])
REFERENCES [RPT_GoalAttribute] ([GoalAttributeID])
GO
ALTER TABLE [RPT_PatientGoalAttribute] CHECK CONSTRAINT [FK_PatientGoalAttribute_GoalAttribute]
GO
ALTER TABLE [RPT_PatientGoalAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientGoalAttribute_PatientGoal] FOREIGN KEY([PatientGoalId])
REFERENCES [RPT_PatientGoal] ([PatientGoalId])
GO
ALTER TABLE [RPT_PatientGoalAttribute] CHECK CONSTRAINT [FK_PatientGoalAttribute_PatientGoal]
GO
ALTER TABLE [RPT_PatientGoalAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientGoalAttribute_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientGoalAttribute] CHECK CONSTRAINT [FK_PatientGoalAttribute_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientGoalAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientGoalAttribute_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientGoalAttribute] CHECK CONSTRAINT [FK_PatientGoalAttribute_UserMongoUpdatedBy]
GO
