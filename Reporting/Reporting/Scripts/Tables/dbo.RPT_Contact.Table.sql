SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_Contact](
	[ContactId] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[ResourceId] [varchar](50) NULL,
	[FirstName] [varchar](100) NULL,
	[MiddleName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[PreferredName] [varchar](100) NULL,
	[Gender] [varchar](50) NULL,
	[MongoTimeZone] [varchar](50) NULL,
	[TimeZone] [int] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[ExtraElements] [varchar](max) NULL,
 CONSTRAINT [PK_Contac] PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_Contact]  WITH CHECK ADD  CONSTRAINT [FK_Contact_Patient] FOREIGN KEY([PatientId])
REFERENCES [RPT_Patient] ([PatientId])
GO
ALTER TABLE [RPT_Contact] CHECK CONSTRAINT [FK_Contact_Patient]
GO
ALTER TABLE [RPT_Contact]  WITH CHECK ADD  CONSTRAINT [FK_Contact_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_Contact] CHECK CONSTRAINT [FK_Contact_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_Contact]  WITH CHECK ADD  CONSTRAINT [FK_Contact_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_Contact] CHECK CONSTRAINT [FK_Contact_UserMongoUpdatedBy]
GO
ALTER TABLE [RPT_Contact]  WITH CHECK ADD  CONSTRAINT [FK_ContactMongoTimeZoneLookUp] FOREIGN KEY([TimeZone])
REFERENCES [RPTMongoTimeZoneLookUp] ([TimeZoneId])
GO
ALTER TABLE [RPT_Contact] CHECK CONSTRAINT [FK_ContactMongoTimeZoneLookUp]
GO
