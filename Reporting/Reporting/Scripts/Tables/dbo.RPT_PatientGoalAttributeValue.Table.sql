SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientGoalAttributeValue](
	[ValueId] [int] IDENTITY(1,1) NOT NULL,
	[MongoGoalAttributeId] [varchar](50) NULL,
	[PatientGoalAttributeId] [int] NOT NULL,
	[Value] [varchar](100) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_PatientGoalAttributeValue] PRIMARY KEY CLUSTERED 
(
	[ValueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientGoalAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_PatientGoalAttributeValue_PatientGoalAttribute] FOREIGN KEY([PatientGoalAttributeId])
REFERENCES [RPT_PatientGoalAttribute] ([PatientGoalAttributeId])
GO
ALTER TABLE [RPT_PatientGoalAttributeValue] CHECK CONSTRAINT [FK_PatientGoalAttributeValue_PatientGoalAttribute]
GO
ALTER TABLE [RPT_PatientGoalAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_PatientGoalAttributeValue_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientGoalAttributeValue] CHECK CONSTRAINT [FK_PatientGoalAttributeValue_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientGoalAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_PatientGoalAttributeValue_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientGoalAttributeValue] CHECK CONSTRAINT [FK_PatientGoalAttributeValue_UserMongoUpdatedBy]
GO
