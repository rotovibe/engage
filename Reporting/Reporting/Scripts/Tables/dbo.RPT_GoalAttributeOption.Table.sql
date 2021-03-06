SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_GoalAttributeOption](
	[GoalAttributeOptionId] [int] IDENTITY(1,1) NOT NULL,
	[GoalAttributeId] [int] NOT NULL,
	[MongoGoalAttributeId] [varchar](50) NULL,
	[Key] [int] NULL,
	[Value] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_GoalAttributeOption] PRIMARY KEY CLUSTERED 
(
	[GoalAttributeOptionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_GoalAttributeOption]  WITH CHECK ADD  CONSTRAINT [FK_GoalAttributeOption_GoalAttribute] FOREIGN KEY([GoalAttributeId])
REFERENCES [RPT_GoalAttribute] ([GoalAttributeID])
GO
ALTER TABLE [RPT_GoalAttributeOption] CHECK CONSTRAINT [FK_GoalAttributeOption_GoalAttribute]
GO
ALTER TABLE [RPT_GoalAttributeOption]  WITH CHECK ADD  CONSTRAINT [FK_GoalAttributeOption_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_GoalAttributeOption] CHECK CONSTRAINT [FK_GoalAttributeOption_UserMongoUpdatedBy]
GO
