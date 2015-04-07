SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_ContactPhone](
	[PhoneId] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NOT NULL,
	[MongoContactId] [varchar](50) NULL,
	[MongoId] [varchar](50) NOT NULL,
	[TypeId] [int] NOT NULL,
	[MongoCommTypeId] [varchar](50) NULL,
	[Number] [varchar](50) NULL,
	[IsText] [varchar](50) NULL,
	[PhonePreferred] [varchar](50) NULL,
	[TextPreferred] [varchar](50) NULL,
	[OptOut] [varchar](50) NULL,
	[Delete] [varchar](50) NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
 CONSTRAINT [PK_ContactPhone] PRIMARY KEY CLUSTERED 
(
	[PhoneId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_ContactPhone]  WITH CHECK ADD  CONSTRAINT [FK_ContactPhone_CommTypeLookUp] FOREIGN KEY([TypeId])
REFERENCES [RPT_CommTypeLookUp] ([CommTypeId])
GO
ALTER TABLE [RPT_ContactPhone] CHECK CONSTRAINT [FK_ContactPhone_CommTypeLookUp]
GO
ALTER TABLE [RPT_ContactPhone]  WITH CHECK ADD  CONSTRAINT [FK_ContactPhone_Contact] FOREIGN KEY([ContactId])
REFERENCES [RPT_Contact] ([ContactId])
GO
ALTER TABLE [RPT_ContactPhone] CHECK CONSTRAINT [FK_ContactPhone_Contact]
GO
ALTER TABLE [RPT_ContactPhone]  WITH CHECK ADD  CONSTRAINT [FK_ContactPhone_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_ContactPhone] CHECK CONSTRAINT [FK_ContactPhone_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_ContactPhone]  WITH CHECK ADD  CONSTRAINT [FK_ContactPhone_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_ContactPhone] CHECK CONSTRAINT [FK_ContactPhone_UserMongoUpdatedBy]
GO
