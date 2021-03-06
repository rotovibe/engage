SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_GoalAttribute](
	[GoalAttributeID] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Type] [varchar](50) NULL,
	[ControlType] [varchar](50) NULL,
	[Order] [int] NULL,
	[Required] [varchar](50) NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[ExtraElements] [varchar](50) NULL,
 CONSTRAINT [PK_GoalAttribute] PRIMARY KEY CLUSTERED 
(
	[GoalAttributeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_GoalAttribute]  WITH CHECK ADD  CONSTRAINT [FK_GoalAttribute_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_GoalAttribute] CHECK CONSTRAINT [FK_GoalAttribute_UserMongoUpdatedBy]
GO
