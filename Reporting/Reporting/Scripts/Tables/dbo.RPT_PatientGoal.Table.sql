SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientGoal](
	[PatientGoalId] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Name] [varchar](500) NULL,
	[Description] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Source] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[StatusDate] [datetime] NULL,
	[TargetDate] [datetime] NULL,
	[TargetValue] [varchar](300) NULL,
	[Type] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[ExtraElements] [varchar](max) NULL,
	[ClosedDate] [datetime] NULL,
	[TemplateId] [varchar](50) NULL,
 CONSTRAINT [PK_PatientGoal] PRIMARY KEY CLUSTERED 
(
	[PatientGoalId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientGoal]  WITH CHECK ADD  CONSTRAINT [FK_PatientGoal_Patient] FOREIGN KEY([PatientId])
REFERENCES [RPT_Patient] ([PatientId])
GO
ALTER TABLE [RPT_PatientGoal] CHECK CONSTRAINT [FK_PatientGoal_Patient]
GO
ALTER TABLE [RPT_PatientGoal]  WITH CHECK ADD  CONSTRAINT [FK_PatientGoal_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedBy])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientGoal] CHECK CONSTRAINT [FK_PatientGoal_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientGoal]  WITH CHECK ADD  CONSTRAINT [FK_PatientGoal_UserMongoUpdatedBy] FOREIGN KEY([UpdatedBy])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientGoal] CHECK CONSTRAINT [FK_PatientGoal_UserMongoUpdatedBy]
GO
