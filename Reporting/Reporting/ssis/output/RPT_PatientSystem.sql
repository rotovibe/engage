/****** Object:  Table [dbo].[RPT_PatientSystem]    Script Date: 05/04/2015 12:02:21 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_Patient]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_Patient]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_UserMongoRecordCreatedBy]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_UserMongoUpdatedBy]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_Patient]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_Patient]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_UserMongoRecordCreatedBy]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] DROP CONSTRAINT [FK_PatientSystem_UserMongoUpdatedBy]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientSystem]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]') AND type in (N'U'))
BEGIN
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
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]') AND name = N'IX_RPT_PatientSystem_PatientId')
CREATE NONCLUSTERED INDEX [IX_RPT_PatientSystem_PatientId] ON [dbo].[RPT_PatientSystem] 
(
	[PatientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_Patient]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem]  WITH CHECK ADD  CONSTRAINT [FK_PatientSystem_Patient] FOREIGN KEY([PatientId])
REFERENCES [dbo].[RPT_Patient] ([PatientId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_Patient]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] CHECK CONSTRAINT [FK_PatientSystem_Patient]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem]  WITH CHECK ADD  CONSTRAINT [FK_PatientSystem_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] CHECK CONSTRAINT [FK_PatientSystem_UserMongoRecordCreatedBy]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem]  WITH CHECK ADD  CONSTRAINT [FK_PatientSystem_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PatientSystem_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_PatientSystem]'))
ALTER TABLE [dbo].[RPT_PatientSystem] CHECK CONSTRAINT [FK_PatientSystem_UserMongoUpdatedBy]
GO
