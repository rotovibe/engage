/****** Object:  Table [dbo].[RPT_User]    Script Date: 05/04/2015 12:02:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_User]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_User]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RPT_User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[ResourceId] [varchar](50) NULL,
	[FirstName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[PreferredName] [varchar](100) NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RPT_User]') AND name = N'IX_RPT_User_MongoId')
CREATE NONCLUSTERED INDEX [IX_RPT_User_MongoId] ON [dbo].[RPT_User] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
