SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_ContactMode](
	[ModeId] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NOT NULL,
	[MongoContactId] [varchar](50) NULL,
	[CommModeLookUpId] [int] NOT NULL,
	[MongoCommModeId] [varchar](50) NULL,
	[Preferred] [varchar](50) NULL,
	[OptOut] [varchar](50) NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
 CONSTRAINT [PK_ContactMode] PRIMARY KEY CLUSTERED 
(
	[ModeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_ContactMode]  WITH CHECK ADD  CONSTRAINT [FK_ContactMode_CommModeLookUp] FOREIGN KEY([CommModeLookUpId])
REFERENCES [RPT_CommModeLookUp] ([CommModeId])
GO
ALTER TABLE [RPT_ContactMode] CHECK CONSTRAINT [FK_ContactMode_CommModeLookUp]
GO
ALTER TABLE [RPT_ContactMode]  WITH CHECK ADD  CONSTRAINT [FK_ContactMode_Contact] FOREIGN KEY([ContactId])
REFERENCES [RPT_Contact] ([ContactId])
GO
ALTER TABLE [RPT_ContactMode] CHECK CONSTRAINT [FK_ContactMode_Contact]
GO
ALTER TABLE [RPT_ContactMode]  WITH CHECK ADD  CONSTRAINT [FK_ContactMode_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_ContactMode] CHECK CONSTRAINT [FK_ContactMode_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_ContactMode]  WITH CHECK ADD  CONSTRAINT [FK_ContactMode_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_ContactMode] CHECK CONSTRAINT [FK_ContactMode_UserMongoUpdatedBy]
GO
