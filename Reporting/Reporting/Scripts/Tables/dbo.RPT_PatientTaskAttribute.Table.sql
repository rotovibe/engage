SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientTaskAttribute](
	[PatientTaskAttributeId] [int] IDENTITY(1,1) NOT NULL,
	[MongoPatientTaskId] [varchar](50) NULL,
	[PatientTaskId] [int] NOT NULL,
	[MongoGoalAttributeId] [varchar](50) NOT NULL,
	[GoalAttributeId] [int] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_PatientTaskAttribute] PRIMARY KEY CLUSTERED 
(
	[PatientTaskAttributeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientTaskAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttribute_GoalAttribute] FOREIGN KEY([GoalAttributeId])
REFERENCES [RPT_GoalAttribute] ([GoalAttributeID])
GO
ALTER TABLE [RPT_PatientTaskAttribute] CHECK CONSTRAINT [FK_PatientTaskAttribute_GoalAttribute]
GO
ALTER TABLE [RPT_PatientTaskAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttribute_PatientTask] FOREIGN KEY([PatientTaskId])
REFERENCES [RPT_PatientTask] ([PatientTaskId])
GO
ALTER TABLE [RPT_PatientTaskAttribute] CHECK CONSTRAINT [FK_PatientTaskAttribute_PatientTask]
GO
ALTER TABLE [RPT_PatientTaskAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttribute_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientTaskAttribute] CHECK CONSTRAINT [FK_PatientTaskAttribute_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientTaskAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttribute_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientTaskAttribute] CHECK CONSTRAINT [FK_PatientTaskAttribute_UserMongoUpdatedBy]
GO
