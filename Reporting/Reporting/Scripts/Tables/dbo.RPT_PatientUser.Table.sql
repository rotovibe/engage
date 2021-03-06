SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientUser](
	[PatientUserId] [int] IDENTITY(1,1) NOT NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[PatientId] [int] NULL,
	[MongoContactId] [varchar](50) NOT NULL,
	[ContactId] [int] NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Flag] [varchar](50) NULL,
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
 CONSTRAINT [PK_PatientUser_1] PRIMARY KEY CLUSTERED 
(
	[PatientUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientUser]  WITH CHECK ADD  CONSTRAINT [FK_PatientUser_Contact] FOREIGN KEY([ContactId])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientUser] CHECK CONSTRAINT [FK_PatientUser_Contact]
GO
ALTER TABLE [RPT_PatientUser]  WITH CHECK ADD  CONSTRAINT [FK_PatientUser_Patient] FOREIGN KEY([PatientId])
REFERENCES [RPT_Patient] ([PatientId])
GO
ALTER TABLE [RPT_PatientUser] CHECK CONSTRAINT [FK_PatientUser_Patient]
GO
ALTER TABLE [RPT_PatientUser]  WITH CHECK ADD  CONSTRAINT [FK_PatientUser_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientUser] CHECK CONSTRAINT [FK_PatientUser_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientUser]  WITH CHECK ADD  CONSTRAINT [FK_PatientUser_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientUser] CHECK CONSTRAINT [FK_PatientUser_UserMongoUpdatedBy]
GO
