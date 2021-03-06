SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientSystem](
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
ALTER TABLE [RPT_PatientSystem]  WITH CHECK ADD  CONSTRAINT [FK_PatientSystem_Patient] FOREIGN KEY([PatientId])
REFERENCES [RPT_Patient] ([PatientId])
GO
ALTER TABLE [RPT_PatientSystem] CHECK CONSTRAINT [FK_PatientSystem_Patient]
GO
ALTER TABLE [RPT_PatientSystem]  WITH CHECK ADD  CONSTRAINT [FK_PatientSystem_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientSystem] CHECK CONSTRAINT [FK_PatientSystem_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientSystem]  WITH CHECK ADD  CONSTRAINT [FK_PatientSystem_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientSystem] CHECK CONSTRAINT [FK_PatientSystem_UserMongoUpdatedBy]
GO
