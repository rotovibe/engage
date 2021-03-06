SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_ContactRecentList](
	[ContactRecentListId] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NOT NULL,
	[MongoContactId] [varchar](50) NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
 CONSTRAINT [PK_ContactRecentList] PRIMARY KEY CLUSTERED 
(
	[ContactRecentListId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_ContactRecentList]  WITH CHECK ADD  CONSTRAINT [FK_ContactRecentList_Contact] FOREIGN KEY([ContactId])
REFERENCES [RPT_Contact] ([ContactId])
GO
ALTER TABLE [RPT_ContactRecentList] CHECK CONSTRAINT [FK_ContactRecentList_Contact]
GO
ALTER TABLE [RPT_ContactRecentList]  WITH CHECK ADD  CONSTRAINT [FK_ContactRecentList_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_ContactRecentList] CHECK CONSTRAINT [FK_ContactRecentList_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_ContactRecentList]  WITH CHECK ADD  CONSTRAINT [FK_ContactRecentList_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_ContactRecentList] CHECK CONSTRAINT [FK_ContactRecentList_UserMongoUpdatedBy]
GO
