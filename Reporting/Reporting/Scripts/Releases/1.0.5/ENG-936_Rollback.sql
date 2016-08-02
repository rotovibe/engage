CREATE TABLE [dbo].[RPT_PatientTaskAttribute](
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttribute_GoalAttribute] FOREIGN KEY([GoalAttributeId])
REFERENCES [dbo].[RPT_GoalAttribute] ([GoalAttributeID])
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute] CHECK CONSTRAINT [FK_PatientTaskAttribute_GoalAttribute]
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttribute_PatientTask] FOREIGN KEY([PatientTaskId])
REFERENCES [dbo].[RPT_PatientTask] ([PatientTaskId])
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute] CHECK CONSTRAINT [FK_PatientTaskAttribute_PatientTask]
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttribute_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute] CHECK CONSTRAINT [FK_PatientTaskAttribute_UserMongoRecordCreatedBy]
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttribute_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttribute] CHECK CONSTRAINT [FK_PatientTaskAttribute_UserMongoUpdatedBy]
GO

CREATE TABLE [dbo].[RPT_PatientTaskAttributeValue](
	[ValueId] [int] IDENTITY(1,1) NOT NULL,
	[PatientTaskAttributeId] [int] NOT NULL,
	[Value] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_PatientTaskAttributeValue] PRIMARY KEY CLUSTERED 
(
	[ValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttributeValue_PatientTaskAttribute] FOREIGN KEY([PatientTaskAttributeId])
REFERENCES [dbo].[RPT_PatientTaskAttribute] ([PatientTaskAttributeId])
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttributeValue] CHECK CONSTRAINT [FK_PatientTaskAttributeValue_PatientTaskAttribute]
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttributeValue_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttributeValue] CHECK CONSTRAINT [FK_PatientTaskAttributeValue_UserMongoRecordCreatedBy]
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_PatientTaskAttributeValue_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO

ALTER TABLE [dbo].[RPT_PatientTaskAttributeValue] CHECK CONSTRAINT [FK_PatientTaskAttributeValue_UserMongoUpdatedBy]
GO


IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'PatientTaskAttributeName' AND object_id = Object_ID(N'[dbo].RPT_PatientGoalMetrics'))
	BEGIN
		ALTER TABLE RPT_PatientGoalMetrics
		ADD [PatientTaskAttributeName] [varchar] (100) NULL
	END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'PatientTaskAttributeValue' AND object_id = Object_ID(N'[dbo].RPT_PatientGoalMetrics'))
	BEGIN
		ALTER TABLE RPT_PatientGoalMetrics
		ADD [PatientTaskAttributeValue] [varchar] (50) NULL
	END
GO