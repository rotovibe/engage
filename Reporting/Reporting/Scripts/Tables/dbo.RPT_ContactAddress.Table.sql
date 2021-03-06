SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_ContactAddress](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NOT NULL,
	[MongoContactId] [varchar](50) NULL,
	[MongoId] [varchar](50) NOT NULL,
	[TypeId] [int] NOT NULL,
	[MongoCommTypeId] [varchar](50) NULL,
	[StateId] [int] NOT NULL,
	[MongoStateId] [varchar](50) NULL,
	[Line1] [varchar](max) NULL,
	[Line2] [varchar](max) NULL,
	[Line3] [varchar](max) NULL,
	[City] [varchar](max) NULL,
	[PostalCode] [varchar](50) NULL,
	[Preferred] [varchar](50) NULL,
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
 CONSTRAINT [PK_ContactAddress] PRIMARY KEY CLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_ContactAddress]  WITH CHECK ADD  CONSTRAINT [FK_ContactAddress_CommTypeLookUp] FOREIGN KEY([TypeId])
REFERENCES [RPT_CommTypeLookUp] ([CommTypeId])
GO
ALTER TABLE [RPT_ContactAddress] CHECK CONSTRAINT [FK_ContactAddress_CommTypeLookUp]
GO
ALTER TABLE [RPT_ContactAddress]  WITH CHECK ADD  CONSTRAINT [FK_ContactAddress_Contact] FOREIGN KEY([ContactId])
REFERENCES [RPT_Contact] ([ContactId])
GO
ALTER TABLE [RPT_ContactAddress] CHECK CONSTRAINT [FK_ContactAddress_Contact]
GO
ALTER TABLE [RPT_ContactAddress]  WITH CHECK ADD  CONSTRAINT [FK_ContactAddress_StateLookUp] FOREIGN KEY([StateId])
REFERENCES [RPT_StateLookUp] ([StateId])
GO
ALTER TABLE [RPT_ContactAddress] CHECK CONSTRAINT [FK_ContactAddress_StateLookUp]
GO
ALTER TABLE [RPT_ContactAddress]  WITH CHECK ADD  CONSTRAINT [FK_ContactAddress_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_ContactAddress] CHECK CONSTRAINT [FK_ContactAddress_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_ContactAddress]  WITH CHECK ADD  CONSTRAINT [FK_ContactAddress_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_ContactAddress] CHECK CONSTRAINT [FK_ContactAddress_UserMongoUpdatedBy]
GO
