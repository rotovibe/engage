SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_UserRecentList](
	[UserRecentListId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedOn] [datetime] NULL,
 CONSTRAINT [PK_UserRecentList] PRIMARY KEY CLUSTERED 
(
	[UserRecentListId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_UserRecentList]  WITH CHECK ADD  CONSTRAINT [FK_UserRecentList_User] FOREIGN KEY([UserId])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_UserRecentList] CHECK CONSTRAINT [FK_UserRecentList_User]
GO
