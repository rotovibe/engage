SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_ContactLanguage](
	[LanguageId] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NOT NULL,
	[MongoContactId] [varchar](50) NULL,
	[LanguageLookUpId] [int] NOT NULL,
	[MongoLanguageLookUpId] [varchar](50) NULL,
	[Preferred] [varchar](50) NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
 CONSTRAINT [PK_ContactLanguage] PRIMARY KEY CLUSTERED 
(
	[LanguageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_ContactLanguage]  WITH CHECK ADD  CONSTRAINT [FK_ContactLanguage_Contact] FOREIGN KEY([ContactId])
REFERENCES [RPT_Contact] ([ContactId])
GO
ALTER TABLE [RPT_ContactLanguage] CHECK CONSTRAINT [FK_ContactLanguage_Contact]
GO
ALTER TABLE [RPT_ContactLanguage]  WITH CHECK ADD  CONSTRAINT [FK_ContactLanguage_LanguageLookUp] FOREIGN KEY([LanguageLookUpId])
REFERENCES [RPT_LanguageLookUp] ([LanguageId])
GO
ALTER TABLE [RPT_ContactLanguage] CHECK CONSTRAINT [FK_ContactLanguage_LanguageLookUp]
GO
ALTER TABLE [RPT_ContactLanguage]  WITH CHECK ADD  CONSTRAINT [FK_ContactLanguage_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_ContactLanguage] CHECK CONSTRAINT [FK_ContactLanguage_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_ContactLanguage]  WITH CHECK ADD  CONSTRAINT [FK_ContactLanguage_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_ContactLanguage] CHECK CONSTRAINT [FK_ContactLanguage_UserMongoUpdatedBy]
GO
