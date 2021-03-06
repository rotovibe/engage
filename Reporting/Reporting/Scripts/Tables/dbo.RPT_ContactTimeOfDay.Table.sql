SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_ContactTimeOfDay](
	[TimeOfDayId] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NOT NULL,
	[MongoContactId] [varchar](50) NULL,
	[TimeOfDayLookUpId] [int] NOT NULL,
	[MongoTimeOfDayId] [varchar](50) NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
 CONSTRAINT [PK_ContactTimeOfDay] PRIMARY KEY CLUSTERED 
(
	[TimeOfDayId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_ContactTimeOfDay]  WITH CHECK ADD  CONSTRAINT [FK_ContactTimeOfDay_Contact] FOREIGN KEY([ContactId])
REFERENCES [RPT_Contact] ([ContactId])
GO
ALTER TABLE [RPT_ContactTimeOfDay] CHECK CONSTRAINT [FK_ContactTimeOfDay_Contact]
GO
ALTER TABLE [RPT_ContactTimeOfDay]  WITH CHECK ADD  CONSTRAINT [FK_ContactTimeOfDay_TimesOfDayLookUp] FOREIGN KEY([TimeOfDayLookUpId])
REFERENCES [RPT_TimesOfDayLookUp] ([TimesOfDayId])
GO
ALTER TABLE [RPT_ContactTimeOfDay] CHECK CONSTRAINT [FK_ContactTimeOfDay_TimesOfDayLookUp]
GO
ALTER TABLE [RPT_ContactTimeOfDay]  WITH CHECK ADD  CONSTRAINT [FK_ContactTimeOfDay_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_ContactTimeOfDay] CHECK CONSTRAINT [FK_ContactTimeOfDay_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_ContactTimeOfDay]  WITH CHECK ADD  CONSTRAINT [FK_ContactTimeOfDay_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_ContactTimeOfDay] CHECK CONSTRAINT [FK_ContactTimeOfDay_UserMongoUpdatedBy]
GO
