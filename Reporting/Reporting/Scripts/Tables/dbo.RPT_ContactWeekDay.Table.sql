SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_ContactWeekDay](
	[WeekDayId] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NOT NULL,
	[MongoContactId] [varchar](50) NULL,
	[WeekDay] [int] NOT NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
 CONSTRAINT [PK_ContactWeekDay] PRIMARY KEY CLUSTERED 
(
	[WeekDayId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_ContactWeekDay]  WITH CHECK ADD  CONSTRAINT [FK_ContactWeekDay_Contact] FOREIGN KEY([ContactId])
REFERENCES [RPT_Contact] ([ContactId])
GO
ALTER TABLE [RPT_ContactWeekDay] CHECK CONSTRAINT [FK_ContactWeekDay_Contact]
GO
ALTER TABLE [RPT_ContactWeekDay]  WITH CHECK ADD  CONSTRAINT [FK_ContactWeekDay_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_ContactWeekDay] CHECK CONSTRAINT [FK_ContactWeekDay_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_ContactWeekDay]  WITH CHECK ADD  CONSTRAINT [FK_ContactWeekDay_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_ContactWeekDay] CHECK CONSTRAINT [FK_ContactWeekDay_UserMongoUpdatedBy]
GO
