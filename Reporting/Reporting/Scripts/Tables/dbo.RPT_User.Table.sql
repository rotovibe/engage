SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_User](
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
GO
