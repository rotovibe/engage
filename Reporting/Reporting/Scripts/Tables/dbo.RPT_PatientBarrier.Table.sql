SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RPT_PatientBarrier](
	[PatientBarrierId] [int] IDENTITY(1,1) NOT NULL,
	[PatientGoalId] [int] NOT NULL,
	[MongoPatientGoalId] [varchar](50) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[CategoryLookUpId] [int] NULL,
	[MongoCategoryLookUpId] [varchar](50) NULL,
	[Name] [varchar](500) NULL,
	[Description] [varchar](max) NULL,
	[Status] [varchar](50) NULL,
	[StatusDate] [datetime] NULL,
	[StartDate] [datetime] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[ExtraElements] [varchar](max) NULL,
 CONSTRAINT [PK_PatientBarrier] PRIMARY KEY CLUSTERED 
(
	[PatientBarrierId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RPT_PatientBarrier]  WITH CHECK ADD  CONSTRAINT [FK_PatientBarrier_BarrierCategoryLookUp] FOREIGN KEY([CategoryLookUpId])
REFERENCES [RPT_BarrierCategoryLookUp] ([BarrierCategoryId])
GO
ALTER TABLE [RPT_PatientBarrier] CHECK CONSTRAINT [FK_PatientBarrier_BarrierCategoryLookUp]
GO
ALTER TABLE [RPT_PatientBarrier]  WITH CHECK ADD  CONSTRAINT [FK_PatientBarrier_PatientGoal] FOREIGN KEY([PatientGoalId])
REFERENCES [RPT_PatientGoal] ([PatientGoalId])
GO
ALTER TABLE [RPT_PatientBarrier] CHECK CONSTRAINT [FK_PatientBarrier_PatientGoal]
GO
ALTER TABLE [RPT_PatientBarrier]  WITH CHECK ADD  CONSTRAINT [FK_PatientBarrier_UserMongoRecordCreatedBy] FOREIGN KEY([RecordCreatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientBarrier] CHECK CONSTRAINT [FK_PatientBarrier_UserMongoRecordCreatedBy]
GO
ALTER TABLE [RPT_PatientBarrier]  WITH CHECK ADD  CONSTRAINT [FK_PatientBarrier_UserMongoUpdatedBy] FOREIGN KEY([UpdatedById])
REFERENCES [RPT_User] ([UserId])
GO
ALTER TABLE [RPT_PatientBarrier] CHECK CONSTRAINT [FK_PatientBarrier_UserMongoUpdatedBy]
GO
