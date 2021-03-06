/****** Object:  Table [dbo].[RPT_Patient]    Script Date: 05/04/2015 12:02:06 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient] DROP CONSTRAINT [FK_Patient_UserMongoRecordCreatedBy]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient] DROP CONSTRAINT [FK_Patient_UserMongoUpdatedBy]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient] DROP CONSTRAINT [FK_Patient_UserMongoRecordCreatedBy]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient] DROP CONSTRAINT [FK_Patient_UserMongoUpdatedBy]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Patient]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_Patient]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Patient]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_Patient](
	[PatientId] [int] IDENTITY(1,1) NOT NULL,
	[DisplayPatientSystemId] [int] NULL,
	[MongoPatientSystemId] [varchar](50) NULL,
	[MongoId] [varchar](50) NOT NULL,
	[FirstName] [varchar](100) NULL,
	[MiddleName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[PreferredName] [varchar](100) NULL,
	[Suffix] [varchar](50) NULL,
	[DateOfBirth] [varchar](50) NULL,
	[Gender] [varchar](50) NULL,
	[Priority] [varchar](50) NULL,
	[Background] [varchar](max) NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[ExtraElements] [varchar](max) NULL,
	[FSSN] [varchar](100) NULL,
	[LSSN] [int] NULL,
 CONSTRAINT [PK_Patient] PRIMARY KEY CLUSTERED 
(
	[PatientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Patient]') AND name = N'IX_RPT_Patient_MongoId')
CREATE NONCLUSTERED INDEX [IX_RPT_Patient_MongoId] ON [dbo].[RPT_Patient] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient]  WITH CHECK ADD  CONSTRAINT [FK_Patient_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoRecordCreatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient] CHECK CONSTRAINT [FK_Patient_UserMongoRecordCreatedBy]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient]  WITH CHECK ADD  CONSTRAINT [FK_Patient_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [dbo].[RPT_User] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_UserMongoUpdatedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[RPT_Patient]'))
ALTER TABLE [dbo].[RPT_Patient] CHECK CONSTRAINT [FK_Patient_UserMongoUpdatedBy]
GO
