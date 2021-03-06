ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_Patient]
GO
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_UserMongoRecordCreatedBy]
GO
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_UserMongoUpdatedBy]
GO
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_Patient]
GO
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_UserMongoRecordCreatedBy]
GO
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_UserMongoUpdatedBy]
GO
DROP TABLE [dbo].[RPT_PatientSystem]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RPT_PatientSystem](
	[PatientSystemId] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Label] [varchar](50) NULL,
	[SystemId] [varchar](50) NULL,
	[SystemName] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[ExtraElements] [varchar](max) NULL,
 CONSTRAINT [PK_PatientSystem] PRIMARY KEY CLUSTERED 
(
	[PatientSystemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_RPT_PatientSystem_PatientId] ON [dbo].[RPT_PatientSystem] 
(
	[PatientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RPT_PatientSystem]  WITH CHECK ADD  CONSTRAINT [FK_PatientSystem_Patient] FOREIGN KEY([PatientId])
REFERENCES [dbo].[RPT_Patient] ([PatientId])
GO
ALTER TABLE [dbo].[RPT_PatientSystem] CHECK CONSTRAINT [FK_PatientSystem_Patient]
GO
ALTER TABLE [dbo].[RPT_PatientSystem]  WITH CHECK ADD  CONSTRAINT [FK_PatientSystem_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO
ALTER TABLE [dbo].[RPT_PatientSystem] CHECK CONSTRAINT [FK_PatientSystem_UserMongoRecordCreatedBy]
GO
ALTER TABLE [dbo].[RPT_PatientSystem]  WITH CHECK ADD  CONSTRAINT [FK_PatientSystem_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO
ALTER TABLE [dbo].[RPT_PatientSystem] CHECK CONSTRAINT [FK_PatientSystem_UserMongoUpdatedBy]
GO
