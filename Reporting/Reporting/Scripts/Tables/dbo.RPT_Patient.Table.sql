SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_Patient](
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
GO
ALTER TABLE [RPT_Patient]  WITH CHECK ADD  CONSTRAINT [FK_Patient_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_Patient] CHECK CONSTRAINT [FK_Patient_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_Patient]  WITH CHECK ADD  CONSTRAINT [FK_Patient_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_Patient] CHECK CONSTRAINT [FK_Patient_UserMongoUpdatedBy]
GO
