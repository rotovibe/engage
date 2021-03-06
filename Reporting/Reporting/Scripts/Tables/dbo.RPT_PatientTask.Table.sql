SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientTask](
	[PatientTaskId] [int] IDENTITY(1,1) NOT NULL,
	[MongoPatientGoalId] [varchar](50) NOT NULL,
	[PatientGoalId] [int] NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Name] [varchar](100) NULL,
	[Description] [varchar](max) NULL,
	[StartDate] [datetime] NULL,
	[Status] [varchar](50) NULL,
	[StatusDate] [datetime] NULL,
	[TargetDate] [datetime] NULL,
	[TargetValue] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[ExtraElements] [varchar](50) NULL,
	[ClosedDate] [datetime] NULL,
	[TemplateId] [varchar](50) NULL,
 CONSTRAINT [PK_PatientTask] PRIMARY KEY CLUSTERED 
(
	[PatientTaskId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientTask]  WITH CHECK ADD  CONSTRAINT [FK_PatientTask_PatientGoal] FOREIGN KEY([PatientGoalId])
REFERENCES [RPT_PatientGoal] ([PatientGoalId])
GO
ALTER TABLE [RPT_PatientTask] CHECK CONSTRAINT [FK_PatientTask_PatientGoal]
GO
ALTER TABLE [RPT_PatientTask]  WITH CHECK ADD  CONSTRAINT [FK_PatientTask_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedBy])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientTask] CHECK CONSTRAINT [FK_PatientTask_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientTask]  WITH CHECK ADD  CONSTRAINT [FK_PatientTask_UserMongoUpdatedBy] FOREIGN KEY([UpdatedBy])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientTask] CHECK CONSTRAINT [FK_PatientTask_UserMongoUpdatedBy]
GO
